using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Quanlybanoto
{
    public partial class frmTraCuuXe : Form
    {
        // Bảng Xe: MaXe, TenXe, HangXe, NamSX, MauSac, GiaBan, SoLuong, GhiChu, HinhAnh

        private bool daNap = false;

        public frmTraCuuXe()
        {
            InitializeComponent();

            // Gắn sự kiện bằng code. Nếu bạn đã double-click nút trong Designer
            // thì xóa dòng tương ứng ở đây để tránh chạy 2 lần.
            this.Load += frmTraCuuXe_Load;
            btnTim.Click += (s, e) => TimKiem();
            btnLamMoi.Click += btnLamMoi_Click;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            txtTuKhoa.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) { TimKiem(); e.SuppressKeyPress = true; }
            };
            cboHang.SelectedIndexChanged += (s, e) => { if (daNap) TimKiem(); };
            cboTonKho.SelectedIndexChanged += (s, e) => { if (daNap) TimKiem(); };
            dgvXe.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) btnSua_Click(s, e); };
        }

        // ====================== LOAD ======================
        private void frmTraCuuXe_Load(object sender, EventArgs e)
        {
            dgvXe.ReadOnly = true;
            dgvXe.AllowUserToAddRows = false;
            dgvXe.AllowUserToDeleteRows = false;
            dgvXe.MultiSelect = false;
            dgvXe.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvXe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvXe.RowHeadersVisible = false;

            cboHang.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTonKho.DropDownStyle = ComboBoxStyle.DropDownList;

            try
            {
                NapComboTonKho();
                NapComboHang();
                daNap = true;
                TimKiem();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không đọc được dữ liệu bảng Xe: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NapComboTonKho()
        {
            cboTonKho.Items.Clear();
            cboTonKho.Items.Add("-- Tất cả --");
            cboTonKho.Items.Add("Còn hàng");
            cboTonKho.Items.Add("Sắp hết (≤ 5)");
            cboTonKho.Items.Add("Hết hàng");
            cboTonKho.SelectedIndex = 0;
        }

        private void NapComboHang()
        {
            DataTable dt = DataProvider.ExecuteQuery(
                "SELECT DISTINCT HangXe FROM Xe WHERE HangXe IS NOT NULL ORDER BY HangXe");

            List<string> ds = new List<string> { "-- Tất cả --" };
            foreach (DataRow r in dt.Rows) ds.Add(r["HangXe"].ToString());

            bool cu = daNap;
            daNap = false;
            cboHang.DataSource = ds;
            cboHang.SelectedIndex = 0;
            daNap = cu;
        }

        // ====================== TÌM KIẾM ======================
        private void TimKiem()
        {
            try
            {
                List<string> dk = new List<string>();
                List<SqlParameter> ps = new List<SqlParameter>();

                string kw = txtTuKhoa.Text.Trim();
                if (kw != "")
                {
                    dk.Add("(MaXe LIKE @kw OR TenXe LIKE @kw OR HangXe LIKE @kw OR MauSac LIKE @kw)");
                    ps.Add(new SqlParameter("@kw", "%" + kw + "%"));
                }

                if (cboHang.SelectedIndex > 0)
                {
                    dk.Add("HangXe = @hang");
                    ps.Add(new SqlParameter("@hang", cboHang.SelectedItem.ToString()));
                }

                switch (cboTonKho.SelectedIndex)
                {
                    case 1: dk.Add("ISNULL(SoLuong,0) > 0"); break;
                    case 2: dk.Add("ISNULL(SoLuong,0) > 0 AND ISNULL(SoLuong,0) <= 5"); break;
                    case 3: dk.Add("ISNULL(SoLuong,0) = 0"); break;
                }

                string sql = "SELECT MaXe, TenXe, HangXe, NamSX, MauSac, GiaBan, SoLuong, GhiChu FROM Xe";
                if (dk.Count > 0) sql += " WHERE " + string.Join(" AND ", dk.ToArray());
                sql += " ORDER BY MaXe";

                DataTable dt = DataProvider.ExecuteQuery(sql, ps.ToArray());

                dgvXe.DataSource = dt;
                DinhDangLuoi();
                this.Text = "Tra cứu xe - " + dt.Rows.Count + " kết quả";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DinhDangLuoi()
        {
            SetCot("MaXe", "Mã xe");
            SetCot("TenXe", "Tên xe");
            SetCot("HangXe", "Hãng");
            SetCot("NamSX", "Năm SX");
            SetCot("MauSac", "Màu sắc");
            SetCot("GiaBan", "Giá bán");
            SetCot("SoLuong", "Số lượng");
            SetCot("GhiChu", "Ghi chú");

            if (dgvXe.Columns.Contains("GiaBan"))
            {
                dgvXe.Columns["GiaBan"].DefaultCellStyle.Format = "N0";
                dgvXe.Columns["GiaBan"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;
            }
            if (dgvXe.Columns.Contains("SoLuong"))
                dgvXe.Columns["SoLuong"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
            if (dgvXe.Columns.Contains("NamSX"))
                dgvXe.Columns["NamSX"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;

            // Tô đỏ xe hết hàng
            foreach (DataGridViewRow row in dgvXe.Rows)
            {
                object v = row.Cells["SoLuong"].Value;
                int sl;
                if (v == null || v == DBNull.Value || (int.TryParse(v.ToString(), out sl) && sl == 0))
                    row.DefaultCellStyle.ForeColor = Color.Red;
            }
        }

        private void SetCot(string ten, string tieuDe)
        {
            if (dgvXe.Columns.Contains(ten))
                dgvXe.Columns[ten].HeaderText = tieuDe;
        }

        // ====================== LÀM MỚI ======================
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            daNap = false;
            txtTuKhoa.Clear();
            NapComboHang();
            cboTonKho.SelectedIndex = 0;
            daNap = true;
            TimKiem();
            txtTuKhoa.Focus();
        }

        // ====================== THÊM ======================
        private void btnThem_Click(object sender, EventArgs e)
        {
            string[] kq = HopThoaiNhapXe(false, null);
            if (kq == null) return;

            try
            {
                if (TonTaiMaXe(kq[0]))
                {
                    MessageBox.Show("Mã xe \"" + kq[0] + "\" đã tồn tại!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataProvider.Execute(
                    "INSERT INTO Xe(MaXe, TenXe, HangXe, NamSX, MauSac, GiaBan, SoLuong, GhiChu) " +
                    "VALUES(@ma, @ten, @hang, @nam, @mau, @gia, @sl, @ghichu)", ThamSo(kq));

                MessageBox.Show("Thêm xe thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                NapComboHang();
                TimKiem();
                ChonDong(kq[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thêm được: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====================== SỬA ======================
        private void btnSua_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = DongDangChon();
            if (row == null)
            {
                MessageBox.Show("Hãy chọn một xe để sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            decimal gia;
            string giaStr = "0";
            if (decimal.TryParse(S(row.Cells["GiaBan"].Value), out gia)) giaStr = gia.ToString("0");

            string[] cu =
            {
                S(row.Cells["MaXe"].Value),
                S(row.Cells["TenXe"].Value),
                S(row.Cells["HangXe"].Value),
                S(row.Cells["NamSX"].Value),
                S(row.Cells["MauSac"].Value),
                giaStr,
                S(row.Cells["SoLuong"].Value) == "" ? "0" : S(row.Cells["SoLuong"].Value),
                S(row.Cells["GhiChu"].Value)
            };

            string[] kq = HopThoaiNhapXe(true, cu);
            if (kq == null) return;

            try
            {
                DataProvider.Execute(
                    "UPDATE Xe SET TenXe=@ten, HangXe=@hang, NamSX=@nam, MauSac=@mau, " +
                    "GiaBan=@gia, SoLuong=@sl, GhiChu=@ghichu WHERE MaXe=@ma", ThamSo(kq));

                MessageBox.Show("Cập nhật thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                NapComboHang();
                TimKiem();
                ChonDong(kq[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không sửa được: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====================== XÓA ======================
        private void btnXoa_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = DongDangChon();
            if (row == null)
            {
                MessageBox.Show("Hãy chọn một xe để xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string ma = S(row.Cells["MaXe"].Value);
            string ten = S(row.Cells["TenXe"].Value);

            if (MessageBox.Show("Xóa xe \"" + ten + "\" (" + ma + ")?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                DataProvider.Execute("DELETE FROM Xe WHERE MaXe=@ma",
                    new SqlParameter("@ma", ma));

                MessageBox.Show("Đã xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                NapComboHang();
                TimKiem();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547) // vi phạm khóa ngoại
                    MessageBox.Show("Không thể xóa: xe này đã có trong hóa đơn.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("Không xóa được: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====================== HÀM PHỤ ======================
        private static string S(object o)
        {
            return (o == null || o == DBNull.Value) ? "" : o.ToString();
        }

        private DataGridViewRow DongDangChon()
        {
            if (dgvXe.CurrentRow == null || dgvXe.CurrentRow.IsNewRow) return null;
            return dgvXe.CurrentRow;
        }

        private void ChonDong(string maXe)
        {
            foreach (DataGridViewRow r in dgvXe.Rows)
            {
                if (S(r.Cells["MaXe"].Value) == maXe)
                {
                    dgvXe.ClearSelection();
                    r.Selected = true;
                    dgvXe.CurrentCell = r.Cells[1];
                    return;
                }
            }
        }

        private bool TonTaiMaXe(string ma)
        {
            object kq = DataProvider.ExecuteScalar(
                "SELECT COUNT(*) FROM Xe WHERE MaXe=@ma", new SqlParameter("@ma", ma));
            return Convert.ToInt32(kq) > 0;
        }

        // v = { MaXe, TenXe, HangXe, NamSX, MauSac, GiaBan, SoLuong, GhiChu }
        private SqlParameter[] ThamSo(string[] v)
        {
            object nam = (v[3] == "") ? (object)DBNull.Value : int.Parse(v[3]);
            object mau = (v[4] == "") ? (object)DBNull.Value : v[4];
            object ghiChu = (v[7] == "") ? (object)DBNull.Value : v[7];

            return new SqlParameter[]
            {
                new SqlParameter("@ma", v[0]),
                new SqlParameter("@ten", v[1]),
                new SqlParameter("@hang", v[2]),
                new SqlParameter("@nam", nam),
                new SqlParameter("@mau", mau),
                new SqlParameter("@gia", decimal.Parse(v[5])),
                new SqlParameter("@sl", int.Parse(v[6])),
                new SqlParameter("@ghichu", ghiChu)
            };
        }

        // Hộp thoại nhập xe (dựng bằng code, dùng chung cho Thêm và Sửa).
        // Trả về null nếu bấm Hủy.
        private string[] HopThoaiNhapXe(bool sua, string[] cu)
        {
            string[] nhan =
            {
                "Mã xe:", "Tên xe:", "Hãng:", "Năm SX:",
                "Màu sắc:", "Giá bán:", "Số lượng:", "Ghi chú:"
            };
            int n = nhan.Length;
            TextBox[] txt = new TextBox[n];

            using (Form f = new Form())
            {
                f.Text = sua ? "Sửa thông tin xe" : "Thêm xe mới";
                f.StartPosition = FormStartPosition.CenterParent;
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.MaximizeBox = false;
                f.MinimizeBox = false;
                f.ClientSize = new Size(400, 40 + n * 36 + 60);

                for (int i = 0; i < n; i++)
                {
                    Label lb = new Label();
                    lb.Text = nhan[i];
                    lb.Location = new Point(20, 24 + i * 36);
                    lb.AutoSize = true;
                    f.Controls.Add(lb);

                    txt[i] = new TextBox();
                    txt[i].Location = new Point(120, 20 + i * 36);
                    txt[i].Width = 250;
                    if (cu != null) txt[i].Text = cu[i];
                    f.Controls.Add(txt[i]);
                }
                if (sua) txt[0].ReadOnly = true; // không cho đổi mã khi sửa
                else
                {
                    txt[2].Text = "VinFast";                      // mặc định hãng
                    txt[3].Text = DateTime.Now.Year.ToString();   // mặc định năm hiện tại
                    txt[6].Text = "0";
                }

                int yNut = 40 + n * 36;
                Button ok = new Button();
                ok.Text = "Lưu";
                ok.Location = new Point(190, yNut);
                ok.Size = new Size(90, 30);
                Button huy = new Button();
                huy.Text = "Hủy";
                huy.Location = new Point(290, yNut);
                huy.Size = new Size(90, 30);
                huy.DialogResult = DialogResult.Cancel;
                f.Controls.Add(ok);
                f.Controls.Add(huy);
                f.AcceptButton = ok;
                f.CancelButton = huy;

                string[] kq = null;
                ok.Click += (s, e) =>
                {
                    string loi = KiemTra(txt);
                    if (loi != null)
                    {
                        MessageBox.Show(loi, "Dữ liệu chưa hợp lệ",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    kq = new string[n];
                    for (int i = 0; i < n; i++) kq[i] = txt[i].Text.Trim();
                    f.DialogResult = DialogResult.OK;
                };

                f.ShowDialog(this);
                return kq;
            }
        }

        private string KiemTra(TextBox[] t)
        {
            if (t[0].Text.Trim() == "") return "Chưa nhập mã xe.";
            if (t[1].Text.Trim() == "") return "Chưa nhập tên xe.";
            if (t[2].Text.Trim() == "") return "Chưa nhập hãng xe.";

            int nam;
            if (t[3].Text.Trim() != "" &&
                (!int.TryParse(t[3].Text.Trim(), out nam) || nam < 1900 || nam > 2100))
                return "Năm sản xuất phải là số từ 1900 đến 2100 (hoặc để trống).";

            decimal gia;
            if (!decimal.TryParse(t[5].Text.Trim(), out gia) || gia < 0)
                return "Giá bán phải là số không âm.";

            int sl;
            if (!int.TryParse(t[6].Text.Trim(), out sl) || sl < 0)
                return "Số lượng phải là số nguyên không âm.";

            return null;
        }

        private void btnTim_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}