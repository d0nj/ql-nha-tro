## 🛠️ Hướng dẫn cài đặt và chạy ứng dụng

1.  **Khởi tạo cơ sở dữ liệu:**
    Mở terminal tại thư mục gốc của dự án và chạy lệnh sau để tạo file database SQLite:

    ```bash
    dotnet ef database update
    ```

2.  **Chạy ứng dụng:**
    Sử dụng lệnh sau để khởi động phần mềm:
    ```bash
    dotnet run
    ```
    Hoặc mở file `.sln` bằng Visual Studio 2022 và nhấn **F5**.

## ✨ Các tính năng chính

- **Dashboard:** Tổng quan trạng thái phòng, doanh thu và hợp đồng sắp hết hạn.
- **Quản lý Phòng:** Theo dõi trạng thái trống/đang thuê, thông tin chi tiết phòng.
- **Quản lý Khách thuê:** Lưu trữ thông tin định danh, số điện thoại và lịch sử thuê.
- **Hợp đồng & Thành viên:** Quản lý hợp đồng thuê và danh sách người ở cùng phòng.
- **Điện nước & Hóa đơn:** Tính toán chỉ số tiêu thụ và tự động xuất hóa đơn hàng tháng.
