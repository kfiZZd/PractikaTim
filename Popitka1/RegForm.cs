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
    public partial class RegForm: Form
    {
        DataBase database = new DataBase();
        public RegForm()
        {
            InitializeComponent();
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            if(checkUser())
            {
                return;
            }

            var login = tbLog.Text;
            var password = tbPass.Text;

            string quertystring = $"insert into Practika1(Login, Password) values('{login}', '{password}')";

            SqlCommand command = new SqlCommand(quertystring, database.getConnection());

            database.openConnection();

            if(command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Аккаунт был успешно создан!", "Регистрация");
                Form1 frm_form1 = new Form1();
                this.Close();
                frm_form1.Show();
            }
            else
            {
                MessageBox.Show("Nety");
            }
            database.closeConnection();

        }
        private Boolean checkUser()
        {
            

            var login = tbLog.Text;
            var password = tbPass.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string quertystring = $"select ID, Login, Password from Practika1 where Login = '{login}' and Password = '{password}'";

            SqlCommand command = new SqlCommand(quertystring, database.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);
            if(table.Rows.Count > 0)
            {
                MessageBox.Show("User exist!");
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
