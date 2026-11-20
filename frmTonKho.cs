using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Quanlybanoto
{
    public partial class frmTonKho : Form
    {
        // Sửa lại cho đúng với máy của bạn (hoặc lấy từ DataProvider)
        private readonly string connStr =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyBanOto;Integrated Security=True";

        public frmTonKho()
        {
            InitializeComponent();

            // Sự kiện Load đã được Designer gắn sẵn (do hàm frmTonKho_Load có từ đầu),
            // nên chỉ gắn 3 nút ở đây. Nếu bạn đã double-click nút trong Designer
            // thì xóa dòng tương ứng để không bị chạy 2 lần.
            btnXem.Click += btnXem_Click;
            btnLamMoi.Click += btnLamMoi_Click;
            btnThoat.Click += btnThoat_Click;
        }

        private void frmTonKho_Load(object sender, EventArgs e)
        {
            nudNguong.Minimum = 0;
            nudNguong.Maximum = 1000;
            nudNguong.Value = 5;

            this.AcceptButton = btnXem;     // Enter = bấm Xem
            this.CancelButton = btnThoat;   // Esc = bấm Thoát
            cboHang.DropDownStyle = ComboBoxStyle.DropDownList;

            dgvTonKho.ReadOnly = true;
            dgvTonKho.AllowUserToAddRows = false;
            dgvTonKho.RowHeadersVisible = false;
            dgvTonKho.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTonKho.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            LoadHang();
            LoadTonKho();
        }

        // Đổ danh sách hãng vào ComboBox, dòng đầu là "Tất cả"
        private void LoadHang()
        {
            try
            {
                cboHang.Items.Clear();
                cboHang.Items.Add("Tất cả");

                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand(
                    "SELECT DISTINCT HangXe FROM Xe WHERE HangXe IS NOT NULL ORDER BY HangXe", conn))
                {
                    conn.Open();
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                            cboHang.Items.Add(rd.GetString(0));
                    }
                }
                cboHang.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách hãng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTonKho()
        {
            // Giả định: Xe(MaXe, TenXe, HangXe, MauSac, GiaBan, SoLuong)
            string sql = @"
                SELECT
                    MaXe                  AS [Mã xe],
                    TenXe                 AS [Tên xe],
                    HangXe                AS [Hãng],
                    MauSac                AS [Màu sắc],
                    GiaBan                AS [Giá bán],
                    SoLuong            AS [Tồn kho],
                    GiaBan * SoLuong   AS [Giá trị tồn (đ)],
                    CASE
                        WHEN SoLuong <= 0        THEN N'Hết hàng'
                        WHEN SoLuong <= @nguong  THEN N'Sắp hết'
                        ELSE N'Còn hàng'
                    END                   AS [Tình trạng]
                FROM Xe
                WHERE (MaXe LIKE @kw OR TenXe LIKE @kw)
                  AND (@hang = N'' OR HangXe = @hang)
                ORDER BY SoLuong ASC, MaXe";

            string hang = (cboHang.SelectedIndex <= 0) ? "" : cboHang.Text;

            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var da = new SqlDataAdapter(sql, conn))
                {
                    da.SelectCommand.Parameters.Add("@nguong", SqlDbType.Int).Value = (int)nudNguong.Value;
                    da.SelectCommand.Parameters.Add("@kw", SqlDbType.NVarChar, 100).Value =
                        "%" + txtTuKhoa.Text.Trim() + "%";
                    da.SelectCommand.Parameters.Add("@hang", SqlDbType.NVarChar, 100).Value = hang;

                    var dt = new DataTable();
                    da.Fill(dt);

                    dgvTonKho.DataSource = dt;
                    dgvTonKho.Columns["Giá bán"].DefaultCellStyle.Format = "N0";
                    dgvTonKho.Columns["Giá trị tồn (đ)"].DefaultCellStyle.Format = "N0";

                    TomMauDong();
                    CapNhatTongKet(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Chữ đỏ: hết hàng, chữ cam: sắp hết
        private void TomMauDong()
        {
            foreach (DataGridViewRow r in dgvTonKho.Rows)
            {
                string tt = Convert.ToString(r.Cells["Tình trạng"].Value);
                if (tt == "Hết hàng")
                    r.DefaultCellStyle.ForeColor = Color.Red;
                else if (tt == "Sắp hết")
                    r.DefaultCellStyle.ForeColor = Color.DarkOrange;
                else
                    r.DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void CapNhatTongKet(DataTable dt)
        {
            long tongXe = 0, hetHang = 0, sapHet = 0;
            decimal giaTri = 0;

            foreach (DataRow row in dt.Rows)
            {
                int ton = Convert.ToInt32(row["Tồn kho"]);
                tongXe += ton;
                giaTri += Convert.ToDecimal(row["Giá trị tồn (đ)"]);

                string tt = Convert.ToString(row["Tình trạng"]);
                if (tt == "Hết hàng") hetHang++;
                else if (tt == "Sắp hết") sapHet++;
            }

            var ci = CultureInfo.InvariantCulture;
            lblTongKet.Text =
                "Tổng " + tongXe.ToString("N0", ci) + " xe trong kho   |   " +
                "Giá trị tồn (theo giá bán): " + giaTri.ToString("N0", ci) + " đ   |   " +
                "Hết hàng: " + hetHang + "   |   " +
                "Sắp hết: " + sapHet;
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            LoadTonKho();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTuKhoa.Clear();
            cboHang.SelectedIndex = 0;
            nudNguong.Value = 5;
            LoadTonKho();
            txtTuKhoa.Focus();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLamMoi_Click_1(object sender, EventArgs e)
        {
            txtTuKhoa.Clear();
            nudNguong.Value = 5;
            LoadHang();      // tải lại danh sách hãng, tự chọn "Tất cả"
            LoadTonKho();
            txtTuKhoa.Focus();
        }
    }
}