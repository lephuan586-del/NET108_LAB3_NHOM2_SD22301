/* =====================================================================
   DỰ ÁN .NET 1 - LAB 3 - BÀI 1: TẠO CƠ SỞ DỮ LIỆU
   Hệ thống quản lý bán đồ uống - Nhóm 2 (DoMixiProject) - Lớp SD22301
   CSDL: QuanLyQuanCafe (SQL Server)
   Gồm 6 bảng theo ERD Level 2 của tài liệu giới thiệu dự án:
   categories, drinks, users, cards, bills, bill_details
   ===================================================================== */

USE master;
GO

-- Xóa CSDL cũ (nếu có) để chạy lại script từ đầu
IF DB_ID(N'QuanLyQuanCafe') IS NOT NULL
BEGIN
    ALTER DATABASE QuanLyQuanCafe SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLyQuanCafe;
END
GO

CREATE DATABASE QuanLyQuanCafe;
GO

USE QuanLyQuanCafe;
GO

/* =====================================================================
   PHẦN 1: TẠO BẢNG
   ===================================================================== */

-- 1. Loại đồ uống (UC09)
CREATE TABLE categories (
    id          INT PRIMARY KEY IDENTITY,
    name        NVARCHAR(250) NOT NULL UNIQUE,
    description NVARCHAR(250) NULL,
    active      BIT NOT NULL DEFAULT 1          -- 1 = đang dùng, 0 = ngừng dùng
);
GO

-- 2. Đồ uống (UC10)
CREATE TABLE drinks (
    id          INT PRIMARY KEY IDENTITY,
    category_id INT NOT NULL FOREIGN KEY REFERENCES categories(id),
    name        NVARCHAR(250) NOT NULL,
    price       INT NOT NULL CHECK (price > 0), -- giá bán hiện tại (đồng)
    image       NVARCHAR(250) NULL,
    description NVARCHAR(MAX) NULL,
    active      BIT NOT NULL DEFAULT 1          -- 1 = đang bán, 0 = ngừng bán
);
GO

-- 3. Người dùng: Nhân viên + Quản lý (UC01, UC02, UC03, UC04, UC11)
CREATE TABLE users (
    id               INT PRIMARY KEY IDENTITY,
    email            VARCHAR(100) NOT NULL UNIQUE,   -- dùng làm tên đăng nhập
    password         VARCHAR(255) NOT NULL,          -- mật khẩu đã mã hóa
    full_name        NVARCHAR(100) NOT NULL,
    phone            VARCHAR(10) NOT NULL UNIQUE,
    role             BIT NOT NULL,                   -- 0 = Nhân viên, 1 = Quản lý
    active           BIT NOT NULL DEFAULT 1,         -- 1 = hoạt động, 0 = bị khóa
    reset_code       VARCHAR(10) NULL,               -- mã xác nhận quên mật khẩu
    reset_expired_at DATETIME NULL                   -- thời hạn của mã
);
GO

-- 4. Thẻ gắn với phiếu bán hàng (UC05)
CREATE TABLE cards (
    id     INT PRIMARY KEY IDENTITY,
    code   INT NOT NULL UNIQUE,                      -- số thẻ
    status BIT NOT NULL DEFAULT 1                    -- 1 = đang dùng, 0 = ngừng dùng
);
GO

-- 5. Phiếu bán hàng / Hóa đơn (UC05, UC06, UC07, UC08, UC12, UC13)
CREATE TABLE bills (
    id            INT PRIMARY KEY IDENTITY,
    user_id       INT NOT NULL FOREIGN KEY REFERENCES users(id),
    card_id       INT NULL FOREIGN KEY REFERENCES cards(id),
    code          VARCHAR(10) NOT NULL UNIQUE,
    created_at    DATETIME NOT NULL DEFAULT GETDATE(),
    total         INT NOT NULL DEFAULT 0,            -- tổng tiền (đồng)
    status        VARCHAR(10) NOT NULL DEFAULT 'PENDING',
    cancel_reason NVARCHAR(250) NULL,                -- lý do hủy / điều chỉnh
    CONSTRAINT CK_bills_status CHECK (status IN ('PENDING', 'PAID', 'CANCELLED'))
);
GO

-- 6. Chi tiết phiếu bán hàng (bảng trung gian bills - drinks)
CREATE TABLE bill_details (
    id       INT PRIMARY KEY IDENTITY,
    bill_id  INT NOT NULL FOREIGN KEY REFERENCES bills(id),
    drink_id INT NOT NULL FOREIGN KEY REFERENCES drinks(id),
    quantity INT NOT NULL CHECK (quantity > 0),
    price    INT NOT NULL,                           -- đơn giá tại thời điểm bán
    note     NVARCHAR(250) NULL                      -- ít đường, nhiều đá, ...
);
GO

/* =====================================================================
   PHẦN 2: THÊM DỮ LIỆU MẪU
   (Chạy trên CSDL vừa tạo mới nên id tự tăng bắt đầu từ 1)
   ===================================================================== */

-- ---------- categories ----------
INSERT INTO categories (name, description, active) VALUES
(N'Cà phê',   N'Các loại cà phê pha phin và pha máy',        1),
(N'Trà',      N'Trà trái cây và trà truyền thống',           1),
(N'Sinh tố',  N'Sinh tố trái cây tươi xay cùng đá và sữa',   1),
(N'Nước ép',  N'Nước ép trái cây nguyên chất',               1),
(N'Đá xay',   N'Các món đá xay (blended) mát lạnh',          1);
GO

-- ---------- drinks ----------
INSERT INTO drinks (category_id, name, price, image, description, active) VALUES
(1, N'Cà phê đen',        20000, N'images/ca-phe-den.jpg',      N'Cà phê phin đậm đà, không đường',               1),
(1, N'Cà phê sữa',        25000, N'images/ca-phe-sua.jpg',      N'Cà phê phin pha cùng sữa đặc',                  1),
(1, N'Bạc xỉu',           29000, N'images/bac-xiu.jpg',         N'Nhiều sữa, ít cà phê, vị ngọt béo',             1),
(2, N'Trà đào cam sả',    35000, N'images/tra-dao-cam-sa.jpg',  N'Trà đen, đào miếng, cam tươi và sả',            1),
(2, N'Trà vải',           32000, N'images/tra-vai.jpg',         N'Trà hoa nhài kết hợp vải thiều',                1),
(2, N'Trà chanh',         25000, N'images/tra-chanh.jpg',       N'Trà chanh giải khát truyền thống',              1),
(3, N'Sinh tố bơ',        40000, N'images/sinh-to-bo.jpg',      N'Bơ sáp xay sữa đặc, béo mịn',                   1),
(3, N'Sinh tố xoài',      38000, N'images/sinh-to-xoai.jpg',    N'Xoài cát chín xay cùng sữa chua',               1),
(3, N'Sinh tố dâu',       38000, N'images/sinh-to-dau.jpg',     N'Dâu tây tươi xay cùng sữa chua',                1),
(4, N'Nước ép cam',       35000, N'images/nuoc-ep-cam.jpg',     N'Cam vắt nguyên chất, không đường',              1),
(4, N'Nước ép dưa hấu',   30000, N'images/nuoc-ep-dua-hau.jpg', N'Dưa hấu tươi mát lạnh',                         1),
(5, N'Matcha đá xay',     45000, N'images/matcha-da-xay.jpg',   N'Matcha Nhật xay đá, phủ kem tươi',              1),
(5, N'Cookie đá xay',     45000, N'images/cookie-da-xay.jpg',   N'Bánh cookie xay đá, phủ kem tươi',              1),
(5, N'Cacao đá xay',      42000, N'images/cacao-da-xay.jpg',    N'Cacao xay đá (hiện ngừng kinh doanh)',          0);
GO

-- ---------- users ----------
-- Mật khẩu mẫu của tất cả tài khoản: 123456 (lưu dạng SHA-256 viết hoa, 64 ký tự)
-- role: 0 = Nhân viên, 1 = Quản lý | active: 1 = hoạt động, 0 = bị khóa
INSERT INTO users (email, password, full_name, phone, role, active) VALUES
('quanly@quancafe.vn',  CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', '123456'), 2), N'Nguyễn Văn Quản', '0901000001', 1, 1),
('mai.tt@quancafe.vn',  CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', '123456'), 2), N'Trần Thị Mai',    '0901000002', 0, 1),
('nam.lh@quancafe.vn',  CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', '123456'), 2), N'Lê Hoàng Nam',    '0901000003', 0, 1),
('ha.pt@quancafe.vn',   CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', '123456'), 2), N'Phạm Thu Hà',     '0901000004', 0, 1),
('khoi.vm@quancafe.vn', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', '123456'), 2), N'Võ Minh Khôi',    '0901000005', 0, 0);
GO

-- ---------- cards ----------
INSERT INTO cards (code, status) VALUES
(1, 1), (2, 1), (3, 1), (4, 1),
(5, 1), (6, 1), (7, 1), (8, 0);
GO

-- ---------- bills ----------
-- status: PAID (đã chốt) | PENDING (chưa chốt) | CANCELLED (đã hủy)
INSERT INTO bills (user_id, card_id, code, created_at, total, status, cancel_reason) VALUES
(2, 1, 'HD001', '2026-11-10 08:15:00',  85000, 'PAID',      NULL),
(2, 2, 'HD002', '2026-11-10 09:30:00',  49000, 'PAID',      NULL),
(3, 3, 'HD003', '2026-11-11 14:05:00', 125000, 'PAID',      NULL),
(3, 1, 'HD004', '2026-11-12 10:20:00',  96000, 'PAID',      NULL),
(4, 4, 'HD005', '2026-11-13 16:45:00',  85000, 'CANCELLED', N'Khách đổi ý, không thanh toán'),
(4, 5, 'HD006', '2026-11-14 19:10:00', 108000, 'PENDING',   NULL),
(2, 2, 'HD007', '2026-11-15 07:50:00',  88000, 'PAID',      NULL);
GO

-- ---------- bill_details ----------
-- price = đơn giá tại thời điểm bán; total của bills = SUM(quantity * price)
INSERT INTO bill_details (bill_id, drink_id, quantity, price, note) VALUES
-- HD001: 2*25000 + 1*35000 = 85000
(1,  2, 2, 25000, N'Ít đường'),
(1,  4, 1, 35000, N'Nhiều đá'),
-- HD002: 1*20000 + 1*29000 = 49000
(2,  1, 1, 20000, NULL),
(2,  3, 1, 29000, N'Ít đá'),
-- HD003: 2*40000 + 1*45000 = 125000
(3,  7, 2, 40000, NULL),
(3, 12, 1, 45000, N'Thêm kem tươi'),
-- HD004: 3*32000 = 96000
(4,  5, 3, 32000, N'Ít đường'),
-- HD005: 1*35000 + 2*25000 = 85000 (đã hủy)
(5, 10, 1, 35000, NULL),
(5,  6, 2, 25000, NULL),
-- HD006: 1*38000 + 1*45000 + 1*25000 = 108000 (chưa chốt)
(6,  8, 1, 38000, NULL),
(6, 13, 1, 45000, N'Ít ngọt'),
(6,  2, 1, 25000, N'Nhiều đá'),
-- HD007: 2*29000 + 1*30000 = 88000
(7,  3, 2, 29000, NULL),
(7, 11, 1, 30000, N'Không đá');
GO

/* =====================================================================
   PHẦN 3: KIỂM TRA DỮ LIỆU
   ===================================================================== */
SELECT * FROM categories;
SELECT * FROM drinks;
SELECT * FROM users;
SELECT * FROM cards;
SELECT * FROM bills;
SELECT * FROM bill_details;

-- Đối chiếu tổng tiền phiếu với tổng các dòng chi tiết (cột chênh lệch phải = 0)
SELECT b.code, b.total AS total_bill, SUM(d.quantity * d.price) AS total_detail,
       b.total - SUM(d.quantity * d.price) AS chenh_lech
FROM bills b
JOIN bill_details d ON d.bill_id = b.id
GROUP BY b.code, b.total;
GO
