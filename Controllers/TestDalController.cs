using Microsoft.AspNetCore.Mvc;
using NET108_LAB3_NHOM2_SD22301.DAL;

namespace NET108_LAB3_NHOM2_SD22301.Controllers
{
    [Route("TestDal")]
    public class TestDalController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            try
            {
                // 1. Thử nghiệm GetAll() cho cả 6 bảng và đếm số dòng
                var categories = CategoryDAL.GetAll();
                var drinks = DrinkDAL.GetAll();
                var users = UserDAL.GetAll();
                var cards = CardDAL.GetAll();
                var bills = BillDAL.GetAll();
                var billDetails = BillDetailDAL.GetAll();

                // 2. Thử nghiệm GetById(1)
                var categoryById1 = CategoryDAL.GetById(1);
                var drinkById1 = DrinkDAL.GetById(1);
                var userById1 = UserDAL.GetById(1);

                // 3. Thử nghiệm Insert -> Update -> Delete với CategoryDAL
                var sampleCat = new CategoryDAL
                {
                    Name = "Test_" + DateTime.Now.Ticks,
                    Description = "Danh mục test tạm",
                    Active = true
                };

                // Insert
                int insertedId = sampleCat.Insert();

                // Update
                sampleCat.Name = "Test_Updated";
                sampleCat.Description = "Đã cập nhật";
                bool updateOk = sampleCat.Update();
                var categoryAfterUpdate = CategoryDAL.GetById(insertedId);

                // Delete
                bool deleteOk = CategoryDAL.Delete(insertedId);
                var categoryAfterDelete = CategoryDAL.GetById(insertedId);

                return Json(new
                {
                    success = true,
                    message = "Kiểm tra toàn bộ DAL thành công!",
                    recordCounts = new
                    {
                        categories = categories.Count,
                        drinks = drinks.Count,
                        users = users.Count,
                        cards = cards.Count,
                        bills = bills.Count,
                        billDetails = billDetails.Count
                    },
                    getByIdSample = new
                    {
                        category1 = categoryById1,
                        drink1 = drinkById1,
                        user1 = userById1
                    },
                    crudTestOnCategory = new
                    {
                        insertedId = insertedId,
                        updateSuccess = updateOk,
                        dataAfterUpdate = categoryAfterUpdate,
                        deleteSuccess = deleteOk,
                        existsAfterDelete = (categoryAfterDelete != null)
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
    }
}
