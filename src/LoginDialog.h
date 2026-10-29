#pragma once
#include <QDialog>

class QLineEdit;
class QLabel;

class LoginDialog : public QDialog {
    Q_OBJECT
public:
    explicit LoginDialog(QWidget *parent = nullptr);
    QString role() const { return m_role; }
    int employeeId() const { return m_employeeId; }

private slots:
    void tryLogin();

private:
    QLineEdit *loginEdit;
    QLineEdit *passwordEdit;
    QLabel *errorLabel;
    QString m_role;
    int m_employeeId = -1;
};
