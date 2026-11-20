using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Quanlybanoto
{
    public partial class frmXeBanChay : Form
    {
        // Sửa lại cho đúng với máy của bạn (hoặc lấy từ DataProvider)
        private readonly string connStr =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyBanOto;Integrated Security=True";

        public frmXeBanChay()
        {
            InitializeComponent();

            btnXem.Click += btnXem_Click;
            btnLamMoi.Click += btnLamMoi_Click;
            btnThoat.Click += btnThoat_Click;
        }

        private void frmXeBanChay_Load(object sender, EventArgs e)
        {
            nudTop.Minimum = 1;
            nudTop.Maximum = 1000;
            DatMacDinh();
        }

        // Mặc định: từ đầu tháng đến hôm nay, top 10
        private void DatMacDinh()
        {
            var now = DateTime.Today;
            dtpTuNgay.Value = new DateTime(now.Year, now.Month, 1);
            dtpDenNgay.Value = now;
            nudTop.Value = 10;
            dgvXeBanChay.DataSource = null;
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            if (dtpTuNgay.Value.Date > dtpDenNgay.Value.Date)
            {
                MessageBox.Show("Từ ngày không được lớn hơn Đến ngày.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpTuNgay.Focus();
                return;
            }
            LoadXeBanChay();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            DatMacDinh();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void LoadXeBanChay()
        {
            // Giả định: Xe(MaXe, TenXe, HangXe)
            //           HoaDon(MaHD, NgayLap)
            //           ChiTietHoaDon(MaHD, MaXe, SoLuong)
            string sql = @"
                SELECT TOP (@top)
                    x.MaXe          AS [Mã xe],
                    x.TenXe         AS [Tên xe],
                    x.HangXe        AS [Hãng],
                    SUM(ct.SoLuong) AS [Số lượng đã bán]
                FROM Xe x
                JOIN ChiTietHoaDon ct ON ct.MaXe = x.MaXe
                JOIN HoaDon hd        ON hd.MaHD = ct.MaHD
                WHERE hd.NgayLap >= @tuNgay
                  AND hd.NgayLap <  @denNgay
                GROUP BY x.MaXe, x.TenXe, x.HangXe
                ORDER BY SUM(ct.SoLuong) DESC";

            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var da = new SqlDataAdapter(sql, conn))
                {
                    da.SelectCommand.Parameters.Add("@top", SqlDbType.Int).Value = (int)nudTop.Value;
                    da.SelectCommand.Parameters.Add("@tuNgay", SqlDbType.DateTime).Value = dtpTuNgay.Value.Date;
                    da.SelectCommand.Parameters.Add("@denNgay", SqlDbType.DateTime).Value = dtpDenNgay.Value.Date.AddDays(1);

                    var dt = new DataTable();
                    da.Fill(dt);

                    dgvXeBanChay.DataSource = dt;
                    dgvXeBanChay.ReadOnly = true;
                    dgvXeBanChay.AllowUserToAddRows = false;
                    dgvXeBanChay.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dgvXeBanChay.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu bán hàng trong khoảng thời gian này.",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Giữ lại vì Designer đang gắn sự kiện này
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}