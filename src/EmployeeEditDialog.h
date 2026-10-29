#pragma once
#include <QDialog>
#include <QSqlRecord>

class QLineEdit;
class QComboBox;
class QDateEdit;
class QSpinBox;

class EmployeeEditDialog : public QDialog {
    Q_OBJECT
public:
    explicit EmployeeEditDialog(QWidget *parent = nullptr);
    void setRecord(const QSqlRecord &record);
    QSqlRecord record() const;

private slots:
    void onSaveClicked();

private:
    QLineEdit *fullNameEdit;
    QDateEdit *birthDateEdit;
    QLineEdit *phoneEdit;
    QLineEdit *emailEdit;
    QComboBox *departmentCombo;
    QComboBox *positionCombo;
    QDateEdit *hireDateEdit;
    QDateEdit *contractEndDateEdit;
    QSpinBox *probationSpin;
    QLineEdit *loginEdit;
    QLineEdit *passwordEdit;
    QComboBox *roleCombo;
    QComboBox *statusCombo;

    QSqlRecord m_record;
    void loadDictionaries();
};
