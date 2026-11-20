using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Quanlybanoto
{
    // Code-behind cho form Khách hàng dựng bằng Designer.
    // Mọi sự kiện được gắn bằng code trong hàm khởi tạo, nên KHÔNG cần
    // nhấp đúp vào nút nào trong Designer. Chỉ cần đặt (Name) đúng:
    //   txtTim, btnTim, txtMaKH, txtHoTen, txtSoDienThoai, txtCCCD, txtEmail,
    //   txtDiaChi, btnThem, btnSua, btnXoa, btnLamMoi, dgvKhachHang
    public partial class frmKhachHang : Form
    {
        private bool dangNap = false;

        public frmKhachHang()
        {
            InitializeComponent();

            // Giới hạn độ dài khớp với bảng KhachHang
            txtMaKH.MaxLength = 20;
            txtHoTen.MaxLength = 100;
            txtSoDienThoai.MaxLength = 15;
            txtCCCD.MaxLength = 12;
            txtEmail.MaxLength = 100;
            txtDiaChi.MaxLength = 200;

            // Cấu hình bảng danh sách
            dgvKhachHang.ReadOnly = true;
            dgvKhachHang.AllowUserToAddRows = false;
            dgvKhachHang.AllowUserToDeleteRows = false;
            dgvKhachHang.MultiSelect = false;
            dgvKhachHang.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKhachHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvKhachHang.RowHeadersVisible = false;

            // Gắn sự kiện
            Load += (s, e) => { TaiDuLieu(""); TaoMaMoi(); };
            btnTim.Click += (s, e) => TaiDuLieu(txtTim.Text.Trim());
            txtTim.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    TaiDuLieu(txtTim.Text.Trim());
                    e.SuppressKeyPress = true;
                }
            };
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnLamMoi.Click += (s, e) => LamMoi();
            dgvKhachHang.SelectionChanged += dgvKhachHang_SelectionChanged;
        }

        // ================= DỮ LIỆU =================
        private void TaiDuLieu(string tuKhoa)
        {
            try
            {
                string sql = "SELECT MaKH, HoTen, SoDienThoai, CCCD, DiaChi, Email FROM KhachHang";
                DataTable dt;

                if (tuKhoa == "")
                {
                    dt = DataProvider.ExecuteQuery(sql + " ORDER BY MaKH");
                }
                else
                {
                    dt = DataProvider.ExecuteQuery(
                        sql + " WHERE MaKH LIKE @k OR HoTen LIKE @k OR SoDienThoai LIKE @k OR CCCD LIKE @k ORDER BY MaKH",
                        new SqlParameter("@k", "%" + tuKhoa + "%"));
                }

                dangNap = true;
                dgvKhachHang.DataSource = dt;
                dgvKhachHang.Columns["MaKH"].HeaderText = "Mã KH";
                dgvKhachHang.Columns["HoTen"].HeaderText = "Họ tên";
                dgvKhachHang.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
                dgvKhachHang.Columns["CCCD"].HeaderText = "CCCD";
                dgvKhachHang.Columns["DiaChi"].HeaderText = "Địa chỉ";
                dgvKhachHang.Columns["Email"].HeaderText = "Email";
                dgvKhachHang.ClearSelection();
                dgvKhachHang.CurrentCell = null;
                dangNap = false;
            }
            catch (Exception ex)
            {
                dangNap = false;
                MessageBox.Show("Không tải được dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Bấm vào một dòng thì thông tin "nhảy" lên các ô nhập
        private void dgvKhachHang_SelectionChanged(object sender, EventArgs e)
        {
            if (dangNap || dgvKhachHang.CurrentRow == null) return;

            var r = dgvKhachHang.CurrentRow;
            txtMaKH.Text = Chuoi(r.Cells["MaKH"].Value);
            txtHoTen.Text = Chuoi(r.Cells["HoTen"].Value);
            txtSoDienThoai.Text = Chuoi(r.Cells["SoDienThoai"].Value);
            txtCCCD.Text = Chuoi(r.Cells["CCCD"].Value);
            txtDiaChi.Text = Chuoi(r.Cells["DiaChi"].Value);
            txtEmail.Text = Chuoi(r.Cells["Email"].Value);
        }

        private static string Chuoi(object o)
        {
            return (o == null || o == DBNull.Value) ? "" : o.ToString();
        }

        // Ô để trống thì lưu NULL vào CSDL
        private static object GiaTri(string s)
        {
            return s == "" ? (object)DBNull.Value : s;
        }

        private static bool ToanSo(string s)
        {
            foreach (char c in s)
                if (c < '0' || c > '9') return false;
            return true;
        }

        // Gợi ý mã khách hàng kế tiếp, ví dụ KH004
        private void TaoMaMoi()
        {
            try
            {
                int max = Convert.ToInt32(DataProvider.ExecuteScalar(
                    "SELECT ISNULL(MAX(TRY_CAST(SUBSTRING(MaKH, 3, 18) AS INT)), 0) " +
                    "FROM KhachHang WHERE MaKH LIKE N'KH%'"));
                txtMaKH.Text = "KH" + (max + 1).ToString("000");
            }
            catch { /* không tạo được mã thì để trống, người dùng tự nhập */ }
        }

        private void LamMoi()
        {
            txtMaKH.Clear();
            txtHoTen.Clear();
            txtSoDienThoai.Clear();
            txtCCCD.Clear();
            txtDiaChi.Clear();
            txtEmail.Clear();
            txtTim.Clear();
            TaiDuLieu("");
            TaoMaMoi();
            txtHoTen.Focus();
        }

        private void BaoLoi(string msg, Control c)
        {
            MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            c.Focus();
        }

        private bool KiemTraDuLieu()
        {
            if (txtMaKH.Text.Trim() == "")
            { BaoLoi("Vui lòng nhập mã khách hàng!", txtMaKH); return false; }

            if (txtHoTen.Text.Trim() == "")
            { BaoLoi("Vui lòng nhập họ tên!", txtHoTen); return false; }

            string sdt = txtSoDienThoai.Text.Trim();
            if (sdt != "" && (!ToanSo(sdt) || sdt.Length < 9 || sdt.Length > 11))
            { BaoLoi("Số điện thoại phải là 9 đến 11 chữ số!", txtSoDienThoai); return false; }

            string cccd = txtCCCD.Text.Trim();
            if (cccd != "" && (!ToanSo(cccd) || cccd.Length != 12))
            { BaoLoi("CCCD phải gồm đúng 12 chữ số!", txtCCCD); return false; }

            string email = txtEmail.Text.Trim();
            if (email != "" && (!email.Contains("@") || !email.Contains(".")))
            { BaoLoi("Email không hợp lệ!", txtEmail); return false; }

            return true;
        }

        // ================= CÁC NÚT =================
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!KiemTraDuLieu()) return;

            string ma = txtMaKH.Text.Trim();
            string cccd = txtCCCD.Text.Trim();

            try
            {
                int daCo = Convert.ToInt32(DataProvider.ExecuteScalar(
                    "SELECT COUNT(*) FROM KhachHang WHERE MaKH = @ma",
                    new SqlParameter("@ma", ma)));
                if (daCo > 0)
                {
                    BaoLoi("Mã khách hàng này đã tồn tại!", txtMaKH);
                    return;
                }

                if (cccd != "")
                {
                    int trung = Convert.ToInt32(DataProvider.ExecuteScalar(
                        "SELECT COUNT(*) FROM KhachHang WHERE CCCD = @c",
                        new SqlParameter("@c", cccd)));
                    if (trung > 0)
                    {
                        BaoLoi("CCCD này đã được đăng ký cho khách hàng khác!", txtCCCD);
                        return;
                    }
                }

                DataProvider.Execute(
                    @"INSERT INTO KhachHang (MaKH, HoTen, SoDienThoai, CCCD, DiaChi, Email)
                      VALUES (@ma, @ten, @sdt, @cccd, @dc, @em)",
                    new SqlParameter("@ma", ma),
                    new SqlParameter("@ten", txtHoTen.Text.Trim()),
                    new SqlParameter("@sdt", GiaTri(txtSoDienThoai.Text.Trim())),
                    new SqlParameter("@cccd", GiaTri(cccd)),
                    new SqlParameter("@dc", GiaTri(txtDiaChi.Text.Trim())),
                    new SqlParameter("@em", GiaTri(txtEmail.Text.Trim())));

                MessageBox.Show("Thêm khách hàng thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LamMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!KiemTraDuLieu()) return;

            string ma = txtMaKH.Text.Trim();
            string cccd = txtCCCD.Text.Trim();

            try
            {
                if (cccd != "")
                {
                    int trung = Convert.ToInt32(DataProvider.ExecuteScalar(
                        "SELECT COUNT(*) FROM KhachHang WHERE CCCD = @c AND MaKH <> @ma",
                        new SqlParameter("@c", cccd),
                        new SqlParameter("@ma", ma)));
                    if (trung > 0)
                    {
                        BaoLoi("CCCD này đã được đăng ký cho khách hàng khác!", txtCCCD);
                        return;
                    }
                }

                int soDong = DataProvider.Execute(
                    @"UPDATE KhachHang SET HoTen = @ten, SoDienThoai = @sdt, CCCD = @cccd,
                             DiaChi = @dc, Email = @em
                      WHERE MaKH = @ma",
                    new SqlParameter("@ma", ma),
                    new SqlParameter("@ten", txtHoTen.Text.Trim()),
                    new SqlParameter("@sdt", GiaTri(txtSoDienThoai.Text.Trim())),
                    new SqlParameter("@cccd", GiaTri(cccd)),
                    new SqlParameter("@dc", GiaTri(txtDiaChi.Text.Trim())),
                    new SqlParameter("@em", GiaTri(txtEmail.Text.Trim())));

                if (soDong == 0)
                {
                    BaoLoi("Không tìm thấy khách hàng có mã này để sửa!", txtMaKH);
                    return;
                }

                MessageBox.Show("Cập nhật thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LamMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string ma = txtMaKH.Text.Trim();
            if (ma == "")
            {
                BaoLoi("Hãy chọn một khách hàng trong danh sách để xóa!", txtMaKH);
                return;
            }

            var hoi = MessageBox.Show("Bạn có chắc muốn xóa khách hàng '" + ma + "' không?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (hoi != DialogResult.Yes) return;

            try
            {
                int soDong = DataProvider.Execute(
                    "DELETE FROM KhachHang WHERE MaKH = @ma",
                    new SqlParameter("@ma", ma));

                if (soDong == 0)
                {
                    BaoLoi("Không tìm thấy khách hàng có mã này!", txtMaKH);
                    return;
                }

                MessageBox.Show("Đã xóa khách hàng.", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LamMoi();
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                MessageBox.Show("Không thể xóa vì khách hàng này đã có hóa đơn.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hai hàm trống dưới do Designer đã nối sự kiện, giữ lại để không bị lỗi build
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult hoi = MessageBox.Show("Bạn có chắc muốn thoát không?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (hoi == DialogResult.Yes)
                this.Close();
        }

        private void frmKhachHang_Load(object sender, EventArgs e)
        {

        }
    }
}