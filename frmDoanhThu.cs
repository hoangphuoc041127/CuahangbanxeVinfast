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
    /*
     * FORM DOANH THU
     *
     * TÊN (Name) CÁC CONTROL TRÊN FORM - phải đặt đúng như sau:
     *   DateTimePicker : dtpTuNgay (ô cạnh "Từ ngày"), dtpDenNgay (ô cạnh "Đến ngày")
     *   ComboBox       : cboXemThem (ô cạnh "Xem thêm")
     *   DataGridView   : dgvDoanhThu (khối xám lớn, hiện đang tên dataGridView1 nên đổi lại)
     *   Button         : btnXem, btnLamMoi, btnThoat
     *
     * CẦN CÓ TRONG PROJECT: DataProvider.cs (bản có GetData)
     *
     * CÁCH DÙNG: chọn khoảng ngày và kiểu xem trong ô "Xem thêm", bấm Xem.
     * Dòng cuối bảng là dòng TỔNG CỘNG.
     *   - Doanh thu theo ngày  : mỗi ngày một dòng (số hóa đơn, doanh thu)
     *   - Doanh thu theo tháng : mỗi tháng một dòng
     *   - Xe bán chạy          : mỗi xe một dòng, xếp theo số lượng bán giảm dần
     *
     * Sự kiện được gắn bằng code ở hàm khởi tạo (gỡ rồi gắn lại nên không bị chạy 2 lần).
     * Đừng bấm đúp vào các control này nữa.
     */
    public partial class frmDoanhThu : Form
    {
        // =====================================================================
        // KHỐI CẤU HÌNH: nếu tên bảng / tên cột trong database của bạn khác thì CHỈ CẦN SỬA Ở ĐÂY.
        // Doanh thu được tính bằng SỐ LƯỢNG x ĐƠN GIÁ của các dòng chi tiết hóa đơn.
        // =====================================================================
        // Bảng hóa đơn
        private const string HD_BANG = "HoaDon";
        private const string HD_MA = "MaHD";
        private const string HD_NGAY = "NgayLap";

        // Bảng chi tiết hóa đơn
        private const string CT_BANG = "ChiTietHoaDon";
        private const string CT_MAHD = "MaHD";
        private const string CT_MAXE = "MaXe";
        private const string CT_SOLUONG = "SoLuong";
        private const string CT_DONGIA = "DonGia";

        // Bảng xe (lấy tên xe cho kiểu "Xe bán chạy")
        private const string XE_BANG = "Xe";
        private const string XE_MA = "MaXe";
        private const string XE_TEN = "TenXe";
        // =====================================================================

        public frmDoanhThu()
        {
            InitializeComponent();

            Load -= frmDoanhThu_Load; Load += frmDoanhThu_Load;
            btnXem.Click -= btnXem_Click; btnXem.Click += btnXem_Click;
            btnLamMoi.Click -= btnLamMoi_Click; btnLamMoi.Click += btnLamMoi_Click;
            btnThoat.Click -= btnThoat_Click; btnThoat.Click += btnThoat_Click;
        }

        // ===== METHOD DO BẤM ĐÚP TRONG DESIGNER SINH RA =====
        // Để trống nhưng PHẢI GIỮ LẠI vì file Designer đang gắn vào nó; xóa là báo lỗi build.
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        // ===================== KHI MỞ FORM =====================
        private void frmDoanhThu_Load(object sender, EventArgs e)
        {
            dtpTuNgay.Format = DateTimePickerFormat.Custom;
            dtpTuNgay.CustomFormat = "dd/MM/yyyy";
            dtpDenNgay.Format = DateTimePickerFormat.Custom;
            dtpDenNgay.CustomFormat = "dd/MM/yyyy";

            cboXemThem.DropDownStyle = ComboBoxStyle.DropDownList;
            cboXemThem.Items.Clear();
            cboXemThem.Items.Add("Doanh thu theo ngày");
            cboXemThem.Items.Add("Doanh thu theo tháng");
            cboXemThem.Items.Add("Xe bán chạy");

            dgvDoanhThu.ReadOnly = true;
            dgvDoanhThu.AllowUserToAddRows = false;
            dgvDoanhThu.MultiSelect = false;
            dgvDoanhThu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDoanhThu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            AcceptButton = btnXem;      // nhấn Enter = bấm Xem
            DatLaiMacDinh();
        }

        // Từ ngày = đầu tháng hiện tại, Đến ngày = hôm nay, kiểu xem = theo ngày, xóa bảng
        private void DatLaiMacDinh()
        {
            DateTime homNay = DateTime.Today;
            dtpTuNgay.Value = new DateTime(homNay.Year, homNay.Month, 1);
            dtpDenNgay.Value = homNay;
            cboXemThem.SelectedIndex = 0;
            dgvDoanhThu.DataSource = null;
        }

        // ===================== NÚT XEM =====================
        private void btnXem_Click(object sender, EventArgs e)
        {
            DateTime tu = dtpTuNgay.Value.Date;
            DateTime den = dtpDenNgay.Value.Date;

            if (tu > den)
            {
                MessageBox.Show("\"Từ ngày\" phải nhỏ hơn hoặc bằng \"Đến ngày\"!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpTuNgay.Focus();
                return;
            }

            int kieuXem = cboXemThem.SelectedIndex;
            if (kieuXem < 0)
            {
                MessageBox.Show("Vui lòng chọn kiểu xem!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboXemThem.Focus();
                return;
            }

            try
            {
                // Lấy từ đầu ngày "Từ ngày" đến hết ngày "Đến ngày" (nhỏ hơn đầu ngày kế tiếp)
                DataTable dt = DataProvider.GetData(TaoCauLenh(kieuXem),
                    new System.Data.SqlClient.SqlParameter("@tu", tu),
                    new System.Data.SqlClient.SqlParameter("@den", den.AddDays(1)));

                if (dt.Rows.Count == 0)
                {
                    dgvDoanhThu.DataSource = null;
                    MessageBox.Show("Không có dữ liệu trong khoảng ngày đã chọn.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ThemDongTongCong(dt, kieuXem);
                dgvDoanhThu.DataSource = dt;
                DinhDangLuoi(kieuXem);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thống kê: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===================== CÂU TRUY VẤN THEO KIỂU XEM =====================
        private string TaoCauLenh(int kieuXem)
        {
            string thanhTien = string.Format("c.{0} * c.{1}", CT_SOLUONG, CT_DONGIA);
            string tuBang = string.Format("FROM {0} h JOIN {1} c ON c.{2} = h.{3}",
                HD_BANG, CT_BANG, CT_MAHD, HD_MA);
            string dieuKien = string.Format("WHERE h.{0} >= @tu AND h.{0} < @den", HD_NGAY);

            if (kieuXem == 0)
            {
                // Theo ngày: mỗi ngày một dòng, hiện dạng dd/mm/yyyy nhưng vẫn xếp theo ngày thật
                return string.Format(
                    "SELECT CONVERT(VARCHAR(10), CAST(h.{0} AS DATE), 103) AS Ngay, " +
                    "COUNT(DISTINCT h.{1}) AS SoHoaDon, " +
                    "CAST(SUM({2}) AS DECIMAL(18,0)) AS DoanhThu " +
                    "{3} {4} " +
                    "GROUP BY CAST(h.{0} AS DATE) ORDER BY CAST(h.{0} AS DATE)",
                    HD_NGAY, HD_MA, thanhTien, tuBang, dieuKien);
            }

            if (kieuXem == 1)
            {
                // Theo tháng: mỗi tháng một dòng, dạng MM/yyyy
                return string.Format(
                    "SELECT RIGHT('0' + CAST(MONTH(h.{0}) AS VARCHAR(2)), 2) + '/' + CAST(YEAR(h.{0}) AS VARCHAR(4)) AS ThangNam, " +
                    "COUNT(DISTINCT h.{1}) AS SoHoaDon, " +
                    "CAST(SUM({2}) AS DECIMAL(18,0)) AS DoanhThu " +
                    "{3} {4} " +
                    "GROUP BY YEAR(h.{0}), MONTH(h.{0}) ORDER BY YEAR(h.{0}), MONTH(h.{0})",
                    HD_NGAY, HD_MA, thanhTien, tuBang, dieuKien);
            }

            // Xe bán chạy: mỗi xe một dòng, xếp theo số lượng bán giảm dần
            return string.Format(
                "SELECT c.{0} AS MaXe, x.{1} AS TenXe, " +
                "CAST(SUM(c.{2}) AS INT) AS SoLuongBan, " +
                "CAST(SUM({3}) AS DECIMAL(18,0)) AS DoanhThu " +
                "{4} LEFT JOIN {5} x ON x.{6} = c.{0} {7} " +
                "GROUP BY c.{0}, x.{1} ORDER BY SUM(c.{2}) DESC",
                CT_MAXE, XE_TEN, CT_SOLUONG, thanhTien, tuBang, XE_BANG, XE_MA, dieuKien);
        }

        // ===================== DÒNG TỔNG CỘNG Ở CUỐI BẢNG =====================
        private void ThemDongTongCong(DataTable dt, int kieuXem)
        {
            string cotDem = kieuXem == 2 ? "SoLuongBan" : "SoHoaDon";
            int tongDem = 0;
            decimal tongTien = 0;

            foreach (DataRow r in dt.Rows)
            {
                tongDem += Convert.ToInt32(r[cotDem]);
                tongTien += Convert.ToDecimal(r["DoanhThu"]);
            }

            DataRow tong = dt.NewRow();
            tong[0] = "TỔNG CỘNG";
            tong[cotDem] = tongDem;
            tong["DoanhThu"] = tongTien;
            dt.Rows.Add(tong);
        }

        // ===================== ĐỊNH DẠNG BẢNG =====================
        private void DinhDangLuoi(int kieuXem)
        {
            if (kieuXem == 0)
            {
                dgvDoanhThu.Columns["Ngay"].HeaderText = "Ngày";
                dgvDoanhThu.Columns["SoHoaDon"].HeaderText = "Số hóa đơn";
            }
            else if (kieuXem == 1)
            {
                dgvDoanhThu.Columns["ThangNam"].HeaderText = "Tháng / Năm";
                dgvDoanhThu.Columns["SoHoaDon"].HeaderText = "Số hóa đơn";
            }
            else
            {
                dgvDoanhThu.Columns["MaXe"].HeaderText = "Mã xe";
                dgvDoanhThu.Columns["TenXe"].HeaderText = "Tên xe";
                dgvDoanhThu.Columns["SoLuongBan"].HeaderText = "Số lượng bán";
            }

            dgvDoanhThu.Columns["DoanhThu"].HeaderText = "Doanh thu (VNĐ)";
            dgvDoanhThu.Columns["DoanhThu"].DefaultCellStyle.Format = "N0";
            dgvDoanhThu.Columns["DoanhThu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Dòng cuối (TỔNG CỘNG) in đậm
            int cuoi = dgvDoanhThu.Rows.Count - 1;
            if (cuoi >= 0)
                dgvDoanhThu.Rows[cuoi].DefaultCellStyle.Font = new Font(dgvDoanhThu.Font, FontStyle.Bold);
        }

        // ===================== LÀM MỚI VÀ THOÁT =====================
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            DatLaiMacDinh();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}