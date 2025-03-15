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

    enum RowState
    {
        Existed,
        New,
        Modified,
        ModifiedNew,
        Deleted
    }
    public partial class MainForm: Form
    {
        DataBase dataBase = new DataBase();

        int selectedRow;

        public MainForm()
        {
            
            InitializeComponent();
        }

       private void CreateColumns()
        {
            dataGridView1.Columns.Add("ID", "ID");
            dataGridView1.Columns.Add("Texnika", "Наименование техники");
            dataGridView1.Columns.Add("Polomka", "Описание поломки");
            dataGridView1.Columns.Add("Prositel", "Заявитель");
            //dataGridView1.Columns.Add("Price", "Цена");
            dataGridView1.Columns.Add("IsNew", String.Empty);
        }

        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), /*record.GetInt32(4),*/ RowState.ModifiedNew);

        }

        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string queryString = $"select * from Zapros";

            SqlCommand command = new SqlCommand(queryString, dataBase.getConnection());

            dataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while(reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CreateColumns();
            CreateColumns2();
            RefreshDataGrid(dataGridView1);
            RefreshDataGrid2(dataGridView2);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if(e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];

                tbID.Text = row.Cells[0].Value.ToString();
                tbTexnika.Text = row.Cells[1].Value.ToString();
                tbPolomka.Text = row.Cells[2].Value.ToString();
                tbZav.Text = row.Cells[3].Value.ToString();
                //tbPrice.Text = row.Cells[4].Value.ToString();


            }
        }

        private void pbRefresh_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddForm addform = new AddForm();
            addform.Show();
        }

        private void Search(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string searchstring = $"select * from Zapros where concat (ID, Texnika, Polomka, Prositel) like '%" + tbSearch.Text + "%'";

            SqlCommand com = new SqlCommand(searchstring, dataBase.getConnection());

            dataBase.openConnection();

            SqlDataReader read = com.ExecuteReader();

            while(read.Read())
            {
                ReadSingleRow(dgw, read);

            }
            read.Close();
        }
        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            if(tbSearch.Text.Length >=1)
            {
                pbDelete.Visible = true;
            }
            else
            {
                pbDelete.Visible = false;
            }
                Search(dataGridView1);
        }

        private void pbDelete_Click(object sender, EventArgs e)
        {
            tbSearch.Text = "";
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();

            var Texn = tbTexnika.Text;
            var Zav = tbZav.Text;
            var Polm = tbPolomka.Text;
            int Price;

            if (int.TryParse(tbPrice.Text, out Price))
            {
                var addQuery = $"INSERT into Complited (Texnika, Polomka, Prositel, Price) values ('{Texn}', '{Polm}', '{Zav}', '{Price}')";


                var command = new SqlCommand(addQuery, dataBase.getConnection());
                command.ExecuteNonQuery();

                MessageBox.Show("Запрос был отмечен как выполненный!!", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                deleteRow();
                UpdateTable();
            }
            else
            {
                MessageBox.Show("Неверный формат цены. Цена должна быть целым числом.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            dataBase.closeConnection();

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[selectedRow];

                tbID2.Text = row.Cells[0].Value.ToString();
                tbTexnika2.Text = row.Cells[1].Value.ToString();
                tbPolomka2.Text = row.Cells[2].Value.ToString();
                tbZav2.Text = row.Cells[3].Value.ToString();
                tbPrice2.Text = row.Cells[4].Value.ToString();


            }
        }
             private void CreateColumns2()
        {
            dataGridView2.Columns.Add("ID", "ID");
            dataGridView2.Columns.Add("Texnika", "Наименование техники");
            dataGridView2.Columns.Add("Polomka", "Описание поломки");
            dataGridView2.Columns.Add("Prositel", "Заявитель");
            dataGridView2.Columns.Add("Price", "Цена");

            dataGridView2.Columns.Add("IsNew", String.Empty);
        }

        private void ReadSingleRow2(DataGridView dgw2, IDataRecord record)
        {
            dgw2.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), record.GetInt32(4), RowState.ModifiedNew);

        }

        private void RefreshDataGrid2(DataGridView dgw2)
        {
            dgw2.Rows.Clear();

            string queryString = $"select * from Complited";

            SqlCommand command = new SqlCommand(queryString, dataBase.getConnection());

            dataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow2(dgw2, reader);
            }
            reader.Close();
        }

        private void pbRefresh2_Click(object sender, EventArgs e)
        {
            RefreshDataGrid2(dataGridView2);
        }
        private void Search2(DataGridView dgw2)
        {
            dgw2.Rows.Clear();

            string searchstring = $"select * from Complited where concat (ID, Texnika, Polomka, Prositel, Price) like '%" + tbSearch.Text + "%'";

            SqlCommand com = new SqlCommand(searchstring, dataBase.getConnection());

            dataBase.openConnection();

            SqlDataReader read = com.ExecuteReader();

            while (read.Read())
            {
                ReadSingleRow(dgw2, read);

            }
            read.Close();
        }

        private void tbSearch2_TextChanged(object sender, EventArgs e)
        {
            if (tbSearch.Text.Length >= 1)
            {
                pbDelete2.Visible = true;
            }
            else
            {
                pbDelete2.Visible = false;
            }
            Search2(dataGridView2);
        }

        private void pbDelete2_Click(object sender, EventArgs e)
        {
            tbSearch2.Text = "";
        }

        private void deleteRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            dataGridView1.Rows[index].Visible = false;

            if (dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView1.Rows[index].Cells[4].Value = RowState.Deleted;

                return;
            }
            dataGridView1.Rows[index].Cells[4].Value = RowState.Deleted;
        }

        private void UpdateTable()
        {
            dataBase.openConnection();

            for(int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView1.Rows[index].Cells[4].Value;

                if (rowState == RowState.Existed)
                  continue;
                

                    if (rowState == RowState.Deleted)
                    {
                        var ID = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                        var deleteQuery = $"delete from Zapros where ID = {ID}";

                        var command = new SqlCommand(deleteQuery, dataBase.getConnection());
                        command.ExecuteNonQuery();
                    }
                
            }

            dataBase.closeConnection();
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            deleteRow();
            UpdateTable();
        }

       
    }
    
}
