using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Popitka1
{
    public partial class AddForm: Form
    {
        DataBase dataBase = new DataBase();
        public AddForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            dataBase.openConnection();

            var Texn = tbTexnika.Text;
            var Zav = tbZav.Text;
            var Polm = tbPolomka.Text;
                                 
                var addQuery = $"INSERT into Zapros (Texnika, Polomka, Prositel) values ('{Texn}', '{Polm}', '{Zav}')";


                var command = new SqlCommand(addQuery, dataBase.getConnection());
                command.ExecuteNonQuery();

                MessageBox.Show("Запрос был успешно создан!!", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                   
            dataBase.closeConnection();

            this.Close();
        }

        private void pbFillEMP_Click(object sender, EventArgs e)
        {
            tbTexnika.Text = "";
            tbZav.Text = "";
            tbPolomka.Text = "";
        }
    }
}
