using System.Data;
using NET108_LAB3_NHOM2_SD22301.Utils;

namespace NET108_LAB3_NHOM2_SD22301.DAL
{
    /// <summary>
    /// Lớp truy xuất dữ liệu cho bảng drinks (Đồ uống)
    /// </summary>
    public class DrinkDAL
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public int Price { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; }

        /// <summary>
        /// Chuyển đổi dữ liệu từ DataRow sang đối tượng DrinkDAL
        /// </summary>
        private static DrinkDAL MapRow(DataRow row)
        {
            return new DrinkDAL
            {
                Id = Convert.ToInt32(row["id"]),
                CategoryId = Convert.ToInt32(row["category_id"]),
                Name = row["name"] == DBNull.Value ? null : row["name"].ToString(),
                Price = Convert.ToInt32(row["price"]),
                Image = row["image"] == DBNull.Value ? null : row["image"].ToString(),
                Description = row["description"] == DBNull.Value ? null : row["description"].ToString(),
                Active = Convert.ToBoolean(row["active"])
            };
        }

        /// <summary>
        /// Lấy toàn bộ danh sách đồ uống
        /// </summary>
        public static List<DrinkDAL> GetAll()
        {
            string sql = "SELECT id, category_id, name, price, image, description, active FROM drinks";
            DataTable dt = DBUtil.queryDataTable(sql, null);
            List<DrinkDAL> list = new List<DrinkDAL>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapRow(row));
            }
            return list;
        }

        /// <summary>
        /// Lấy thông tin đồ uống theo Id
        /// </summary>
        public static DrinkDAL? GetById(int id)
        {
            string sql = "SELECT id, category_id, name, price, image, description, active FROM drinks WHERE id = @1";
            List<object> parameters = new List<object> { id };
            DataTable dt = DBUtil.queryDataTable(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                return MapRow(dt.Rows[0]);
            }
            return null;
        }

        /// <summary>
        /// Thêm mới đồ uống vào CSDL và trả về Id vừa tạo
        /// </summary>
        public int Insert()
        {
            string sql = "INSERT INTO drinks (category_id, name, price, image, description, active) VALUES (@1, @2, @3, @4, @5, @6); SELECT SCOPE_IDENTITY();";
            List<object> parameters = new List<object>
            {
                CategoryId,
                Name ?? (object)DBNull.Value,
                Price,
                Image ?? (object)DBNull.Value,
                Description ?? (object)DBNull.Value,
                Active
            };
            object? result = DBUtil.executeScalar(sql, parameters);
            this.Id = result != null ? Convert.ToInt32(result) : 0;
            return this.Id;
        }

        /// <summary>
        /// Cập nhật thông tin đồ uống theo Id
        /// </summary>
        public bool Update()
        {
            string sql = "UPDATE drinks SET category_id = @1, name = @2, price = @3, image = @4, description = @5, active = @6 WHERE id = @7";
            List<object> parameters = new List<object>
            {
                CategoryId,
                Name ?? (object)DBNull.Value,
                Price,
                Image ?? (object)DBNull.Value,
                Description ?? (object)DBNull.Value,
                Active,
                Id
            };
            int rowsAffected = DBUtil.executeNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Xóa đồ uống theo Id
        /// </summary>
        public static bool Delete(int id)
        {
            string sql = "DELETE FROM drinks WHERE id = @1";
            List<object> parameters = new List<object> { id };
            int rowsAffected = DBUtil.executeNonQuery(sql, parameters);
            return rowsAffected > 0;
        }
    }
}
