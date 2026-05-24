using System;
using System.Linq;

namespace ComputerStore.WebForms
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Trang đăng ký không cần chặn quyền, ai cũng vào được
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPass = txtConfirmPassword.Text.Trim();

            // 1. Kiểm tra hai mật khẩu có khớp nhau không
            if (password != confirmPass)
            {
                lblMessage.Text = "Mật khẩu xác nhận không khớp!";
                lblMessage.CssClass = "text-danger fw-bold mb-3 d-block";
                return;
            }

            using (var db = new ComputerStoreDBEntities())
            {
                // 2. Kiểm tra xem Username đã có ai dùng chưa
                bool isExist = db.AppUsers.Any(u => u.Username == username);
                if (isExist)
                {
                    lblMessage.Text = "Tên đăng nhập này đã có người sử dụng. Vui lòng chọn tên khác!";
                    lblMessage.CssClass = "text-danger fw-bold mb-3 d-block";
                    return;
                }

                // 3. Nếu mọi thứ OK, tạo người dùng mới
                var newUser = new AppUser
                {
                    FullName = txtFullName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Username = username,
                    PasswordHash = password, // Khớp với tên cột trong DB của bạn
                    Role = "Member",         // Mặc định khách đăng ký mới sẽ là Member
                    CreatedAt = DateTime.Now // Lưu thời gian tạo tài khoản lúc này
                };

                // 4. Lưu vào Database
                db.AppUsers.Add(newUser);
                db.SaveChanges();

                // 5. Thông báo thành công và có thể chuyển hướng về trang Đăng nhập
                lblMessage.Text = "Đăng ký thành công! Đang chuyển hướng về trang đăng nhập...";
                lblMessage.CssClass = "text-success fw-bold mb-3 d-block";

                // Trì hoãn 2 giây để người dùng đọc thông báo rồi đá về trang Login
                Response.AddHeader("REFRESH", "2;URL=Login.aspx");
            }
        }
    }
}