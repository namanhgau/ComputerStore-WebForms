using System;
using System.Collections.Generic;
using System.Linq;

namespace ComputerStore.WebForms
{
    public partial class Checkout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Bắt buộc đăng nhập
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCheckoutData();
            }
        }

        private void LoadCheckoutData()
        {
            List<CartItem> cart = Session["Cart"] as List<CartItem>;

            // Chỉ hiển thị và tính tiền những món được tích chọn trong giỏ
            if (cart != null && cart.Any(c => c.IsSelected))
            {
                var selectedItems = cart.Where(c => c.IsSelected).ToList();
                rptCheckoutItems.DataSource = selectedItems;
                rptCheckoutItems.DataBind();

                // Tính tiền tạm tính
                decimal subTotal = selectedItems.Sum(c => c.Total);
                lblSubTotal.Text = string.Format("{0:N0}", subTotal);

                // Tính tiền giảm giá (nếu có mã lưu trong Session)
                decimal discountAmount = 0;
                if (Session["DiscountPercent"] != null)
                {
                    int percent = (int)Session["DiscountPercent"];
                    discountAmount = subTotal * percent / 100;
                }

                // Cập nhật lên giao diện
                lblDiscountAmount.Text = string.Format("{0:N0}", discountAmount);
                lblTotalAmount.Text = string.Format("{0:N0}", subTotal - discountAmount);
            }
            else
            {
                // Nếu không có món nào được chọn, đá về lại giỏ hàng
                Response.Redirect("~/Cart.aspx");
            }
        }

        protected void btnApplyPromo_Click(object sender, EventArgs e)
        {
            string code = txtPromoCode.Text.Trim();
            if (string.IsNullOrEmpty(code)) return;

            using (var db = new ComputerStoreDBEntities())
            {
                var promo = db.Promotions.FirstOrDefault(p => p.Code == code && p.IsActive == true);

                if (promo != null)
                {
                    if (promo.ExpiryDate >= DateTime.Now)
                    {
                        Session["PromotionId"] = promo.PromotionId;
                        Session["DiscountPercent"] = promo.DiscountPercent;
                        lblPromoMsg.Text = $"Áp dụng thành công mã giảm {promo.DiscountPercent}%!";
                        lblPromoMsg.CssClass = "d-block mb-3 text-center text-success fw-bold";
                    }
                    else
                    {
                        lblPromoMsg.Text = "Mã giảm giá đã hết hạn sử dụng!";
                        lblPromoMsg.CssClass = "d-block mb-3 text-center text-danger fw-bold";
                        Session.Remove("PromotionId");
                        Session.Remove("DiscountPercent");
                    }
                }
                else
                {
                    lblPromoMsg.Text = "Mã giảm giá không hợp lệ!";
                    lblPromoMsg.CssClass = "d-block mb-3 text-center text-danger fw-bold";
                    Session.Remove("PromotionId");
                    Session.Remove("DiscountPercent");
                }
            }
            // Gọi lại hàm load để tính toán lại tiền
            LoadCheckoutData();
        }

        protected void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            List<CartItem> cart = Session["Cart"] as List<CartItem>;
            if (cart == null || !cart.Any(c => c.IsSelected)) return;

            var selectedItems = cart.Where(c => c.IsSelected).ToList();

            decimal subTotal = selectedItems.Sum(c => c.Total);
            decimal discountAmount = 0;
            int? appliedPromoId = null;

            if (Session["DiscountPercent"] != null)
            {
                int percent = (int)Session["DiscountPercent"];
                discountAmount = subTotal * percent / 100;
                appliedPromoId = (int)Session["PromotionId"];
            }

            decimal finalTotal = subTotal - discountAmount;

            using (var db = new ComputerStoreDBEntities())
            {
                // 1. Tạo mới hóa đơn
                var newOrder = new Order
                {
                    UserId = int.Parse(Session["UserId"].ToString()),
                    TotalAmount = finalTotal,
                    OrderDate = DateTime.Now,
                    Status = "Chờ duyệt",
                    FullName = txtFullName.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    ShippingAddress = txtAddress.Text.Trim(),
                    Note = txtNote.Text.Trim(),
                    PromotionId = appliedPromoId
                };

                db.Orders.Add(newOrder);
                db.SaveChanges(); // Lưu để lấy mã OrderId tự sinh ra

                // 2. Lưu chi tiết từng món hàng và Trừ tồn kho
                foreach (var item in selectedItems)
                {
                    var detail = new OrderDetail
                    {
                        OrderId = newOrder.OrderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.Price
                    };
                    db.OrderDetails.Add(detail);

                    var productInDb = db.Products.FirstOrDefault(p => p.ProductId == item.ProductId);
                    if (productInDb != null)
                    {
                        productInDb.StockQuantity -= item.Quantity; // Cập nhật trừ đi số lượng kho
                    }
                }
                db.SaveChanges();

                // 3. Hiện nút In Hóa Đơn và nối mã ID vừa tạo vào link
                btnInHoaDon.Visible = true;
                btnInHoaDon.HRef = "~/Invoice.aspx?id=" + newOrder.OrderId;
            }

            // 4. Dọn dẹp giỏ hàng và xóa mã giảm giá khỏi Session
            cart.RemoveAll(c => c.IsSelected);
            Session["Cart"] = cart;
            Session.Remove("PromotionId");
            Session.Remove("DiscountPercent");

            // 5. Báo thành công và ẩn nút thanh toán
            lblMessage.Text = "🎉 Đặt hàng thành công! Đơn hàng của bạn đang được xử lý.";
            lblMessage.CssClass = "d-block mt-3 fw-bold text-success text-center fs-5";
            btnPlaceOrder.Visible = false;
            btnApplyPromo.Enabled = false;
        }
    }
}