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

namespace Quanlybanoto
{
    public partial class frmDangKy : Form
    {
        // Tài khoản vừa đăng ký, form đăng nhập có thể lấy giá trị này
        public string TaiKhoanMoi { get; private set; }

        public frmDangKy()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void BaoLoi(string msg, Control c)
        {
            MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            c.Focus();
        }

        private bool KiemTraDuLieu()
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            { BaoLoi("Vui lòng nhập họ tên!", txtHoTen); return false; }

            if (string.IsNullOrWhiteSpace(txtTaiKhoan.Text))
            { BaoLoi("Vui lòng nhập tài khoản!", txtTaiKhoan); return false; }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !txtEmail.Text.Contains("@"))
            { BaoLoi("Email không hợp lệ!", txtEmail); return false; }

            if (txtMatKhau.Text.Length < 6)
            { BaoLoi("Mật khẩu tối thiểu 6 ký tự!", txtMatKhau); return false; }

            if (txtMatKhau.Text != txtNhapLai.Text)
            { BaoLoi("Mật khẩu nhập lại không khớp!", txtNhapLai); return false; }

            return true;
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            if (!KiemTraDuLieu()) return;

            string tk = txtTaiKhoan.Text.Trim();
            string email = txtEmail.Text.Trim();

            try
            {
                // Tài khoản đã tồn tại chưa?
                int daCoTK = Convert.ToInt32(DataProvider.ExecuteScalar(
                    "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap = @tk",
                    new SqlParameter("@tk", tk)));
                if (daCoTK > 0)
                {
                    BaoLoi("Tài khoản này đã tồn tại, hãy chọn tên khác!", txtTaiKhoan);
                    return;
                }

                // Email đã được dùng chưa?
                int daCoEmail = Convert.ToInt32(DataProvider.ExecuteScalar(
                    "SELECT COUNT(*) FROM TaiKhoan WHERE Email = @em",
                    new SqlParameter("@em", email)));
                if (daCoEmail > 0)
                {
                    BaoLoi("Email này đã được đăng ký cho tài khoản khác!", txtEmail);
                    return;
                }

                // Ghi vào CSDL: mật khẩu được băm, quyền mặc định là NhanVien
                DataProvider.Execute(
                    @"INSERT INTO TaiKhoan (TenDangNhap, MatKhau, HoTen, Email, Quyen)
                      VALUES (@tk, @mk, @ten, @em, N'NhanVien')",
                    new SqlParameter("@tk", tk),
                    new SqlParameter("@mk", DataProvider.BamMatKhau(txtMatKhau.Text)),
                    new SqlParameter("@ten", txtHoTen.Text.Trim()),
                    new SqlParameter("@em", email));

                MessageBox.Show("Đăng ký thành công! Hãy đăng nhập.", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                TaiKhoanMoi = tk;
                DialogResult = DialogResult.OK;      // tự đóng form
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                BaoLoi("Tài khoản hoặc email này đã tồn tại!", txtTaiKhoan);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}