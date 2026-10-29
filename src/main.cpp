#include <QApplication>
#include <QSqlDatabase>
#include <QSqlQuery>
#include <QSqlError>
#include <QMessageBox>
#include <QCryptographicHash>
#include <QDate>
#include <QDebug>

#include "MainWindow.h"
#include "LoginDialog.h"

static bool initDatabase() {
    QSqlDatabase db = QSqlDatabase::addDatabase("QSQLITE");
    db.setDatabaseName("kapital.db");

    if (!db.open()) {
        QMessageBox::critical(nullptr, "Ошибка БД", db.lastError().text());
        return false;
    }

    QSqlQuery q;

    // 1. Таблица подразделений
    q.exec(R"(
        CREATE TABLE IF NOT EXISTS Departments (
            DepartmentId INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL UNIQUE
        );
    )");

    // 2. Таблица должностей
    q.exec(R"(
        CREATE TABLE IF NOT EXISTS Positions (
            PositionId INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL UNIQUE,
            BaseSalary REAL NOT NULL DEFAULT 0 CHECK (BaseSalary >= 0)
        );
    )");

    // 3. Таблица сотрудников
    q.exec(R"(
        CREATE TABLE IF NOT EXISTS Employees (
            EmployeeId INTEGER PRIMARY KEY AUTOINCREMENT,
            FullName TEXT NOT NULL,
            BirthDate TEXT NOT NULL,
            Phone TEXT NOT NULL,
            Email TEXT,
            DepartmentId INTEGER NOT NULL REFERENCES Departments(DepartmentId),
            PositionId INTEGER NOT NULL REFERENCES Positions(PositionId),
            HireDate TEXT NOT NULL,
            ContractEndDate TEXT,
            ProbationMonths INTEGER NOT NULL DEFAULT 3 CHECK (ProbationMonths >= 0),
            Login TEXT NOT NULL UNIQUE,
            PasswordHash TEXT NOT NULL,
            Role TEXT NOT NULL CHECK (Role IN ('Администратор','Менеджер','Сотрудник')),
            Status TEXT NOT NULL DEFAULT 'Работает' CHECK (Status IN ('Работает','В отпуске','Уволен'))
        );
    )");

    // 4. Таблица истории кадровых операций
    q.exec(R"(
        CREATE TABLE IF NOT EXISTS EmployeeHistory (
            HistoryId INTEGER PRIMARY KEY AUTOINCREMENT,
            EmployeeId INTEGER NOT NULL REFERENCES Employees(EmployeeId),
            OperationType TEXT NOT NULL,
            OperationDate TEXT NOT NULL,
            Details TEXT
        );
    )");

    // Индексы
    q.exec("CREATE INDEX IF NOT EXISTS IX_Emp_Dept ON Employees(DepartmentId);");
    q.exec("CREATE INDEX IF NOT EXISTS IX_Emp_Status ON Employees(Status);");

    // Заполнение справочников
    q.exec("SELECT COUNT(*) FROM Departments");
    if (q.next() && q.value(0).toInt() == 0) {
        q.exec("INSERT INTO Departments (Name) VALUES ('Полевые работы'), ('Камеральные работы'), ('Администрация');");
        q.exec("INSERT INTO Positions (Name, BaseSalary) VALUES ('Кадастровый инженер', 90000), ('Менеджер', 70000), ('Системный администратор', 85000);");

        q.exec(R"(
            INSERT INTO Employees (FullName, BirthDate, Phone, Email, DepartmentId, PositionId, HireDate, ContractEndDate, ProbationMonths, Login, PasswordHash, Role, Status)
            VALUES 
            ('Иванов Иван Иванович', '1990-01-01', '+79000000000', 'admin@example.com', 3, 3, '2020-03-15', NULL, 3, 'admin', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', 'Администратор', 'Работает'),
            ('Петрова Анна Сергеевна', '1995-05-15', '+79000000001', 'petrova@example.com', 1, 2, '2023-06-01', NULL, 3, 'keeper', '3FEF5C1BB564C29EB2F645042E37DBB360D322F66361148424F8B13258530C46', 'Менеджер', 'Работает'),
            ('Сидоров Пётр Андреевич', '1998-03-20', '+79000000002', NULL, 1, 1, '2025-09-01', '2026-12-31', 3, 'engineer1', 'F63248EFA4A61EFC9F4C9F6E5DE25B34B6F2B827717CD4C6B3905481C3BD483B', 'Сотрудник', 'Работает');
        )");
    }

    return true;
}

int main(int argc, char *argv[]) {
    QApplication app(argc, argv);

    if (!initDatabase()) {
        return 1;
    }

    LoginDialog login;
    if (login.exec() != QDialog::Accepted) {
        return 0;
    }

    MainWindow w(login.role(), login.employeeId());
    w.show();

    return app.exec();
}
