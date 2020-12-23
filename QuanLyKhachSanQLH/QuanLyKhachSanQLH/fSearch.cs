using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKhachSanQLH
{
    public partial class fSearch : Form
    {
        public fSearch()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string cmnd = txt_Card.Text;
            string phone = txt_Phone.Text;
            string email = txt_Email.Text;
            //
            string query = "Select ClientCode From Client " +
                "Where CMND = N'" + cmnd + "' and PhoneNumber = N'" + phone + "' and Email like N'%" + email + "%'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);
            DataRow r = dtb.Rows[0];
            if(dtb.Rows.Count > 0)
            {
            fCustomer f = new fCustomer(r["ClientCode"].ToString());
            f.ShowDialog();
            this.Show();
            }
            else
            {
                MessageBox.Show("Khách Hàng Không Tồn Tại!!!");
            }
        }
    }
}
