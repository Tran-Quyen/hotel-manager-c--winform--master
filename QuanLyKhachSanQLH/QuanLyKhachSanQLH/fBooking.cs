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
    public partial class fBooking : Form
    {
        public string UserId { get; set; }
        public string customerId;
        public fBooking()
        {
            InitializeComponent();
        }
        private void loadBookingNone()
        {
            String query = "Select RoomCode,r.TypeRoomCode,Status,Price from Room r join TypeRoom t on r.TypeRoomCode = t.TypeRoomCode Where Status like N'%Trống%'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);

            dgvRoomNone.DataSource = dtb;
            dgvRoomNone.Refresh();
        }
        public fBooking(string userId)
        {
            InitializeComponent();
            this.UserId = userId;
        }
        private void loadComboboxTypeRoom()
        {
            string query = "Select TypeRoomCode From TypeRoom";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);

            cb_Room.DataSource = dtb;
            cb_Room.DisplayMember = "TypeRoomCode";
            cb_Room.ValueMember = "TypeRoomCode";
        }
        private void Clear()
        {
            txt_Name.Text = "";
            txt_Address.Text = "";
            txt_Card.Text = "";
            txt_Email.Text = "";
            txt_Phone.Text = "";
            nudAmount.Value = 1;
            txtDeposit.Text = "400000";
            btnCreateAcc.Enabled = false;
        }
        //Quay lại
        private void button1_Click(object sender, EventArgs e)
        { 
            Clear();
            this.Close();
        }
        private bool checkDate(DateTime toDate,DateTime fromDate)
        {
            TimeSpan Time = toDate - fromDate;
            return (Time.Days>=0)?true:false;
        }
        private void save(string status)
        {
            String query = "Select count(*) as 'SL' from Client";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);
            DataRow r = dtb.Rows[0];
            string id = (Convert.ToInt16(r["SL"].ToString()) + 1) + "";
            string name = txt_Name.Text;
            string address = txt_Address.Text;
            string card = txt_Card.Text;
            string email = txt_Email.Text;
            string phone = txt_Phone.Text;
            int amount = Convert.ToInt32(nudAmount.Value);
            DateTime fromDate = dtp_FromDate.Value;
            DateTime toDate = dtp_ToDate.Value;
            DateTime now = DateTime.Now;
            string typeRoom = cb_Room.SelectedValue.ToString();

            if(name!="" && address!="" && card!="" && email!="" && phone!="" && txtSelectRoom.Text!="")
            { 
                if (checkDate(toDate,fromDate) && checkDate(fromDate, now))
                {
                if (MessageBox.Show(string.Format("Bạn có thật sự muốn thêm thông tin khách hàng {0} ?", name), "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    query = "Insert into Client values(N'" + "KH00" + id + "',N'" + name + "',N'" + card + "',N'" + phone + "',N'" + email + "',N'" + address + "')";
                    d = new DBAccess();
                    dtb = d.getDS(query);

                    query = "Insert into Booking values(N'" + "DP00" + id + "',N'" + this.UserId + "',N'" + "KH00" + id + "',N'" + typeRoom + "', N'" + now.ToString("yyyy-MM-dd") + "', N'" + fromDate.ToString("yyyy-MM-dd") + "', N'" + toDate.ToString("yyyy-MM-dd") + "',N'" + Convert.ToInt32(txtDeposit.Text) + "',N'" + amount + "','false')";
                    d = new DBAccess();
                    dtb = d.getDS(query);

                    query = "Update Room Set Status = N'" + status + "' Where RoomCode=N'" + txtSelectRoom.Text + "'";
                    d = new DBAccess();
                    dtb = d.getDS(query);

                    query = "Insert into ReservationsDetails values(N'" + "DP00" + id + "', N'"+ txtSelectRoom.Text + "')";
                    d = new DBAccess();
                    dtb = d.getDS(query);
                    btnCreateAcc.Enabled = true; 

                    this.customerId = "KH00" + id;
                    loadBookingNone();
                    MessageBox.Show("Thêm khách hàng thành công!");
                    }
                }
                else
                {
                    MessageBox.Show("Lỗi: Hãy chọn lại ngày!!!");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng điền đủ thông tin!!!");
            }
        }

        private void btnRent_Click(object sender, EventArgs e)
        {
            save("Có Khách");
        }

        private void nudAmount_ValueChanged(object sender, EventArgs e)
        {
            txtDeposit.Text = (nudAmount.Value * 400000) + "";
        }

        private void fBooking_Load(object sender, EventArgs e)
        {
            btnCreateAcc.Enabled = false;
            txtDeposit.Text = (nudAmount.Value * 400000) + "";
            loadBookingNone();
            loadComboboxTypeRoom();
        }

        private void dgvRoomNone_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            txtSelectRoom.Text = dgvRoomNone.Rows[i].Cells[0].Value.ToString();
            cb_Room.SelectedValue = dgvRoomNone.Rows[i].Cells[1].Value.ToString();
        }

        private void cb_Room_SelectedValueChanged(object sender, EventArgs e)
        {
            String query = "Select RoomCode,r.TypeRoomCode,Status,Price from Room r join TypeRoom t on r.TypeRoomCode = t.TypeRoomCode Where Status like N'%Trống%'and r.TypeRoomCode=N'"+cb_Room.SelectedValue+"'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);

            dgvRoomNone.DataSource = dtb;
            dgvRoomNone.Refresh();
        }

        private void btnBooking_Click(object sender, EventArgs e)
        {
            save("Đã Đặt");
        }

        private void cb_ShowHide_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_ShowHide.Checked)
            {
                txtPassword.UseSystemPasswordChar = true;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = false;
            }
        }

        private void btnCreateAcc_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            if(username !="" && password != "")
            {
                String query = "Insert into Users values(N'"+this.customerId+ "',N'" + username + "',N'" + password + "', N'C')";
                DBAccess d = new DBAccess();
                DataTable dtb = d.getDS(query);
                MessageBox.Show("Thêm tài khoản thành công!");
            }
            else
            {
                MessageBox.Show("Lỗi: Tài khoản hoặc mật khẩu không được để trống!!!");
            }
        }

        private void fBooking_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
