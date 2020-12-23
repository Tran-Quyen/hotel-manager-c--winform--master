using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace QuanLyKhachSanQLH
{
    public partial class fPay : Form
    {
        private string clientCode = "";
        private string typeRoom = "";
        private string bookingCode = "";
        private long totalPrice = 0;
        public fPay()
        {
            InitializeComponent();
        }

        private void fpay_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

        }
        private void loadDGVCustomers()
        {
            String query = "Select c.ClientCode, Fullname, CMND, PhoneNumber, RoomCode from Client c join Booking b On c.ClientCode = b.ClientCode " +
                "join ReservationsDetails r On r.BookingCode = b.BookingCode " +
                "Where b.Status = 'false'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);

            dgvCustomer.DataSource = dtb;
            dgvCustomer.Refresh();
        }

        private void loadClientService()
        {
            String query = "Select c.Fullname, s.Name, Amount ,BookingDate, OrderCode, (Amount * s.Price)as 'total' from Service s join Orders o On o.ServiceId = s.Id " +
                "join Client c On c.ClientCode = o.ClientCode " +
                "Where c.ClientCode = N'" + clientCode + "'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);

            dgvListServiceUsed.DataSource = dtb;
            dgvListServiceUsed.Refresh();
        }

        private void loadBillInfo()
        {
            String query = "Select * from Booking Where ClientCode = N'" + clientCode + "'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);
            DataRow r = dtb.Rows[0];

            dtp_FromDate.Value = DateTime.Parse(r["ComingDate"].ToString());
            dtp_ToDate.Value = DateTime.Parse(r["LeavingDate"].ToString());
            txt_Amount.Text = r["Amount"].ToString();
            typeRoom = r["TypeRoomCode"].ToString();


            query = "Select * from ReservationsDetails Where BookingCode = N'" + r["BookingCode"].ToString() + "'";
            dtb = d.getDS(query);
            r = dtb.Rows[0];

            txt_Room.Text = r["RoomCode"].ToString() + " - " + typeRoom;

            query = "Select * from Room Where RoomCode=N'" + r["RoomCode"].ToString() + "'";
            dtb = d.getDS(query);
            r = dtb.Rows[0];


            txtStatus.Text = r["Status"].ToString();

        }

        private void totalPriceCacultate()
        {

            DateTime fromDate = dtp_FromDate.Value;
            DateTime toDate = dtp_ToDate.Value;
            TimeSpan Time = toDate - fromDate;
            long totalDays = Time.Days;

            long amount = Convert.ToInt64(txt_Amount.Text);

            String query = "Select * from TypeRoom Where TypeRoomCode = N'" + typeRoom + "'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);
            DataRow r = dtb.Rows[0];

            long price = Convert.ToInt64(r["Price"].ToString());

            long roomPrice = totalDays * amount * price;

            query = "Select sum(Amount*Price) as 'price' from Orders o join Service s On o.ServiceId=s.Id " +
               "Where ClientCode = N'" + clientCode + "'";
            d = new DBAccess();
            dtb = d.getDS(query);
            r = dtb.Rows[0];

            long servicePrice = 0;
            //Here
            if (r["price"].ToString()!="")
            {
                 servicePrice = Convert.ToInt64(r["price"].ToString());
            }
            

            totalPrice = roomPrice + servicePrice;

            txt_totalPrice.Text = string.Format(new CultureInfo("vi-VN"), "{0:#,##0 đồng chẵn}", totalPrice);
        }
        private void fPay_Load(object sender, EventArgs e)
        {
            loadDGVCustomers();
        }

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            clientCode = dgvCustomer.Rows[i].Cells[0].Value.ToString();
            txt_Name.Text = dgvCustomer.Rows[i].Cells[1].Value.ToString();
            if(clientCode!="" && txt_Name.Text != "")
            {

            loadClientService();
            loadBillInfo();
            totalPriceCacultate();
            }
            else
            {
                MessageBox.Show("Hãy Chọn Lại Khách Hàng!!!");
            }
        }


        private void btnPay_Click(object sender, EventArgs e)
        {
            if (clientCode != "")
            {
                try
                {
                    string[] roomCode = txt_Room.Text.Split('-');
                    fBill f = new fBill(clientCode, bookingCode, txt_Name.Text, roomCode[0].Trim(), totalPrice);
                    f.ShowDialog();
                    this.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Lỗi Thanh Toán!!!");
                }
            }
            else
            {
                MessageBox.Show("Hãy Chọn Khách Hàng!!!");
            }

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
