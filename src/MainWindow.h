#pragma once
#include <QMainWindow>
#include <QSqlQueryModel> // <--- Изменено с QSqlTableModel

class QTableView;
class QLineEdit;
class QComboBox;
class QPushButton;
class HistoryWidget;

class MainWindow : public QMainWindow {
    Q_OBJECT
public:
    explicit MainWindow(const QString &role, int employeeId, QWidget *parent = nullptr);

private slots:
    void addEmployee();
    void dismissEmployee();
    void applyFilter();
    void editEmployee();
    void showHelp();
    void exportToCSV(); // <--- Добавлено

private:
    QSqlQueryModel *model; // <--- Изменен тип
    QTableView *view;
    QLineEdit *searchEdit;
    QComboBox *statusFilter;
    QPushButton *btnAdd;
    QPushButton *btnDismiss;
    QPushButton *btnSave;
    QPushButton *btnEdit;
    QPushButton *btnExport; // <--- Добавлено
    HistoryWidget *historyTab;

    QString m_role;
    int m_employeeId;
};
