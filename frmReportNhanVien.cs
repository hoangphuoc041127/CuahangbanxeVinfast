using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace Quanlybanoto
{
    public partial class frmReportNhanVien : Form
    {
        private readonly DataTable dt;

        public frmReportNhanVien(DataTable data)
        {
            InitializeComponent();
            dt = data;

            // Gỡ trước rồi gắn lại để không bị chạy hai lần nếu Designer đã gắn sẵn
            Load -= frmReportNhanVien_Load;
            Load += frmReportNhanVien_Load;
        }

        private void frmReportNhanVien_Load(object sender, EventArgs e)
        {
            try
            {
                reportViewer1.LocalReport.ReportPath =
                    Path.Combine(Application.StartupPath, "rptNhanVien.rdlc");

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(
                    new ReportDataSource("dsNhanVien", dt));

                reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.PageWidth;
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không hiển thị được báo cáo: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}