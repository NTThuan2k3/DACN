# 💼 Job Connect - Website tìm việc làm

**Job Connect** là một website giúp kết nối giữa **người tìm việc** và **nhà tuyển dụng**. Dự án được xây dựng bằng ASP.NET Core MVC và SQL Server, hỗ trợ các chức năng như đăng tin tuyển dụng, tìm kiếm việc làm, ứng tuyển online và quản lý hồ sơ ứng viên.

---

## 🛠️ Công nghệ sử dụng / Technologies Used

- **Back-end**: ASP.NET Core MVC, Entity Framework
- **Front-end**: HTML, CSS, JavaScript, Bootstrap
- **Cơ sở dữ liệu / Database**: SQL Server
- **IDE**: Visual Studio 2022

---

## 📌 Các chức năng / Features

### 👤 Người tìm việc / For Job Seekers:
- Đăng ký & đăng nhập  
- Tìm kiếm việc làm theo tiêu chí: vị trí, ngành nghề, tỉnh thành, mức lương, số năm kinh nghiệm,...  
- Tải lên và quản lý CV  
- Ứng tuyển công việc online  
- Xem lịch sử ứng tuyển

### 🏢 Nhà tuyển dụng / For Employers:
- Đăng ký & đăng nhập  
- Đăng tin tuyển dụng mới  
- Quản lý bài đăng tuyển dụng  
- Xem danh sách ứng viên đã ứng tuyển  
- Duyệt hoặc từ chối ứng viên

### 🛡️ Quản trị viên / For Admin:
- Quản lý người dùng  
- Quản lý ngành nghề, danh mục việc làm  
- Thống kê & báo cáo  

---

## ⚙️ Hướng dẫn chạy dự án / How to Run the Project

### 1. Yêu cầu cài đặt / Prerequisites

- [.NET SDK 7.0+](https://dotnet.microsoft.com/en-us/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)

### 2. Clone dự án

    git clone https://github.com/NTThuan2k3/DACN.git
    cd DACN

### 3. Cấu hình cơ sở dữ liệu / Configure Database

- Mở file appsettings.json, sửa lại chuỗi kết nối:

    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=DATABASE_NAME;Trusted_Connection=True;"
    }
    ```

- Vào Visual Studio chọn **Tools -> NuGet Package Manager -> Package Manager Console** sau đó gõ lệnh:

    ```bash
    update-database
    ```
