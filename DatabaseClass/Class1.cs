using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseClass
{
    public class clsDatabase
    {
        static string strConnection = "";
        static SqlConnection objConnection = null;
        static SqlCommand objCommand = null;
        static SqlDataReader objDataReader = null;
        static SqlDataAdapter objDataAdapter = null;
        static DataTable objDataTable = null;
        static DataSet objDataSet = null;

        public static SqlConnection CreateConnection()
        {
            strConnection = "server=localhost;database=randomdb;Trusted_Connection=yes";
            //strConnection = "Data Source=mssql6.gear.host;Persist Security Info=True;User ID=randomdb;Password=Notbeefsteak1!";
            objConnection = new SqlConnection(strConnection);
            objConnection.Open();
			MessageBox.Show("Open");
            return objConnection;
        }

        public static SqlDataReader ExecuteQuery(string str)
        {
            try
            {
                objCommand = new SqlCommand(str, objConnection);
                objDataReader = objCommand.ExecuteReader();
                return objDataReader;
            }
            catch (SqlException es)
            {
                MessageBox.Show(es.Message);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                objCommand.Dispose();
            }
        }

        public static DataTable CreateDataTable(string str)
        {
            try
            {
                objDataAdapter = new SqlDataAdapter(str, objConnection);
                objDataTable = new DataTable();
                objDataAdapter.Fill(objDataTable);
                return objDataTable;
            }
            catch (SqlException es)
            {
                MessageBox.Show(es.Message);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                objDataAdapter.Dispose();
                objConnection.Close();
            }
        }

        public static int ExecuteNonQuery(string str)
        {
            int noOfRows = 0;
            try
            {
                objCommand = new SqlCommand(str, objConnection);
                objCommand.ExecuteNonQuery();
                return noOfRows;
            }
            catch (SqlException es)
            {
                if (es.Number == 2627)
                {
                    MessageBox.Show("Primary Key Violation");
                }
                else if (es.Number == 547)
                {
                    MessageBox.Show("Foreign Key Violation");
                }
                return -1;
            }
            finally
            {
                objCommand.Dispose();
            }
        }

        public static string GetTableName(string errmessage)
        {
            string tablename = "";
            if (errmessage.Contains("course_location"))
            {
                tablename = "course_location";
            }
            else if (errmessage.Contains("course_fk"))
            {
                tablename = "course";
            }
            else if (errmessage.Contains("location_fk"))
            {
                tablename = "course";
            }
            return tablename;
        }

        public static DataSet CreateDataSet(string strSql)
        {
            DataSet objDataSet = new DataSet();
            SqlDataAdapter objDataAdapter = new SqlDataAdapter(strSql, objConnection);
            objDataAdapter.Fill(objDataSet);
            return objDataSet;
        }
    }
}
