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
            dataGridView1.Columns.Add("Price", "Цена");

            dataGridView1.Columns.Add("IsNew", String.Empty);
        }

        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), record.GetInt32(4), RowState.ModifiedNew);

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
            RefreshDataGrid(dataGridView1);
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
                tbPrice.Text = row.Cells[4].Value.ToString();


            }
        }
    }
}
