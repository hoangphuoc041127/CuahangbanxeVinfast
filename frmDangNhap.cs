using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quanlybanoto
{
    public partial class frmDangNhap : Form
    {
        public frmDangNhap()
        {
            InitializeComponent();
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

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string tk = txtTaiKhoan.Text.Trim();   // đổi cho đúng tên ô nhập tài khoản trong Designer
            string mk = txtMatKhau.Text;           // đổi cho đúng tên ô nhập mật khẩu trong Designer

            if (tk == "" || mk == "")
            {
                MessageBox.Show("Vui lòng nhập tài khoản và mật khẩu!");
                return;
            }

            // TODO: thay dòng dưới bằng kiểm tra trong CSDL của bạn
            bool hopLe = (tk == "admin" && mk == "123");

            if (hopLe)
            {
                PhienDangNhap.TaiKhoan = tk;   // lưu lại để frmMain phân quyền
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
            }
        }

        private void linkDangKy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frmDangKy frm = new frmDangKy())
            {
                frm.ShowDialog();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frmQuenMatKhau frmQuen = new frmQuenMatKhau())
            {
                frmQuen.ShowDialog();
            }
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {

        }
    }
}