using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace ComputerStore.WebForms
{
    public partial class OrderManager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 1. Bảo mật: Chỉ có Manager mới được vào trang này
            if (Session["Role"] == null || Session["Role"].ToString() != "Manager")
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadOrders();
            }
        }

        // Hàm tải danh sách đơn hàng từ mới nhất đến cũ nhất
        private void LoadOrders()
        {
            using (var db = new ComputerStoreDBEntities())
            {
                var orders = db.Orders.OrderByDescending(o => o.OrderDate).ToList();

                if (orders.Any())
                {
                    rptOrders.DataSource = orders;
                    rptOrders.DataBind();
                    pnlNoOrders.Visible = false;
                }
                else
                {
                    rptOrders.DataSource = null;
                    rptOrders.DataBind();
                    pnlNoOrders.Visible = true;
                }
            }
        }

        // Sự kiện khi bấm nút "Lưu" để đổi trạng thái
        protected void rptOrders_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "UpdateStatus")
            {
                // Tìm đến các ô chứa dữ liệu trên cái hàng vừa được bấm
                HiddenField hdnId = (HiddenField)e.Item.FindControl("hdnOrderId");
                DropDownList ddlStatus = (DropDownList)e.Item.FindControl("ddlStatus");

                int orderId = int.Parse(hdnId.Value);
                string newStatus = ddlStatus.SelectedValue;

                using (var db = new ComputerStoreDBEntities())
                {
                    // Tìm hóa đơn và cập nhật trạng thái mới
                    var order = db.Orders.FirstOrDefault(o => o.OrderId == orderId);
                    if (order != null)
                    {
                        order.Status = newStatus;
                        db.SaveChanges();
                    }
                }

                // Tải lại bảng và hiển thị thông báo nhỏ
                LoadOrders();
                string script = "alert('✅ Cập nhật trạng thái đơn hàng thành công!');";
                ClientScript.RegisterStartupScript(this.GetType(), "UpdateSuccess", script, true);
            }
        }
    }
}