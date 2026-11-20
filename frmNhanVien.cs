using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Quanlybanoto
{
    // Form "Quản lý nhân sự".
    // Các control trong Designer phải đặt (Name) đúng:
    //   txtID, txtHoTen, dtpNgaySinh, txtDiaChi, txtEmail, rdoNam, rdoNu, txtSoDienThoai,
    //   btnThem, btnSua, btnXoa, btnReport, btnThoat,
    //   cboSapXep, txtTim, btnTim, dgvNhanVien
    public partial class frmNhanVien : Form
    {
        private const string TU_DONG = "(tự động)";

        private bool dangNap = false;
        private bool khoiTao = true;

        public frmNhanVien()
        {
            InitializeComponent();

            // ID do chương trình tự tạo, người dùng không gõ
            txtID.ReadOnly = true;

            // Giới hạn độ dài khớp với bảng NhanVien
            txtHoTen.MaxLength = 100;
            txtDiaChi.MaxLength = 200;
            txtEmail.MaxLength = 100;
            txtSoDienThoai.MaxLength = 15;

            dtpNgaySinh.Format = DateTimePickerFormat.Short;

            // Ô chọn cách sắp xếp
            cboSapXep.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSapXep.Items.Clear();
            cboSapXep.Items.AddRange(new object[]
            {
                "Mã nhân viên",
                "Họ tên (A-Z)",
                "Ngày sinh (tăng dần)",
                "Ngày sinh (giảm dần)",
                "Giới tính"
            });

            // Cấu hình bảng danh sách
            dgvNhanVien.ReadOnly = true;
            dgvNhanVien.AllowUserToAddRows = false;
            dgvNhanVien.AllowUserToDeleteRows = false;
            dgvNhanVien.MultiSelect = false;
            dgvNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNhanVien.RowHeadersVisible = false;

            // Gắn sự kiện (gỡ trước rồi gắn lại để không bị gắn đôi nếu Designer đã gắn sẵn)
            Load -= frmNhanVien_Load;
            Load += frmNhanVien_Load;

            cboSapXep.SelectedIndexChanged += (s, e) =>
            {
                if (!khoiTao) TaiDuLieu(txtTim.Text.Trim());
            };
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

            // Gỡ trước rồi gắn lại để không bị mở report hai lần nếu Designer đã gắn sẵn
            btnReport.Click -= btnReport_Click;
            btnReport.Click += btnReport_Click;

            btnThoat.Click += (s, e) => Close();
            dgvNhanVien.SelectionChanged += dgvNhanVien_SelectionChanged;
        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            cboSapXep.SelectedIndex = 0;
            khoiTao = false;
            LamMoi();
        }

        // ================= DỮ LIỆU =================
        // Cách sắp xếp lấy từ danh sách cố định (không ghép chữ người dùng vào SQL)
        private string OrderBy()
        {
            switch (cboSapXep.SelectedIndex)
            {
                case 1: return "HoTen COLLATE Vietnamese_CI_AS";
                case 2: return "NgaySinh ASC";
                case 3: return "NgaySinh DESC";
                case 4: return "GioiTinh, HoTen COLLATE Vietnamese_CI_AS";
                default: return "MaNV";
            }
        }

        private void TaiDuLieu(string tuKhoa)
        {
            try
            {
                string sql = "SELECT MaNV, HoTen, GioiTinh, NgaySinh, Email, DiaChi, SoDienThoai " +
                             "FROM NhanVien";
                DataTable dt;

                if (tuKhoa == "")
                {
                    dt = DataProvider.ExecuteQuery(sql + " ORDER BY " + OrderBy());
                }
                else
                {
                    dt = DataProvider.ExecuteQuery(
                        sql + " WHERE MaNV LIKE @k OR HoTen LIKE @k OR SoDienThoai LIKE @k " +
                              "OR Email LIKE @k OR DiaChi LIKE @k ORDER BY " + OrderBy(),
                        new SqlParameter("@k", "%" + tuKhoa + "%"));
                }

                dangNap = true;
                dgvNhanVien.DataSource = dt;
                dgvNhanVien.Columns["MaNV"].HeaderText = "ID";
                dgvNhanVien.Columns["HoTen"].HeaderText = "Họ Tên";
                dgvNhanVien.Columns["GioiTinh"].HeaderText = "Giới tính";
                dgvNhanVien.Columns["NgaySinh"].HeaderText = "Ngày sinh";
                dgvNhanVien.Columns["NgaySinh"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvNhanVien.Columns["Email"].HeaderText = "Email";
                dgvNhanVien.Columns["DiaChi"].HeaderText = "Địa chỉ";
                dgvNhanVien.Columns["SoDienThoai"].HeaderText = "Phone";
                dgvNhanVien.ClearSelection();
                dgvNhanVien.CurrentCell = null;
                dangNap = false;
            }
            catch (Exception ex)
            {
                dangNap = false;
                string goiY = (ex.Message.Contains("NhanVien") || ex.Message.Contains("Email"))
                    ? "\n\nBạn chưa chạy file CapNhatBangNhanVien.sql để tạo bảng NhanVien (có cột Email)."
                    : "";
                MessageBox.Show("Không tải được dữ liệu: " + ex.Message + goiY, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Bấm vào một dòng thì thông tin "nhảy" lên các ô nhập
        private void dgvNhanVien_SelectionChanged(object sender, EventArgs e)
        {
            if (dangNap || dgvNhanVien.CurrentRow == null) return;

            var r = dgvNhanVien.CurrentRow;
            txtID.Text = Chuoi(r.Cells["MaNV"].Value);
            txtHoTen.Text = Chuoi(r.Cells["HoTen"].Value);

            if (Chuoi(r.Cells["GioiTinh"].Value) == "Nữ") rdoNu.Checked = true;
            else rdoNam.Checked = true;

            object ns = r.Cells["NgaySinh"].Value;
            if (ns is DateTime)
            {
                DateTime t = (DateTime)ns;
                if (t < dtpNgaySinh.MinDate) t = dtpNgaySinh.MinDate;
                if (t > dtpNgaySinh.MaxDate) t = dtpNgaySinh.MaxDate;
                dtpNgaySinh.Value = t;
            }
            else
            {
                dtpNgaySinh.Value = DateTime.Today.AddYears(-25);
            }

            txtEmail.Text = Chuoi(r.Cells["Email"].Value);
            txtDiaChi.Text = Chuoi(r.Cells["DiaChi"].Value);
            txtSoDienThoai.Text = Chuoi(r.Cells["SoDienThoai"].Value);
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

        private string GioiTinh()
        {
            return rdoNu.Checked ? "Nữ" : "Nam";
        }

        // Tạo ID nhân viên kế tiếp, ví dụ NV004
        private string TaoMaMoi()
        {
            int max = Convert.ToInt32(DataProvider.ExecuteScalar(
                "SELECT ISNULL(MAX(TRY_CAST(SUBSTRING(MaNV, 3, 18) AS INT)), 0) " +
                "FROM NhanVien WHERE MaNV LIKE N'NV%'"));
            return "NV" + (max + 1).ToString("000");
        }

        private void LamMoi()
        {
            txtID.Text = TU_DONG;
            txtHoTen.Clear();
            txtEmail.Clear();
            txtDiaChi.Clear();
            txtSoDienThoai.Clear();
            rdoNam.Checked = true;
            dtpNgaySinh.Value = DateTime.Today.AddYears(-25);
            txtTim.Clear();
            TaiDuLieu("");
            txtHoTen.Focus();
        }

        private void BaoLoi(string msg, Control c)
        {
            MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            c.Focus();
        }

        private bool KiemTraDuLieu()
        {
            if (txtHoTen.Text.Trim() == "")
            { BaoLoi("Vui lòng nhập họ và tên!", txtHoTen); return false; }

            if (dtpNgaySinh.Value.Date > DateTime.Today.AddYears(-18))
            { BaoLoi("Nhân viên phải đủ 18 tuổi!", dtpNgaySinh); return false; }

            string email = txtEmail.Text.Trim();
            if (email != "" && (!email.Contains("@") || !email.Contains(".")))
            { BaoLoi("Email không hợp lệ!", txtEmail); return false; }

            string sdt = txtSoDienThoai.Text.Trim();
            if (sdt != "" && (!ToanSo(sdt) || sdt.Length < 9 || sdt.Length > 11))
            { BaoLoi("Số điện thoại phải là 9 đến 11 chữ số!", txtSoDienThoai); return false; }

            return true;
        }

        // Đang chọn một nhân viên có sẵn (ID khác "(tự động)")?
        private bool DangChonNhanVien()
        {
            string id = txtID.Text.Trim();
            return id != "" && id != TU_DONG;
        }

        // ================= CÁC NÚT =================
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!KiemTraDuLieu()) return;

            try
            {
                string ma = TaoMaMoi();

                DataProvider.Execute(
                    @"INSERT INTO NhanVien (MaNV, HoTen, GioiTinh, NgaySinh, Email, DiaChi, SoDienThoai)
                      VALUES (@ma, @ten, @gt, @ns, @em, @dc, @sdt)",
                    new SqlParameter("@ma", ma),
                    new SqlParameter("@ten", txtHoTen.Text.Trim()),
                    new SqlParameter("@gt", GioiTinh()),
                    new SqlParameter("@ns", dtpNgaySinh.Value.Date),
                    new SqlParameter("@em", GiaTri(txtEmail.Text.Trim())),
                    new SqlParameter("@dc", GiaTri(txtDiaChi.Text.Trim())),
                    new SqlParameter("@sdt", GiaTri(txtSoDienThoai.Text.Trim())));

                MessageBox.Show("Thêm nhân viên " + ma + " thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LamMoi();
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                MessageBox.Show("ID bị trùng (có người vừa thêm nhân viên). Hãy bấm Thêm lại.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!DangChonNhanVien())
            {
                BaoLoi("Hãy chọn một nhân viên trong danh sách để sửa!", dgvNhanVien);
                return;
            }
            if (!KiemTraDuLieu()) return;

            try
            {
                int soDong = DataProvider.Execute(
                    @"UPDATE NhanVien SET HoTen = @ten, GioiTinh = @gt, NgaySinh = @ns,
                             Email = @em, DiaChi = @dc, SoDienThoai = @sdt
                      WHERE MaNV = @ma",
                    new SqlParameter("@ma", txtID.Text.Trim()),
                    new SqlParameter("@ten", txtHoTen.Text.Trim()),
                    new SqlParameter("@gt", GioiTinh()),
                    new SqlParameter("@ns", dtpNgaySinh.Value.Date),
                    new SqlParameter("@em", GiaTri(txtEmail.Text.Trim())),
                    new SqlParameter("@dc", GiaTri(txtDiaChi.Text.Trim())),
                    new SqlParameter("@sdt", GiaTri(txtSoDienThoai.Text.Trim())));

                if (soDong == 0)
                {
                    BaoLoi("Không tìm thấy nhân viên này để sửa!", dgvNhanVien);
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
            if (!DangChonNhanVien())
            {
                BaoLoi("Hãy chọn một nhân viên trong danh sách để xóa!", dgvNhanVien);
                return;
            }

            string ma = txtID.Text.Trim();
            var hoi = MessageBox.Show(
                "Bạn có chắc muốn xóa nhân viên '" + ma + " - " + txtHoTen.Text.Trim() + "' không?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (hoi != DialogResult.Yes) return;

            try
            {
                int soDong = DataProvider.Execute(
                    "DELETE FROM NhanVien WHERE MaNV = @ma",
                    new SqlParameter("@ma", ma));

                if (soDong == 0)
                {
                    BaoLoi("Không tìm thấy nhân viên này!", dgvNhanVien);
                    return;
                }

                MessageBox.Show("Đã xóa nhân viên.", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LamMoi();
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                MessageBox.Show("Không thể xóa vì nhân viên này đang được dùng ở dữ liệu khác.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // In báo cáo đúng danh sách đang hiển thị (theo tìm kiếm + sắp xếp hiện tại)
        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = dgvNhanVien.DataSource as DataTable;

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để in báo cáo!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var f = new frmReportNhanVien(dt))
                {
                    f.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}