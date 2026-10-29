#include "HistoryWidget.h"
#include <QVBoxLayout>
#include <QTableView>
#include <QSqlTableModel>
#include <QHeaderView>

HistoryWidget::HistoryWidget(QWidget *parent) : QWidget(parent) {
    auto *lay = new QVBoxLayout(this);
    
    model = new QSqlTableModel(this);
    model->setTable("EmployeeHistory");
    model->setEditStrategy(QSqlTableModel::OnManualSubmit);
    model->select();
    
    model->setHeaderData(1, Qt::Horizontal, "ID Сотрудника");
    model->setHeaderData(2, Qt::Horizontal, "Тип операции");
    model->setHeaderData(3, Qt::Horizontal, "Дата");
    model->setHeaderData(4, Qt::Horizontal, "Детали");

    view = new QTableView;
    view->setModel(model);
    view->hideColumn(0);
    view->horizontalHeader()->setStretchLastSection(true);
    view->setEditTriggers(QAbstractItemView::NoEditTriggers);
    view->setSelectionBehavior(QAbstractItemView::SelectRows);

    lay->addWidget(view);
}

void HistoryWidget::refresh() {
    model->select();
}
