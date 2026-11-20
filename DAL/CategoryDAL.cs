using System.Data;
using NET108_LAB3_NHOM2_SD22301.Utils;

namespace NET108_LAB3_NHOM2_SD22301.DAL
{
    /// <summary>
    /// Lớp truy xuất dữ liệu cho bảng categories (Danh mục đồ uống)
    /// </summary>
    public class CategoryDAL
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; }

        /// <summary>
        /// Chuyển đổi dữ liệu từ DataRow sang đối tượng CategoryDAL
        /// </summary>
        private static CategoryDAL MapRow(DataRow row)
        {
            return new CategoryDAL
            {
                Id = Convert.ToInt32(row["id"]),
                Name = row["name"] == DBNull.Value ? null : row["name"].ToString(),
                Description = row["description"] == DBNull.Value ? null : row["description"].ToString(),
                Active = Convert.ToBoolean(row["active"])
            };
        }

        /// <summary>
        /// Lấy toàn bộ danh sách danh mục
        /// </summary>
        public static List<CategoryDAL> GetAll()
        {
            string sql = "SELECT id, name, description, active FROM categories";
            DataTable dt = DBUtil.queryDataTable(sql, null);
            List<CategoryDAL> list = new List<CategoryDAL>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapRow(row));
            }
            return list;
        }

        /// <summary>
        /// Lấy thông tin danh mục theo Id
        /// </summary>
        public static CategoryDAL? GetById(int id)
        {
            string sql = "SELECT id, name, description, active FROM categories WHERE id = @1";
            List<object> parameters = new List<object> { id };
            DataTable dt = DBUtil.queryDataTable(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                return MapRow(dt.Rows[0]);
            }
            return null;
        }

        /// <summary>
        /// Thêm mới danh mục vào CSDL và trả về Id vừa tạo
        /// </summary>
        public int Insert()
        {
            string sql = "INSERT INTO categories (name, description, active) VALUES (@1, @2, @3); SELECT SCOPE_IDENTITY();";
            List<object> parameters = new List<object>
            {
                Name ?? (object)DBNull.Value,
                Description ?? (object)DBNull.Value,
                Active
            };
            object? result = DBUtil.executeScalar(sql, parameters);
            this.Id = result != null ? Convert.ToInt32(result) : 0;
            return this.Id;
        }

        /// <summary>
        /// Cập nhật thông tin danh mục theo Id
        /// </summary>
        public bool Update()
        {
            string sql = "UPDATE categories SET name = @1, description = @2, active = @3 WHERE id = @4";
            List<object> parameters = new List<object>
            {
                Name ?? (object)DBNull.Value,
                Description ?? (object)DBNull.Value,
                Active,
                Id
            };
            int rowsAffected = DBUtil.executeNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Xóa danh mục theo Id
        /// </summary>
        public static bool Delete(int id)
        {
            string sql = "DELETE FROM categories WHERE id = @1";
            List<object> parameters = new List<object> { id };
            int rowsAffected = DBUtil.executeNonQuery(sql, parameters);
            return rowsAffected > 0;
        }
    }
}
