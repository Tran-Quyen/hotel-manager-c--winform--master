using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
namespace QuanLyKhachSanQLH
{
    public partial class fManager : Form
    {
        public static string connectString = "Data Source=DESKTOP-3L4FS0A;Initial Catalog=QL_KhachSan;Integrated Security=True";//Url
        public fManager()
        {
            InitializeComponent();
        }
        //Load
        private void loadPageUser()
        {
          
            string query = "Select * From Users";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);
            dgvAccount.DataSource = dtb;
            dgvAccount.Refresh();
           
        }
        private void loadPageDecen()
        {
            
            string query = "Select * From Decentralization";
           
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);
            dgvDecentralization.DataSource = dtb;
            dgvDecentralization.Refresh();
           
            loadPageUser();
        }
        private void loadComboboxDecentralization()
        {
           
            string query = "Select * From Decentralization";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);

            cbDecentralizationId.DataSource = dtb;
            cbDecentralizationId.DisplayMember = "Permission";
            cbDecentralizationId.ValueMember = "Id";
        }
        private void loadPageRoom()
        {
           
            string query = "Select * From Room";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);
            dgvRoom.DataSource = dtb;
            dgvRoom.Refresh();
           
        }
        private void loadComboboxTypeRoom()
        {
           
            string query = "Select TypeRoomCode From TypeRoom";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);

            cbTypeRoom.DataSource = dtb;
            cbTypeRoom.DisplayMember = "TypeRoomCode";
            cbTypeRoom.ValueMember = "TypeRoomCode";
        }
        private void loadPageTypeRoom()
        {
            String query = "Select * from TypeRoom ";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);
            dgvTypeRoom.DataSource = dtb;
            dgvTypeRoom.Refresh();

            loadPageRoom();
        }
        private void loadPageThongKe()
        {
            string query = "Select ROW_NUMBER() over(order by BillCode asc) as STT, BillCode as N'Mã Hóa Đơn',BookingCode as N'Mã Đặt Phòng',InvoiceDate as N'Ngày Thanh Toán', TotalMoney as N'Tổng Tiền' From Bill";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);
            dgvThongKe.DataSource = dtb;
            dgvThongKe.Refresh();
        }
        private void fManager_Load(object sender, EventArgs e)
        {
            loadPageDecen();
            loadComboboxDecentralization();
            loadComboboxTypeRoom();
            loadPageTypeRoom();
            loadPageThongKe();
        }
       
        //Account
        private void dgvAccount_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            txtCode.Text = dgvAccount.Rows[i].Cells[0].Value.ToString();
            txtUsername.Text = dgvAccount.Rows[i].Cells[1].Value.ToString();
            txtPassword.Text = dgvAccount.Rows[i].Cells[2].Value.ToString();
            cbDecentralizationId.SelectedValue = dgvAccount.Rows[i].Cells[3].Value;
            txtCode.Enabled = false;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string code = txtCode.Text;
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string id = cbDecentralizationId.SelectedValue.ToString();
            SqlConnection conn = new SqlConnection(connectString);
            conn.Open();
            string query = "Insert into Users values(N'"+code+"',N'"+username+"',N'"+password+"',N'"+id+"')";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
            loadPageUser();
            MessageBox.Show("Thêm Thành Công!");
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            txtCode.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            cbDecentralizationId.SelectedIndex=0;
            txtCode.Enabled = true;
            loadPageUser();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            string code = txtCode.Text;
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string id = cbDecentralizationId.SelectedValue.ToString();
            SqlConnection conn = new SqlConnection(connectString);
            conn.Open();
            string query = "Update Users Set Username=N'"+username+"', Password=N'"+password+"', DecentralizationId=N'"+id+"' " +
                "Where Code=N'"+code+"'";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
            loadPageUser();
            MessageBox.Show("Sửa thành công!");
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string code = txtCode.Text;
            string name = txtUsername.Text;
            if (MessageBox.Show(string.Format("Bạn có thật sự muốn xóa tài khoản {0} ?", name), "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                SqlConnection conn = new SqlConnection(connectString);
                conn.Open();
                string query = "Delete Users Where Code=N'" + code + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();
                loadPageUser();
                MessageBox.Show("Xóa quyền {0} thành công!", name);
            }

        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string code = txtCode.Text;
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string id = cbDecentralizationId.SelectedValue.ToString();
            SqlConnection conn = new SqlConnection(connectString);
            conn.Open();
            string query = "Select * From Users Where Code like N'%"+code+"%' and Username like N'%"+username+"%' and Password like N'%"+password+"%'" +
                "and DecentralizationId like N'%"+id+"%'";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            dgvAccount.DataSource = dataTable;
            dgvAccount.Refresh();
            cmd.Dispose();
            conn.Close();
            MessageBox.Show("Tìm kiếm thành công!");
        }
        //Decentralization
        private void btnSave2_Click(object sender, EventArgs e)
        {
            string id = txtID.Text;
            string name = txtDecentralization.Text;
            SqlConnection conn = new SqlConnection(connectString);
            conn.Open();
            string query = "Insert into Decentralization values(N'"+id+"', N'"+name+"')";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
            loadPageDecen();
            MessageBox.Show("Thêm thành công!");
        }
        private void dgvDecentralization_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            txtID.Text = dgvDecentralization.Rows[i].Cells[0].Value.ToString();
            txtDecentralization.Text = dgvDecentralization.Rows[i].Cells[1].Value.ToString();
            txtID.Enabled = false;
        }
        private void btnReset2_Click(object sender, EventArgs e)
        {
            txtID.Text = "";
            txtDecentralization.Text = "";
            txtID.Enabled = true;
            loadPageDecen();
        }
        private void btnDelete2_Click(object sender, EventArgs e)
        {
            string id = txtID.Text;
            string name = txtDecentralization.Text;
            if (MessageBox.Show(string.Format("Bạn có thật sự muốn xóa quyền {0} ?", name), "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                SqlConnection conn = new SqlConnection(connectString);
                conn.Open();
                string query = "Delete Decentralization Where Id=N'" + id + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();
                loadPageDecen();
                MessageBox.Show("Xóa quyền {0} thành công!", name);
            }
        }
        private void btnUpdate2_Click(object sender, EventArgs e)
        {
            string id = txtID.Text;
            string name = txtDecentralization.Text;
            SqlConnection conn = new SqlConnection(connectString);
            conn.Open();
            string query = "Update Decentralization Set Permission=N'"+name+"' Where Id=N'"+id+"'";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
            loadPageDecen();
            MessageBox.Show("Sửa thành công!");
        }
        private void btnSearch2_Click(object sender, EventArgs e)
        {
            string id = txtID.Text;
            string name = txtDecentralization.Text;
            SqlConnection conn = new SqlConnection(connectString);
            conn.Open();
            string query = "Select * From Decentralization Where Id like N'%" + id + "%' and Permission like N'%" + name + "%'";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            dgvDecentralization.DataSource = dataTable;
            dgvDecentralization.Refresh();
            cmd.Dispose();
            conn.Close();
            MessageBox.Show("Tìm kiếm thành công!");
        }
        //Room
        private void dgvRoom_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            txtRoom.Text = dgvRoom.Rows[i].Cells[0].Value.ToString();
            cbTypeRoom.SelectedValue = dgvRoom.Rows[i].Cells[1].Value;
            cbStatus.SelectedItem = dgvRoom.Rows[i].Cells[2].Value;
            txtRoom.Enabled = false;
        }
        private void btnReset3_Click(object sender, EventArgs e)
        {
            txtRoom.Text = "";
            cbTypeRoom.SelectedIndex = 0;
            cbStatus.SelectedIndex= 0;
            txtRoom.Enabled = true;
            loadPageRoom();
        }
        private void btnSave3_Click(object sender, EventArgs e)
        {
            string room = txtRoom.Text;
            string type= cbTypeRoom.Text;
            string note = txt_Note.Text;
            SqlConnection conn = new SqlConnection(connectString);
            conn.Open();

            string query = "Insert into Room values(N'" + room + "', N'" + type + "',N'Trống',N'"+ note + "')";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();

            query = "Select count(*) From Room Where TypeRoomCode=N'" + type + "'";
            cmd = new SqlCommand(query, conn);
            int count = (int)cmd.ExecuteScalar();

            query = "Update TypeRoom Set Counts=" + count + "Where TypeRoomCode=N'" + type + "'";
            cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();

            cmd.Dispose();
            conn.Close();
            loadPageRoom();
            MessageBox.Show("Thêm thành công!");
        }
        private void btnUpdate3_Click(object sender, EventArgs e)
        {
            string room = txtRoom.Text;
            string type = cbTypeRoom.SelectedValue.ToString();
            string status = cbStatus.SelectedItem.ToString();
            string note = txt_Note.Text;
            SqlConnection conn = new SqlConnection(connectString);
            conn.Open();
            string query = "Update Room Set TypeRoomCode=N'" + type + "',Status=N'" + status + "',Note = N'"+ note + "' Where RoomCode=N'" + room + "'";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
            loadPageRoom();
            MessageBox.Show("Sửa thành công!");
        }
        private void btnDelete3_Click(object sender, EventArgs e)
        {
            string room = txtRoom.Text;
            string type = cbTypeRoom.SelectedValue.ToString();
            if (MessageBox.Show(string.Format("Bạn có thật sự muốn xóa phòng {0} ?", room), "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                SqlConnection conn = new SqlConnection(connectString);
                conn.Open();

                string query = "Delete Room Where RoomCode=N'" + room + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                query = "Select count(*) From Room Where TypeRoomCode=N'" + type + "'";
                cmd = new SqlCommand(query, conn);
                int count = (int)cmd.ExecuteScalar();

                query = "Update TypeRoom Set Counts=" + count + "Where TypeRoomCode=N'" + type + "'";
                cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                conn.Close();
                loadPageRoom();
                MessageBox.Show("Xóa {0} Thành Công!", room);
            }
        }
        private void btnSearch3_Click(object sender, EventArgs e)
        {
            string room = txtRoom.Text;
            string type = cbTypeRoom.Text;
            string status = cbStatus.Text;
            SqlConnection conn = new SqlConnection(connectString);
            conn.Open();
            string query = "Select * From Room Where RoomCode like N'%" + room + "%' and TypeRoomCode like N'%" + type + "%'"
               + "and Status like N'%" + status + "%'";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            dgvRoom.DataSource = dataTable;
            dgvRoom.Refresh();
            cmd.Dispose();
            conn.Close();
            MessageBox.Show("Tìm kiếm thành công!");
        }

        private void tpTypeRoom_Click(object sender, EventArgs e)
        {

        }


        private void button5_Click(object sender, EventArgs e)
        {
            string typeRoom = txtTypeRoom.Text;
            long count = (long)nudCounts.Value;
            long price = (long)nudPrice.Value;
            string query = "Insert into TypeRoom values(N'"+typeRoom+"',"+count+","+price+")";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);
            MessageBox.Show("Thêm thành công!");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string typeRoom = txtTypeRoom.Text;
            long count = (long)nudCounts.Value;
            long price = (long)nudPrice.Value;
            string query = "Update TypeRoom Set Counts=" + count + ",Price=" + price + " Where TypeRoomCode=N'" + typeRoom + "'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);
            MessageBox.Show("Sửa thành công!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string type = txtTypeRoom.Text;
            if (MessageBox.Show(string.Format("Bạn có thật sự muốn xóa loại phòng {0} ?", type), "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                SqlConnection conn = new SqlConnection(connectString);
                conn.Open();

                string query = "Delete Room Where TypeRoomCode=N'" + type + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                query = "Delete TypeRoom Where TypeRoomCode=N'" + type + "'";
                cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                conn.Close();
                loadPageRoom();
                MessageBox.Show("Xóa {0} Thành Công!", type);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string typeRoom = txtTypeRoom.Text;
            long count = (long)nudCounts.Value;
            long price = (long)nudPrice.Value;
            string query = "Select * From TypeRoom Where TypeRoomCode like N'" + typeRoom + "' and Counts="+count+" and Price="+price+"";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);

            dgvTypeRoom.DataSource = dtb;
            dgvTypeRoom.Refresh();

            MessageBox.Show("Tìm kiếm thành công!");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            txtTypeRoom.Text = "";
            nudCounts.Value = 0;
            nudPrice.Value = 100000;
            txtTypeRoom.Enabled = true;
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            invoice();
        }
        private void invoice()
        {
            DateTime fromDate = dtpFromDate.Value;
            DateTime toDate = dtpToDate.Value;
            string query = "Select ROW_NUMBER() over(order by BillCode asc) as STT, BillCode as N'Mã Hóa Đơn',BookingCode as N'Mã Đặt Phòng',InvoiceDate as N'Ngày Thanh Toán', TotalMoney as N'Tổng Tiền' From Bill " +
                "Where InvoiceDate between N'"+fromDate.ToString("yyyyMMdd")+"' and N'"+ toDate.ToString("yyyyMMdd") + "'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);
            XuatExcel(dtb, "Thống Kê");
        }

        public void XuatExcel(DataTable dataTable, string sheetName)
        {
            //Tạo các đối tượng Excel
            Excel.Application oExcel = new Excel.Application();
            Workbooks oBooks;
            Sheets oSheets;
            Workbook oBook;
            Worksheet oSheet;

            //Tạo mới một Excel WorkBook 
            oExcel.Visible = true;
            oExcel.DisplayAlerts = false;
            oExcel.Application.SheetsInNewWorkbook = 1;
            oBooks = oExcel.Workbooks;
            oBook = (Workbook)(oExcel.Workbooks.Add(Type.Missing));
            oSheets = oBook.Worksheets;
            oSheet = (Worksheet)oSheets.get_Item(1);
            oSheet.Name = sheetName;

            // Tạo phần đầu nếu muốn
            Range head = oSheet.get_Range("A1", "E1");
            head.MergeCells = true;
            head.Value2 = sheetName.ToUpper();
            head.Font.Bold = true;
            head.Font.Name = "Time New Roman";
            head.Font.Size = "16";
            head.HorizontalAlignment = XlHAlign.xlHAlignCenter;

            // Tạo tiêu đề cột 
            Range cl1 = oSheet.get_Range("A3", "A3");
            cl1.Value2 = "STT";
            cl1.ColumnWidth = 7.5;
            cl1.HorizontalAlignment = XlHAlign.xlHAlignCenter;

            Range cl2 = oSheet.get_Range("B3", "B3");
            cl2.Value2 = "Mã Hóa Đơn";
            cl2.ColumnWidth = 20.0;
            cl2.HorizontalAlignment = XlHAlign.xlHAlignCenter;


            Range cl3 = oSheet.get_Range("C3", "C3");
            cl3.Value2 = "Mã Đặt Phòng";
            cl3.ColumnWidth = 25.0;
            cl3.HorizontalAlignment = XlHAlign.xlHAlignCenter;

            Range cl4 = oSheet.get_Range("D3", "D3");
            cl4.Value2 = "Ngày Thanh Toán";
            cl4.ColumnWidth = 30.0;
            cl4.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            Range ngay_sinh = oSheet.get_Range("D4", "D10000");//D10000
            ngay_sinh.Columns.NumberFormat = "dd/MM/yyyy";
            ngay_sinh.HorizontalAlignment = XlHAlign.xlHAlignCenter;

            Range cl5 = oSheet.get_Range("E3", "E3");
            cl5.Value2 = "Tổng Tiền";
            cl5.ColumnWidth = 15.0;
            cl5.HorizontalAlignment = XlHAlign.xlHAlignCenter;

            // Kẻ viền
            Range rowHead = oSheet.get_Range("A3", "E3");
            rowHead.Font.Bold = true;
            rowHead.Font.Name = "Time New Roman";
            rowHead.Font.Size = "12";
            rowHead.Borders.LineStyle = Constants.xlSolid;

            // Thiết lập màu nền
            rowHead.Interior.ColorIndex = 15;
            rowHead.HorizontalAlignment = XlHAlign.xlHAlignCenter;

            // Tạo mảng đối tượng để lưu dữ toàn bồ dữ liệu trong DataTable,
            // vì dữ liệu được được gán vào các Cell trong Excel phải thông qua object thuần.
            object[,] arr = new object[dataTable.Rows.Count, dataTable.Columns.Count];
            //Chuyển dữ liệu từ DataTable vào mảng đối tượng
            for (int r = 0; r < dataTable.Rows.Count; r++)
            {
                DataRow dr = dataTable.Rows[r];
                for (int c = 0; c < dataTable.Columns.Count; c++)
                {
                    arr[r, c] = dr[c];
                }
            }

            //Thiết lập vùng điền dữ liệu
            int rowStart = 4;
            int columnStart = 1;
            int rowEnd = rowStart + dataTable.Rows.Count - 1;
            int columnEnd = dataTable.Columns.Count;

            // Ô bắt đầu điền dữ liệu
            Range c1 = (Range)oSheet.Cells[rowStart, columnStart];
            // Ô kết thúc điền dữ liệu
            Range c2 = (Range)oSheet.Cells[rowEnd, columnEnd];

            // Lấy về vùng điền dữ liệu
            Range range = oSheet.get_Range(c1, c2);

            //Điền dữ liệu vào vùng đã thiết lập
            range.Value2 = arr;
            range.Font.Name = "Time New Roman";
            range.Font.Size = "12";

            // Kẻ viền
            range.Borders.LineStyle = Constants.xlSolid;

            // Căn giữa cột STT
            Range c3 = (Range)oSheet.Cells[rowEnd, columnStart];
            Range c4 = oSheet.get_Range(c1, c3);
            oSheet.get_Range(c3, c4).HorizontalAlignment = XlHAlign.xlHAlignCenter;

        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            DateTime fromDate = dtpFromDate.Value;
            DateTime toDate = dtpToDate.Value;
            string query = "Select ROW_NUMBER() over(order by BillCode asc) as STT, BillCode as N'Mã Hóa Đơn',BookingCode as N'Mã Đặt Phòng',InvoiceDate as N'Ngày Thanh Toán', TotalMoney as N'Tổng Tiền' From Bill " +
                "Where InvoiceDate between N'" + fromDate.ToString("yyyyMMdd") + "' and N'" + toDate.ToString("yyyyMMdd") + "'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);
            dgvThongKe.DataSource = dtb;
            dgvThongKe.Refresh();
        }

        private void dgvTypeRoom_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            txtTypeRoom.Text = dgvTypeRoom.Rows[i].Cells[0].Value.ToString();
            string query = "Select * From TypeRoom Where TypeRoomCode like N'"+txtTypeRoom.Text+"'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);
            DataRow r = dtb.Rows[0];
            nudCounts.Value = Decimal.Parse(r["Counts"].ToString());
            nudPrice.Value = Decimal.Parse(r["Price"].ToString());
            txtTypeRoom.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string room = txtRoom.Text;
            string type = txtTypeRoom.Text;
            string status = cbStatus.SelectedItem.ToString();
            string note = txt_Note.Text;
            SqlConnection conn = new SqlConnection(connectString);
            conn.Open();
            string query = "Update Room Set RoomCode=N'" + type + "',Status=N'" + status + "',Note = N'" + note + "' Where TypeRoomCode=N'" + type + "'";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
            loadPageRoom();
            MessageBox.Show("Sửa thành công!");

        }
    }
}
