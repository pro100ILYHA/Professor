using System;
using System.Drawing;
using System.Windows.Forms;

namespace CollectWordGame
{
    public class RecordsForm : Form
    {
        private RecordManager records;

        public RecordsForm()
        {
            records = new RecordManager();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Рекорды";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            DataGridView dgv = new DataGridView
            {
                Location = new Point(10, 10),
                Size = new Size(360, 200),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { Font = new Font("Arial", 10, FontStyle.Bold) }
            };
            dgv.Columns.Add("Place", "Место");
            dgv.Columns.Add("Name", "Игрок");
            dgv.Columns.Add("Score", "Очки");
            dgv.Columns.Add("Date", "Дата");

            var top = records.GetTopRecords();
            for (int i = 0; i < top.Count; i++)
            {
                dgv.Rows.Add(i + 1, top[i].Name, top[i].Score, top[i].Date.ToString("g"));
            }

            Button btnClose = new Button { Text = "Закрыть", Location = new Point(150, 220), Size = new Size(100, 30) };
            btnClose.Click += (s, e) => this.Close();

            Controls.Add(dgv);
            Controls.Add(btnClose);
        }
    }
}