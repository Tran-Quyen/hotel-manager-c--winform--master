using System;
using System.Data;
using System.Windows.Forms;

namespace QuanLyKhachSanQLH
{
    public partial class fService : Form
    {
        public static string clientId = "";
        public static string serviceId = "";
        public static string OrderId = "";

        public fService()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loadDGVCustomers()
        {
            String query = "Select Fullname,CMND,PhoneNumber,Email,Address, c.ClientCode from Client c join Booking b On c.ClientCode = b.ClientCode " +
                "Where b.Status = 'false'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);

            dgvCustomer.DataSource = dtb;
            dgvCustomer.Refresh();
        }

        private void loadDGVServices()
        {
            String query = "Select * from Service";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);

            dgvService.DataSource = dtb;
            dgvService.Refresh();
        }

        private void loadCBService()
        {
            String query = "Select * from Service";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);

            cb_Service.DataSource = dtb;
            cb_Service.DisplayMember = "Name";
            cb_Service.ValueMember = "Id";
        }

        private void loadClientService()
        {
            String query = "Select c.Fullname, s.Name, BookingDate, Amount , OrderCode, (Amount * s.Price)as 'total' from Service s join Orders o On o.ServiceId = s.Id " +
                "join Client c On c.ClientCode = o.ClientCode " +
                "Where c.ClientCode = N'" + clientId + "'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);

            dgvListServiceUsed.DataSource = dtb;
            dgvListServiceUsed.Refresh();
        }

        private void fService_Load(object sender, EventArgs e)
        {
            loadDGVCustomers();
            loadDGVServices();
            loadCBService();
        }

        private void dgvService_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
        }

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            clientId = dgvCustomer.Rows[i].Cells[5].Value.ToString();
            txt_Name.Text = dgvCustomer.Rows[i].Cells[0].Value.ToString();
            loadClientService();
        }

        private void dgvListServiceUsed_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            OrderId = dgvListServiceUsed.Rows[i].Cells[4].Value.ToString();
        }

        private void btnSave3_Click(object sender, EventArgs e)
        {
            if (clientId != "")
            {
                DateTime now = DateTime.Now;
                DBAccess d = new DBAccess();
                string id = d.GetLastID("Orders", "OrderCode");
                String query = "Insert into Orders values(N'" + (Convert.ToInt32(id) + 1) + "',N'" + clientId + "',N'" + cb_Service.SelectedValue.ToString() + "',N'" + nudAmount.Value.ToString() + "', N'" + now.ToString("yyyy-MM-dd") + "')";

                DataTable dtb = d.getDS(query);

                loadClientService();
                MessageBox.Show("Thêm Dịch Vụ Thành Công!");
            }
            else
            {
                MessageBox.Show("Hãy Chọn Khách Hàng!!!");
            }
        }

        private void btnDelete3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("Bạn có thật sự muốn xóa Dịch Vụ ?"), "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    DBAccess d = new DBAccess();
                    String query = "Delete Orders Where OrderCode = " + OrderId + "";

                    DataTable dtb = d.getDS(query);
                    loadClientService();
                    MessageBox.Show("Xóa Dịch Vụ Thành Công !");
                }
                catch (Exception)
                {

                    MessageBox.Show("Lỗi: Không Tìm Thấy Khách Hàng Được Chọn!!!");
                }
            }

        }
    }
}
