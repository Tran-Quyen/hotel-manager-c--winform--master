//using DevExpress.LookAndFeel;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyKhachSanQLH
{
    internal class DBAccess
    {
        SqlConnection con;
        SqlDataAdapter dap;
        SqlCommand cmd;

        public DBAccess()
        {
            connectDB();
        }

        //Kết nối
        public void connectDB()
        {
            string strCon = "Data Source=DESKTOP-3L4FS0A;Initial Catalog=QL_KhachSan;Integrated Security=True";//Url
            con = new SqlConnection(strCon);
            try
            {
                con.Open();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                con.Dispose();
            }
        }

        //Thực hiện câu lệnh sql trả về bảng dữ liệu
        public DataTable getDS(string sql)
        {
            DataTable tbl = new DataTable();
            dap = new SqlDataAdapter(sql, con);
            dap.Fill(tbl);
            return tbl;
        }

        //Thực thi lệnh sql chèn, xóa, sửa
        public bool ExecuteNonQueryPara(string sql, string[] parameters, object[] value)
        {
            int num = 0;
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                cmd = new SqlCommand(sql, con);
                SqlParameter p;
                for (int i = 0; i < parameters.Length; i++)
                {
                    p = new SqlParameter(parameters[i], value[i]);
                    cmd.Parameters.Add(p);
                }
                num = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            if (num > 0)
                return true;
            else
                return false;
        }

        public bool ExecuteQuery(string sql)
        {
            int num = 0;
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                cmd = new SqlCommand(sql, con);
                num = (int)cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            if (num > 0)
                return true;
            else
                return false;
        }

        //Kiểm tra sự tồn tại
        public bool checkExist(string tblName, string field, string value)
        {
            string sql = "Select Count (*) From " + tblName + " where " + field + "='" + value + "'";
            cmd = new SqlCommand(sql, con);
            int num = 0;
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                num = (int)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            if (num > 0)
                return true;
            else
                return false;
        }

        // Lấy mã cuối cùng
        public string GetLastID(string nameTable, string nameFiled)
        {
            string sql = "SELECT TOP (1) " + nameFiled + " FROM " + nameTable + " ORDER BY " + nameFiled + " DESC";
            return getDS(sql).Rows[0][nameFiled].ToString();
        }
    }
}