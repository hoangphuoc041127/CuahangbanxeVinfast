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
    public partial class frmQuenMatKhau : Form
    {
        public frmQuenMatKhau()
        {
            InitializeComponent();
        }

        private void chkHien_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhauMoi.UseSystemPasswordChar = !chkHien.Checked;
            txtNhapLai.UseSystemPasswordChar = !chkHien.Checked;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void BaoLoi(string msg, Control c)
        {
            MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            c.Focus();
        }

        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            string tk = txtTaiKhoan.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (tk == "")
            { BaoLoi("Vui lòng nhập tài khoản!", txtTaiKhoan); return; }

            if (email == "" || !email.Contains("@"))
            { BaoLoi("Email không hợp lệ!", txtEmail); return; }

            if (txtMatKhauMoi.Text.Length < 6)
            { BaoLoi("Mật khẩu mới tối thiểu 6 ký tự!", txtMatKhauMoi); return; }

            if (txtMatKhauMoi.Text != txtNhapLai.Text)
            { BaoLoi("Mật khẩu nhập lại không khớp!", txtNhapLai); return; }

            try
            {
                int khop = Convert.ToInt32(DataProvider.ExecuteScalar(
                    "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap = @tk AND Email = @em",
                    new SqlParameter("@tk", tk),
                    new SqlParameter("@em", email)));

                if (khop == 0)
                {
                    BaoLoi("Tài khoản và email không khớp!", txtTaiKhoan);
                    return;
                }

                DataProvider.Execute(
                    "UPDATE TaiKhoan SET MatKhau = @mk WHERE TenDangNhap = @tk",
                    new SqlParameter("@mk", DataProvider.BamMatKhau(txtMatKhauMoi.Text)),
                    new SqlParameter("@tk", tk));

                MessageBox.Show("Đổi mật khẩu thành công! Hãy đăng nhập lại.", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}