using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ComputerStore.WebForms
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            // 1. Xóa sạch mọi thứ trong Session
            Session.Clear();
            Session.Abandon();

            // 2. Đuổi người dùng về trang chủ hoặc trang đăng nhập
            Response.Redirect("Default.aspx");
        }
    }
}