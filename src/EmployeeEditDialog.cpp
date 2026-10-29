#include "EmployeeEditDialog.h"
#include <QFormLayout>
#include <QVBoxLayout>
#include <QHBoxLayout>
#include <QLineEdit>
#include <QComboBox>
#include <QDateEdit>
#include <QSpinBox>
#include <QPushButton>
#include <QSqlQuery>
#include <QMessageBox>
#include <QCryptographicHash>

EmployeeEditDialog::EmployeeEditDialog(QWidget *parent) : QDialog(parent) {
    setWindowTitle("Карточка сотрудника");
    resize(400, 600);

    fullNameEdit = new QLineEdit;
    birthDateEdit = new QDateEdit(QDate(1990, 1, 1));
    birthDateEdit->setCalendarPopup(true);
    birthDateEdit->setDisplayFormat("yyyy-MM-dd");
    phoneEdit = new QLineEdit;
    emailEdit = new QLineEdit;
    departmentCombo = new QComboBox;
    positionCombo = new QComboBox;
    
    hireDateEdit = new QDateEdit(QDate::currentDate());
    hireDateEdit->setCalendarPopup(true);
    hireDateEdit->setDisplayFormat("yyyy-MM-dd");
    
    contractEndDateEdit = new QDateEdit(QDate::currentDate().addYears(1));
    contractEndDateEdit->setCalendarPopup(true);
    contractEndDateEdit->setDisplayFormat("yyyy-MM-dd");
    
    probationSpin = new QSpinBox;
    probationSpin->setRange(0, 12);
    probationSpin->setSuffix(" мес.");
    
    loginEdit = new QLineEdit;
    passwordEdit = new QLineEdit;
    passwordEdit->setEchoMode(QLineEdit::Password);
    passwordEdit->setPlaceholderText("Оставьте пустым, чтобы не менять");
    
    roleCombo = new QComboBox;
    roleCombo->addItems({"Администратор", "Менеджер", "Сотрудник"});
    statusCombo = new QComboBox;
    statusCombo->addItems({"Работает", "В отпуске", "Уволен"});

    auto *form = new QFormLayout;
    form->addRow("ФИО:", fullNameEdit);
    form->addRow("Дата рождения:", birthDateEdit);
    form->addRow("Телефон:", phoneEdit);
    form->addRow("E-mail:", emailEdit);
    form->addRow("Подразделение:", departmentCombo);
    form->addRow("Должность:", positionCombo);
    form->addRow("Дата приёма:", hireDateEdit);
    form->addRow("Конец контракта:", contractEndDateEdit);
    form->addRow("Испытательный срок:", probationSpin);
    form->addRow("Логин:", loginEdit);
    form->addRow("Пароль:", passwordEdit);
    form->addRow("Роль:", roleCombo);
    form->addRow("Статус:", statusCombo);

    auto *btnSave = new QPushButton("Сохранить");
    auto *btnCancel = new QPushButton("Отмена");
    auto *btnLayout = new QHBoxLayout;
    btnLayout->addStretch();
    btnLayout->addWidget(btnSave);
    btnLayout->addWidget(btnCancel);

    auto *mainLayout = new QVBoxLayout(this);
    mainLayout->addLayout(form);
    mainLayout->addLayout(btnLayout);

    connect(btnSave, &QPushButton::clicked, this, &EmployeeEditDialog::onSaveClicked);
    connect(btnCancel, &QPushButton::clicked, this, &QDialog::reject);

    loadDictionaries();
}

void EmployeeEditDialog::loadDictionaries() {
    QSqlQuery q;
    q.exec("SELECT DepartmentId, Name FROM Departments");
    while (q.next()) departmentCombo->addItem(q.value(1).toString(), q.value(0).toInt());
    q.exec("SELECT PositionId, Name FROM Positions");
    while (q.next()) positionCombo->addItem(q.value(1).toString(), q.value(0).toInt());
}

void EmployeeEditDialog::setRecord(const QSqlRecord &record) {
    m_record = record;
    fullNameEdit->setText(record.value("FullName").toString());
    birthDateEdit->setDate(QDate::fromString(record.value("BirthDate").toString(), "yyyy-MM-dd"));
    phoneEdit->setText(record.value("Phone").toString());
    emailEdit->setText(record.value("Email").toString());
    
    int deptIndex = departmentCombo->findData(record.value("DepartmentId").toInt());
    if (deptIndex >= 0) departmentCombo->setCurrentIndex(deptIndex);
    int posIndex = positionCombo->findData(record.value("PositionId").toInt());
    if (posIndex >= 0) positionCombo->setCurrentIndex(posIndex);
    
    hireDateEdit->setDate(QDate::fromString(record.value("HireDate").toString(), "yyyy-MM-dd"));
    if (!record.value("ContractEndDate").isNull()) {
        contractEndDateEdit->setDate(QDate::fromString(record.value("ContractEndDate").toString(), "yyyy-MM-dd"));
    }
    probationSpin->setValue(record.value("ProbationMonths").toInt());
    loginEdit->setText(record.value("Login").toString());
    roleCombo->setCurrentText(record.value("Role").toString());
    statusCombo->setCurrentText(record.value("Status").toString());
}

QSqlRecord EmployeeEditDialog::record() const {
    QSqlRecord rec = m_record;
    rec.setValue("FullName", fullNameEdit->text());
    rec.setValue("BirthDate", birthDateEdit->date().toString("yyyy-MM-dd"));
    rec.setValue("Phone", phoneEdit->text());
    rec.setValue("Email", emailEdit->text());
    rec.setValue("DepartmentId", departmentCombo->currentData().toInt());
    rec.setValue("PositionId", positionCombo->currentData().toInt());
    rec.setValue("HireDate", hireDateEdit->date().toString("yyyy-MM-dd"));
    rec.setValue("ContractEndDate", contractEndDateEdit->date().toString("yyyy-MM-dd"));
    rec.setValue("ProbationMonths", probationSpin->value());
    rec.setValue("Login", loginEdit->text());
    rec.setValue("Role", roleCombo->currentText());
    rec.setValue("Status", statusCombo->currentText());
    
    if (!passwordEdit->text().isEmpty()) {
        QString hash = QString(QCryptographicHash::hash(passwordEdit->text().toUtf8(), QCryptographicHash::Sha256).toHex().toUpper());
        rec.setValue("PasswordHash", hash);
    }
    return rec;
}

void EmployeeEditDialog::onSaveClicked() {
    if (fullNameEdit->text().isEmpty() || loginEdit->text().isEmpty()) {
        QMessageBox::warning(this, "Ошибка", "ФИО и Логин не могут быть пустыми.");
        return;
    }
    accept();
}
