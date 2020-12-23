using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace QuanLyKhachSanQLH
{
    public partial class fBill : Form
    {
        private string clientCode;
        private string bookingCode;
        private string name;
        private string roomCode;
        private long totalPrice;

        public fBill()
        {
            InitializeComponent();
        }

        public fBill(string clientCode, string bookingCode, string name, string roomCode, long totalPrice)
        {
            this.clientCode = clientCode;
            this.bookingCode = bookingCode;
            this.name = name;
            this.roomCode = roomCode;
            this.totalPrice = totalPrice;
            InitializeComponent();
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("Bạn có thật sự muốn thanh toán cho khách hàng: \"{0}\" ?", name), "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    String query = "Select BookingCode from Booking Where ClientCode = N'" + clientCode + "'";
                    DBAccess d = new DBAccess();
                    DataTable dtb = d.getDS(query);
                    DataRow r = dtb.Rows[0];

                    bookingCode = r["BookingCode"].ToString();


                    query = "Update Room Set Status=N'Trống' Where RoomCode = N'" + roomCode + "'";
                    d = new DBAccess();
                    dtb = d.getDS(query);

                    query = "Select Count(*) as 'SL' from Bill";
                    d = new DBAccess();
                    dtb = d.getDS(query);
                    r = dtb.Rows[0];

                    long count = Convert.ToInt64(r["SL"].ToString()) + 1;
                    DateTime now = DateTime.Now;

                    query = "Insert into Bill values(N'" + "HD00" + count + "',N'" + bookingCode + "',N'" + now.ToString("yyyy-MM-dd") + "', " + (long)totalPrice + ")";
                    d = new DBAccess();
                    dtb = d.getDS(query);

                    query = "Update Booking Set Status='true' Where BookingCode = N'" + bookingCode + "'";
                    d = new DBAccess();
                    dtb = d.getDS(query);

                    MessageBox.Show("Thanh toán thành công!");
                   
                }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi: Không Tìm Thấy Khách Hàng Được Chọn!!!");
                }
            }
        }

        private void invoice()
        {
            String query = "Select ROW_NUMBER() over(order by OrderCode asc) as STT, s.Name as N'Tên Dịch Vụ', BookingDate as N'Ngày Đặt', Amount as N'Số Lượng', (Amount * s.Price) as 'Tổng Tiền' from Service s join Orders o On o.ServiceId = s.Id " +
               "join Client c On c.ClientCode = o.ClientCode " +
               "Where c.ClientCode = N'" + this.clientCode + "'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);
            XuatExcel(dtb,"Hóa Đơn");
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
            head.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
            head.Font.Bold = true;
            head.Font.Name = "Time New Roman";
            head.Font.Size = "16";
            head.HorizontalAlignment = XlHAlign.xlHAlignCenter;

            //Mã HD
            Range title = oSheet.get_Range("A3", "A3");
            title.MergeCells = true;
            title.Value2 = "Số HD:";
            title.Font.Bold = true;
            title.Font.Name = "Time New Roman";
            title.Font.Size = "14";
            title.HorizontalAlignment = XlHAlign.xlHAlignLeft;

            Range content = oSheet.get_Range("B3", "B3");
            content.MergeCells = true;
            content.Value2 = lb_MaHD.Text;
            content.Font.Bold = true;
            content.Font.Name = "Time New Roman";
            content.Font.Size = "14";
            title.HorizontalAlignment = XlHAlign.xlHAlignLeft;

            //Họ và Tên
            title = oSheet.get_Range("A5", "A5");
            title.MergeCells = true;
            title.Value2 = "Họ và Tên:";
            title.Font.Name = "Time New Roman";
            title.Font.Size = "14";
            title.HorizontalAlignment = XlHAlign.xlHAlignLeft;

            content = oSheet.get_Range("B5", "B5");
            content.MergeCells = true;
            content.Value2 = lb_name.Text;
            content.Font.Name = "Time New Roman";
            content.Font.Size = "14";
            content.HorizontalAlignment = XlHAlign.xlHAlignLeft;

            //CMND
            title = oSheet.get_Range("A6", "A6");
            title.MergeCells = true;
            title.Value2 = "CMND:";
            title.Font.Name = "Time New Roman";
            title.Font.Size = "14";
            title.HorizontalAlignment = XlHAlign.xlHAlignLeft;

            content = oSheet.get_Range("B6", "B6");
            content.MergeCells = true;
            content.Value2 = lb_CMND.Text;
            content.Font.Name = "Time New Roman";
            content.Font.Size = "14";
            content.HorizontalAlignment = XlHAlign.xlHAlignLeft;

            //Ngày Đến
            title = oSheet.get_Range("A7", "A7");
            title.MergeCells = true;
            title.Value2 = "Ngày Đến:";
            title.Font.Name = "Time New Roman";
            title.Font.Size = "14";
            title.HorizontalAlignment = XlHAlign.xlHAlignLeft;

            content = oSheet.get_Range("B7", "B7");
            content.MergeCells = true;
            content.Value2 = dtp_FromDate.Value;
            content.Columns.NumberFormat = "dd/MM/yyyy";
            content.Font.Name = "Time New Roman";
            content.Font.Size = "14";
            content.HorizontalAlignment = XlHAlign.xlHAlignLeft;

            //Phòng Thuê
            title = oSheet.get_Range("D5", "D5");
            title.MergeCells = true;
            title.Value2 = "Phòng Thuê:";
            title.Font.Name = "Time New Roman";
            title.Font.Size = "14";
            title.HorizontalAlignment = XlHAlign.xlHAlignLeft;

            content = oSheet.get_Range("E5", "E5");
            content.MergeCells = true;
            content.Value2 = lb_Room.Text;
            content.Font.Name = "Time New Roman";
            content.Font.Size = "14";
            content.HorizontalAlignment = XlHAlign.xlHAlignLeft;

            //Số Lượng
            title = oSheet.get_Range("D6", "D6");
            title.MergeCells = true;
            title.Value2 = "Số Lượng:";
            title.Font.Name = "Time New Roman";
            title.Font.Size = "14";
            title.HorizontalAlignment = XlHAlign.xlHAlignLeft;

            content = oSheet.get_Range("E6", "E6");
            content.MergeCells = true;
            content.Value2 = lb_Amount.Text;
            content.Font.Name = "Time New Roman";
            content.Font.Size = "14";
            content.HorizontalAlignment = XlHAlign.xlHAlignLeft;

            //Ngày Đi
            title = oSheet.get_Range("D7", "D7");
            title.MergeCells = true;
            title.Value2 = "Ngày Đi:";
            title.Font.Name = "Time New Roman";
            title.Font.Size = "14";
            title.HorizontalAlignment = XlHAlign.xlHAlignLeft;

            content = oSheet.get_Range("E7", "E7");
            content.MergeCells = true;
            content.Value2 = dtp_ToDate.Value;
            content.Columns.NumberFormat = "dd/MM/yyyy";
            content.Font.Name = "Time New Roman";
            content.Font.Size = "14";
            content.HorizontalAlignment = XlHAlign.xlHAlignLeft;

            //Dịch Vụ
            title = oSheet.get_Range("A8", "A8");
            title.MergeCells = true;
            title.Value2 = "Dịch Vụ:";
            title.Font.Name = "Time New Roman";
            title.Font.Size = "14";
            title.HorizontalAlignment = XlHAlign.xlHAlignLeft;

            //Kẻ đường
            Range line = oSheet.get_Range("A4", "E4");
            line.MergeCells = true;
            line.Value2 = "_______________________________________________________________";
            line.Font.Name = "Time New Roman";
            line.Font.Size = "16";
            line.HorizontalAlignment = XlHAlign.xlHAlignCenter;

            // Tạo tiêu đề cột 
            Range cl1 = oSheet.get_Range("A10", "A10");
            cl1.Value2 = "STT";
            cl1.ColumnWidth = 15;
            cl1.HorizontalAlignment = XlHAlign.xlHAlignCenter;


            Range cl2 = oSheet.get_Range("B10", "B10");
            cl2.Value2 = "Tên Dịch Vụ";
            cl2.ColumnWidth = 20.0;
            cl1.HorizontalAlignment = XlHAlign.xlHAlignCenter;


            Range cl3 = oSheet.get_Range("C10", "C10");
            cl3.Value2 = "Ngày Đặt";
            cl3.ColumnWidth = 25.0;
            cl3.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            Range ngay_sinh = oSheet.get_Range("C10", "C10000");
            ngay_sinh.Columns.NumberFormat = "dd/mm/yyyy";
            ngay_sinh.HorizontalAlignment = XlHAlign.xlHAlignCenter;

            Range cl4 = oSheet.get_Range("D10", "D10");
            cl4.Value2 = "Số Lượng";
            cl4.ColumnWidth = 16.0;
            cl4.HorizontalAlignment = XlHAlign.xlHAlignCenter;

            Range cl5 = oSheet.get_Range("E10", "E10");
            cl5.Value2 = "Tổng Tiền";
            cl5.ColumnWidth = 30.0;
            cl5.HorizontalAlignment = XlHAlign.xlHAlignCenter;


            // Kẻ viền
            Range rowHead = oSheet.get_Range("A10", "E10");
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
            int rowStart = 11;//Hàng bắt đầu điền
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
            range.HorizontalAlignment = XlHAlign.xlHAlignCenter;

            // Kẻ viền
            range.Borders.LineStyle = Constants.xlSolid;

            // Căn giữa cột STT
            Range c3 = (Range)oSheet.Cells[rowEnd, columnStart];
            Range c4 = oSheet.get_Range(c1, c3);
            oSheet.get_Range(c3, c4).HorizontalAlignment = XlHAlign.xlHAlignCenter;

            //Tổng Tiền Dịch Vụ
            title = oSheet.get_Range("A"+ (rowEnd+2), "A" + (rowEnd + 2));
            title.MergeCells = true;
            title.Value2 = "Thành Tiền:";
            title.Font.Name = "Time New Roman";
            title.Font.Size = "14";
            title.HorizontalAlignment = XlHAlign.xlHAlignLeft;

            content = oSheet.get_Range("E" + (rowEnd + 2), "E" + (rowEnd + 2));
            content.MergeCells = true;
            content.Value2 = lb_ServiceMoney.Text;
            content.Font.Name = "Time New Roman";
            content.Font.Size = "14";
            content.HorizontalAlignment = XlHAlign.xlHAlignRight;

            //Tổng Tiền Phòng
            title = oSheet.get_Range("A" + (rowEnd + 3), "A" + (rowEnd + 3));
            title.MergeCells = true;
            title.Value2 = "Tiền Phòng:";
            title.Font.Name = "Time New Roman";
            title.Font.Size = "14";
            title.HorizontalAlignment = XlHAlign.xlHAlignLeft;

            content = oSheet.get_Range("E" + (rowEnd + 3), "E" + (rowEnd + 3));
            content.MergeCells = true;
            content.Value2 = lb_RoomMoney.Text;
            content.Font.Name = "Time New Roman";
            content.Font.Size = "14";
            content.HorizontalAlignment = XlHAlign.xlHAlignRight;

            line = oSheet.get_Range("A" + (rowEnd + 4), "E" + (rowEnd + 4));
            line.MergeCells = true;
            line.Value2 = "_______________________________________________________________";
            line.Font.Name = "Time New Roman";
            line.Font.Size = "16";
            line.HorizontalAlignment = XlHAlign.xlHAlignCenter;

            //Tiền Thanh Toán
            title = oSheet.get_Range("A" + (rowEnd + 5), "B" + (rowEnd + 5));
            title.MergeCells = true;
            title.Value2 = "Tiền Thanh Toán:";
            title.Font.Name = "Time New Roman";
            title.Font.Size = "16";
            title.HorizontalAlignment = XlHAlign.xlHAlignLeft;
            title.Font.Bold = true;
            title.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);


            content = oSheet.get_Range("E" + (rowEnd + 5), "E" + (rowEnd + 5));
            content.MergeCells = true;
            content.Value2 = lb_InvoiceMoney.Text;
            content.Font.Name = "Time New Roman";
            content.Font.Size = "16";
            content.HorizontalAlignment = XlHAlign.xlHAlignRight;
            content.Font.Bold = true;
            content.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);

        }
        private void fBill_Load(object sender, EventArgs e)
        {
            loadInfo();
            loadDGVService();
            loadMoney();
        }

        private void loadMoney()
        {
            DateTime fromDate = dtp_FromDate.Value;
            DateTime toDate = dtp_ToDate.Value;
            TimeSpan Time = toDate - fromDate;
            long totalDays = Time.Days;

            long amount = Convert.ToInt64(lb_Amount.Text);

            String query = "Select TypeRoomCode from Room Where RoomCode = N'" + this.roomCode + "'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);
            DataRow r = dtb.Rows[0];
            string typeRoom = r["TypeRoomCode"].ToString();
            query = "Select * from TypeRoom Where TypeRoomCode = N'" + typeRoom + "'";
            d = new DBAccess();
            dtb = d.getDS(query);
            r = dtb.Rows[0];

            long price = Convert.ToInt64(r["Price"].ToString());

            query = "Select sum(Amount*Price) as 'price' from Orders o join Service s On o.ServiceId=s.Id " +
               "Where ClientCode = N'" + clientCode + "'";
            d = new DBAccess();
            dtb = d.getDS(query);
            r = dtb.Rows[0];

            long servicePrice = 0;
            //Here
            if (r["price"].ToString() != "")
            {
                servicePrice = Convert.ToInt64(r["price"].ToString());
            }
            lb_ServiceMoney.Text = string.Format(new CultureInfo("vi-VN"), "{0:#,##0 đồng chẵn}", servicePrice);
            long roomPrice = totalDays * amount * price;
            lb_RoomMoney.Text = string.Format(new CultureInfo("vi-VN"), "{0:#,##0 đồng chẵn}", roomPrice);
            lb_InvoiceMoney.Text = string.Format(new CultureInfo("vi-VN"), "{0:#,##0 đồng chẵn}", this.totalPrice);
        }

        private void loadDGVService()
        {

            String query = "Select  ROW_NUMBER() over(order by OrderCode asc) as STT, s.Name, BookingDate, Amount , OrderCode, (Amount * s.Price)as 'total' from Service s join Orders o On o.ServiceId = s.Id " +
                "join Client c On c.ClientCode = o.ClientCode " +
                "Where c.ClientCode = N'" + this.clientCode + "'";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);

            dgvListServiceUsed.DataSource = dtb;
            dgvListServiceUsed.Refresh();
        }

        private void loadInfo()
        {
            string query = "Select Count(*) as 'SL' from Bill";
            DBAccess d = new DBAccess();
            DataTable dtb = d.getDS(query);
            DataRow r = dtb.Rows[0];

            long count = Convert.ToInt64(r["SL"].ToString()) + 1;
            lb_MaHD.Text = "HD00" + count;
            lb_name.Text = this.name;

            query = "Select * From Client Where ClientCode = N'" + this.clientCode + "'";
            d = new DBAccess();
            dtb = d.getDS(query);
            r = dtb.Rows[0];

            lb_CMND.Text = r["CMND"].ToString();
            query = "Select * from Booking Where ClientCode = N'" + clientCode + "'";
            d = new DBAccess();
            dtb = d.getDS(query);
            r = dtb.Rows[0];

            bookingCode = r["BookingCode"].ToString();

            lb_Amount.Text = r["Amount"].ToString();
            lb_Room.Text = this.roomCode;
            dtp_FromDate.Value = DateTime.Parse(r["ComingDate"].ToString());
            dtp_ToDate.Value = DateTime.Parse(r["LeavingDate"].ToString());
        }


        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            invoice();
        }
    }
}
