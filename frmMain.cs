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
    public partial class frmMain : Form
    {
        // Bảng ánh xạ: tên menu (đã chuẩn hóa) -> tên ảnh trong Resources (không có đuôi .png)
        private static readonly Dictionary<string, string> IconMenu = new Dictionary<string, string>
        {
            // Menu cha
            { "hệ thống", "settings" },
            { "quản lý", "car" },
            { "hóa đơn", "invoice" },
            { "tra cứu", "search" },
            { "thống kê - báo cáo", "chart" },
            { "trợ giúp", "help" },

            // Hệ thống
            { "đăng nhập", "login" },
            { "đăng ký", "register" },
            { "đổi mật khẩu", "lock" },
            { "quên mật khẩu", "lock" },
            { "thoát", "exit" },

            // Quản lý
            { "quản lý xe", "car" },
            { "xe", "car" },
            { "khách hàng", "customer" },
            { "nhân viên", "businessman" },
            { "nhà cung cấp", "delivery" },
            { "hãng xe", "factory" },

            // Hóa đơn
            { "bán hàng", "cash_register" },
            { "nhập xe", "import_icon" },

            // Tra cứu
            { "tra cứu xe", "search_car" },
            { "tra cứu hóa đơn", "search_document" },
            { "tra cứu hóa hơn", "search_document" },

            // Thống kê - Báo cáo
            { "doanh thu", "money_bag" },
            { "xe bán chạy", "trophy" },
            { "tồn kho", "inventory" },

            // Trợ giúp
            { "giới thiệu", "info" }
        };

        public frmMain()
        {
            InitializeComponent();

            // Chống nháy cho form chính
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            GanSuKienMenu();
        }

        // Vẽ cả cây control trong một lần (WS_EX_COMPOSITED) để hết nháy khi form con đóng
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        private void đăngNhậpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmDangNhap frm = new frmDangNhap())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Đăng nhập thành công!");
                }
            }
        }

        private static string Chuan(string s)
        {
            return (s ?? "").Trim().Normalize(NormalizationForm.FormC).ToLowerInvariant();
        }

        // Lấy ảnh từ Resources theo tên. Nếu chưa import ảnh thì trả về null (menu không có icon, không báo lỗi)
        private static Image LayIcon(string tenAnh)
        {
            try
            {
                return Properties.Resources.ResourceManager.GetObject(tenAnh) as Image;
            }
            catch
            {
                return null;
            }
        }

        // Mở form con ở dạng cửa sổ nhỏ gọn (mặc định 900x550) và căn giữa màn hình
        private void MoForm(Form f, int width = 900, int height = 550)
        {
            f.StartPosition = FormStartPosition.CenterParent;
            f.WindowState = FormWindowState.Normal;
            f.Size = new Size(width, height);
            f.ShowDialog(this);
        }

        private void GanSuKienMenu()
        {
            foreach (Control c in this.Controls)
            {
                MenuStrip ms = c as MenuStrip;
                if (ms != null)
                {
                    // Ảnh 50x50 sẽ tự thu nhỏ về 24x24 cho vừa menu
                    ms.ImageScalingSize = new Size(24, 24);
                    DuyetMenu(ms.Items, "");
                }
            }
        }

        private void DuyetMenu(ToolStripItemCollection items, string cha)
        {
            foreach (ToolStripItem it in items)
            {
                string ten = Chuan(it.Text);

                // Gắn icon cho menu (nếu có ảnh tương ứng trong Resources)
                string tenAnh;
                if (it is ToolStripMenuItem && IconMenu.TryGetValue(ten, out tenAnh))
                {
                    Image img = LayIcon(tenAnh);
                    if (img != null)
                    {
                        it.Image = img;
                        it.ImageScaling = ToolStripItemImageScaling.SizeToFit;
                    }
                }

                if (ten == Chuan("Đổi mật khẩu"))
                {
                    it.Click += (s, e) =>
                    {
                        using (frmDoiMatKhau f = new frmDoiMatKhau())
                            f.ShowDialog();
                    };
                }
                else if (ten == Chuan("Quên mật khẩu"))
                {
                    it.Click += (s, e) =>
                    {
                        using (frmQuenMatKhau f = new frmQuenMatKhau())
                            f.ShowDialog();
                    };
                }
                else if (ten == Chuan("Quản lý xe")
                         || (ten == Chuan("Xe") && cha == Chuan("Quản lý")))
                {
                    it.Click += (s, e) =>
                    {
                        using (frmQuanLyXe f = new frmQuanLyXe())
                            MoForm(f, 920, 560); // Kích thước siêu gọn cho Quản lý xe
                    };
                }
                else if (ten == Chuan("Khách hàng") && cha == Chuan("Quản lý"))
                {
                    it.Click += (s, e) =>
                    {
                        using (frmKhachHang f = new frmKhachHang())
                            MoForm(f, 850, 520);
                    };
                }
                else if (ten == Chuan("Nhân viên") && cha == Chuan("Quản lý"))
                {
                    it.Click += (s, e) =>
                    {
                        using (frmNhanVien f = new frmNhanVien())
                            MoForm(f, 850, 520);
                    };
                }
                else if (ten == Chuan("Nhà cung cấp"))
                {
                    it.Click += (s, e) =>
                    {
                        using (frmNhaCungCap f = new frmNhaCungCap())
                            MoForm(f, 800, 480);
                    };
                }
                else if (ten == Chuan("Bán hàng") && cha == Chuan("Hóa đơn"))
                {
                    it.Click += (s, e) =>
                    {
                        using (frmHoaDon f = new frmHoaDon())
                            MoForm(f, 950, 580); // Form Hóa đơn vừa vặn
                    };
                }
                else if (ten == Chuan("Nhập xe") && cha == Chuan("Hóa đơn"))
                {
                    it.Click += (s, e) =>
                    {
                        using (frmNhapXe f = new frmNhapXe())
                            MoForm(f, 950, 580);
                    };
                }
                else if (ten == Chuan("Tra cứu xe"))
                {
                    it.Click += (s, e) =>
                    {
                        using (frmTraCuuXe f = new frmTraCuuXe())
                            MoForm(f, 880, 520);
                    };
                }
                else if (ten == Chuan("Tra cứu hóa đơn") || ten == Chuan("Tra cứu hóa hơn"))
                {
                    it.Click += (s, e) =>
                    {
                        using (frmTraCuuHoaDon f = new frmTraCuuHoaDon())
                            MoForm(f, 900, 540);
                    };
                }
                else if (ten == Chuan("Doanh thu"))
                {
                    it.Click += (s, e) =>
                    {
                        using (frmDoanhThu f = new frmDoanhThu())
                            MoForm(f, 880, 520);
                    };
                }
                else if (ten == Chuan("Xe bán chạy"))
                {
                    it.Click += (s, e) =>
                    {
                        using (frmXeBanChay f = new frmXeBanChay())
                            MoForm(f, 880, 520);
                    };
                }
                else if (ten == Chuan("Tồn kho"))
                {
                    it.Click += (s, e) =>
                    {
                        using (frmTonKho f = new frmTonKho())
                            MoForm(f, 880, 520);
                    };
                }
                else if (ten == Chuan("Giới thiệu"))
                {
                    it.Click += (s, e) =>
                    {
                        using (frmGioiThieu f = new frmGioiThieu())
                            f.ShowDialog(this);
                    };
                }
                else if (ten == Chuan("Đăng ký"))
                {
                    it.Click += (s, e) =>
                    {
                        using (frmDangKy f = new frmDangKy())
                            f.ShowDialog();
                    };
                }
                else if (ten == Chuan("Thoát"))
                {
                    it.Click += (s, e) => Application.Exit();
                }

                ToolStripMenuItem m = it as ToolStripMenuItem;
                if (m != null && m.HasDropDownItems)
                    DuyetMenu(m.DropDownItems, ten);
            }
        }

        // Phân quyền: tài khoản không phải admin sẽ bị ẩn các chức năng quản trị
        private void PhanQuyen(string taiKhoan)
        {
            string tkLower = Chuan(taiKhoan);
            if (tkLower == "admin") return; // Thay "admin" bằng tài khoản quản trị của bạn

            string[] anDi =
            {
                Chuan("Quản lý"), Chuan("Hóa đơn"),
                Chuan("Doanh thu"), Chuan("Tồn kho"), Chuan("Xe bán chạy")
            };

            foreach (Control c in this.Controls)
            {
                MenuStrip ms = c as MenuStrip;
                if (ms != null) AnMenu(ms.Items, anDi);
            }
        }

        private void AnMenu(ToolStripItemCollection items, string[] anDi)
        {
            foreach (ToolStripItem item in items)
            {
                if (Array.IndexOf(anDi, Chuan(item.Text)) >= 0)
                {
                    item.Visible = false; // hoặc item.Enabled = false; để mờ đi
                    continue;
                }

                ToolStripMenuItem mi = item as ToolStripMenuItem;
                if (mi != null && mi.HasDropDownItems)
                {
                    AnMenu(mi.DropDownItems, anDi);

                    // Nếu tất cả menu con đều bị ẩn thì ẩn luôn menu cha
                    bool conHienThi = false;
                    foreach (ToolStripItem con in mi.DropDownItems)
                    {
                        if (con.Visible && !(con is ToolStripSeparator))
                        {
                            conHienThi = true;
                            break;
                        }
                    }
                    if (!conHienThi) mi.Visible = false;
                }
            }
        }

        private void hóaĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Form chính vẫn giữ nguyên phóng to toàn màn hình
            this.WindowState = FormWindowState.Maximized;

            // Phân quyền dựa vào tài khoản đang đăng nhập
            PhanQuyen(PhienDangNhap.TaiKhoan);
        }

        private void hãngXeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmHangXe f = new frmHangXe();
            f.StartPosition = FormStartPosition.CenterParent;
            f.Size = new Size(800, 480);
            f.ShowDialog(this);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void hệThốngToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}