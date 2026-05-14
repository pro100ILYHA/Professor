using System;
using System.Windows.Forms;

namespace lr4
{
    public partial class Form1 : Form
    {
        private double cm3 = 0, dm3 = 0, m3 = 0, km3 = 0;
        private bool calculated = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            calculated = false;
            label5.Text = "—";
        }

        private void Calculate()
        {
            cm3 = dm3 = m3 = km3 = 0;
            calculated = false;

            if (!double.TryParse(textBox1.Text.Replace('.', ','), out double value))
            {
                MessageBox.Show("Введите корректное числовое значение", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Focus();
                textBox1.SelectAll();
                return;
            }

            // Приводим к кубическим метрам как базовой единице
            if (сантиметрыToolStripMenuItem.Checked)
                m3 = value * 1e-6;          // 1 см³ = 0.000001 м³
            else if (дециметрыToolStripMenuItem.Checked)
                m3 = value * 1e-3;          // 1 дм³ = 0.001 м³
            else if (метрыToolStripMenuItem.Checked)
                m3 = value;                 // 1 м³ = 1 м³
            else if (километрыToolStripMenuItem.Checked)
                m3 = value * 1e9;           // 1 км³ = 1 000 000 000 м³
            else
            {
                MessageBox.Show("Выберите исходную единицу измерения в меню «Расчет»",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Из м³ вычисляем все остальные
            cm3 = m3 * 1e6;
            dm3 = m3 * 1e3;
            km3 = m3 / 1e9;

            calculated = true;
            UpdateResultLabel();
        }

        // Выводит в label5 только ту единицу, которую выбрал пользователь кнопкой ToolStrip
        private void UpdateResultLabel()
        {
            if (!calculated)
            {
                label5.Text = "—";
                return;
            }

            if (toolStripButton1.Checked)
                label5.Text = $"{cm3:G10} см³";
            else if (toolStripButton2.Checked)
                label5.Text = $"{dm3:G10} дм³";
            else if (toolStripButton3.Checked)
                label5.Text = $"{m3:G10} м³";
            else if (toolStripButton4.Checked)
                label5.Text = $"{km3:G10} км³";
            else
                label5.Text = "Выберите единицу на панели инструментов";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Calculate();
        }

        // ─── ToolStripButton — один активен одновременно (радио-режим) ───────

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            toolStripButton2.Checked = false;
            toolStripButton3.Checked = false;
            toolStripButton4.Checked = false;
            toolStripButton1.Checked = true;
            UpdateResultLabel();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            toolStripButton1.Checked = false;
            toolStripButton3.Checked = false;
            toolStripButton4.Checked = false;
            toolStripButton2.Checked = true;
            UpdateResultLabel();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            toolStripButton1.Checked = false;
            toolStripButton2.Checked = false;
            toolStripButton4.Checked = false;
            toolStripButton3.Checked = true;
            UpdateResultLabel();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            toolStripButton1.Checked = false;
            toolStripButton2.Checked = false;
            toolStripButton3.Checked = false;
            toolStripButton4.Checked = true;
            UpdateResultLabel();
        }

        // ─── Меню «Расчет» — выбор исходной единицы ─────────────────────────

        private void сантиметрыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            дециметрыToolStripMenuItem.Checked = false;
            метрыToolStripMenuItem.Checked = false;
            километрыToolStripMenuItem.Checked = false;
            сантиметрыToolStripMenuItem.Checked = true;
            calculated = false;
            label5.Text = "Источник: см³";
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutbox1 = new AboutBox1();
            aboutbox1.Show();
        }

        private void дециметрыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            сантиметрыToolStripMenuItem.Checked = false;
            метрыToolStripMenuItem.Checked = false;
            километрыToolStripMenuItem.Checked = false;
            дециметрыToolStripMenuItem.Checked = true;
            calculated = false;
            label5.Text = "Источник: дм³";
        }

        private void метрыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            сантиметрыToolStripMenuItem.Checked = false;
            дециметрыToolStripMenuItem.Checked = false;
            километрыToolStripMenuItem.Checked = false;
            метрыToolStripMenuItem.Checked = true;
            calculated = false;
            label5.Text = "Источник: м³";
        }

        private void километрыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            сантиметрыToolStripMenuItem.Checked = false;
            дециметрыToolStripMenuItem.Checked = false;
            метрыToolStripMenuItem.Checked = false;
            километрыToolStripMenuItem.Checked = true;
            calculated = false;
            label5.Text = "Источник: км³";
        }
    }
}