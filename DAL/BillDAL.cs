using System.Data;
using NET108_LAB3_NHOM2_SD22301.Utils;

namespace NET108_LAB3_NHOM2_SD22301.DAL
{
    /// <summary>
    /// Lớp truy xuất dữ liệu cho bảng bills (Hóa đơn)
    /// </summary>
    public class BillDAL
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? CardId { get; set; } // Cho phép NULL
        public string? Code { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Total { get; set; }
        public string? Status { get; set; } // PENDING / PAID / CANCELLED
        public string? CancelReason { get; set; }

        /// <summary>
        /// Chuyển đổi dữ liệu từ DataRow sang đối tượng BillDAL
        /// </summary>
        private static BillDAL MapRow(DataRow row)
        {
            return new BillDAL
            {
                Id = Convert.ToInt32(row["id"]),
                UserId = Convert.ToInt32(row["user_id"]),
                CardId = row["card_id"] == DBNull.Value ? null : Convert.ToInt32(row["card_id"]),
                Code = row["code"] == DBNull.Value ? null : row["code"].ToString(),
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                Total = Convert.ToInt32(row["total"]),
                Status = row["status"] == DBNull.Value ? null : row["status"].ToString(),
                CancelReason = row["cancel_reason"] == DBNull.Value ? null : row["cancel_reason"].ToString()
            };
        }

        /// <summary>
        /// Lấy toàn bộ danh sách hóa đơn
        /// </summary>
        public static List<BillDAL> GetAll()
        {
            string sql = "SELECT id, user_id, card_id, code, created_at, total, status, cancel_reason FROM bills";
            DataTable dt = DBUtil.queryDataTable(sql, null);
            List<BillDAL> list = new List<BillDAL>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapRow(row));
            }
            return list;
        }

        /// <summary>
        /// Lấy thông tin hóa đơn theo Id
        /// </summary>
        public static BillDAL? GetById(int id)
        {
            string sql = "SELECT id, user_id, card_id, code, created_at, total, status, cancel_reason FROM bills WHERE id = @1";
            List<object> parameters = new List<object> { id };
            DataTable dt = DBUtil.queryDataTable(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                return MapRow(dt.Rows[0]);
            }
            return null;
        }

        /// <summary>
        /// Thêm mới hóa đơn vào CSDL và trả về Id vừa tạo
        /// </summary>
        public int Insert()
        {
            string sql = "INSERT INTO bills (user_id, card_id, code, created_at, total, status, cancel_reason) VALUES (@1, @2, @3, @4, @5, @6, @7); SELECT SCOPE_IDENTITY();";
            List<object> parameters = new List<object>
            {
                UserId,
                CardId ?? (object)DBNull.Value,
                Code ?? (object)DBNull.Value,
                CreatedAt,
                Total,
                Status ?? (object)DBNull.Value,
                CancelReason ?? (object)DBNull.Value
            };
            object? result = DBUtil.executeScalar(sql, parameters);
            this.Id = result != null ? Convert.ToInt32(result) : 0;
            return this.Id;
        }

        /// <summary>
        /// Cập nhật thông tin hóa đơn theo Id
        /// </summary>
        public bool Update()
        {
            string sql = "UPDATE bills SET user_id = @1, card_id = @2, code = @3, created_at = @4, total = @5, status = @6, cancel_reason = @7 WHERE id = @8";
            List<object> parameters = new List<object>
            {
                UserId,
                CardId ?? (object)DBNull.Value,
                Code ?? (object)DBNull.Value,
                CreatedAt,
                Total,
                Status ?? (object)DBNull.Value,
                CancelReason ?? (object)DBNull.Value,
                Id
            };
            int rowsAffected = DBUtil.executeNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Xóa hóa đơn theo Id
        /// </summary>
        public static bool Delete(int id)
        {
            string sql = "DELETE FROM bills WHERE id = @1";
            List<object> parameters = new List<object> { id };
            int rowsAffected = DBUtil.executeNonQuery(sql, parameters);
            return rowsAffected > 0;
        }
    }
}
