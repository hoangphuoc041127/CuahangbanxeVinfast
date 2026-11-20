using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Quanlybanoto
{
    /*
     * FORM LẬP HÓA ĐƠN BÁN XE (kéo thả control trong Designer)
     *
     * TÊN (Name) CÁC CONTROL - phải đặt đúng như sau:
     *   TextBox        : txtMaHD, txtNguoiLap, txtDonGia, txtGhiChu, txtTongTien
     *   DateTimePicker : dtpNgayLap
     *   ComboBox       : cboKhachHang, cboXe
     *   NumericUpDown  : numSoLuong
     *   DataGridView   : dgvChiTiet
     *   Button         : btnThemXe, btnBoXe, btnSua, btnLuu, btnLamMoi, btnThoat
     *
     * Sự kiện được gắn bằng code ở hàm khởi tạo (gỡ rồi gắn lại nên không bị chạy 2 lần).
     * Đừng bấm đúp vào các control nữa.
     */
    public partial class frmTraCuuHoaDon : Form
    {
        private DataTable gio;                                   // giỏ hàng: các xe đã chọn cho hóa đơn
        private Dictionary<string, int> tonKho = new Dictionary<string, int>();   // mã xe -> tồn kho

        public frmTraCuuHoaDon()
        {
            InitializeComponent();

            txtMaHD.ReadOnly = true;
            txtNguoiLap.ReadOnly = true;
            txtDonGia.ReadOnly = true;
            txtTongTien.ReadOnly = true;
            txtTongTien.TextAlign = HorizontalAlignment.Right;
            txtGhiChu.MaxLength = 200;

            btnThemXe.Text = "Thêm xe vào hóa đơn";
            btnBoXe.Text = "Bỏ xe đã chọn";

            cboKhachHang.DropDownStyle = ComboBoxStyle.DropDownList;
            cboXe.DropDownStyle = ComboBoxStyle.DropDownList;

            dtpNgayLap.Format = DateTimePickerFormat.Custom;
            dtpNgayLap.CustomFormat = "dd/MM/yyyy HH:mm";

            numSoLuong.Minimum = 1;
            numSoLuong.Maximum = 1000;
            numSoLuong.Value = 1;

            // Bảng tạm chứa các xe đã chọn
            gio = new DataTable();
            gio.Columns.Add("MaXe", typeof(string));
            gio.Columns.Add("TenXe", typeof(string));
            gio.Columns.Add("SoLuong", typeof(int));
            gio.Columns.Add("DonGia", typeof(decimal));
            gio.Columns.Add("ThanhTien", typeof(decimal));

            Load -= frmLapHoaDon_Load; Load += frmLapHoaDon_Load;
            cboXe.SelectedIndexChanged -= cboXe_SelectedIndexChanged;
            cboXe.SelectedIndexChanged += cboXe_SelectedIndexChanged;
            btnThemXe.Click -= btnThemXe_Click; btnThemXe.Click += btnThemXe_Click;
            btnBoXe.Click -= btnBoXe_Click; btnBoXe.Click += btnBoXe_Click;
            btnSua.Click -= btnSua_Click; btnSua.Click += btnSua_Click;
            btnLuu.Click -= btnLuu_Click; btnLuu.Click += btnLuu_Click;
            btnLamMoi.Click -= btnLamMoi_Click; btnLamMoi.Click += btnLamMoi_Click;
            btnThoat.Click -= btnThoat_Click; btnThoat.Click += btnThoat_Click;
        }

        // Hàm do bấm đúp vào Label trong Designer sinh ra. Để trống nhưng PHẢI GIỮ LẠI
        // vì file Designer đang gắn vào nó; xóa là báo lỗi build.
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmLapHoaDon_Load(object sender, EventArgs e)
        {
            CauHinhBang();
            TaiDanhSach();
            HoaDonMoi();
        }

        private void cboXe_SelectedIndexChanged(object sender, EventArgs e)
        {
            HienDonGia();
        }

        // ================= GIAO DIỆN / DỮ LIỆU =================
        private void CauHinhBang()
        {
            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.AllowUserToAddRows = false;
            dgvChiTiet.AllowUserToDeleteRows = false;
            dgvChiTiet.MultiSelect = false;
            dgvChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTiet.RowHeadersVisible = false;
            dgvChiTiet.DataSource = gio;

            dgvChiTiet.Columns["MaXe"].HeaderText = "Mã xe";
            dgvChiTiet.Columns["TenXe"].HeaderText = "Tên xe";
            dgvChiTiet.Columns["SoLuong"].HeaderText = "Số lượng";
            dgvChiTiet.Columns["DonGia"].HeaderText = "Đơn giá";
            dgvChiTiet.Columns["DonGia"].DefaultCellStyle.Format = "N0";
            dgvChiTiet.Columns["ThanhTien"].HeaderText = "Thành tiền";
            dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
        }

        // Nạp danh sách khách hàng và xe còn hàng vào hai ô chọn
        private void TaiDanhSach()
        {
            try
            {
                DataTable kh = DataProvider.ExecuteQuery(
                    "SELECT MaKH, MaKH + N' - ' + HoTen AS Hien FROM KhachHang ORDER BY MaKH");
                cboKhachHang.DisplayMember = "Hien";
                cboKhachHang.ValueMember = "MaKH";
                cboKhachHang.DataSource = kh;
                cboKhachHang.SelectedIndex = -1;

                DataTable xe = DataProvider.ExecuteQuery(
                    @"SELECT MaXe, TenXe, GiaBan, SoLuong,
                             MaXe + N' - ' + TenXe + N' (' + ISNULL(MauSac, N'') + N') - '
                                  + CASE WHEN SoLuong > 0
                                         THEN N'còn ' + CAST(SoLuong AS NVARCHAR(10))
                                         ELSE N'HẾT HÀNG' END AS Hien
                      FROM Xe
                      WHERE HangXe = N'VinFast'
                      ORDER BY MaXe");

                tonKho.Clear();
                foreach (DataRow r in xe.Rows)
                    tonKho[r["MaXe"].ToString()] = Convert.ToInt32(r["SoLuong"]);

                cboXe.DisplayMember = "Hien";
                cboXe.ValueMember = "MaXe";
                cboXe.DataSource = xe;
                cboXe.SelectedIndex = -1;
                txtDonGia.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataRowView XeDangChon()
        {
            if (cboXe.SelectedIndex < 0) return null;
            return cboXe.SelectedItem as DataRowView;
        }

        private void HienDonGia()
        {
            DataRowView r = XeDangChon();
            if (r == null) { txtDonGia.Clear(); return; }
            txtDonGia.Text = Convert.ToDecimal(r["GiaBan"]).ToString("N0");
        }

        private decimal TongTien()
        {
            decimal t = 0;
            foreach (DataRow d in gio.Rows) t += (decimal)d["ThanhTien"];
            return t;
        }

        private void CapNhatTong()
        {
            txtTongTien.Text = TongTien().ToString("N0");
        }

        // Gợi ý mã hóa đơn kế tiếp, ví dụ HD0004
        private void TaoMaMoi()
        {
            try
            {
                int max = Convert.ToInt32(DataProvider.ExecuteScalar(
                    "SELECT ISNULL(MAX(TRY_CAST(SUBSTRING(MaHD, 3, 18) AS INT)), 0) " +
                    "FROM HoaDon WHERE MaHD LIKE N'HD%'"));
                txtMaHD.Text = "HD" + (max + 1).ToString("0000");
            }
            catch { txtMaHD.Clear(); }
        }

        // Bắt đầu một hóa đơn trống
        private void HoaDonMoi()
        {
            gio.Rows.Clear();
            CapNhatTong();
            dtpNgayLap.Value = DateTime.Now;
            txtGhiChu.Clear();
            numSoLuong.Value = 1;
            txtNguoiLap.Text = string.IsNullOrEmpty(PhienLamViec.TenDangNhap)
                ? "(chưa đăng nhập)" : PhienLamViec.TenDangNhap;
            TaoMaMoi();
            cboKhachHang.SelectedIndex = -1;
            cboXe.SelectedIndex = -1;
            txtDonGia.Clear();
        }

        private void BaoLoi(string msg, Control c)
        {
            MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            c.Focus();
        }

        // ================= CÁC NÚT =================
        private void btnThemXe_Click(object sender, EventArgs e)
        {
            DataRowView r = XeDangChon();
            if (r == null)
            { BaoLoi("Hãy chọn một xe!", cboXe); return; }

            string ma = r["MaXe"].ToString();
            string ten = r["TenXe"].ToString();
            decimal gia = Convert.ToDecimal(r["GiaBan"]);
            int ton = Convert.ToInt32(r["SoLuong"]);
            int sl = (int)numSoLuong.Value;

            if (ton <= 0)
            {
                MessageBox.Show("Xe " + ten + " đã HẾT HÀNG, không thể bán!\nHãy nhập thêm xe ở mục Hóa đơn → Nhập xe.",
                    "Hết hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboXe.Focus();
                return;
            }

            // Xe này đã có trong hóa đơn chưa?
            DataRow dong = null;
            foreach (DataRow d in gio.Rows)
            {
                if (d["MaXe"].ToString() == ma) { dong = d; break; }
            }
            int daChon = (dong == null) ? 0 : (int)dong["SoLuong"];

            if (daChon + sl > ton)
            {
                BaoLoi("Không đủ hàng! Trong kho chỉ còn " + ton + " xe " + ten +
                       " (hóa đơn đã chọn " + daChon + ").", numSoLuong);
                return;
            }

            if (dong == null)
            {
                gio.Rows.Add(ma, ten, sl, gia, gia * sl);
            }
            else
            {
                dong["SoLuong"] = daChon + sl;
                dong["ThanhTien"] = gia * (daChon + sl);
            }

            numSoLuong.Value = 1;
            CapNhatTong();
        }

        private void btnBoXe_Click(object sender, EventArgs e)
        {
            if (dgvChiTiet.CurrentRow == null)
            {
                MessageBox.Show("Hãy chọn một dòng xe trong hóa đơn để bỏ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataRowView drv = dgvChiTiet.CurrentRow.DataBoundItem as DataRowView;
            if (drv == null) return;

            gio.Rows.Remove(drv.Row);
            CapNhatTong();
        }

        // SỬA: đổi số lượng của dòng xe đang chọn trong hóa đơn
        private void btnSua_Click(object sender, EventArgs e)
        {
            DataRowView drv = dgvChiTiet.CurrentRow == null
                ? null : dgvChiTiet.CurrentRow.DataBoundItem as DataRowView;
            if (drv == null)
            {
                MessageBox.Show("Hãy chọn một dòng xe trong hóa đơn để sửa số lượng!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string ma = drv["MaXe"].ToString();
            int hienTai = (int)drv["SoLuong"];
            int ton;
            if (!tonKho.TryGetValue(ma, out ton)) ton = hienTai;

            int moi;
            if (!HoiSoLuong(drv["TenXe"].ToString(), hienTai, ton, out moi)) return;

            decimal gia = (decimal)drv["DonGia"];
            drv["SoLuong"] = moi;
            drv["ThanhTien"] = gia * moi;
            CapNhatTong();
        }

        // Hộp thoại nhỏ để nhập số lượng mới (dựng bằng code)
        private bool HoiSoLuong(string tenXe, int hienTai, int toiDa, out int ketQua)
        {
            ketQua = hienTai;
            int max = Math.Max(1, toiDa);

            using (Form f = new Form())
            {
                f.Text = "Sửa số lượng";
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterParent;
                f.MaximizeBox = false;
                f.MinimizeBox = false;
                f.ClientSize = new Size(340, 130);

                Label lb = new Label();
                lb.Text = "Số lượng mới cho xe " + tenXe + " (tối đa " + max + "):";
                lb.Left = 15; lb.Top = 15; lb.Width = 310; lb.Height = 36;
                f.Controls.Add(lb);

                NumericUpDown num = new NumericUpDown();
                num.Minimum = 1;
                num.Maximum = max;
                num.Value = Math.Min(hienTai, max);
                num.Left = 15; num.Top = 58; num.Width = 100;
                f.Controls.Add(num);

                Button ok = new Button();
                ok.Text = "Đồng ý"; ok.DialogResult = DialogResult.OK;
                ok.Left = 150; ok.Top = 88; ok.Width = 80;
                f.Controls.Add(ok);

                Button huy = new Button();
                huy.Text = "Hủy"; huy.DialogResult = DialogResult.Cancel;
                huy.Left = 240; huy.Top = 88; huy.Width = 80;
                f.Controls.Add(huy);

                f.AcceptButton = ok;
                f.CancelButton = huy;

                if (f.ShowDialog(this) != DialogResult.OK) return false;
                ketQua = (int)num.Value;
                return true;
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            if (gio.Rows.Count > 0)
            {
                var hoi = MessageBox.Show("Hóa đơn đang lập sẽ bị xóa. Tiếp tục?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (hoi != DialogResult.Yes) return;
            }

            TaiDanhSach();
            HoaDonMoi();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (gio.Rows.Count > 0)
            {
                var hoi = MessageBox.Show("Hóa đơn đang lập chưa lưu sẽ bị mất. Bạn có chắc muốn thoát?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (hoi != DialogResult.Yes) return;
            }
            Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (cboKhachHang.SelectedIndex < 0 || cboKhachHang.SelectedValue == null)
            { BaoLoi("Hãy chọn khách hàng!", cboKhachHang); return; }

            if (gio.Rows.Count == 0)
            { BaoLoi("Hóa đơn chưa có xe nào!", cboXe); return; }

            string maHD = txtMaHD.Text.Trim();
            if (maHD == "")
            { BaoLoi("Chưa có mã hóa đơn, hãy bấm Hóa đơn mới.", btnLamMoi); return; }

            string maKH = cboKhachHang.SelectedValue.ToString();
            decimal tong = TongTien();
            object nguoiLap = string.IsNullOrEmpty(PhienLamViec.TenDangNhap)
                ? (object)DBNull.Value : PhienLamViec.TenDangNhap;
            object ghiChu = string.IsNullOrWhiteSpace(txtGhiChu.Text)
                ? (object)DBNull.Value : txtGhiChu.Text.Trim();

            try
            {
                using (SqlConnection conn = DataProvider.TaoKetNoi())
                {
                    conn.Open();
                    using (SqlTransaction tran = conn.BeginTransaction())
                    {
                        try
                        {
                            // 1) Tạo hóa đơn
                            using (var cmd = new SqlCommand(
                                @"INSERT INTO HoaDon (MaHD, NgayLap, MaKH, NguoiLap, TongTien, GhiChu)
                                  VALUES (@ma, @ngay, @kh, @nl, @tong, @gc)", conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@ma", maHD);
                                cmd.Parameters.AddWithValue("@ngay", dtpNgayLap.Value);
                                cmd.Parameters.AddWithValue("@kh", maKH);
                                cmd.Parameters.AddWithValue("@nl", nguoiLap);
                                cmd.Parameters.AddWithValue("@tong", tong);
                                cmd.Parameters.AddWithValue("@gc", ghiChu);
                                cmd.ExecuteNonQuery();
                            }

                            // 2) Với mỗi xe: trừ tồn kho (chỉ trừ được khi còn đủ) và ghi chi tiết
                            foreach (DataRow d in gio.Rows)
                            {
                                string maXe = d["MaXe"].ToString();
                                int sl = (int)d["SoLuong"];
                                decimal donGia = (decimal)d["DonGia"];

                                int soDong;
                                using (var cmd = new SqlCommand(
                                    "UPDATE Xe SET SoLuong = SoLuong - @sl WHERE MaXe = @xe AND SoLuong >= @sl",
                                    conn, tran))
                                {
                                    cmd.Parameters.AddWithValue("@sl", sl);
                                    cmd.Parameters.AddWithValue("@xe", maXe);
                                    soDong = cmd.ExecuteNonQuery();
                                }

                                if (soDong == 0)
                                    throw new InvalidOperationException(
                                        "Xe '" + maXe + "' không còn đủ số lượng trong kho!");

                                using (var cmd = new SqlCommand(
                                    @"INSERT INTO ChiTietHoaDon (MaHD, MaXe, SoLuong, DonGia)
                                      VALUES (@ma, @xe, @sl, @gia)", conn, tran))
                                {
                                    cmd.Parameters.AddWithValue("@ma", maHD);
                                    cmd.Parameters.AddWithValue("@xe", maXe);
                                    cmd.Parameters.AddWithValue("@sl", sl);
                                    cmd.Parameters.AddWithValue("@gia", donGia);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            tran.Commit();
                        }
                        catch
                        {
                            tran.Rollback();   // có lỗi thì hủy hết, không lưu nửa chừng
                            throw;
                        }
                    }
                }

                MessageBox.Show("Lưu hóa đơn " + maHD + " thành công!\nTổng tiền: " +
                                tong.ToString("N0") + " đ", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                TaiDanhSach();   // nạp lại để thấy tồn kho mới
                HoaDonMoi();
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                TaoMaMoi();
                MessageBox.Show("Mã hóa đơn bị trùng (có người vừa lập hóa đơn). " +
                                "Đã tạo mã mới, hãy bấm Lưu lại.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message + "\nDanh sách xe sẽ được nạp lại.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TaiDanhSach();
                gio.Rows.Clear();
                CapNhatTong();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}