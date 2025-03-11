using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Popitka1
{
    class DataBase
    {


        SqlConnection con1 = new SqlConnection(@"Data Source=39-12\SQLEXPRESS;Initial Catalog=Practika1;Integrated Security=true");

        public void openConnection()
        {
            if(con1.State == System.Data.ConnectionState.Closed)
            {
                con1.Open();
            }
        }

        public void closeConnection()
        {
            if (con1.State == System.Data.ConnectionState.Open)
            {
                con1.Close();
            }
        }

        public SqlConnection getConnection()
        {
            return con1;
        }
    }
}
