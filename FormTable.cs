using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ShopLib;

namespace ShopApp
{
    public partial class FormTable : Form
    {
        private readonly ProductRepository _repository;

        public FormTable(ProductRepository repository)
        {
            InitializeComponent();
            _repository = repository;

            // Подписка на событие обновления данных
            _repository.DataChanged += OnDataChanged;
            this.FormClosed += (s, e) => _repository.DataChanged -= OnDataChanged;

            // Показать все данные при открытии
            ShowAll();
        }

        /// <summary>
        /// Отображает все товары.
        /// </summary>
        public void ShowAll()
        {
            var products = _repository.GetAll();
            UpdateGrid(products);
        }

        /// <summary>
        /// Показывает отфильтрованные данные.
        /// </summary>
        public void ShowFiltered(string field, string value)
        {
            var filtered = _repository.FindByField(field, value);
            UpdateGrid(filtered);
        }

        private void OnDataChanged()
        {
            // При изменении данных обновляем таблицу (всегда показываем полный список после изменений)
            if (InvokeRequired)
            {
                Invoke(new Action(ShowAll));
            }
            else
            {
                ShowAll();
            }
        }

        private void UpdateGrid(List<Product> products)
        {
            // Очищаем привязку и задаем новый источник
            dgvProducts.DataSource = null;
            dgvProducts.DataSource = products;

            // Настройка заголовков столбцов (опционально)
            if (dgvProducts.Columns.Count > 0)
            {
                dgvProducts.Columns["Name"].HeaderText = "Название";
                dgvProducts.Columns["Price"].HeaderText = "Цена";
                dgvProducts.Columns["Quantity"].HeaderText = "Кол-во";
                dgvProducts.Columns["ExpiryDate"].HeaderText = "Годен до";

                // Автоматическая ширина столбцов
                dgvProducts.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
        }
    }
}