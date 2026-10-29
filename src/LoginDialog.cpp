#include "LoginDialog.h"
#include <QLineEdit>
#include <QLabel>
#include <QPushButton>
#include <QVBoxLayout>
#include <QSqlQuery>
#include <QCryptographicHash>

LoginDialog::LoginDialog(QWidget *parent) : QDialog(parent) {
    setWindowTitle("Вход в систему — ООО «Капитал»");
    setModal(true);
    resize(300, 150);

    loginEdit = new QLineEdit;
    loginEdit->setPlaceholderText("Логин");

    passwordEdit = new QLineEdit;
    passwordEdit->setPlaceholderText("Пароль");
    passwordEdit->setEchoMode(QLineEdit::Password);

    auto *btnLogin = new QPushButton("Войти");
    errorLabel = new QLabel;
    errorLabel->setStyleSheet("color: red;");
    errorLabel->setWordWrap(true);

    auto *lay = new QVBoxLayout(this);
    lay->addWidget(loginEdit);
    lay->addWidget(passwordEdit);
    lay->addWidget(btnLogin);
    lay->addWidget(errorLabel);

    connect(btnLogin, &QPushButton::clicked, this, &LoginDialog::tryLogin);
    connect(passwordEdit, &QLineEdit::returnPressed, this, &LoginDialog::tryLogin);
}

void LoginDialog::tryLogin() {
    if (loginEdit->text().isEmpty() || passwordEdit->text().isEmpty()) {
        errorLabel->setText("Заполните логин и пароль");
        return;
    }

    QString hash = QString(QCryptographicHash::hash(
        passwordEdit->text().toUtf8(), 
        QCryptographicHash::Sha256
    ).toHex().toUpper());

    QSqlQuery q;
    q.prepare("SELECT EmployeeId, Role FROM Employees WHERE Login = :login AND PasswordHash = :hash AND Status != 'Уволен'");
    q.bindValue(":login", loginEdit->text());
    q.bindValue(":hash", hash);

    if (q.exec() && q.next()) {
        m_employeeId = q.value(0).toInt();
        m_role = q.value(1).toString();
        accept();
    } else {
        errorLabel->setText("Неверный логин или пароль");
        passwordEdit->clear();
    }
}
