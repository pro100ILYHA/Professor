#pragma once
#include <QWidget>

class QTableView;
class QSqlTableModel;

class HistoryWidget : public QWidget {
    Q_OBJECT
public:
    explicit HistoryWidget(QWidget *parent = nullptr);
    void refresh();

private:
    QSqlTableModel *model;
    QTableView *view;
};
