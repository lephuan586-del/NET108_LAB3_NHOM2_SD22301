using System.Data;
using NET108_LAB3_NHOM2_SD22301.Utils;

namespace NET108_LAB3_NHOM2_SD22301.DAL
{
    /// <summary>
    /// Lớp truy xuất dữ liệu cho bảng bill_details (Chi tiết hóa đơn)
    /// </summary>
    public class BillDetailDAL
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        public int DrinkId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public string? Note { get; set; }

        /// <summary>
        /// Chuyển đổi dữ liệu từ DataRow sang đối tượng BillDetailDAL
        /// </summary>
        private static BillDetailDAL MapRow(DataRow row)
        {
            return new BillDetailDAL
            {
                Id = Convert.ToInt32(row["id"]),
                BillId = Convert.ToInt32(row["bill_id"]),
                DrinkId = Convert.ToInt32(row["drink_id"]),
                Quantity = Convert.ToInt32(row["quantity"]),
                Price = Convert.ToInt32(row["price"]),
                Note = row["note"] == DBNull.Value ? null : row["note"].ToString()
            };
        }

        /// <summary>
        /// Lấy toàn bộ danh sách chi tiết hóa đơn
        /// </summary>
        public static List<BillDetailDAL> GetAll()
        {
            string sql = "SELECT id, bill_id, drink_id, quantity, price, note FROM bill_details";
            DataTable dt = DBUtil.queryDataTable(sql, null);
            List<BillDetailDAL> list = new List<BillDetailDAL>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapRow(row));
            }
            return list;
        }

        /// <summary>
        /// Lấy thông tin chi tiết hóa đơn theo Id
        /// </summary>
        public static BillDetailDAL? GetById(int id)
        {
            string sql = "SELECT id, bill_id, drink_id, quantity, price, note FROM bill_details WHERE id = @1";
            List<object> parameters = new List<object> { id };
            DataTable dt = DBUtil.queryDataTable(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                return MapRow(dt.Rows[0]);
            }
            return null;
        }

        /// <summary>
        /// Thêm mới chi tiết hóa đơn vào CSDL và trả về Id vừa tạo
        /// </summary>
        public int Insert()
        {
            string sql = "INSERT INTO bill_details (bill_id, drink_id, quantity, price, note) VALUES (@1, @2, @3, @4, @5); SELECT SCOPE_IDENTITY();";
            List<object> parameters = new List<object>
            {
                BillId,
                DrinkId,
                Quantity,
                Price,
                Note ?? (object)DBNull.Value
            };
            object? result = DBUtil.executeScalar(sql, parameters);
            this.Id = result != null ? Convert.ToInt32(result) : 0;
            return this.Id;
        }

        /// <summary>
        /// Cập nhật thông tin chi tiết hóa đơn theo Id
        /// </summary>
        public bool Update()
        {
            string sql = "UPDATE bill_details SET bill_id = @1, drink_id = @2, quantity = @3, price = @4, note = @5 WHERE id = @6";
            List<object> parameters = new List<object>
            {
                BillId,
                DrinkId,
                Quantity,
                Price,
                Note ?? (object)DBNull.Value,
                Id
            };
            int rowsAffected = DBUtil.executeNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Xóa chi tiết hóa đơn theo Id
        /// </summary>
        public static bool Delete(int id)
        {
            string sql = "DELETE FROM bill_details WHERE id = @1";
            List<object> parameters = new List<object> { id };
            int rowsAffected = DBUtil.executeNonQuery(sql, parameters);
            return rowsAffected > 0;
        }
    }
}
