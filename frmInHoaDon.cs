using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using CrystalDecisions.Windows.Forms;

namespace Quanlybanoto
{
    public partial class frmInHoaDon : Form
    {
        private string maHD;
        private CrystalReportViewer crystalReportViewer1;

        private string connStr =
            @"Data Source=DESKTOP-8GD7JPJ\SQLEXPRESS;Initial Catalog=QuanLyBanOTo;Integrated Security=True";

        public frmInHoaDon(string maHD)
        {
            InitializeComponent();
            this.maHD = maHD;

            crystalReportViewer1 = new CrystalReportViewer();
            crystalReportViewer1.Dock = DockStyle.Fill;
            this.Controls.Add(crystalReportViewer1);
        }

        private void frmInHoaDon_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = @"
                    SELECT hd.MaHD, hd.NgayLap, hd.MaKH, hd.NguoiLap, hd.TongTien, hd.GhiChu,
                           ct.MaXe, ct.SoLuong, ct.DonGia,
                           kh.HoTen, kh.SoDienThoai, kh.CCCD, kh.DiaChi, kh.Email,
                           x.TenXe, x.HangXe, x.NamSX, x.MauSac, x.GiaBan
                    FROM HoaDon hd
                    JOIN ChiTietHoaDon ct ON hd.MaHD = ct.MaHD
                    JOIN KhachHang kh ON hd.MaKH = kh.MaKH
                    JOIN Xe x ON ct.MaXe = x.MaXe
                    WHERE hd.MaHD = @MaHD";

                DataTable dt = new DataTable();
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                {
                    da.SelectCommand.Parameters.AddWithValue("@MaHD", maHD);
                    da.Fill(dt);
                }

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn!");
                    return;
                }

                rptHoaDon rpt = new rptHoaDon();
                //rpt.SetDataSource(dt);
                crystalReportViewer1.ReportSource = rpt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}