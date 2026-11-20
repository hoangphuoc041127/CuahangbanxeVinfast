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
using CrystalDecisions.Windows.Forms;

namespace Quanlybanoto
{
    public partial class frmHoaDon : Form
    {
        // Dùng chung chuỗi kết nối với DataProvider
        private static string KetNoi
        {
            get { return DataProvider.ConnectionString; }
        }

        // HoaDon(MaHD, NgayLap, MaKH, NguoiLap, TongTien, GhiChu)
        // ChiTietHoaDon(MaHD, MaXe, SoLuong, DonGia)
        // Xe(MaXe, TenXe, GiaBan, SoLuong)

        private decimal donGiaHienTai = 0;
        private bool dangNap = false;

        private Button btnThemXe;
        private Button btnBoXe;

        private void TaoNutThemBo()
        {
            btnThemXe = new Button();
            btnThemXe.Name = "btnThemXe";
            btnThemXe.Text = "Thêm xe vào hóa đơn";
            btnThemXe.Size = new Size(170, 30);
            btnThemXe.Location = new Point(cboXe.Left, dgvChiTiet.Top);

            btnBoXe = new Button();
            btnBoXe.Name = "btnBoXe";
            btnBoXe.Text = "Bỏ xe đã chọn";
            btnBoXe.Size = new Size(140, 30);
            btnBoXe.Location = new Point(btnThemXe.Right + 10, dgvChiTiet.Top);

            dgvChiTiet.Top += 38;
            dgvChiTiet.Height -= 38;

            this.Controls.Add(btnThemXe);
            this.Controls.Add(btnBoXe);
        }

        public frmHoaDon()
        {
            InitializeComponent();
            TaoNutThemBo();

            // Gỡ trước rồi gắn lại để mỗi sự kiện chỉ chạy 1 lần,
            // kể cả khi Designer đã gắn sẵn.
            this.Load -= frmHoaDon_Load;
            this.Load += frmHoaDon_Load;

            cboXe.SelectedIndexChanged -= cboXe_SelectedIndexChanged;
            cboXe.SelectedIndexChanged += cboXe_SelectedIndexChanged;

            btnThemXe.Click += btnThemXe_Click;
            btnBoXe.Click += btnBoXe_Click;

            btnLuu.Click -= btnLuu_Click;
            btnLuu.Click += btnLuu_Click;

            btnMoi.Click -= btnMoi_Click;
            btnMoi.Click += btnMoi_Click;

            btnXoa.Click -= btnXoa_Click;
            btnXoa.Click += btnXoa_Click;

            btnSua.Click -= btnSua_Click;
            btnSua.Click += btnSua_Click;

            btnThoat.Click -= btnThoat_Click;
            btnThoat.Click += btnThoat_Click;

            btnIn.Click -= btnIn_Click;
            btnIn.Click += btnIn_Click;

            txtMaHD.KeyDown -= txtMaHD_KeyDown;
            txtMaHD.KeyDown += txtMaHD_KeyDown;
        }

        // =====================================================
        //  KHỞI TẠO
        // =====================================================
        private void frmHoaDon_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;

            dtpNgayLap.Format = DateTimePickerFormat.Custom;
            dtpNgayLap.CustomFormat = "dd/MM/yyyy HH:mm";

            txtNguoiLap.ReadOnly = true;
            txtNguoiLap.Text = "(chưa đăng nhập)";
            txtDonGia.ReadOnly = true;

            nudSoLuong.Minimum = 1;
            nudSoLuong.Maximum = 1000;
            nudSoLuong.Value = 1;

            TaoCotLuoi();
            NapKhachHang();
            NapXe();
            HoaDonMoi();
        }

        private void TaoCotLuoi()
        {
            dgvChiTiet.AllowUserToAddRows = false;
            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTiet.RowHeadersVisible = false;
            dgvChiTiet.BackgroundColor = Color.White;

            dgvChiTiet.Columns.Clear();
            dgvChiTiet.Columns.Add("MaXe", "Mã xe");
            dgvChiTiet.Columns.Add("TenXe", "Tên xe");
            dgvChiTiet.Columns.Add("SoLuong", "Số lượng");
            dgvChiTiet.Columns.Add("DonGia", "Đơn giá");
            dgvChiTiet.Columns.Add("ThanhTien", "Thành tiền");

            dgvChiTiet.Columns["DonGia"].DefaultCellStyle.Format = "N0";
            dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
            dgvChiTiet.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvChiTiet.Columns["DonGia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private DataTable LayBang(string sql, params SqlParameter[] ps)
        {
            using (SqlConnection cn = new SqlConnection(KetNoi))
            using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
            {
                if (ps != null) da.SelectCommand.Parameters.AddRange(ps);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // Tìm tên cột chứa tên khách hàng trong bảng KhachHang
        private string TimCotTenKhach(out string danhSachCot)
        {
            DataTable cot = LayBang(
                "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'KhachHang'");

            List<string> ds = new List<string>();
            foreach (DataRow r in cot.Rows) ds.Add(r[0].ToString());
            danhSachCot = string.Join(", ", ds.ToArray());

            string[] uuTien = { "TenKH", "HoTen", "TenKhachHang", "HoTenKH", "TenKhach", "Ten" };
            foreach (string u in uuTien)
                foreach (string c in ds)
                    if (string.Equals(c, u, StringComparison.OrdinalIgnoreCase)) return c;

            foreach (string c in ds)
                if (!c.StartsWith("Ma", StringComparison.OrdinalIgnoreCase)) return c;

            return null;
        }

        private void NapKhachHang()
        {
            try
            {
                string dsCot;
                string cotTen = TimCotTenKhach(out dsCot);
                if (cotTen == null)
                    throw new Exception("Bảng KhachHang không có cột tên khách hàng. Các cột hiện có: " + dsCot);

                DataTable dt = LayBang(
                    "SELECT MaKH, [" + cotTen + "] AS TenKH FROM KhachHang ORDER BY [" + cotTen + "]");
                cboKhachHang.DataSource = dt;
                cboKhachHang.DisplayMember = "TenKH";
                cboKhachHang.ValueMember = "MaKH";
                cboKhachHang.DropDownStyle = ComboBoxStyle.DropDownList;
                cboKhachHang.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không nạp được khách hàng: " + ex.Message);
            }
        }

        private void NapXe()
        {
            try
            {
                dangNap = true;
                DataTable dt = LayBang("SELECT MaXe, TenXe, GiaBan, SoLuong FROM Xe ORDER BY TenXe");
                cboXe.DataSource = dt;
                cboXe.DisplayMember = "TenXe";
                cboXe.ValueMember = "MaXe";
                cboXe.DropDownStyle = ComboBoxStyle.DropDownList;
                cboXe.SelectedIndex = -1;
                dangNap = false;
            }
            catch (Exception ex)
            {
                dangNap = false;
                MessageBox.Show("Không nạp được danh sách xe: " + ex.Message);
            }
        }

        // =====================================================
        //  MÃ HÓA ĐƠN
        // =====================================================
        private string SinhMaHD()
        {
            try
            {
                DataTable dt = LayBang("SELECT TOP 1 MaHD FROM HoaDon ORDER BY MaHD DESC");
                if (dt.Rows.Count == 0) return "HD0001";

                string ma = dt.Rows[0][0].ToString().Trim();
                int so;
                if (ma.Length > 2 && int.TryParse(ma.Substring(2), out so))
                    return "HD" + (so + 1).ToString("D4");
            }
            catch { }
            return "HD0001";
        }

        private bool HoaDonDaTonTai(string ma)
        {
            DataTable dt = LayBang("SELECT 1 FROM HoaDon WHERE MaHD = @ma",
                new SqlParameter("@ma", ma));
            return dt.Rows.Count > 0;
        }

        // =====================================================
        //  CHỌN XE -> HIỆN ĐƠN GIÁ
        // =====================================================
        private void cboXe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dangNap) return;

            DataRowView drv = cboXe.SelectedItem as DataRowView;
            if (drv == null)
            {
                donGiaHienTai = 0;
                txtDonGia.Text = "";
                return;
            }
            donGiaHienTai = Convert.ToDecimal(drv["GiaBan"]);
            txtDonGia.Text = donGiaHienTai.ToString("N0");
        }

        // =====================================================
        //  THÊM / BỎ XE TRONG BẢNG
        // =====================================================
        private void btnThemXe_Click(object sender, EventArgs e)
        {
            DataRowView drv = cboXe.SelectedItem as DataRowView;
            if (drv == null)
            {
                MessageBox.Show("Vui lòng chọn xe!");
                return;
            }

            string maXe = drv["MaXe"].ToString();
            string tenXe = drv["TenXe"].ToString();
            int ton = Convert.ToInt32(drv["SoLuong"]);
            int sl = (int)nudSoLuong.Value;

            DataGridViewRow dongCo = null;
            foreach (DataGridViewRow r in dgvChiTiet.Rows)
            {
                if (Convert.ToString(r.Cells["MaXe"].Value) == maXe)
                {
                    dongCo = r;
                    break;
                }
            }

            int daCo = dongCo == null ? 0 : Convert.ToInt32(dongCo.Cells["SoLuong"].Value);
            if (daCo + sl > ton)
            {
                MessageBox.Show("Không đủ hàng trong kho! Tồn kho hiện có: " + ton);
                return;
            }

            if (dongCo != null)
            {
                int slMoi = daCo + sl;
                dongCo.Cells["SoLuong"].Value = slMoi;
                dongCo.Cells["ThanhTien"].Value = slMoi * donGiaHienTai;
            }
            else
            {
                dgvChiTiet.Rows.Add(maXe, tenXe, sl, donGiaHienTai, sl * donGiaHienTai);
            }

            nudSoLuong.Value = 1;
            TinhTongTien();
        }

        private void btnBoXe_Click(object sender, EventArgs e)
        {
            if (dgvChiTiet.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn dòng xe cần bỏ!");
                return;
            }
            dgvChiTiet.Rows.Remove(dgvChiTiet.CurrentRow);
            TinhTongTien();
        }

        private decimal TinhTongTien()
        {
            decimal tong = 0;
            foreach (DataGridViewRow row in dgvChiTiet.Rows)
            {
                if (row.Cells["ThanhTien"].Value != null)
                    tong += Convert.ToDecimal(row.Cells["ThanhTien"].Value);
            }
            lblTongTien.Text = "Tổng tiền: " + tong.ToString("N0") + " đ";
            return tong;
        }

        // =====================================================
        //  HÓA ĐƠN MỚI
        // =====================================================
        private void HoaDonMoi()
        {
            txtMaHD.Text = SinhMaHD();
            dtpNgayLap.Value = DateTime.Now;
            cboKhachHang.SelectedIndex = -1;
            cboXe.SelectedIndex = -1;
            txtDonGia.Text = "";
            donGiaHienTai = 0;
            nudSoLuong.Value = 1;
            txtGhiChu.Text = "";
            dgvChiTiet.Rows.Clear();
            TinhTongTien();
        }

        private void btnMoi_Click(object sender, EventArgs e)
        {
            HoaDonMoi();
        }

        // =====================================================
        //  LƯU HÓA ĐƠN (trừ tồn kho) - lưu xong hỏi có in không
        // =====================================================
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (cboKhachHang.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng!");
                return;
            }
            if (dgvChiTiet.Rows.Count == 0)
            {
                MessageBox.Show("Hóa đơn chưa có xe nào!");
                return;
            }

            string ma = txtMaHD.Text.Trim();
            if (ma == "")
            {
                MessageBox.Show("Mã hóa đơn không được để trống!");
                return;
            }

            try
            {
                if (HoaDonDaTonTai(ma))
                {
                    MessageBox.Show("Hóa đơn " + ma + " đã được lưu. Hãy bấm \"Hóa đơn mới\" để lập hóa đơn khác, hoặc \"Sửa\" để đổi khách hàng / ghi chú.");
                    return;
                }

                if (MessageBox.Show("Lưu hóa đơn " + ma + "?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                decimal tong = TinhTongTien();

                using (SqlConnection cn = new SqlConnection(KetNoi))
                {
                    cn.Open();
                    using (SqlTransaction tran = cn.BeginTransaction())
                    {
                        try
                        {
                            SqlCommand cmd = new SqlCommand(
                                "INSERT INTO HoaDon(MaHD, NgayLap, MaKH, GhiChu, TongTien) " +
                                "VALUES(@ma, @ngay, @kh, @ghichu, @tong)", cn, tran);
                            cmd.Parameters.AddWithValue("@ma", ma);
                            cmd.Parameters.AddWithValue("@ngay", dtpNgayLap.Value);
                            cmd.Parameters.AddWithValue("@kh", cboKhachHang.SelectedValue);
                            cmd.Parameters.AddWithValue("@ghichu", txtGhiChu.Text.Trim());
                            cmd.Parameters.AddWithValue("@tong", tong);
                            cmd.ExecuteNonQuery();

                            foreach (DataGridViewRow r in dgvChiTiet.Rows)
                            {
                                string maXe = Convert.ToString(r.Cells["MaXe"].Value);
                                int sl = Convert.ToInt32(r.Cells["SoLuong"].Value);
                                decimal dg = Convert.ToDecimal(r.Cells["DonGia"].Value);

                                SqlCommand ct = new SqlCommand(
                                    "INSERT INTO ChiTietHoaDon(MaHD, MaXe, SoLuong, DonGia) " +
                                    "VALUES(@ma, @xe, @sl, @dg)", cn, tran);
                                ct.Parameters.AddWithValue("@ma", ma);
                                ct.Parameters.AddWithValue("@xe", maXe);
                                ct.Parameters.AddWithValue("@sl", sl);
                                ct.Parameters.AddWithValue("@dg", dg);
                                ct.ExecuteNonQuery();

                                SqlCommand ton = new SqlCommand(
                                    "UPDATE Xe SET SoLuong = SoLuong - @sl " +
                                    "WHERE MaXe = @xe AND SoLuong >= @sl", cn, tran);
                                ton.Parameters.AddWithValue("@sl", sl);
                                ton.Parameters.AddWithValue("@xe", maXe);
                                if (ton.ExecuteNonQuery() == 0)
                                    throw new Exception("Xe " + maXe + " không đủ số lượng tồn kho.");
                            }

                            tran.Commit();
                        }
                        catch
                        {
                            tran.Rollback();
                            throw;
                        }
                    }
                }

                // Đã lưu thành công -> hỏi in ngay (trước khi form sinh mã mới)
                if (MessageBox.Show("Lưu hóa đơn thành công! In hóa đơn ngay?", "In hóa đơn",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    MoFormIn(ma);
                }

                NapXe();
                HoaDonMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu hóa đơn: " + ex.Message);
            }
        }

        // =====================================================
        //  MỞ LẠI HÓA ĐƠN CŨ: gõ mã vào ô Mã hóa đơn rồi nhấn Enter
        // =====================================================
        private void txtMaHD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;

            string ma = txtMaHD.Text.Trim();
            if (ma == "") return;

            try
            {
                DataTable hd = LayBang(
                    "SELECT MaHD, NgayLap, MaKH, GhiChu FROM HoaDon WHERE MaHD = @ma",
                    new SqlParameter("@ma", ma));

                if (hd.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn " + ma);
                    return;
                }

                DataRow r = hd.Rows[0];
                dtpNgayLap.Value = Convert.ToDateTime(r["NgayLap"]);
                cboKhachHang.SelectedValue = r["MaKH"];
                txtGhiChu.Text = Convert.ToString(r["GhiChu"]);

                DataTable ct = LayBang(
                    "SELECT c.MaXe, x.TenXe, c.SoLuong, c.DonGia " +
                    "FROM ChiTietHoaDon c JOIN Xe x ON c.MaXe = x.MaXe WHERE c.MaHD = @ma",
                    new SqlParameter("@ma", ma));

                dgvChiTiet.Rows.Clear();
                foreach (DataRow d in ct.Rows)
                {
                    int sl = Convert.ToInt32(d["SoLuong"]);
                    decimal dg = Convert.ToDecimal(d["DonGia"]);
                    dgvChiTiet.Rows.Add(d["MaXe"], d["TenXe"], sl, dg, sl * dg);
                }
                TinhTongTien();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở hóa đơn: " + ex.Message);
            }
        }

        // =====================================================
        //  SỬA (khách hàng + ghi chú + ngày lập của hóa đơn đã lưu)
        // =====================================================
        private void btnSua_Click(object sender, EventArgs e)
        {
            string ma = txtMaHD.Text.Trim();
            try
            {
                if (!HoaDonDaTonTai(ma))
                {
                    MessageBox.Show("Hóa đơn này chưa được lưu. Hãy nhập mã hóa đơn cũ rồi nhấn Enter để mở, hoặc bấm \"Lưu hóa đơn\".");
                    return;
                }
                if (cboKhachHang.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn khách hàng!");
                    return;
                }

                using (SqlConnection cn = new SqlConnection(KetNoi))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE HoaDon SET NgayLap = @ngay, MaKH = @kh, GhiChu = @ghichu WHERE MaHD = @ma", cn);
                    cmd.Parameters.AddWithValue("@ngay", dtpNgayLap.Value);
                    cmd.Parameters.AddWithValue("@kh", cboKhachHang.SelectedValue);
                    cmd.Parameters.AddWithValue("@ghichu", txtGhiChu.Text.Trim());
                    cmd.Parameters.AddWithValue("@ma", ma);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Đã cập nhật hóa đơn " + ma);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa hóa đơn: " + ex.Message);
            }
        }

        // =====================================================
        //  XÓA (cộng lại tồn kho)
        // =====================================================
        private void btnXoa_Click(object sender, EventArgs e)
        {
            string ma = txtMaHD.Text.Trim();
            try
            {
                if (!HoaDonDaTonTai(ma))
                {
                    if (MessageBox.Show("Hóa đơn chưa lưu. Xóa nội dung đang lập?", "Xác nhận",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        HoaDonMoi();
                    return;
                }

                if (MessageBox.Show("Xóa hóa đơn " + ma + "? Số lượng xe sẽ được cộng lại vào kho.",
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;

                DataTable ct = LayBang("SELECT MaXe, SoLuong FROM ChiTietHoaDon WHERE MaHD = @ma",
                    new SqlParameter("@ma", ma));

                using (SqlConnection cn = new SqlConnection(KetNoi))
                {
                    cn.Open();
                    using (SqlTransaction tran = cn.BeginTransaction())
                    {
                        try
                        {
                            foreach (DataRow r in ct.Rows)
                            {
                                SqlCommand ton = new SqlCommand(
                                    "UPDATE Xe SET SoLuong = SoLuong + @sl WHERE MaXe = @xe", cn, tran);
                                ton.Parameters.AddWithValue("@sl", Convert.ToInt32(r["SoLuong"]));
                                ton.Parameters.AddWithValue("@xe", r["MaXe"].ToString());
                                ton.ExecuteNonQuery();
                            }

                            SqlCommand xoaCT = new SqlCommand(
                                "DELETE FROM ChiTietHoaDon WHERE MaHD = @ma", cn, tran);
                            xoaCT.Parameters.AddWithValue("@ma", ma);
                            xoaCT.ExecuteNonQuery();

                            SqlCommand xoaHD = new SqlCommand(
                                "DELETE FROM HoaDon WHERE MaHD = @ma", cn, tran);
                            xoaHD.Parameters.AddWithValue("@ma", ma);
                            xoaHD.ExecuteNonQuery();

                            tran.Commit();
                        }
                        catch
                        {
                            tran.Rollback();
                            throw;
                        }
                    }
                }

                MessageBox.Show("Đã xóa hóa đơn " + ma);
                NapXe();
                HoaDonMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa hóa đơn: " + ex.Message);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // =====================================================
        //  FORM IN HÓA ĐƠN (tạo bằng code, không cần file frmInHoaDon riêng)
        // =====================================================
        private void MoFormIn(string maHD)
        {
            try
            {
                DataSetHoaDon ds = new DataSetHoaDon();
                ds.EnforceConstraints = false;

                using (SqlConnection conn = new SqlConnection(KetNoi))
                {
                    conn.Open();

                    var daHD = new SqlDataAdapter("SELECT * FROM HoaDon WHERE MaHD = @MaHD", conn);
                    daHD.SelectCommand.Parameters.AddWithValue("@MaHD", maHD);
                    daHD.Fill(ds, "HoaDon");

                    var daCT = new SqlDataAdapter("SELECT * FROM ChiTietHoaDon WHERE MaHD = @MaHD", conn);
                    daCT.SelectCommand.Parameters.AddWithValue("@MaHD", maHD);
                    daCT.Fill(ds, "ChiTietHoaDon");

                    var daKH = new SqlDataAdapter(
                        "SELECT * FROM KhachHang WHERE MaKH IN (SELECT MaKH FROM HoaDon WHERE MaHD = @MaHD)", conn);
                    daKH.SelectCommand.Parameters.AddWithValue("@MaHD", maHD);
                    daKH.Fill(ds, "KhachHang");

                    var daXe = new SqlDataAdapter(
                        "SELECT * FROM Xe WHERE MaXe IN (SELECT MaXe FROM ChiTietHoaDon WHERE MaHD = @MaHD)", conn);
                    daXe.SelectCommand.Parameters.AddWithValue("@MaHD", maHD);
                    daXe.Fill(ds, "Xe");
                }

                if (ds.Tables["HoaDon"].Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn!");
                    return;
                }

                rptHoaDon rpt = new rptHoaDon();
                rpt.SetDataSource(ds);

                CrystalReportViewer viewer = new CrystalReportViewer();
                viewer.Dock = DockStyle.Fill;
                viewer.ReportSource = rpt;

                Form f = new Form();
                f.Text = "In hóa đơn " + maHD;
                f.StartPosition = FormStartPosition.CenterScreen;
                f.WindowState = FormWindowState.Maximized;
                f.Controls.Add(viewer);
                f.ShowDialog();
                f.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi in: " + ex.ToString());
            }
        }

        // =====================================================
        //  IN HÓA ĐƠN (chỉ in được hóa đơn đã lưu vào database)
        // =====================================================
        private void btnIn_Click(object sender, EventArgs e)
        {
            string ma = txtMaHD.Text.Trim();
            if (ma == "")
            {
                MessageBox.Show("Chưa có mã hóa đơn!");
                return;
            }
            try
            {
                if (!HoaDonDaTonTai(ma))
                {
                    MessageBox.Show("Hóa đơn " + ma + " chưa được lưu. Hãy bấm \"Lưu hóa đơn\" trước khi in.");
                    return;
                }
                MoFormIn(ma);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi in hóa đơn: " + ex.Message);
            }
        }
    }
}