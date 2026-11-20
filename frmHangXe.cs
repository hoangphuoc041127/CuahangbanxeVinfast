using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Quanlybanoto
{
    public partial class frmHangXe : Form
    {
        public frmHangXe()
        {
            InitializeComponent();
        }

        private void frmHangXe_Load(object sender, EventArgs e)
        {
            txtMaHang.ReadOnly = true;
            LoadData();
        }

        void LoadData()
        {
            dgvHangXe.DataSource = DataProvider.ExecuteQuery(
                "SELECT MaHang, TenHang, QuocGia FROM HangXe");
            txtMaHang.Clear();
            txtTenHang.Clear();
            txtQuocGia.Clear();
        }

        private void dgvHangXe_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvHangXe.Rows[e.RowIndex];
            txtMaHang.Text = row.Cells["MaHang"].Value.ToString();
            txtTenHang.Text = row.Cells["TenHang"].Value.ToString();
            txtQuocGia.Text = row.Cells["QuocGia"].Value.ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenHang.Text))
            {
                MessageBox.Show("Vui lòng nhập tên hãng!");
                return;
            }
            try
            {
                DataProvider.Execute(
                    "INSERT INTO HangXe(TenHang, QuocGia) VALUES (@ten, @qg)",
                    new SqlParameter("@ten", txtTenHang.Text.Trim()),
                    new SqlParameter("@qg", txtQuocGia.Text.Trim()));
                MessageBox.Show("Thêm thành công!");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaHang.Text == "")
            {
                MessageBox.Show("Hãy chọn hãng cần sửa!");
                return;
            }
            try
            {
                DataProvider.Execute(
                    "UPDATE HangXe SET TenHang=@ten, QuocGia=@qg WHERE MaHang=@ma",
                    new SqlParameter("@ten", txtTenHang.Text.Trim()),
                    new SqlParameter("@qg", txtQuocGia.Text.Trim()),
                    new SqlParameter("@ma", txtMaHang.Text));
                MessageBox.Show("Sửa thành công!");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaHang.Text == "")
            {
                MessageBox.Show("Hãy chọn hãng cần xóa!");
                return;
            }
            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận",
                MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            try
            {
                DataProvider.Execute("DELETE FROM HangXe WHERE MaHang=@ma",
                    new SqlParameter("@ma", txtMaHang.Text));
                MessageBox.Show("Xóa thành công!");
                LoadData();
            }
            catch
            {
                MessageBox.Show("Không xóa được, hãng này đang được xe sử dụng!");
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        // Nút Thoát
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult kq = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát không?",
                "Thông báo",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (kq == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}