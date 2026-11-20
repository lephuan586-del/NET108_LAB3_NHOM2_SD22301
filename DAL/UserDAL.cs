using System.Data;
using NET108_LAB3_NHOM2_SD22301.Utils;

namespace NET108_LAB3_NHOM2_SD22301.DAL
{
    /// <summary>
    /// Lớp truy xuất dữ liệu cho bảng users (Người dùng/Nhân viên)
    /// </summary>
    public class UserDAL
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public bool Role { get; set; } // 0 = Nhân viên, 1 = Quản lý
        public bool Active { get; set; }
        public string? ResetCode { get; set; }
        public DateTime? ResetExpiredAt { get; set; }

        /// <summary>
        /// Chuyển đổi dữ liệu từ DataRow sang đối tượng UserDAL
        /// </summary>
        private static UserDAL MapRow(DataRow row)
        {
            return new UserDAL
            {
                Id = Convert.ToInt32(row["id"]),
                Email = row["email"] == DBNull.Value ? null : row["email"].ToString(),
                Password = row["password"] == DBNull.Value ? null : row["password"].ToString(),
                FullName = row["full_name"] == DBNull.Value ? null : row["full_name"].ToString(),
                Phone = row["phone"] == DBNull.Value ? null : row["phone"].ToString(),
                Role = Convert.ToBoolean(row["role"]),
                Active = Convert.ToBoolean(row["active"]),
                ResetCode = row["reset_code"] == DBNull.Value ? null : row["reset_code"].ToString(),
                ResetExpiredAt = row["reset_expired_at"] == DBNull.Value ? null : Convert.ToDateTime(row["reset_expired_at"])
            };
        }

        /// <summary>
        /// Lấy toàn bộ danh sách người dùng
        /// </summary>
        public static List<UserDAL> GetAll()
        {
            string sql = "SELECT id, email, password, full_name, phone, role, active, reset_code, reset_expired_at FROM users";
            DataTable dt = DBUtil.queryDataTable(sql, null);
            List<UserDAL> list = new List<UserDAL>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapRow(row));
            }
            return list;
        }

        /// <summary>
        /// Lấy thông tin người dùng theo Id
        /// </summary>
        public static UserDAL? GetById(int id)
        {
            string sql = "SELECT id, email, password, full_name, phone, role, active, reset_code, reset_expired_at FROM users WHERE id = @1";
            List<object> parameters = new List<object> { id };
            DataTable dt = DBUtil.queryDataTable(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                return MapRow(dt.Rows[0]);
            }
            return null;
        }

        /// <summary>
        /// Thêm mới người dùng vào CSDL và trả về Id vừa tạo
        /// </summary>
        public int Insert()
        {
            string sql = "INSERT INTO users (email, password, full_name, phone, role, active, reset_code, reset_expired_at) VALUES (@1, @2, @3, @4, @5, @6, @7, @8); SELECT SCOPE_IDENTITY();";
            List<object> parameters = new List<object>
            {
                Email ?? (object)DBNull.Value,
                Password ?? (object)DBNull.Value,
                FullName ?? (object)DBNull.Value,
                Phone ?? (object)DBNull.Value,
                Role,
                Active,
                ResetCode ?? (object)DBNull.Value,
                ResetExpiredAt ?? (object)DBNull.Value
            };
            object? result = DBUtil.executeScalar(sql, parameters);
            this.Id = result != null ? Convert.ToInt32(result) : 0;
            return this.Id;
        }

        /// <summary>
        /// Cập nhật thông tin người dùng theo Id
        /// </summary>
        public bool Update()
        {
            string sql = "UPDATE users SET email = @1, password = @2, full_name = @3, phone = @4, role = @5, active = @6, reset_code = @7, reset_expired_at = @8 WHERE id = @9";
            List<object> parameters = new List<object>
            {
                Email ?? (object)DBNull.Value,
                Password ?? (object)DBNull.Value,
                FullName ?? (object)DBNull.Value,
                Phone ?? (object)DBNull.Value,
                Role,
                Active,
                ResetCode ?? (object)DBNull.Value,
                ResetExpiredAt ?? (object)DBNull.Value,
                Id
            };
            int rowsAffected = DBUtil.executeNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Xóa người dùng theo Id
        /// </summary>
        public static bool Delete(int id)
        {
            string sql = "DELETE FROM users WHERE id = @1";
            List<object> parameters = new List<object> { id };
            int rowsAffected = DBUtil.executeNonQuery(sql, parameters);
            return rowsAffected > 0;
        }
    }
}
