using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ComputerStore.WebForms
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            string pass = txtPassword.Text.Trim();

            using (var db = new ComputerStoreDBEntities())
            {
                // Đã đổi Password thành PasswordHash
                var account = db.AppUsers.FirstOrDefault(u => u.Username == user && u.PasswordHash == pass);

                if (account != null)
                {
                    Session["UserId"] = account.UserId;
                    Session["Username"] = account.Username;
                    Session["Role"] = account.Role;

                    // Đã đổi Admin thành Manager theo đúng Data của bạn
                    if (account.Role == "Manager" || account.Role == "Staff")
                    {
                        // Chú ý: Tạm thời Redirect về Default.aspx nếu bạn chưa tạo trang Admin/Dashboard.aspx để tránh lỗi 404 nhé
                        Response.Redirect("Default.aspx");
                    }
                    else
                    {
                        Response.Redirect("Default.aspx");
                    }
                }
                else
                {
                    lblError.Text = "Sai tên đăng nhập hoặc mật khẩu!";
                }
            }
        }
    }
}