using System;
using System.Data;
using System.Windows.Forms;

namespace QuanLyKhachSanQLH
{
    public partial class fCheckIn : Form
    {
        public static string roomCode = "";
        public fCheckIn()
        {
            InitializeComponent();
        }
        private void loadBooked()
        {
            String query = "Select RoomCode,r.TypeRoomCode,Status,Price from Room r join TypeRoom t on r.TypeRoomCode = t.TypeRoomCode Where Status like N'%Đã Đặt%'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);

            dgvRoomBooked.DataSource = dtb;
            dgvRoomBooked.Refresh();
        }
        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            if (roomCode != "")
            {
                String query = "Update Room Set Status = N'Có Khách' Where RoomCode =N'" + roomCode + "'";
                DBAccess d = new DBAccess();
                DataTable dtb = d.getDS(query);

                loadBooked();

                MessageBox.Show("Nhận Phòng Thành Công!!!");
            }
            else
            {
                MessageBox.Show("Lỗi: Bạn chưa tìm thông tin khách hàng!!!");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string cmnd = txt_Card.Text;
            string phone = txt_Phone.Text;
            string email = txt_Email.Text;
            //
            string query = "Select Fullname, CMND, PhoneNumber, Email, Address, BookingDate, Amount From Client as c join Booking as b On c.ClientCode = b.ClientCode " +
                "Where CMND = N'" + cmnd + "' and PhoneNumber = N'" + phone + "' and Email like N'%" + email + "%'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);

            dgvCustomer.DataSource = dtb;
            dgvCustomer.Refresh();

            query = "Select BookingCode From Client as c join Booking as b On c.ClientCode = b.ClientCode " +
               "Where CMND = N'" + cmnd + "' and PhoneNumber = N'" + phone + "' and Email like N'%" + email + "%'";
            d = new DBAccess();
            dtb = d.getDS(query);
            DataRow r = dtb.Rows[0];
            string bookingCocde = r["BookingCode"].ToString();

            query = "Select RoomCode From ReservationsDetails " +
               "Where BookingCode = N'" + bookingCocde + "'";
            d = new DBAccess();
            dtb = d.getDS(query);
            r = dtb.Rows[0];
            roomCode = r["RoomCode"].ToString();

            query = "Select RoomCode,r.TypeRoomCode,Status,Price from Room r join TypeRoom t on r.TypeRoomCode = t.TypeRoomCode " +
               "Where RoomCode = N'" + roomCode + "' and Status like N'%Đã Đặt%'";
            d = new DBAccess();
            dtb = d.getDS(query);

            dgvRoomBooked.DataSource = dtb;
            dgvRoomBooked.Refresh();
        }

        private void fCheckIn_Load(object sender, EventArgs e)
        {
            loadBooked();
        }

        private void fCheckIn_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        private void Clear()
        {
            txt_Card.Text = "";
            txt_Email.Text = "";
            txt_Phone.Text = "";
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            Clear();
            this.Close();
        }
    }
}
