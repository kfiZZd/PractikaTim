using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Popitka1
{
    public partial class Form1 : Form
    {
        DataBase database = new DataBase();

        public Form1()
        {
            
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var login = tbLog.Text;
            var password = tbPass.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string querystring = $"select ID, Login, Password from Account where Login = '{login}' and Password = '{password}'";

            SqlCommand command = new SqlCommand(querystring, database.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if(table.Rows.Count == 1)
            {
                MessageBox.Show("Вы успешно вошли!", "Вход", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MainForm mainForm = new MainForm();
                this.Hide();
                mainForm.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!!");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegForm frm_reg = new RegForm();
            frm_reg.Show();
            this.Hide();
        }

        private void pbShowPas_Click(object sender, EventArgs e)
        {
            tbPass.UseSystemPasswordChar = false;
            pbShowPas.Visible = false;
            pbClosePas.Visible = true;
        }

        private void pbClosePas_Click(object sender, EventArgs e)
        {
            tbPass.UseSystemPasswordChar = true;
            pbShowPas.Visible = true;
            pbClosePas.Visible = false;
        }
    }
}
