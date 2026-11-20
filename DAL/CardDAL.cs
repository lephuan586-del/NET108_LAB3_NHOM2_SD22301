using System.Data;
using NET108_LAB3_NHOM2_SD22301.Utils;

namespace NET108_LAB3_NHOM2_SD22301.DAL
{
    /// <summary>
    /// Lớp truy xuất dữ liệu cho bảng cards (Thẻ bàn/Thẻ rung)
    /// </summary>
    public class CardDAL
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public bool Status { get; set; }

        /// <summary>
        /// Chuyển đổi dữ liệu từ DataRow sang đối tượng CardDAL
        /// </summary>
        private static CardDAL MapRow(DataRow row)
        {
            return new CardDAL
            {
                Id = Convert.ToInt32(row["id"]),
                Code = Convert.ToInt32(row["code"]),
                Status = Convert.ToBoolean(row["status"])
            };
        }

        /// <summary>
        /// Lấy toàn bộ danh sách thẻ
        /// </summary>
        public static List<CardDAL> GetAll()
        {
            string sql = "SELECT id, code, status FROM cards";
            DataTable dt = DBUtil.queryDataTable(sql, null);
            List<CardDAL> list = new List<CardDAL>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapRow(row));
            }
            return list;
        }

        /// <summary>
        /// Lấy thông tin thẻ theo Id
        /// </summary>
        public static CardDAL? GetById(int id)
        {
            string sql = "SELECT id, code, status FROM cards WHERE id = @1";
            List<object> parameters = new List<object> { id };
            DataTable dt = DBUtil.queryDataTable(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                return MapRow(dt.Rows[0]);
            }
            return null;
        }

        /// <summary>
        /// Thêm mới thẻ vào CSDL và trả về Id vừa tạo
        /// </summary>
        public int Insert()
        {
            string sql = "INSERT INTO cards (code, status) VALUES (@1, @2); SELECT SCOPE_IDENTITY();";
            List<object> parameters = new List<object>
            {
                Code,
                Status
            };
            object? result = DBUtil.executeScalar(sql, parameters);
            this.Id = result != null ? Convert.ToInt32(result) : 0;
            return this.Id;
        }

        /// <summary>
        /// Cập nhật thông tin thẻ theo Id
        /// </summary>
        public bool Update()
        {
            string sql = "UPDATE cards SET code = @1, status = @2 WHERE id = @3";
            List<object> parameters = new List<object>
            {
                Code,
                Status,
                Id
            };
            int rowsAffected = DBUtil.executeNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Xóa thẻ theo Id
        /// </summary>
        public static bool Delete(int id)
        {
            string sql = "DELETE FROM cards WHERE id = @1";
            List<object> parameters = new List<object> { id };
            int rowsAffected = DBUtil.executeNonQuery(sql, parameters);
            return rowsAffected > 0;
        }
    }
}
