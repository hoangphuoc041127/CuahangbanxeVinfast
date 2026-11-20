using System;
using System.Data;
using System.Data.SqlClient;

namespace Quanlybanoto
{
    public static class NhaCungCapDAL
    {
        public const string BANG = "NhaCungCap";
        public const string COT_MA = "MaNCC";
        public const string COT_TEN = "TenNCC";
        public const string COT_SDT = "DienThoai";   // đổi nếu cột của bạn tên khác
        public const string COT_EMAIL = "Email";
        public const string COT_DIACHI = "DiaChi";

        public static DataTable LayDanhSach(string tuKhoa)
        {
            string sql = string.Format("SELECT * FROM {0}", BANG);
            tuKhoa = (tuKhoa ?? "").Trim();

            if (tuKhoa == "")
            {
                sql += string.Format(" ORDER BY {0}", COT_TEN);
                return DataProvider.GetData(sql);
            }

            sql += string.Format(" WHERE {0} LIKE @kw OR {1} LIKE @kw ORDER BY {1}", COT_MA, COT_TEN);
            using (SqlConnection conn = new SqlConnection(DataProvider.ConnectionString))
            using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
            {
                da.SelectCommand.Parameters.AddWithValue("@kw", "%" + tuKhoa + "%");
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static bool TonTai(string ma)
        {
            string sql = string.Format("SELECT COUNT(*) FROM {0} WHERE {1}=@ma", BANG, COT_MA);
            using (SqlConnection conn = new SqlConnection(DataProvider.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ma", ma);
                conn.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        private static int ChayLenh(string sql, string ma, string ten, string sdt, string email, string diaChi)
        {
            using (SqlConnection conn = new SqlConnection(DataProvider.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ma", ma);
                cmd.Parameters.AddWithValue("@ten", ten);
                cmd.Parameters.AddWithValue("@sdt", sdt);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@dc", diaChi);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static int Them(string ma, string ten, string sdt, string email, string diaChi)
        {
            string sql = string.Format(
                "INSERT INTO {0}({1},{2},{3},{4},{5}) VALUES(@ma,@ten,@sdt,@email,@dc)",
                BANG, COT_MA, COT_TEN, COT_SDT, COT_EMAIL, COT_DIACHI);
            return ChayLenh(sql, ma, ten, sdt, email, diaChi);
        }

        public static int Sua(string ma, string ten, string sdt, string email, string diaChi)
        {
            string sql = string.Format(
                "UPDATE {0} SET {2}=@ten,{3}=@sdt,{4}=@email,{5}=@dc WHERE {1}=@ma",
                BANG, COT_MA, COT_TEN, COT_SDT, COT_EMAIL, COT_DIACHI);
            return ChayLenh(sql, ma, ten, sdt, email, diaChi);
        }

        public static int Xoa(string ma)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}=@ma", BANG, COT_MA);
            using (SqlConnection conn = new SqlConnection(DataProvider.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ma", ma);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}