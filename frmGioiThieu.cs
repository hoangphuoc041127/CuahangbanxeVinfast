using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quanlybanoto
{
    public partial class frmGioiThieu : Form
    {
        private readonly ToolTip tip = new ToolTip();

        public frmGioiThieu()
        {
            InitializeComponent();

            // 1. Nút Thoát: bấm nút, Enter hoặc Esc đều đóng form
            Button btn = TimNut(this);
            if (btn != null)
            {
                btn.Click += (s, e) => this.Close();
                this.AcceptButton = btn;
                this.CancelButton = btn;
            }

            // 2. Label có địa chỉ web (http/https): bấm vào sẽ mở trình duyệt
            GanLink(this);

            // 3. Label nằm trong banner xanh: nền trong suốt, chữ trắng
            LamBanner(this);
        }

        // Tìm nút (Button) đầu tiên trên form
        private Button TimNut(Control cha)
        {
            foreach (Control c in cha.Controls)
            {
                if (c is Button b) return b;
                Button kq = TimNut(c);
                if (kq != null) return kq;
            }
            return null;
        }

        private void GanLink(Control cha)
        {
            foreach (Control c in cha.Controls)
            {
                if (c is Label lb)
                {
                    Match m = Regex.Match(lb.Text ?? "", @"https?://\S+");
                    if (m.Success)
                    {
                        string url = m.Value;
                        lb.Cursor = Cursors.Hand;
                        tip.SetToolTip(lb, "Bấm để mở: " + url);
                        lb.Click += (s, e) => MoLink(url);
                    }
                }
                GanLink(c);
            }
        }

        private void MoLink(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không mở được liên kết: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Label nằm trong Panel có nền tối thì cho nền trong suốt + chữ trắng
        private void LamBanner(Control cha)
        {
            foreach (Control c in cha.Controls)
            {
                if (c is Label lb && lb.Parent is Panel p && p.BackColor.GetBrightness() < 0.4f)
                {
                    lb.BackColor = Color.Transparent;
                    lb.ForeColor = Color.White;
                }
                LamBanner(c);
            }
        }

        private void frmGioiThieu_Load(object sender, EventArgs e)
        {

        }
    }
}