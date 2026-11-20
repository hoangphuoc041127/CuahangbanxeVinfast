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
    public partial class frmDoiMatKhau : Form
    {
        public frmDoiMatKhau()
        {
            InitializeComponent();

            // Gắn sự kiện bằng code. Dùng "-=" trước "+=" để không bị gắn 2 lần
            // nếu bạn đã bấm đúp vào control trong Designer.
            chkHien.CheckedChanged -= chkHien_CheckedChanged;
            chkHien.CheckedChanged += chkHien_CheckedChanged;

            btnDoiMatKhau.Click -= btnDoiMatKhau_Click;
            btnDoiMatKhau.Click += btnDoiMatKhau_Click;
        }

        private void frmDoiMatKhau_Load(object sender, EventArgs e)
        {
            // Mặc định ẩn mật khẩu
            txtMatKhauCu.UseSystemPasswordChar = true;
            txtMatKhauMoi.UseSystemPasswordChar = true;
            txtNhapLai.UseSystemPasswordChar = true;

            AcceptButton = btnDoiMatKhau;   // nhấn Enter = Đổi mật khẩu
            CancelButton = btnHuy;          // nhấn Esc = Hủy
            btnHuy.DialogResult = DialogResult.Cancel;
        }

        private void chkHien_CheckedChanged(object sender, EventArgs e)
        {
            bool an = !chkHien.Checked;
            txtMatKhauCu.UseSystemPasswordChar = an;
            txtMatKhauMoi.UseSystemPasswordChar = an;
            txtNhapLai.UseSystemPasswordChar = an;
        }

        private void BaoLoi(string msg, Control c)
        {
            MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            c.Focus();
        }

        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            string tk = txtTaiKhoan.Text.Trim();

            if (tk == "")
            { BaoLoi("Vui lòng nhập tài khoản!", txtTaiKhoan); return; }

            if (txtMatKhauCu.Text == "")
            { BaoLoi("Vui lòng nhập mật khẩu cũ!", txtMatKhauCu); return; }

            if (txtMatKhauMoi.Text.Length < 6)
            { BaoLoi("Mật khẩu mới tối thiểu 6 ký tự!", txtMatKhauMoi); return; }

            if (txtMatKhauMoi.Text != txtNhapLai.Text)
            { BaoLoi("Mật khẩu nhập lại không khớp!", txtNhapLai); return; }

            if (txtMatKhauMoi.Text == txtMatKhauCu.Text)
            { BaoLoi("Mật khẩu mới phải khác mật khẩu cũ!", txtMatKhauMoi); return; }

            try
            {
                // Kiểm tra tài khoản + mật khẩu cũ có đúng không
                int dung = Convert.ToInt32(DataProvider.ExecuteScalar(
                    "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap = @tk AND MatKhau = @mk",
                    new SqlParameter("@tk", tk),
                    new SqlParameter("@mk", DataProvider.BamMatKhau(txtMatKhauCu.Text))));

                if (dung == 0)
                {
                    BaoLoi("Tài khoản hoặc mật khẩu cũ không đúng!", txtMatKhauCu);
                    return;
                }

                DataProvider.Execute(
                    "UPDATE TaiKhoan SET MatKhau = @mk WHERE TenDangNhap = @tk",
                    new SqlParameter("@mk", DataProvider.BamMatKhau(txtMatKhauMoi.Text)),
                    new SqlParameter("@tk", tk));

                MessageBox.Show("Đổi mật khẩu thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}