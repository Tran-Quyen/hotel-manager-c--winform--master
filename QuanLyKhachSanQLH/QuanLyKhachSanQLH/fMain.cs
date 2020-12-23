using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyKhachSanQLH
{
    public partial class fMain : Form
    {
        public static string connectString = "Data Source=DESKTOP-3L4FS0A;Initial Catalog=QL_KhachSan;Integrated Security=True";//Url

        private String UserId;
        public fMain(string code)
        {
            InitializeComponent();
            this.UserId = code;
            checkPermision();
        }

        public fMain()
        {
        }

        private void checkPermision()
        {
            SqlConnection conn = new SqlConnection(connectString);
            conn.Open();
            string query = "Select * From Users Where Code=N'" + this.UserId + "'";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            DataRow r = dataTable.Rows[0];
            string permission = r["DecentralizationId"].ToString();
            cmd.Dispose();
            conn.Close();
            if (permission != "A") tsmiManager.Enabled = false;
        }

        private void managerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fManager f = new fManager();
            f.ShowDialog();
            this.Show();
        }

        private void loadBookingNone()
        {
            String query = "Select RoomCode,r.TypeRoomCode,Status,Price from Room r join TypeRoom t on r.TypeRoomCode = t.TypeRoomCode Where Status like N'%Trống%'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);

            dgvRoomNone.DataSource = dtb;
            dgvRoomNone.Refresh();
        }
        private void loadBooked()
        {
            String query = "Select RoomCode,r.TypeRoomCode,Status,Price from Room r join TypeRoom t on r.TypeRoomCode = t.TypeRoomCode Where Status like N'%Đã Đặt%'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);

            dgvRoomBooked.DataSource = dtb;
            dgvRoomBooked.Refresh();
        }

        private void loadHasStaying()
        {
            String query = "Select RoomCode,r.TypeRoomCode,Status,Price from Room r join TypeRoom t on r.TypeRoomCode = t.TypeRoomCode Where Status like N'%Có Khách%'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);

            dgvStaying.DataSource = dtb;
            dgvStaying.Refresh();
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            loadBookingNone();
            loadBooked();
            loadHasStaying();
        }

        private void fMain_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsmiLiscene_Click(object sender, EventArgs e)
        {
            fLisence f = new fLisence();
            f.ShowDialog();
            this.Show();
        }

        private void tsmi_infor_Click(object sender, EventArgs e)
        {
            fAccount f = new fAccount();
            DialogResult result = f.ShowDialog();
            if (result == DialogResult.OK)
            {

            }
            else
            {
                loadBookingNone();
                loadBooked();
                loadHasStaying();
            }
            this.Show();
        }

        private void btnBooking_Click(object sender, EventArgs e)
        {
            fBooking f = new fBooking(UserId);
            f.ShowDialog();
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fCheckIn f = new fCheckIn();
            DialogResult result = f.ShowDialog();
            if (result == DialogResult.OK)
            {

            }
            else
            {
                loadBookingNone();
                loadBooked();
                loadHasStaying();
            }
            this.Show();
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            fPay f = new fPay();
            DialogResult result = f.ShowDialog();
            if (result == DialogResult.OK)
            {

            }
            else
            {
                loadBookingNone();
                loadBooked();
                loadHasStaying();
            }
            this.Show();
        }

        private void dịchVụToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fService f = new fService();
            f.ShowDialog();
            this.Show();
        }

        private void tìmKiếmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fSearch f = new fSearch();
            f.ShowDialog();
            this.Show();
        }
    }
}
