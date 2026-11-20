using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Quanlybanoto
{
    // Form BÁN HÀNG (lập hóa đơn bán xe). Giao diện dựng hoàn toàn bằng code,
    // chỉ cần tạo form trống tên frmBanHang rồi dán file này vào.
    public partial class frmBanHang : Form
    {
        private DataTable gio;   // giỏ hàng: các xe đã chọn cho hóa đơn

        // ---- Các control (dựng bằng code) ----
        private TextBox txtMaHD, txtNguoiLap, txtDonGia, txtGhiChu;
        private DateTimePicker dtpNgayLap;
        private ComboBox cboKhachHang, cboXe;
        private NumericUpDown numSoLuong;
        private Button btnThemXe, btnBoXe, btnLuu, btnLamMoi;
        private DataGridView dgvChiTiet;
        private Label lblTongTien;

        private Label Nhan(string chu, int x, int y)
        {
            Label l = new Label();
            l.Text = chu; l.Left = x; l.Top = y + 4; l.AutoSize = true;
            Controls.Add(l);
            return l;
        }

        private T Dat<T>(T c, int x, int y, int w) where T : Control
        {
            c.Left = x; c.Top = y; c.Width = w;
            Controls.Add(c);
            return c;
        }

        private Button Nut(string chu, int x, int y, int w)
        {
            Button b = new Button();
            b.Text = chu; b.Height = 32;
            return Dat(b, x, y, w);
        }

        // ===== Hàm Designer đã nối sẵn, giữ lại để không lỗi build =====
        private void frmBanHang_Load(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }

        private void DungGiaoDien()
        {
            Controls.Clear();   // bỏ các control kéo trong Designer, giao diện dựng hoàn toàn bằng code
            Text = "Bán hàng - Lập hóa đơn";
            ClientSize = new System.Drawing.Size(900, 640);
            StartPosition = FormStartPosition.CenterScreen;

            Label td = new Label();
            td.Text = "LẬP HÓA ĐƠN BÁN XE";
            td.Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold);
            td.AutoSize = true; td.Left = 20; td.Top = 12;
            Controls.Add(td);

            Nhan("Mã hóa đơn:", 20, 58); txtMaHD = Dat(new TextBox(), 120, 58, 130);
            Nhan("Ngày lập:", 270, 58); dtpNgayLap = Dat(new DateTimePicker(), 340, 58, 170);
            Nhan("Người lập:", 530, 58); txtNguoiLap = Dat(new TextBox(), 610, 58, 270);

            Nhan("Khách hàng:", 20, 98); cboKhachHang = Dat(new ComboBox(), 120, 98, 760);

            Nhan("Chọn xe:", 20, 138); cboXe = Dat(new ComboBox(), 120, 138, 400);
            Nhan("SL:", 535, 138); numSoLuong = Dat(new NumericUpDown(), 565, 138, 60);
            Nhan("Đơn giá:", 640, 138); txtDonGia = Dat(new TextBox(), 700, 138, 180);

            btnThemXe = Nut("Thêm xe vào hóa đơn", 120, 178, 180);
            btnBoXe = Nut("Bỏ xe đã chọn", 310, 178, 150);

            dgvChiTiet = Dat(new DataGridView(), 20, 222, 860);
            dgvChiTiet.Height = 250;

            Nhan("Ghi chú:", 20, 488); txtGhiChu = Dat(new TextBox(), 120, 488, 760);

            lblTongTien = Dat(new Label(), 20, 534, 500);
            lblTongTien.AutoSize = false; lblTongTien.Height = 30;
            lblTongTien.Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold);
            lblTongTien.ForeColor = System.Drawing.Color.Firebrick;
            lblTongTien.Text = "Tổng tiền: 0 đ";

            btnLuu = Nut("Lưu hóa đơn", 560, 530, 150);
            btnLamMoi = Nut("Hóa đơn mới", 730, 530, 150);
        }

        public frmBanHang()
        {
            InitializeComponent();
            DungGiaoDien();

            txtMaHD.ReadOnly = true;
            txtNguoiLap.ReadOnly = true;
            txtDonGia.ReadOnly = true;
            txtGhiChu.MaxLength = 200;

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

            Load += (s, e) =>
            {
                CauHinhBang();
                TaiDanhSach();
                HoaDonMoi();
            };

            cboXe.SelectedIndexChanged += (s, e) => HienDonGia();
            btnThemXe.Click += btnThemXe_Click;
            btnBoXe.Click += btnBoXe_Click;
            btnLuu.Click += btnLuu_Click;
            btnLamMoi.Click += btnLamMoi_Click;
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
            lblTongTien.Text = "Tổng tiền: " + TongTien().ToString("N0") + " đ";
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
