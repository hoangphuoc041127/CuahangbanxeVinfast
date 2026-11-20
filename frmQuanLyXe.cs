using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Quanlybanoto
{
    /*
     * BẢNG Xe TRONG DATABASE (đúng như bạn đã chụp):
     *   MaXe (nvarchar, bắt buộc, nhập tay) | TenXe | HangXe | NamSX | MauSac
     *   GiaBan | SoLuong | GhiChu | HinhAnh
     *
     * TÊN (Name) CÁC CONTROL TRÊN FORM:
     *   TextBox      : txtTimKiem, txtMaXe, txtTenXe, txtNamSX, txtMauSac, txtGiaBan, txtSoLuong, txtGhiChu
     *   ComboBox     : cboLocDong
     *   PictureBox   : picAnh
     *   DataGridView : dgvXe
     *   Button       : btnTim, btnThem, btnSua, btnXoa, btnLamMoi, btnChon
     *
     * SỰ KIỆN: btnTim và dgvXe đã gắn sẵn trong Designer. Các sự kiện còn lại được gắn bằng code
     * ở hàm khởi tạo, nên KHÔNG bấm đúp vào các nút Thêm, Sửa, Xóa, Làm mới, Chọn.
     *
     * CÁCH DÙNG:
     *   - Thêm xe mới: bấm Làm mới -> nhập Mã xe và các thông tin -> bấm Thêm.
     *   - Sửa / Xóa: bấm vào dòng trong bảng (Mã xe sẽ bị khóa vì là khóa chính) -> Sửa hoặc Xóa.
     */
    public partial class frmQuanLyXe : Form
    {
        // Thư mục lưu ảnh xe, nằm cạnh file .exe (bin\Debug\Images)
        private readonly string thuMucAnh = Path.Combine(Application.StartupPath, "Images");

        private string anhMoiChon = null;      // đường dẫn ảnh vừa chọn, chưa lưu vào CSDL
        private string tenAnhHienTai = null;   // tên file ảnh đang lưu của xe được chọn
        private bool dangTaiCombo = false;     // tránh combo kích hoạt lọc khi đang nạp dữ liệu

        public frmQuanLyXe()
        {
            InitializeComponent();

            Load += frmQuanLyXe_Load;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnLamMoi.Click += btnLamMoi_Click;
            btnChon.Click += btnChon_Click;
            cboLocDong.SelectedIndexChanged += cboLocDong_SelectedIndexChanged;

            // Dùng CellClick (bấm vào bất kỳ chỗ nào của dòng) thay cho CellContentClick
            dgvXe.CellContentClick -= dgvXe_CellContentClick;
            dgvXe.CellClick += dgvXe_CellContentClick;
        }

        // ===================== KHI MỞ FORM =====================
        private void frmQuanLyXe_Load(object sender, EventArgs e)
        {
            txtTimKiem.Clear();          // bỏ chữ mẫu trong ô tìm kiếm để không lọc mất dữ liệu
            txtMaXe.ReadOnly = false;    // mã xe do người dùng tự nhập khi thêm mới
            picAnh.SizeMode = PictureBoxSizeMode.Zoom;
            TaiLaiDuLieu();
        }

        // ===================== TẢI DỮ LIỆU =====================
        // Nạp combo "Lọc dòng xe" gồm mục "Tất cả" và các tên xe đang có trong bảng
        private void LoadLocDong()
        {
            try
            {
                DataTable dt = DataProvider.GetData("SELECT DISTINCT TenXe FROM Xe ORDER BY TenXe");
                DataRow dr = dt.NewRow();
                dr["TenXe"] = "-- Tất cả --";
                dt.Rows.InsertAt(dr, 0);

                dangTaiCombo = true;
                cboLocDong.DataSource = dt;
                cboLocDong.DisplayMember = "TenXe";
                cboLocDong.ValueMember = "TenXe";
                dangTaiCombo = false;
            }
            catch (Exception ex)
            {
                dangTaiCombo = false;
                MessageBox.Show("Lỗi tải danh sách dòng xe: " + ex.Message);
            }
        }

        // Tải danh sách xe lên bảng, kết hợp từ khóa tìm kiếm (theo mã hoặc tên) và bộ lọc dòng xe
        private void LoadXe()
        {
            try
            {
                string sql = @"SELECT MaXe, TenXe, NamSX, MauSac, GiaBan, SoLuong, GhiChu, HinhAnh
                               FROM Xe WHERE (TenXe LIKE @kw OR MaXe LIKE @kw)";
                List<SqlParameter> ps = new List<SqlParameter>();
                ps.Add(new SqlParameter("@kw", "%" + txtTimKiem.Text.Trim() + "%"));

                if (cboLocDong.SelectedIndex > 0)      // vị trí 0 là "Tất cả"
                {
                    sql += " AND TenXe = @dong";
                    ps.Add(new SqlParameter("@dong", cboLocDong.Text));
                }
                sql += " ORDER BY MaXe";

                dgvXe.DataSource = DataProvider.GetData(sql, ps.ToArray());
                DinhDangLuoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void TaiLaiDuLieu()
        {
            LoadLocDong();
            LoadXe();
        }

        private void DinhDangLuoi()
        {
            dgvXe.ReadOnly = true;
            dgvXe.AllowUserToAddRows = false;
            dgvXe.MultiSelect = false;
            dgvXe.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvXe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvXe.Columns["MaXe"].HeaderText = "Mã xe";
            dgvXe.Columns["TenXe"].HeaderText = "Tên xe";
            dgvXe.Columns["NamSX"].HeaderText = "Năm SX";
            dgvXe.Columns["MauSac"].HeaderText = "Màu sắc";
            dgvXe.Columns["GiaBan"].HeaderText = "Giá bán";
            dgvXe.Columns["SoLuong"].HeaderText = "Số lượng";
            dgvXe.Columns["GhiChu"].HeaderText = "Ghi chú";
            dgvXe.Columns["GiaBan"].DefaultCellStyle.Format = "N0";
            dgvXe.Columns["HinhAnh"].Visible = false;
        }

        // ===================== NÚT TÌM VÀ LỌC =====================
        private void btnTim_Click(object sender, EventArgs e)
        {
            LoadXe();
        }

        private void cboLocDong_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dangTaiCombo) return;
            LoadXe();
        }

        // ===================== BẤM VÀO DÒNG: ĐIỀN THÔNG TIN LÊN CÁC Ô =====================
        private void dgvXe_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow r = dgvXe.Rows[e.RowIndex];

            txtMaXe.Text = r.Cells["MaXe"].Value.ToString();
            txtMaXe.ReadOnly = true;             // mã xe là khóa chính, đã lưu thì không cho đổi
            txtTenXe.Text = r.Cells["TenXe"].Value.ToString();
            txtNamSX.Text = r.Cells["NamSX"].Value.ToString();
            txtMauSac.Text = r.Cells["MauSac"].Value.ToString();
            txtGiaBan.Text = Convert.ToDecimal(r.Cells["GiaBan"].Value).ToString("0");
            txtSoLuong.Text = r.Cells["SoLuong"].Value.ToString();
            txtGhiChu.Text = r.Cells["GhiChu"].Value.ToString();

            anhMoiChon = null;
            object anh = r.Cells["HinhAnh"].Value;
            tenAnhHienTai = (anh == null || anh == DBNull.Value) ? null : anh.ToString();
            HienAnh(tenAnhHienTai);
        }

        // ===================== ẢNH =====================
        private void HienAnhTuDuongDan(string duongDan)
        {
            if (picAnh.Image != null)
            {
                picAnh.Image.Dispose();
                picAnh.Image = null;
            }
            if (string.IsNullOrEmpty(duongDan) || !File.Exists(duongDan)) return;

            try
            {
                // Sao chép vào Bitmap để không bị khóa file ảnh gốc
                using (Image img = Image.FromFile(duongDan))
                {
                    picAnh.Image = new Bitmap(img);
                }
            }
            catch
            {
                MessageBox.Show("Không đọc được file ảnh này!");
            }
        }

        private void HienAnh(string tenFile)
        {
            if (string.IsNullOrEmpty(tenFile))
                HienAnhTuDuongDan(null);
            else
                HienAnhTuDuongDan(Path.Combine(thuMucAnh, tenFile));
        }

        private void btnChon_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Chọn ảnh xe";
                dlg.Filter = "Ảnh (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    anhMoiChon = dlg.FileName;
                    HienAnhTuDuongDan(anhMoiChon);
                }
            }
        }

        // Nếu người dùng vừa chọn ảnh mới thì sao chép vào thư mục Images và trả về tên file;
        // nếu không thì giữ nguyên tên ảnh cũ
        private string LuuAnhNeuCo(string tenAnhCu)
        {
            if (string.IsNullOrEmpty(anhMoiChon)) return tenAnhCu;

            Directory.CreateDirectory(thuMucAnh);
            string tenMoi = Guid.NewGuid().ToString("N") + Path.GetExtension(anhMoiChon);
            File.Copy(anhMoiChon, Path.Combine(thuMucAnh, tenMoi));
            return tenMoi;
        }

        // ===================== KIỂM TRA DỮ LIỆU NHẬP =====================
        private bool BaoLoi(string noiDung, Control control)
        {
            MessageBox.Show(noiDung, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.Focus();
            return false;
        }

        private bool KiemTraDuLieu(out int nam, out decimal gia, out int soLuong)
        {
            nam = 0;
            gia = 0;
            soLuong = 0;

            if (txtMaXe.Text.Trim() == "")
                return BaoLoi("Vui lòng nhập mã xe!", txtMaXe);

            if (txtTenXe.Text.Trim() == "")
                return BaoLoi("Vui lòng nhập tên xe!", txtTenXe);

            if (!int.TryParse(txtNamSX.Text.Trim(), out nam) || nam < 1990 || nam > DateTime.Now.Year + 1)
                return BaoLoi("Năm sản xuất phải là số từ 1990 đến " + (DateTime.Now.Year + 1) + "!", txtNamSX);

            // Cho phép gõ kiểu 1.050.000.000 hoặc 1,050,000,000: bỏ dấu phân cách trước khi đổi sang số
            string giaChuoi = txtGiaBan.Text.Replace(".", "").Replace(",", "").Trim();
            if (!decimal.TryParse(giaChuoi, out gia) || gia < 0)
                return BaoLoi("Giá bán phải là số không âm!", txtGiaBan);

            if (!int.TryParse(txtSoLuong.Text.Trim(), out soLuong) || soLuong < 0)
                return BaoLoi("Số lượng phải là số nguyên không âm!", txtSoLuong);

            return true;
        }

        // ===================== THÊM =====================
        private void btnThem_Click(object sender, EventArgs e)
        {
            // Mã xe đang bị khóa nghĩa là đang chọn một xe có sẵn trong bảng
            if (txtMaXe.ReadOnly)
            {
                MessageBox.Show("Bạn đang chọn một xe có sẵn. Hãy bấm Làm mới rồi nhập thông tin xe mới!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int nam, soLuong;
            decimal gia;
            if (!KiemTraDuLieu(out nam, out gia, out soLuong)) return;

            string ma = txtMaXe.Text.Trim();

            try
            {
                // Mã xe đã tồn tại chưa?
                int daCo = Convert.ToInt32(DataProvider.ExecuteScalar(
                    "SELECT COUNT(*) FROM Xe WHERE MaXe = @ma",
                    new SqlParameter("@ma", ma)));
                if (daCo > 0)
                {
                    BaoLoi("Mã xe này đã tồn tại, hãy nhập mã khác!", txtMaXe);
                    return;
                }

                string tenAnh = LuuAnhNeuCo(null);

                string sql = @"INSERT INTO Xe (MaXe, TenXe, HangXe, NamSX, MauSac, GiaBan, SoLuong, GhiChu, HinhAnh)
                               VALUES (@ma, @ten, N'VinFast', @nam, @mau, @gia, @sl, @ghichu, @anh)";
                DataProvider.Execute(sql,
                    new SqlParameter("@ma", ma),
                    new SqlParameter("@ten", txtTenXe.Text.Trim()),
                    new SqlParameter("@nam", nam),
                    new SqlParameter("@mau", txtMauSac.Text.Trim()),
                    new SqlParameter("@gia", gia),
                    new SqlParameter("@sl", soLuong),
                    new SqlParameter("@ghichu", txtGhiChu.Text.Trim()),
                    new SqlParameter("@anh", (object)tenAnh ?? DBNull.Value));

                MessageBox.Show("Thêm xe thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                XoaTrang();
                TaiLaiDuLieu();
            }
            catch (SqlException ex) when (ex.Number == 8152 || ex.Number == 2628)
            {
                MessageBox.Show("Có thông tin nhập quá dài so với độ dài cột trong CSDL (thường là mã xe hoặc tên xe)!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===================== SỬA =====================
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!txtMaXe.ReadOnly || txtMaXe.Text.Trim() == "")
            {
                MessageBox.Show("Hãy bấm chọn xe cần sửa trong bảng!");
                return;
            }

            int nam, soLuong;
            decimal gia;
            if (!KiemTraDuLieu(out nam, out gia, out soLuong)) return;

            try
            {
                string tenAnh = LuuAnhNeuCo(tenAnhHienTai);

                string sql = @"UPDATE Xe SET TenXe = @ten, NamSX = @nam, MauSac = @mau, GiaBan = @gia,
                                      SoLuong = @sl, GhiChu = @ghichu, HinhAnh = @anh
                               WHERE MaXe = @ma";
                DataProvider.Execute(sql,
                    new SqlParameter("@ten", txtTenXe.Text.Trim()),
                    new SqlParameter("@nam", nam),
                    new SqlParameter("@mau", txtMauSac.Text.Trim()),
                    new SqlParameter("@gia", gia),
                    new SqlParameter("@sl", soLuong),
                    new SqlParameter("@ghichu", txtGhiChu.Text.Trim()),
                    new SqlParameter("@anh", (object)tenAnh ?? DBNull.Value),
                    new SqlParameter("@ma", txtMaXe.Text.Trim()));

                MessageBox.Show("Cập nhật thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                XoaTrang();
                TaiLaiDuLieu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===================== XÓA =====================
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (!txtMaXe.ReadOnly || txtMaXe.Text.Trim() == "")
            {
                MessageBox.Show("Hãy bấm chọn xe cần xóa trong bảng!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa xe này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                DataProvider.Execute("DELETE FROM Xe WHERE MaXe = @ma",
                    new SqlParameter("@ma", txtMaXe.Text.Trim()));

                MessageBox.Show("Đã xóa xe!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                XoaTrang();
                TaiLaiDuLieu();
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                // Lỗi khóa ngoại: xe đã có trong hóa đơn hoặc phiếu nhập
                MessageBox.Show("Không thể xóa vì xe này đã có trong hóa đơn hoặc phiếu nhập!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===================== LÀM MỚI =====================
        private void XoaTrang()
        {
            txtMaXe.Clear();
            txtMaXe.ReadOnly = false;            // mở khóa để nhập mã xe mới
            txtTenXe.Clear();
            txtNamSX.Clear();
            txtMauSac.Clear();
            txtGiaBan.Clear();
            txtSoLuong.Clear();
            txtGhiChu.Clear();
            anhMoiChon = null;
            tenAnhHienTai = null;
            HienAnhTuDuongDan(null);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            XoaTrang();
            txtTimKiem.Clear();
            TaiLaiDuLieu();
            txtMaXe.Focus();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult hoi = MessageBox.Show("Bạn có chắc muốn thoát không?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (hoi == DialogResult.Yes)
                this.Close();
        }

        private void btnLamMoi_Click_1(object sender, EventArgs e)
        {

        }
    }
}