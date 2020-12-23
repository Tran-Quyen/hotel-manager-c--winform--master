using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKhachSanQLH
{
    public partial class fCustomer : Form
    {
        public static string connectString = "Data Source=DESKTOP-3L4FS0A;Initial Catalog=QL_KhachSan;Integrated Security=True";//Url

        private String UserId;
        private String typeRoom;

        public fCustomer(string code)
        {
            InitializeComponent();
            this.UserId = code;
        }

        private void loadAccount()
        {
            String query = "Select * from Users Where Code=N'"+UserId+"'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);
            DataRow r = dtb.Rows[0];
            txt_Id.Text = r["Code"].ToString();
            txt_Username.Text = r["Username"].ToString();
            txt_Pass.Text = r["Password"].ToString();   
        }
        private void loadCustomerInfo()
        {
            String query = "Select * from Client Where ClientCode = N'" + UserId + "'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);
            DataRow r = dtb.Rows[0];
            txt_Name.Text = r["Fullname"].ToString();
            txt_Card.Text = r["CMND"].ToString();
            txt_Phone.Text = r["PhoneNumber"].ToString();
            txt_Email.Text = r["Email"].ToString();
            txt_Address.Text = r["Address"].ToString();

        }

        private void loadBillInfo()
        {
            String query = "Select * from Booking Where ClientCode=N'" + UserId + "'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);
            DataRow r = dtb.Rows[0];
            dtp_FromDate.Value = DateTime.Parse(r["ComingDate"].ToString());
            dtp_ToDate.Value = DateTime.Parse(r["LeavingDate"].ToString());
            txt_Amount.Text = r["Amount"].ToString();
            String typeRoom = r["TypeRoomCode"].ToString();
            this.typeRoom = typeRoom;

            query = "Select * from ReservationsDetails Where BookingCode=N'" + r["BookingCode"].ToString() + "'";
            dtb = d.getDS(query);
            r = dtb.Rows[0];

            txt_Room.Text = r["RoomCode"].ToString() + " - " + typeRoom;

            query = "Select * from Room Where RoomCode=N'" + r["RoomCode"].ToString() + "'";
            dtb = d.getDS(query);
            r = dtb.Rows[0];

            if(r["Status"].ToString()=="Có Khách")
            {
                txtStatus.Text = "Đang Sử Dụng";
            }
            else { txtStatus.Text = "Đã Đặt"; }
        }
        private void loadDGVService()
        {
            string query = "Select BookingDate,Name,Amount,Price From Service s join Orders o On s.Id = o.ServiceId " +
               "Where ClientCode =N'"+UserId+"'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);

            dgvService.DataSource = dtb;
            dgvService.Refresh();
        }

        private void totalPrice()
        {
            DateTime fromDate = dtp_FromDate.Value;
            DateTime toDate = dtp_ToDate.Value;
            TimeSpan Time = toDate - fromDate;
            long totalDays = Time.Days;

            long amount = Convert.ToInt64(txt_Amount.Text);

            String query = "Select * from TypeRoom Where TypeRoomCode like N'%" + this.typeRoom + "%'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);
            DataRow r = dtb.Rows[0];

            long price = Convert.ToInt64(r["Price"].ToString());

            long roomPrice = totalDays * amount * price;

             query = "Select sum(Amount*Price) as 'price' from Orders o join Service s On o.ServiceId=s.Id" +" "+
                "Where ClientCode = N'"+ UserId + "'";
             d = new DBAccess();
             dtb = d.getDS(query);
             r = dtb.Rows[0];

            long servicePrice = 0;
            //Here
            if (r["price"].ToString() != "")
            {
                servicePrice = Convert.ToInt64(r["price"].ToString());
            }

            long totalPrice = roomPrice + servicePrice;

            txt_totalPrice.Text = string.Format(new CultureInfo("vi-VN"), "{0:#,##0 đồng chẵn}", totalPrice);
        }

        private void fCustomer_Load(object sender, EventArgs e)
        {
            loadAccount();
            loadCustomerInfo();
            loadBillInfo();
            totalPrice();
            loadDGVService();
        }

        private void cb_ShowHide_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_ShowHide.Checked)
            {
                txt_Pass.UseSystemPasswordChar = true;
            }
            else
            {
                txt_Pass.UseSystemPasswordChar = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
