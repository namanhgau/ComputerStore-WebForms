using System;
using System.Linq;
using System.Collections.Generic;

namespace ComputerStore.WebForms
{
    public partial class Details : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 1. Kiểm tra đăng nhập để ẩn/hiện Form bình luận
                if (Session["UserId"] != null)
                {
                    pnlReviewForm.Visible = true;
                    pnlRequireLogin.Visible = false;
                }
                else
                {
                    pnlReviewForm.Visible = false;
                    pnlRequireLogin.Visible = true;
                }

                // 2. Lấy ID sản phẩm từ URL xuống
                string idStr = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(idStr))
                {
                    int productId = int.Parse(idStr);

                    using (var db = new ComputerStoreDBEntities())
                    {
                        // 3. Lấy thông tin chi tiết sản phẩm
                        var product = db.Products.FirstOrDefault(p => p.ProductId == productId);

                        if (product != null)
                        {
                            lblName.Text = product.ProductName;
                            lblPrice.Text = string.Format("{0:N0} VNĐ", product.Price);
                            lblDescription.Text = product.Description;
                            imgProduct.ImageUrl = product.ImageUrl;

                            // KIỂM TRA TỒN KHO VÀ ẨN/HIỆN NÚT
                            if (product.StockQuantity > 0)
                            {
                                lblStock.Text = product.StockQuantity.ToString();
                                lblStock.CssClass = "text-dark";
                                btnAddToCart.Enabled = true;
                            }
                            else
                            {
                                lblStock.Text = "Hết hàng";
                                lblStock.CssClass = "text-danger fw-bold";

                                // Khóa nút Thêm vào giỏ và đổi màu xám
                                btnAddToCart.Enabled = false;
                                btnAddToCart.Text = "🚫 Đã bán hết";
                                btnAddToCart.CssClass = "btn btn-secondary btn-lg fw-bold px-5 shadow-sm";
                            }

                            // Vẫn gọi hàm load bình luận bình thường
                            LoadReviews(productId);
                        }
                        else
                        {
                            lblName.Text = "Không tìm thấy sản phẩm!";
                        }
                    }
                }
            }
        }

        // HÀM HIỂN THỊ BÌNH LUẬN
        private void LoadReviews(int productId)
        {
            using (var db = new ComputerStoreDBEntities())
            {
                // .Include("AppUser") giúp lấy luôn tên người dùng từ bảng AppUser
                var reviews = db.Reviews.Where(r => r.ProductId == productId)
                                        .OrderByDescending(r => r.CreatedAt).ToList();
                if (reviews.Any())
                {
                    rptReviews.DataSource = reviews;
                    rptReviews.DataBind();
                    lblNoReview.Visible = false;
                }
                else
                {
                    lblNoReview.Visible = true;
                }
            }
        }

        // HÀM XỬ LÝ KHI BẤM NÚT GỬI BÌNH LUẬN
        protected void btnSubmitReview_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] == null) return;

            string idStr = Request.QueryString["id"];
            if (string.IsNullOrEmpty(idStr)) return;

            int productId = int.Parse(idStr);
            int userId = int.Parse(Session["UserId"].ToString());

            if (string.IsNullOrEmpty(txtComment.Text.Trim()))
            {
                lblReviewMsg.Text = "Vui lòng nhập nội dung đánh giá!";
                lblReviewMsg.CssClass = "text-danger ms-2 fw-bold";
                return;
            }

            using (var db = new ComputerStoreDBEntities())
            {
                var newReview = new Review
                {
                    ProductId = productId,
                    AppUserUserId = userId,
                    Rating = int.Parse(ddlRating.SelectedValue),
                    Comment = txtComment.Text.Trim(),
                    CreatedAt = DateTime.Now
                };

                db.Reviews.Add(newReview);
                db.SaveChanges();
            }

            // Reset form và tải lại danh sách bình luận
            txtComment.Text = "";
            lblReviewMsg.Text = "Cảm ơn bạn đã đánh giá!";
            lblReviewMsg.CssClass = "text-success ms-2 fw-bold";
            LoadReviews(productId);
        }
        // HÀM XỬ LÝ KHI BẤM NÚT THÊM VÀO GIỎ HÀNG
        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            // 1. Lấy mã ID sản phẩm từ URL
            string idStr = Request.QueryString["id"];
            if (string.IsNullOrEmpty(idStr)) return;

            int productId = int.Parse(idStr);

            using (var db = new ComputerStoreDBEntities())
            {
                // Tìm sản phẩm trong CSDL
                var product = db.Products.FirstOrDefault(p => p.ProductId == productId);
                if (product != null)
                {
                    // 2. Lấy giỏ hàng từ Session. Nếu chưa có (người dùng mới) thì tạo một cái giỏ mới
                    List<CartItem> cart = Session["Cart"] as List<CartItem>;
                    if (cart == null)
                    {
                        cart = new List<CartItem>();
                    }

                    // 3. Kiểm tra xem món hàng này đã nằm trong giỏ chưa
                    var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);
                    if (existingItem != null)
                    {
                        // Đã có thì cộng dồn số lượng
                        existingItem.Quantity += 1;
                    }
                    else
                    {
                        // Chưa có thì thêm mới nguyên một dòng vào giỏ
                        cart.Add(new CartItem
                        {
                            ProductId = product.ProductId,
                            ProductName = product.ProductName,
                            ImageUrl = product.ImageUrl,
                            Price = product.Price,
                            Quantity = 1
                        });
                    }

                    // 4. Cất cái giỏ hàng vào lại Session để lát sang trang khác tính tiền
                    Session["Cart"] = cart;

                    // Hiện thông báo thành công ra màn hình
                    lblCartMessage.Text = "🎉 Đã thêm \"" + product.ProductName + "\" vào giỏ hàng thành công!";
                }
            }
        }
    }
}