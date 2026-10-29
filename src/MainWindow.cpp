#include "MainWindow.h"
#include "EmployeeEditDialog.h"
#include "HistoryWidget.h"

#include <QCryptographicHash>
#include <QDate>
#include <QDateTime>
#include <QSqlError>
#include <QSqlQuery>
#include <QSqlRecord>
#include <QSqlField>
#include <QTableView>
#include <QLineEdit>
#include <QComboBox>
#include <QPushButton>
#include <QVBoxLayout>
#include <QHBoxLayout>
#include <QMessageBox>
#include <QHeaderView>
#include <QLabel>
#include <QTabWidget>
#include <QFileDialog>
#include <QFile>
#include <QTextStream>

MainWindow::MainWindow(const QString &role, int employeeId, QWidget *parent)
    : QMainWindow(parent), m_role(role), m_employeeId(employeeId) {

    setWindowTitle("Учёт сотрудников — ООО «Капитал» [" + role + "]");
    resize(1100, 650);

    // --- Модель данных с JOIN ---
    model = new QSqlQueryModel(this);
    
    view = new QTableView;
    view->setModel(model);
    view->hideColumn(0); // Скрываем ID
    view->setSelectionBehavior(QAbstractItemView::SelectRows);
    view->setSelectionMode(QAbstractItemView::SingleSelection);
    view->setAlternatingRowColors(true);
    view->horizontalHeader()->setStretchLastSection(true);
    view->setEditTriggers(QAbstractItemView::NoEditTriggers); // Запрещаем прямое редактирование

    // Вкладки
    QTabWidget *tabs = new QTabWidget;
    tabs->addTab(view, "Сотрудники");

    historyTab = new HistoryWidget(this);
    if (m_role != "Сотрудник") {
        tabs->addTab(historyTab, "История операций");
    }

    // Поиск и фильтр
    searchEdit = new QLineEdit;
    searchEdit->setPlaceholderText("Поиск по ФИО...");
    statusFilter = new QComboBox;
    statusFilter->addItems({"Все", "Работает", "В отпуске", "Уволен"});

    // Кнопки
    btnAdd     = new QPushButton("Принять на работу");
    btnDismiss = new QPushButton("Уволить");
    btnSave    = new QPushButton("Обновить");
    btnEdit    = new QPushButton("Редактировать");
    btnExport  = new QPushButton("Экспорт в Excel (CSV)");
    auto *btnHelp = new QPushButton("Справка");

    auto *filterRow = new QHBoxLayout;
    filterRow->addWidget(new QLabel("Поиск:"));
    filterRow->addWidget(searchEdit);
    filterRow->addWidget(new QLabel("Статус:"));
    filterRow->addWidget(statusFilter);
    filterRow->addStretch();

    auto *btnRow = new QHBoxLayout;
    btnRow->addWidget(btnAdd);
    btnRow->addWidget(btnEdit);
    btnRow->addWidget(btnDismiss);
    btnRow->addWidget(btnSave);
    btnRow->addStretch();
    btnRow->addWidget(btnExport);
    btnRow->addWidget(btnHelp);

    auto *central = new QWidget;
    auto *lay = new QVBoxLayout(central);
    lay->addLayout(filterRow);
    lay->addWidget(tabs);
    lay->addLayout(btnRow);
    setCentralWidget(central);

    // Сигналы
    connect(searchEdit, &QLineEdit::textChanged, this, &MainWindow::applyFilter);
    connect(statusFilter, &QComboBox::currentTextChanged, this, &MainWindow::applyFilter);
    connect(btnSave, &QPushButton::clicked, this, &MainWindow::applyFilter);
    connect(btnAdd, &QPushButton::clicked, this, &MainWindow::addEmployee);
    connect(btnDismiss, &QPushButton::clicked, this, &MainWindow::dismissEmployee);
    connect(btnEdit, &QPushButton::clicked, this, &MainWindow::editEmployee);
    connect(btnExport, &QPushButton::clicked, this, &MainWindow::exportToCSV);
    connect(btnHelp, &QPushButton::clicked, this, &MainWindow::showHelp);

    // Права
    if (m_role != "Администратор") {
        btnAdd->setVisible(false);
        btnDismiss->setVisible(false);
        btnExport->setVisible(false);
    }
    if (m_role == "Сотрудник") {
        btnSave->setVisible(false);
        btnEdit->setVisible(false);
        searchEdit->setEnabled(false);
        statusFilter->setEnabled(false);
    }

    // Первичная загрузка
    applyFilter();
}

void MainWindow::applyFilter() {
    QString baseQuery = R"(
        SELECT 
            e.EmployeeId AS ID, 
            e.FullName AS ФИО, 
            e.Phone AS Телефон, 
            e.Email AS Email, 
            d.Name AS Подразделение, 
            p.Name AS Должность, 
            e.HireDate AS [Дата приема], 
            e.Role AS Роль, 
            e.Status AS Статус
        FROM Employees e
        LEFT JOIN Departments d ON e.DepartmentId = d.DepartmentId
        LEFT JOIN Positions p ON e.PositionId = p.PositionId
    )";

    QStringList whereConds;
    if (m_role == "Сотрудник") {
        whereConds << QString("e.EmployeeId = %1").arg(m_employeeId);
    }
    if (!searchEdit->text().isEmpty()) {
        whereConds << QString("e.FullName LIKE '%%1%'").arg(searchEdit->text());
    }
    if (statusFilter->currentText() != "Все") {
        whereConds << QString("e.Status = '%1'").arg(statusFilter->currentText());
    }

    if (!whereConds.isEmpty()) {
        baseQuery += " WHERE " + whereConds.join(" AND ");
    }
    
    model->setQuery(baseQuery);
}

void MainWindow::addEmployee() {
    EmployeeEditDialog dlg(this);
    if (dlg.exec() == QDialog::Accepted) {
        QSqlRecord rec = dlg.record();
        
        QSqlQuery q;
        q.prepare(R"(
            INSERT INTO Employees (FullName, BirthDate, Phone, Email, DepartmentId, PositionId, HireDate, ContractEndDate, ProbationMonths, Login, PasswordHash, Role, Status) 
            VALUES (:fio, :bdate, :phone, :email, :dept, :pos, :hdate, :cdate, :prob, :login, :hash, :role, :status)
        )");
        q.bindValue(":fio", rec.value("FullName"));
        q.bindValue(":bdate", rec.value("BirthDate"));
        q.bindValue(":phone", rec.value("Phone"));
        q.bindValue(":email", rec.value("Email"));
        q.bindValue(":dept", rec.value("DepartmentId"));
        q.bindValue(":pos", rec.value("PositionId"));
        q.bindValue(":hdate", rec.value("HireDate"));
        q.bindValue(":cdate", rec.value("ContractEndDate"));
        q.bindValue(":prob", rec.value("ProbationMonths"));
        q.bindValue(":login", rec.value("Login"));
        q.bindValue(":hash", rec.value("PasswordHash"));
        q.bindValue(":role", rec.value("Role"));
        q.bindValue(":status", rec.value("Status"));
        
        if (q.exec()) {
            int newId = q.lastInsertId().toInt();
            QSqlQuery histQ;
            histQ.prepare("INSERT INTO EmployeeHistory (EmployeeId, OperationType, OperationDate, Details) VALUES (:id, 'Прием на работу', :date, 'Оформление в штат')");
            histQ.bindValue(":id", newId);
            histQ.bindValue(":date", QDateTime::currentDateTime().toString("yyyy-MM-dd HH:mm:ss"));
            histQ.exec();
            applyFilter();
            historyTab->refresh();
        } else {
            QMessageBox::warning(this, "Ошибка", q.lastError().text());
        }
    }
}

void MainWindow::editEmployee() {
    auto idx = view->currentIndex();
    if (!idx.isValid()) {
        QMessageBox::warning(this, "Внимание", "Выберите сотрудника для редактирования.");
        return;
    }
    int empId = model->data(model->index(idx.row(), 0)).toInt(); 

    QSqlQuery q;
    q.prepare("SELECT * FROM Employees WHERE EmployeeId = :id");
    q.bindValue(":id", empId);
    q.exec();
    q.next();
    
    QSqlRecord rec;
    for (int i = 0; i < q.record().count(); ++i) {
        rec.append(q.record().field(i));
        rec.setValue(i, q.value(i));
    }

    EmployeeEditDialog dlg(this);
    dlg.setRecord(rec);

    if (dlg.exec() == QDialog::Accepted) {
        QSqlRecord newRec = dlg.record();
        
        QSqlQuery updQ;
        updQ.prepare(R"(
            UPDATE Employees SET 
            FullName = :fio, BirthDate = :bdate, Phone = :phone, Email = :email, 
            DepartmentId = :dept, PositionId = :pos, HireDate = :hdate, ContractEndDate = :cdate, 
            ProbationMonths = :prob, Login = :login, PasswordHash = :hash, Role = :role, Status = :status
            WHERE EmployeeId = :id
        )");
        updQ.bindValue(":fio", newRec.value("FullName"));
        updQ.bindValue(":bdate", newRec.value("BirthDate"));
        updQ.bindValue(":phone", newRec.value("Phone"));
        updQ.bindValue(":email", newRec.value("Email"));
        updQ.bindValue(":dept", newRec.value("DepartmentId"));
        updQ.bindValue(":pos", newRec.value("PositionId"));
        updQ.bindValue(":hdate", newRec.value("HireDate"));
        updQ.bindValue(":cdate", newRec.value("ContractEndDate"));
        updQ.bindValue(":prob", newRec.value("ProbationMonths"));
        updQ.bindValue(":login", newRec.value("Login"));
        updQ.bindValue(":hash", newRec.value("PasswordHash"));
        updQ.bindValue(":role", newRec.value("Role"));
        updQ.bindValue(":status", newRec.value("Status"));
        updQ.bindValue(":id", empId);

        if (updQ.exec()) {
            QSqlQuery histQ;
            histQ.prepare("INSERT INTO EmployeeHistory (EmployeeId, OperationType, OperationDate, Details) VALUES (:id, 'Изменение данных', :date, 'Редактирование карточки')");
            histQ.bindValue(":id", empId);
            histQ.bindValue(":date", QDateTime::currentDateTime().toString("yyyy-MM-dd HH:mm:ss"));
            histQ.exec();
            applyFilter();
            historyTab->refresh();
        }
    }
}

void MainWindow::dismissEmployee() {
    auto idx = view->currentIndex();
    if (!idx.isValid()) {
        QMessageBox::warning(this, "Внимание", "Выберите сотрудника для увольнения.");
        return;
    }
    int empId = model->data(model->index(idx.row(), 0)).toInt();

    if (QMessageBox::question(this, "Увольнение", "Уволить выбранного сотрудника?") == QMessageBox::Yes) {
        QSqlQuery q;
        q.prepare("UPDATE Employees SET Status = 'Уволен' WHERE EmployeeId = :id");
        q.bindValue(":id", empId);
        
        if (q.exec()) {
            QSqlQuery histQ;
            histQ.prepare("INSERT INTO EmployeeHistory (EmployeeId, OperationType, OperationDate, Details) VALUES (:id, 'Увольнение', :date, 'Расторжение трудового договора')");
            histQ.bindValue(":id", empId);
            histQ.bindValue(":date", QDateTime::currentDateTime().toString("yyyy-MM-dd HH:mm:ss"));
            histQ.exec();
            applyFilter();
            historyTab->refresh();
        }
    }
}

void MainWindow::exportToCSV() {
    QString fileName = QFileDialog::getSaveFileName(this, "Сохранить отчет", "Отчет_Сотрудники.csv", "CSV файлы (*.csv)");
    if (fileName.isEmpty()) return;

    QFile file(fileName);
    if (!file.open(QIODevice::WriteOnly | QIODevice::Text)) {
        QMessageBox::warning(this, "Ошибка", "Не удалось создать файл.");
        return;
    }

    QTextStream out(&file);
    out.setEncoding(QStringConverter::Utf8);
    out << "\xEF\xBB\xBF"; // BOM для корректного открытия в Excel

    // Заголовки
    for (int col = 1; col < model->columnCount(); ++col) {
        out << model->headerData(col, Qt::Horizontal).toString() << ";";
    }
    out << "\n";

    // Данные
    for (int row = 0; row < model->rowCount(); ++row) {
        for (int col = 1; col < model->columnCount(); ++col) {
            QString text = model->data(model->index(row, col)).toString();
            text.replace(";", ",");
            out << text << ";";
        }
        out << "\n";
    }
    file.close();
    QMessageBox::information(this, "Успех", "Отчет сохранен в файл!");
}

void MainWindow::showHelp() {
    QMessageBox::information(this, "Справка", 
        "Информационная система учёта сотрудников.\n\n"
        "Роли:\n"
        "- Администратор: полный доступ.\n"
        "- Менеджер: прием, редактирование, увольнение.\n"
        "- Сотрудник: просмотр только своей карточки.\n\n"
        "Вкладка 'История операций' показывает все действия с кадрами.\n"
        "Кнопка 'Экспорт' сохраняет текущий список в файл Excel (CSV).");
}
