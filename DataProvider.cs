using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Quanlybanoto
{
    public static class DataProvider
    {
        // Đã trỏ sẵn vào database QuanLyBanOTo trên .\SQLEXPRESS
        private static readonly string connStr =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyBanOTo;Integrated Security=True";

        // Cho các lớp khác (NhaCungCapDAL, frmNhapXe) dùng chung chuỗi kết nối
        public static string ConnectionString
        {
            get { return connStr; }
        }

        public static object ExecuteScalar(string sql, params SqlParameter[] ps)
        {
            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (ps != null) cmd.Parameters.AddRange(ps);
                conn.Open();
                return cmd.ExecuteScalar();
            }
        }

        public static int Execute(string sql, params SqlParameter[] ps)
        {
            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (ps != null) cmd.Parameters.AddRange(ps);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        // Trả về một bảng dữ liệu (dùng để đổ vào DataGridView)
        public static DataTable ExecuteQuery(string sql, params SqlParameter[] ps)
        {
            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand(sql, conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                if (ps != null) cmd.Parameters.AddRange(ps);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // Tên khác của ExecuteQuery, để các form viết theo kiểu GetData vẫn chạy được
        public static DataTable GetData(string sql, params SqlParameter[] ps)
        {
            return ExecuteQuery(sql, ps);
        }

        // Tạo kết nối mới, dùng khi cần giao dịch (transaction), ví dụ lưu hóa đơn
        public static SqlConnection TaoKetNoi()
        {
            return new SqlConnection(connStr);
        }

        public static string BamMatKhau(string matKhau)
        {
            using (var sha = SHA256.Create())
            {
                byte[] data = sha.ComputeHash(Encoding.UTF8.GetBytes(matKhau));
                var sb = new StringBuilder();
                foreach (byte b in data) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }

    // Lưu tài khoản đang đăng nhập (để hóa đơn ghi được nhân viên nào lập).
    // Chưa đăng nhập thì TenDangNhap = null, hóa đơn vẫn lưu được (cột NguoiLap để trống).
    public static class PhienLamViec
    {
        public static string TenDangNhap { get; set; }
        public static string HoTen { get; set; }
        public static string Quyen { get; set; }
    }
}