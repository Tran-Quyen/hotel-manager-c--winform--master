//using DevExpress.LookAndFeel;
using System;
using System.Data;
using System.Windows.Forms;


namespace QuanLyKhachSanQLH
{
    public partial class fLogin : Form
    {
        public static string decentralizationId = "";
        public static string Code = "";
        //public static frmMain fm;

        public fLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            login();
        }

        private void login()
        {
            string query = "Select * From Users Where Username = N'" + txtUser.Text + "' and Password = N'" + txtPass.Text + "'";
            try
            {
                DBAccess d = new DBAccess();
                DataTable dtb = d.getDS(query);
                if (dtb.Rows.Count > 0)
                {
                    DataRow q = dtb.Rows[0];
                    Code = q["Code"].ToString();
                    decentralizationId = q["DecentralizationId"].ToString();
                    if (decentralizationId != "C")
                    {
                        fMain f = new fMain(Code);
                        string sql = "Select Name From Employee Where EmployeeCode = N'" + Code + "'";
                        DataTable dt = d.getDS(sql);
                        DataRow r = dt.Rows[0];
                        MessageBox.Show("Xin chào " + r["Name"].ToString() + "!", "Xin chào", MessageBoxButtons.OK);
                        this.Hide();
                        this.Dispose(false);
                        f.ShowDialog();
                        this.Show();
                    }
                    else
                    {
                        fCustomer f = new fCustomer(Code);
                        string sql = "Select Fullname From Client Where ClientCode = N'" + Code + "'";
                        DataTable dt = d.getDS(sql);
                        DataRow r = dt.Rows[0];
                        MessageBox.Show("Xin chào " + r["Fullname"].ToString() + "!", "Xin chào", MessageBoxButtons.OK);
                        this.Hide();
                        this.Dispose(false);
                        f.ShowDialog();
                        this.Show();
                    }
                }
                else
                    MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác!","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                login();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có thật sự muốn thoát chương trình?", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void fLogin_Load(object sender, EventArgs e)
        {

        }

        private void cb_ShowHide_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_ShowHide.Checked)
            {
                txtPass.UseSystemPasswordChar = true;
            }
            else
            {
                txtPass.UseSystemPasswordChar = false;
            }
        }
    }
}
