using ShopLib;
using System;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ShopApp
{
    public partial class FormMain : Form
    {
        private readonly ProductRepository _repository = new ProductRepository("shop.json");
        private FormTable _tableForm;

        public FormMain()
        {
            InitializeComponent();
            cmbField.SelectedIndex = 0; // по умолчанию "Name"
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название товара.");
                return;
            }

            var product = new Product
            {
                Name = txtName.Text.Trim(),
                Price = numPrice.Value,
                Quantity = (int)numQuantity.Value,
                ExpiryDate = dtpExpiry.Value
            };

            try
            {
                _repository.Add(product);
                MessageBox.Show("Товар добавлен.");
                ClearInputs();
            }
            catch (InvalidOperationException ex)
            {
                var result = MessageBox.Show(ex.Message + "\nХотите обновить запись?", "Дубликат",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    _repository.Update(product.Name, product);
                    MessageBox.Show("Запись обновлена.");
                    ClearInputs();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название товара для корректировки.");
                return;
            }

            var product = new Product
            {
                Name = txtName.Text.Trim(),
                Price = numPrice.Value,
                Quantity = (int)numQuantity.Value,
                ExpiryDate = dtpExpiry.Value
            };

            try
            {
                _repository.Update(product.Name, product);
                MessageBox.Show("Товар обновлён.");
                ClearInputs();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnShowTable_Click(object sender, EventArgs e)
        {
            if (_tableForm == null || _tableForm.IsDisposed)
            {
                _tableForm = new FormTable(_repository);
                _tableForm.Show();
            }
            else
            {
                _tableForm.Focus();
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (_tableForm == null || _tableForm.IsDisposed)
            {
                MessageBox.Show("Сначала откройте таблицу.");
                return;
            }

            string field = cmbField.SelectedItem?.ToString() ?? "Name";
            string value = txtFilterValue.Text.Trim();
            _tableForm.ShowFiltered(field, value);
        }

        private void btnResetFilter_Click(object sender, EventArgs e)
        {
            txtFilterValue.Clear();
            if (_tableForm != null && !_tableForm.IsDisposed)
                _tableForm.ShowAll();
        }

        private void ClearInputs()
        {
            txtName.Clear();
            numPrice.Value = 0;
            numQuantity.Value = 0;
            dtpExpiry.Value = DateTime.Today;
        }
    }
}