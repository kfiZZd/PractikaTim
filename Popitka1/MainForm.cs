﻿using System;
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
        private readonly CheckUser _user;

        DataBase dataBase = new DataBase();

        int selectedRow;

        public MainForm(CheckUser user)
        {

            _user = user;
            InitializeComponent();
        }

        private void IsAdmin()
        {
            btnEdit.Enabled = _user.IsAdmin;
            btnDelete.Enabled = _user.IsAdmin;
            btnComplete.Enabled = _user.IsAdmin;
            if (_user.IsAdmin == true)
            {
                tabPage2.Parent = tabControl1;
            }
            else if (_user.IsAdmin == false)
            {
                tabPage2.Parent = null;
            }
            //tabControl1.Enabled = _user.IsAdmin;

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
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), RowState.ModifiedNew);

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
            tbUserStatus.Text = $"{_user.Login}: {_user.Status}";
            IsAdmin();
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
                ClearFields();
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

            string searchstring = $"select * from Complited where concat (ID, Texnika, Polomka, Prositel, Price) like '%" + tbSearch2.Text + "%'";

            SqlCommand com = new SqlCommand(searchstring, dataBase.getConnection());

            dataBase.openConnection();

            SqlDataReader read = com.ExecuteReader();

            while (read.Read())
            {
                ReadSingleRow2(dgw2, read);

            }
            read.Close();
        }

        private void tbSearch2_TextChanged(object sender, EventArgs e)
        {
            if (tbSearch2.Text.Length >= 1)
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
                if(rowState == RowState.Modified)
                {
                    var ID = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var Texn = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var Polomka = dataGridView1.Rows[index].Cells[2].Value.ToString();
                    var Zav = dataGridView1.Rows[index].Cells[3].Value.ToString();

                    var changeQuery = $"update Zapros set Texnika = '{Texn}', Polomka = '{Polomka}', Prositel = '{Zav}' where ID = '{ID}'";

                    var command = new SqlCommand(changeQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();

                }
            }

            dataBase.closeConnection();
            
        }
        private void deleteRow2()
        {
            int index = dataGridView2.CurrentCell.RowIndex;

            dataGridView2.Rows[index].Visible = false;

            if (dataGridView2.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView2.Rows[index].Cells[5].Value = RowState.Deleted;

                return;
            }
            dataGridView2.Rows[index].Cells[5].Value = RowState.Deleted;
        }

        private void UpdateTable2()
        {
            dataBase.openConnection();

            for (int index = 0; index < dataGridView2.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView2.Rows[index].Cells[5].Value;

                if (rowState == RowState.Existed)
                    continue;


                if (rowState == RowState.Deleted)
                {
                    var ID = Convert.ToInt32(dataGridView2.Rows[index].Cells[0].Value);
                    var deleteQuery = $"delete from Complited where ID = {ID}";

                    var command = new SqlCommand(deleteQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }
                if (rowState == RowState.Modified)
                {
                    var ID = dataGridView2.Rows[index].Cells[0].Value.ToString();
                    var Texn = dataGridView2.Rows[index].Cells[1].Value.ToString();
                    var Polomka = dataGridView2.Rows[index].Cells[2].Value.ToString();
                    var Zav = dataGridView2.Rows[index].Cells[3].Value.ToString();
                    var Price = dataGridView2.Rows[index].Cells[4].Value.ToString();

                    var changeQuery = $"update Complited set Texnika = '{Texn}', Polomka = '{Polomka}', Prositel = '{Zav}', Price = '{Price}' where ID = '{ID}'";

                    var command = new SqlCommand(changeQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();

                }

            }

            dataBase.closeConnection();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Удалить выбранный запрос?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                deleteRow();
                UpdateTable();
                ClearFields();
            }
            else if (dialogResult == DialogResult.No)
            {

            }
        }

        private void btnDelete2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Удалить выбранный запрос?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                deleteRow2();
                UpdateTable2();
                ClearFields2();
            }
            else if (dialogResult == DialogResult.No)
            {

            }
        }

        private void Change()
        {
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;

            var ID = tbID.Text;
            var Texn = tbTexnika.Text;
            var Zav = tbZav.Text;
            var Polom = tbPolomka.Text;

            dataGridView1.Rows[selectedRowIndex].SetValues(ID, Texn, Polom, Zav);
            dataGridView1.Rows[selectedRowIndex].Cells[4].Value = RowState.Modified;

        }
        private void Change2()
        {
            var selectedRowIndex = dataGridView2.CurrentCell.RowIndex;

            var ID = tbID2.Text;
            var Texn = tbTexnika2.Text;
            var Zav = tbZav2.Text;
            var Polom = tbPolomka2.Text;
            int Price;

            if (dataGridView2.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                if (int.TryParse(tbPrice2.Text, out Price))
                {
                    dataGridView2.Rows[selectedRowIndex].SetValues(ID, Texn, Polom, Zav, Price);
                    dataGridView2.Rows[selectedRowIndex].Cells[5].Value = RowState.Modified;
                }
                else
                {
                    MessageBox.Show("Цена должна быть числом!!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Change();
            UpdateTable();
            ClearFields();
        }

        private void btnEdit2_Click(object sender, EventArgs e)
        {
            Change2();
            UpdateTable2();
            ClearFields2();
        }
        private void ClearFields()
        {
            tbID.Text = "";
            tbTexnika.Text = "";
            tbZav.Text = "";
            tbPolomka.Text = "";
            tbPrice.Text = "";
        }
        private void ClearFields2()
        {
            tbID2.Text = "";
            tbTexnika2.Text = "";
            tbZav2.Text = "";
            tbPolomka2.Text = "";
            tbPrice2.Text = "";
        }
    }
    
}
