using System;
using System.Data;
using System.Windows.Forms;

namespace Quanlybanoto
{
    public partial class frmNhaCungCap : Form
    {
        public frmNhaCungCap()
        {
            InitializeComponent();

            btnThem.Click += Them_Click;
            btnSua.Click += Sua_Click;
            btnXoa.Click += Xoa_Click;
            btnThemMoi.Click += ThemMoi_Click;
            btnThoat.Click += Thoat_Click;
            dgvNCC.CellClick += Luoi_CellClick;

            TaiDuLieu("");
        }

        private void TaiDuLieu(string tuKhoa)
        {
            try
            {
                dgvNCC.DataSource = NhaCungCapDAL.LayDanhSach(tuKhoa);
                dgvNCC.ReadOnly = true;
                dgvNCC.AllowUserToAddRows = false;
                dgvNCC.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvNCC.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void LamTrong()
        {
            txtMaNCC.Clear(); txtTenNCC.Clear(); txtSDT.Clear();
            txtEmail.Clear(); txtDiaChi.Clear();
            txtMaNCC.ReadOnly = false;
            txtMaNCC.Focus();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            TaiDuLieu(txtTimKiem.Text);
        }

        private void dgvNCC_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void Luoi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow r = dgvNCC.Rows[e.RowIndex];
            txtMaNCC.Text = Convert.ToString(r.Cells[NhaCungCapDAL.COT_MA].Value);
            txtTenNCC.Text = Convert.ToString(r.Cells[NhaCungCapDAL.COT_TEN].Value);
            txtSDT.Text = Convert.ToString(r.Cells[NhaCungCapDAL.COT_SDT].Value);
            txtEmail.Text = Convert.ToString(r.Cells[NhaCungCapDAL.COT_EMAIL].Value);
            txtDiaChi.Text = Convert.ToString(r.Cells[NhaCungCapDAL.COT_DIACHI].Value);
            txtMaNCC.ReadOnly = true;   // không cho sửa mã khi đang chọn dòng
        }

        private void Them_Click(object sender, EventArgs e)
        {
            string ma = txtMaNCC.Text.Trim();
            string ten = txtTenNCC.Text.Trim();
            if (ma == "" || ten == "")
            {
                MessageBox.Show("Vui lòng nhập Mã NCC và Tên NCC.");
                return;
            }
            try
            {
                if (NhaCungCapDAL.TonTai(ma))
                {
                    MessageBox.Show("Mã NCC đã tồn tại.");
                    return;
                }
                NhaCungCapDAL.Them(ma, ten, txtSDT.Text.Trim(), txtEmail.Text.Trim(), txtDiaChi.Text.Trim());
                MessageBox.Show("Thêm thành công!");
                TaiDuLieu("");
                LamTrong();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private void Sua_Click(object sender, EventArgs e)
        {
            string ma = txtMaNCC.Text.Trim();
            if (ma == "")
            {
                MessageBox.Show("Hãy chọn một nhà cung cấp trong danh sách.");
                return;
            }
            try
            {
                int n = NhaCungCapDAL.Sua(ma, txtTenNCC.Text.Trim(), txtSDT.Text.Trim(),
                                          txtEmail.Text.Trim(), txtDiaChi.Text.Trim());
                MessageBox.Show(n > 0 ? "Sửa thành công!" : "Không tìm thấy nhà cung cấp.");
                TaiDuLieu("");
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private void Xoa_Click(object sender, EventArgs e)
        {
            string ma = txtMaNCC.Text.Trim();
            if (ma == "")
            {
                MessageBox.Show("Hãy chọn một nhà cung cấp trong danh sách.");
                return;
            }
            if (MessageBox.Show("Xóa nhà cung cấp " + ma + "?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                NhaCungCapDAL.Xoa(ma);
                MessageBox.Show("Đã xóa.");
                TaiDuLieu("");
                LamTrong();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không xóa được (có thể đang được dùng ở phiếu nhập): " + ex.Message);
            }
        }

        private void ThemMoi_Click(object sender, EventArgs e)
        {
            LamTrong();
            txtTimKiem.Clear();
            TaiDuLieu("");
        }

        private void Thoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmNhaCungCap_Load(object sender, EventArgs e)
        {

        }
    }
}