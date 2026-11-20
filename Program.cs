using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quanlybanoto
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (frmDangNhap dn = new frmDangNhap())
            {
                if (dn.ShowDialog() != DialogResult.OK)
                    return; // Hủy hoặc đóng form đăng nhập thì thoát chương trình
            }

            Application.Run(new frmMain());
        }
    }
}