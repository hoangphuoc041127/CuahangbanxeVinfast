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
    /*
     * FORM PHIẾU NHẬP XE
     */
    public partial class frmNhapXe : Form
    {
        // =====================================================================
        // KHỐI CẤU HÌNH CƠ SỞ DỮ LIỆU
        // =====================================================================
        private const string PN_BANG = "PhieuNhap";
        private const string PN_MA = "MaPN";
        private const string PN_NGAY = "NgayLap";
        private const string PN_MANCC = "MaNCC";
        private const string PN_NGUOILAP = "NguoiLap";
        private const string PN_TONGTIEN = "TongTien";
        private const string PN_GHICHU = "GhiChu";

        // Bảng chi tiết phiếu nhập
        private const string CT_BANG = "ChiTietPhieuNhap";
        private const string CT_MAPHIEU = "MaPN";
        private const string CT_MAXE = "MaXe";
        private const string CT_SOLUONG = "SoLuong";
        private const string CT_DONGIA = "DonGia";

        // Bảng xe
        private const string XE_BANG = "Xe";
        private const string XE_MA = "MaXe";
        private const string XE_TEN = "TenXe";
        private const string XE_TON = "SoLuong";

        private const string TIEN_TO = "PN";
        private static readonly bool MA_PHIEU_TU_TANG = false;
        private static readonly bool TRIGGER_TU_CAP_NHAT_TON = false;

        private DataTable gioHang;      // danh sách xe của phiếu đang lập

        public frmNhapXe()
        {
            InitializeComponent();

            Load -= frmNhapXe_Load; Load += frmNhapXe_Load;
            btnThemXe.Click -= btnThemXe_Click; btnThemXe.Click += btnThemXe_Click;
            btnXoaXe.Click -= btnXoaXe_Click; btnXoaXe.Click += btnXoaXe_Click;
            btnLuu.Click -= btnLuu_Click; btnLuu.Click += btnLuu_Click;
            btnPhieuMoi.Click -= btnPhieuMoi_Click; btnPhieuMoi.Click += btnPhieuMoi_Click;
        }

        private void label4_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }

        // ===================== KHI MỞ FORM =====================
        private void frmNhapXe_Load(object sender, EventArgs e)
        {
            gioHang = new DataTable();
            gioHang.Columns.Add("MaXe", typeof(string));
            gioHang.Columns.Add("TenXe", typeof(string));
            gioHang.Columns.Add("SoLuong", typeof(int));
            gioHang.Columns.Add("DonGia", typeof(decimal));
            gioHang.Columns.Add("ThanhTien", typeof(decimal));
            gioHang.PrimaryKey = new DataColumn[] { gioHang.Columns["MaXe"] };

            dgvChiTiet.DataSource = gioHang;
            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.AllowUserToAddRows = false;
            dgvChiTiet.MultiSelect = false;
            dgvChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTiet.Columns["MaXe"].HeaderText = "Mã xe";
            dgvChiTiet.Columns["TenXe"].HeaderText = "Tên xe";
            dgvChiTiet.Columns["SoLuong"].HeaderText = "Số lượng";
            dgvChiTiet.Columns["DonGia"].HeaderText = "Đơn giá nhập";
            dgvChiTiet.Columns["ThanhTien"].HeaderText = "Thành tiền";
            dgvChiTiet.Columns["DonGia"].DefaultCellStyle.Format = "N0";
            dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";

            dtpNgayLap.Format = DateTimePickerFormat.Custom;
            dtpNgayLap.CustomFormat = "dd/MM/yyyy";

            numSoLuong.Minimum = 1;
            numSoLuong.Maximum = 100000;
            numSoLuong.Value = 1;

            txtMaPhieu.ReadOnly = true;
            txtTongTien.ReadOnly = true;
            txtTongTien.TextAlign = HorizontalAlignment.Right;

            NapCombo();
            PhieuMoi();
        }

        // ===================== NẠP DỮ LIỆU CHO HAI COMBO =====================
        private void NapCombo()
        {
            try
            {
                // Nạp Nhà Cung Cấp trực tiếp từ bảng NhaCungCap
                cboNhaCungCap.DropDownStyle = ComboBoxStyle.DropDownList;
                DataTable dtNCC = DataProvider.GetData("SELECT MaNCC, TenNCC FROM NhaCungCap ORDER BY TenNCC");
                cboNhaCungCap.DataSource = dtNCC;
                cboNhaCungCap.DisplayMember = "TenNCC";
                cboNhaCungCap.ValueMember = "MaNCC";
                cboNhaCungCap.SelectedIndex = -1;

                // Nạp danh sách Xe trực tiếp từ bảng Xe
                cboXe.DropDownStyle = ComboBoxStyle.DropDownList;
                DataTable dtXe = DataProvider.GetData(string.Format(
                    "SELECT {0}, {1} FROM {2} ORDER BY {1}", XE_MA, XE_TEN, XE_BANG));
                cboXe.DataSource = dtXe;
                cboXe.DisplayMember = XE_TEN;
                cboXe.ValueMember = XE_MA;
                cboXe.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách nhà cung cấp / xe: " + ex.Message);
            }
        }

        // ===================== SINH MÃ PHIẾU MỚI =====================
        private string TaoMaPhieuMoi()
        {
            int max = 0;
            DataTable dt = DataProvider.GetData(string.Format("SELECT {0} FROM {1}", PN_MA, PN_BANG));
            foreach (DataRow r in dt.Rows)
            {
                string ma = r[0].ToString().Trim();
                int so;
                if (ma.StartsWith(TIEN_TO, StringComparison.OrdinalIgnoreCase) &&
                    int.TryParse(ma.Substring(TIEN_TO.Length), out so) && so > max)
                {
                    max = so;
                }
            }
            return TIEN_TO + (max + 1).ToString("000");
        }

        // ===================== TỔNG TIỀN =====================
        private decimal TinhTongTien()
        {
            object tong = gioHang.Compute("SUM(ThanhTien)", "");
            return tong == DBNull.Value ? 0 : Convert.ToDecimal(tong);
        }

        private void CapNhatTongTien()
        {
            txtTongTien.Text = TinhTongTien().ToString("N0");
        }

        private bool BaoLoi(string noiDung, Control control)
        {
            MessageBox.Show(noiDung, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.Focus();
            return false;
        }

        // ===================== THÊM XE VÀO DANH SÁCH =====================
        private void btnThemXe_Click(object sender, EventArgs e)
        {
            if (cboXe.SelectedValue == null)
            {
                BaoLoi("Vui lòng chọn xe!", cboXe);
                return;
            }

            decimal gia;
            string giaChuoi = txtDonGia.Text.Replace(".", "").Replace(",", "").Trim();
            if (!decimal.TryParse(giaChuoi, out gia) || gia <= 0)
            {
                BaoLoi("Đơn giá nhập phải là số lớn hơn 0!", txtDonGia);
                return;
            }

            string maXe = cboXe.SelectedValue.ToString();
            int soLuong = (int)numSoLuong.Value;

            if (gioHang.Rows.Find(maXe) != null)
            {
                BaoLoi("Xe này đã có trong phiếu. Hãy xóa dòng cũ rồi thêm lại nếu muốn đổi số lượng hoặc giá!", cboXe);
                return;
            }

            gioHang.Rows.Add(maXe, cboXe.Text, soLuong, gia, soLuong * gia);
            CapNhatTongTien();

            txtDonGia.Clear();
            numSoLuong.Value = 1;
            cboXe.SelectedIndex = -1;
        }

        // ===================== XÓA XE KHỎI DANH SÁCH =====================
        private void btnXoaXe_Click(object sender, EventArgs e)
        {
            if (dgvChiTiet.CurrentRow == null)
            {
                MessageBox.Show("Hãy chọn dòng cần xóa trong danh sách!");
                return;
            }
            DataRow dong = ((DataRowView)dgvChiTiet.CurrentRow.DataBoundItem).Row;
            gioHang.Rows.Remove(dong);
            CapNhatTongTien();
        }

        private void ThemCot(List<string> cot, List<string> thamSo, List<SqlParameter> ps,
                             string tenCot, string tenThamSo, object giaTri)
        {
            if (string.IsNullOrEmpty(tenCot)) return;
            cot.Add(tenCot);
            thamSo.Add("@" + tenThamSo);
            ps.Add(new SqlParameter("@" + tenThamSo, giaTri ?? DBNull.Value));
        }

        // ===================== GHI PHIẾU XUỐNG CSDL =====================
        private string GhiPhieu(string maPhieu)
        {
            using (SqlConnection conn = new SqlConnection(DataProvider.ConnectionString))
            {
                conn.Open();
                using (SqlTransaction tx = conn.BeginTransaction())
                {
                    try
                    {
                        List<string> cot = new List<string>();
                        List<string> thamSo = new List<string>();
                        List<SqlParameter> ps = new List<SqlParameter>();

                        if (!MA_PHIEU_TU_TANG)
                            ThemCot(cot, thamSo, ps, PN_MA, "ma", maPhieu);
                        ThemCot(cot, thamSo, ps, PN_NGAY, "ngay", dtpNgayLap.Value.Date);
                        ThemCot(cot, thamSo, ps, PN_MANCC, "ncc", cboNhaCungCap.SelectedValue);
                        ThemCot(cot, thamSo, ps, PN_NGUOILAP, "nguoi", txtNguoiLap.Text.Trim());
                        ThemCot(cot, thamSo, ps, PN_TONGTIEN, "tong", TinhTongTien());
                        ThemCot(cot, thamSo, ps, PN_GHICHU, "ghichu", txtGhiChu.Text.Trim());

                        string sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2})",
                            PN_BANG, string.Join(", ", cot), string.Join(", ", thamSo));

                        using (SqlCommand cmd = new SqlCommand(sql, conn, tx))
                        {
                            cmd.Parameters.AddRange(ps.ToArray());
                            cmd.ExecuteNonQuery();
                        }

                        foreach (DataRow r in gioHang.Rows)
                        {
                            string sqlCT = string.Format(
                                "INSERT INTO {0} ({1}, {2}, {3}, {4}) VALUES (@pn, @xe, @sl, @gia)",
                                CT_BANG, CT_MAPHIEU, CT_MAXE, CT_SOLUONG, CT_DONGIA);
                            using (SqlCommand cmdCT = new SqlCommand(sqlCT, conn, tx))
                            {
                                cmdCT.Parameters.AddWithValue("@pn", maPhieu);
                                cmdCT.Parameters.AddWithValue("@xe", r["MaXe"]);
                                cmdCT.Parameters.AddWithValue("@sl", r["SoLuong"]);
                                cmdCT.Parameters.AddWithValue("@gia", r["DonGia"]);
                                cmdCT.ExecuteNonQuery();
                            }

                            if (!TRIGGER_TU_CAP_NHAT_TON)
                            {
                                string sqlTon = string.Format("UPDATE {0} SET {1} = {1} + @sl WHERE {2} = @xe",
                                    XE_BANG, XE_TON, XE_MA);
                                using (SqlCommand cmdTon = new SqlCommand(sqlTon, conn, tx))
                                {
                                    cmdTon.Parameters.AddWithValue("@sl", r["SoLuong"]);
                                    cmdTon.Parameters.AddWithValue("@xe", r["MaXe"]);
                                    cmdTon.ExecuteNonQuery();
                                }
                            }
                        }

                        tx.Commit();
                        return maPhieu;
                    }
                    catch
                    {
                        try { tx.Rollback(); } catch { }
                        throw;
                    }
                }
            }
        }

        // ===================== NÚT LƯU =====================
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (cboNhaCungCap.SelectedValue == null)
            {
                BaoLoi("Vui lòng chọn nhà cung cấp!", cboNhaCungCap);
                return;
            }
            if (!string.IsNullOrEmpty(PN_NGUOILAP) && txtNguoiLap.Text.Trim() == "")
            {
                BaoLoi("Vui lòng nhập người lập phiếu!", txtNguoiLap);
                return;
            }
            if (gioHang.Rows.Count == 0)
            {
                MessageBox.Show("Phiếu chưa có xe nào, hãy chọn xe rồi bấm Thêm xe!");
                return;
            }

            try
            {
                string maPhieu = MA_PHIEU_TU_TANG ? "" : TaoMaPhieuMoi();
                string maDaLuu = GhiPhieu(maPhieu);

                MessageBox.Show("Lưu phiếu nhập thành công! Mã phiếu: " + maDaLuu,
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PhieuMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không lưu được phiếu nhập: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===================== PHIẾU MỚI =====================
        private void PhieuMoi()
        {
            gioHang.Clear();
            CapNhatTongTien();

            dtpNgayLap.Value = DateTime.Now;
            cboNhaCungCap.SelectedIndex = -1;
            cboXe.SelectedIndex = -1;
            txtDonGia.Clear();
            txtGhiChu.Clear();
            numSoLuong.Value = 1;

            try
            {
                txtMaPhieu.Text = MA_PHIEU_TU_TANG ? "(tự động)" : TaoMaPhieuMoi();
            }
            catch (Exception ex)
            {
                txtMaPhieu.Text = "";
                MessageBox.Show("Không tạo được mã phiếu: " + ex.Message);
            }
        }

        private void btnPhieuMoi_Click(object sender, EventArgs e)
        {
            if (gioHang.Rows.Count > 0 &&
                MessageBox.Show("Phiếu đang lập sẽ bị xóa. Bạn có chắc muốn lập phiếu mới?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            PhieuMoi();
        }

        private void frmNhapXe_Load_1(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}