using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace ComputerStore.WebForms
{
    public partial class UserManager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Bảo mật: Chỉ duy nhất Manager được vào trang này
            if (Session["Role"] == null || Session["Role"].ToString() != "Manager")
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadUsers();
            }
        }

        // Hàm bảo vệ: Ép kiểu Role an toàn khi đưa dữ liệu lên giao diện
        protected string GetSafeRole(object roleObj)
        {
            string role = Convert.ToString(roleObj);

            // Nếu Role là Manager thì giữ nguyên
            if (role == "Manager")
            {
                return "Manager";
            }

            // Tất cả các trường hợp còn lại (bao gồm cả NULL) đều quy về Member
            return "Member";
        }

        private void LoadUsers()
        {
            using (var db = new ComputerStoreDBEntities())
            {
                // Load toàn bộ danh sách tài khoản, đẩy tài khoản mới lên trên
                var users = db.AppUsers.OrderByDescending(u => u.UserId).ToList();
                rptUsers.DataSource = users;
                rptUsers.DataBind();
            }
        }

        protected void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            HiddenField hdnId = (HiddenField)e.Item.FindControl("hdnUserId");
            int userId = int.Parse(hdnId.Value);

            // Không cho phép Quản lý tự khóa chính mình hoặc tự hạ quyền của chính mình
            int currentSessionUserId = int.Parse(Session["UserId"].ToString());
            if (userId == currentSessionUserId)
            {
                string alertScript = "alert('❌ Bạn không thể tự khóa hoặc thay đổi quyền của chính mình!');";
                ClientScript.RegisterStartupScript(this.GetType(), "ErrorSelf", alertScript, true);
                return;
            }

            using (var db = new ComputerStoreDBEntities())
            {
                var user = db.AppUsers.FirstOrDefault(u => u.UserId == userId);
                if (user != null)
                {
                    // HÀNH ĐỘNG 1: ĐỔI QUYỀN HẠN (ROLE)
                    if (e.CommandName == "ChangeRole")
                    {
                        DropDownList ddlRole = (DropDownList)e.Item.FindControl("ddlRole");
                        user.Role = ddlRole.SelectedValue;
                        db.SaveChanges();

                        ClientScript.RegisterStartupScript(this.GetType(), "SuccessRole", "alert('✅ Đã cập nhật quyền thành công!');", true);
                    }

                    // HÀNH ĐỘNG 2: KHÓA / MỞ KHÓA TÀI KHOẢN
                    if (e.CommandName == "ToggleLock")
                    {
                        // Đảo ngược trạng thái IsActive (nếu true đổi thành false và ngược lại)
                        user.IsActive = !user.IsActive;
                        db.SaveChanges();

                        ClientScript.RegisterStartupScript(this.GetType(), "SuccessLock", "alert('✅ Đã thay đổi trạng thái tài khoản!');", true);
                    }
                }
            }
            LoadUsers(); // Tải lại bảng sau khi chỉnh sửa
        }
    }
}