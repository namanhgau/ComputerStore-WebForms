using System;
using System.Linq;

namespace ComputerStore.WebForms
{
    public partial class Invoice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Bảo mật: Chỉ người đã đăng nhập mới được xem hóa đơn
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                string idStr = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(idStr))
                {
                    int orderId = int.Parse(idStr);
                    LoadInvoiceData(orderId);
                }
            }
        }

        private void LoadInvoiceData(int orderId)
        {
            using (var db = new ComputerStoreDBEntities()) // Thay tên Entities chuẩn của bạn
            {
                // 1. Lấy thông tin chung của đơn hàng từ bảng Orders
                var order = db.Orders.FirstOrDefault(o => o.OrderId == orderId);
                if (order != null)
                {
                    lblOrderId.Text = order.OrderId.ToString();
                    lblOrderDate.Text = order.OrderDate.Value.ToString("dd/MM/yyyy HH:mm");
                    lblCustomerName.Text = order.FullName;
                    lblPhone.Text = order.Phone;
                    lblAddress.Text = order.ShippingAddress;
                    lblNote.Text = string.IsNullOrEmpty(order.Note) ? "Không có" : order.Note;
                    lblTotalAmount.Text = string.Format("{0:N0}", order.TotalAmount);

                    // Lấy thông tin mã khuyến mãi (nếu có)
                    if (order.PromotionId != null)
                    {
                        var promo = db.Promotions.FirstOrDefault(p => p.PromotionId == order.PromotionId);
                        if (promo != null)
                        {
                            lblPromoCode.Text = $"{promo.Code} (-{promo.DiscountPercent}%)";
                        }
                    }

                    // 2. Lấy chi tiết các món hàng bằng cách JOIN bảng OrderDetails với bảng Products
                    var detailsList = (from detail in db.OrderDetails
                                       join product in db.Products on detail.ProductId equals product.ProductId
                                       where detail.OrderId == orderId
                                       select new
                                       {
                                           ProductName = product.ProductName,
                                           Quantity = detail.Quantity,
                                           UnitPrice = detail.UnitPrice
                                       }).ToList();

                    // Đổ dữ liệu vào Repeater để vẽ bảng hóa đơn
                    rptInvoiceDetails.DataSource = detailsList;
                    rptInvoiceDetails.DataBind();
                }
                else
                {
                    Response.Write("<script>alert('Không tìm thấy hóa đơn này!'); window.close();</script>");
                }
            }
        }
    }
}