using System.Data;
using Microsoft.Data.SqlClient;

namespace NET108_LAB3_NHOM2_SD22301.Utils
{
    /// <summary>
    /// Lớp tiện ích thực thi các thao tác cơ sở dữ liệu qua ADO.NET
    /// </summary>
    public static class DBUtil
    {
        /// <summary>
        /// Chuỗi kết nối tới cơ sở dữ liệu SQL Server
        /// </summary>
        public static string connString = "";

        /// <summary>
        /// Thêm danh sách tham số vào SqlCommand theo vị trí @1, @2, @3...
        /// </summary>
        private static void AddParameters(SqlCommand cmd, List<object>? parameters)
        {
            if (parameters == null || parameters.Count == 0) return;

            for (int i = 0; i < parameters.Count; i++)
            {
                string paramName = $"@{i + 1}";
                object val = parameters[i] ?? DBNull.Value;

                if (val is string strVal)
                {
                    // Tham số chuỗi dùng SqlDbType.NVarChar để hỗ trợ Unicode tiếng Việt
                    var param = new SqlParameter(paramName, SqlDbType.NVarChar)
                    {
                        Value = strVal
                    };
                    cmd.Parameters.Add(param);
                }
                else
                {
                    cmd.Parameters.AddWithValue(paramName, val);
                }
            }
        }

        /// <summary>
        /// Thực thi câu lệnh SELECT và trả về kết quả dưới dạng DataTable
        /// </summary>
        public static DataTable queryDataTable(string sql, List<object>? parameters)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        AddParameters(cmd, parameters);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi queryDataTable: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Thực thi câu lệnh trả về giá trị đơn (1 ô dữ liệu) như COUNT, SCOPE_IDENTITY...
        /// </summary>
        public static object? executeScalar(string sql, List<object>? parameters)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        AddParameters(cmd, parameters);
                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi executeScalar: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Thực thi câu lệnh INSERT, UPDATE, DELETE và trả về số dòng bị ảnh hưởng
        /// </summary>
        public static int executeNonQuery(string sql, List<object>? parameters)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        AddParameters(cmd, parameters);
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi executeNonQuery: {ex.Message}");
                throw;
            }
        }
    }
}
