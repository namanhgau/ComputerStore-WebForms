using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace ComputerStore.WebForms
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Kiểm tra phân quyền hiện nút Admin
            if (Session["Role"] != null && Session["Role"].ToString() == "Manager")
            {
                // pnlManager.Visible = true; // (Bỏ comment nếu bạn có thẻ pnlManager bên giao diện)
            }

            // CHỈ CHẠY 1 LẦN KHI MỚI VÀO TRANG
            if (!IsPostBack)
            {
                LoadCategories();

                // Lấy thông tin tìm kiếm từ thanh địa chỉ URL (nếu có)
                string kw = Request.QueryString["kw"] ?? "";
                int cat = 0;
                if (Request.QueryString["cat"] != null)
                {
                    int.TryParse(Request.QueryString["cat"], out cat);
                    // Giữ lại trạng thái hãng máy tính đang chọn trên giao diện
                    ddlCategory.SelectedValue = cat.ToString();
                }
                txtSearch.Text = kw;

                // Tải máy tính dựa trên từ khóa và danh mục
                LoadProducts(kw, cat);
            }
        }

        // HÀM 1: Lấy Danh mục từ CSDL đổ vào DropDownList
        private void LoadCategories()
        {
            using (var db = new ComputerStoreDBEntities())
            {
                var categories = db.Categories.ToList();
                ddlCategory.DataSource = categories;
                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryId";
                ddlCategory.DataBind();

                ddlCategory.Items.Insert(0, new ListItem("-- Tất cả danh mục --", "0"));
            }
        }

        // HÀM 2: Lọc sản phẩm & Phân trang (Đã bổ sung 2 tham số)
        private void LoadProducts(string keyword, int categoryId)
        {
            int pageSize = 8; // Số lượng laptop trên 1 trang
            int pageIndex = 1;

            if (Request.QueryString["page"] != null)
            {
                int.TryParse(Request.QueryString["page"], out pageIndex);
            }
            if (pageIndex < 1) pageIndex = 1;

            using (var db = new ComputerStoreDBEntities())
            {
                // Dùng AsQueryable để xếp chồng các điều kiện lọc lên nhau
                var query = db.Products.AsQueryable();

                // Nếu khách có chọn Hãng (Dell, Asus...)
                if (categoryId > 0)
                {
                    query = query.Where(p => p.CategoryId == categoryId);
                }

                // Nếu khách có gõ chữ tìm kiếm
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(p => p.ProductName.Contains(keyword));
                }

                // Tính toán tổng số trang dựa trên kết quả ĐÃ LỌC
                int totalProducts = query.Count();
                int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

                if (pageIndex > totalPages && totalPages > 0) pageIndex = totalPages;

                // Cắt lấy dữ liệu của trang hiện tại
                var products = query.OrderByDescending(p => p.ProductId)
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();

                rptProducts.DataSource = products;
                rptProducts.DataBind();

                // Gọi hàm vẽ nút phân trang và truyền theo keyword, categoryId để "nhớ" trạng thái
                RenderPagination(pageIndex, totalPages, keyword, categoryId);
            }
        }

        // HÀM 3: Tự động sinh HTML cho thanh Phân trang
        private void RenderPagination(int currentPage, int totalPages, string keyword, int categoryId)
        {
            if (totalPages <= 1)
            {
                litPagination.Text = "";
                return;
            }

            string html = "";
            // Gắn sẵn từ khóa và hãng vào link để khi khách bấm chuyển trang không bị mất kết quả lọc
            string baseUrl = $"Default.aspx?kw={Server.UrlEncode(keyword)}&cat={categoryId}&page=";

            // Nút "Trang Trước"
            if (currentPage > 1)
            {
                html += $"<li class='page-item'><a class='page-link text-primary fw-bold' href='{baseUrl}{currentPage - 1}'>&laquo; Trước</a></li>";
            }
            else
            {
                html += $"<li class='page-item disabled'><span class='page-link'>&laquo; Trước</span></li>";
            }

            // Các nút số
            for (int i = 1; i <= totalPages; i++)
            {
                if (i == currentPage)
                {
                    html += $"<li class='page-item active'><span class='page-link fw-bold'>{i}</span></li>";
                }
                else
                {
                    html += $"<li class='page-item'><a class='page-link text-dark' href='{baseUrl}{i}'>{i}</a></li>";
                }
            }

            // Nút "Trang Sau"
            if (currentPage < totalPages)
            {
                html += $"<li class='page-item'><a class='page-link text-primary fw-bold' href='{baseUrl}{currentPage + 1}'>Sau &raquo;</a></li>";
            }
            else
            {
                html += $"<li class='page-item disabled'><span class='page-link'>Sau &raquo;</span></li>";
            }

            litPagination.Text = html;
        }

        // SỰ KIỆN NÚT BẤM: Khi người dùng nhấn nút Tìm kiếm
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            int categoryId = int.Parse(ddlCategory.SelectedValue);

            // Chuyển hướng trình duyệt tạo ra một URL mới có chứa điều kiện lọc. 
            // Điều này giúp thuật toán Phân trang ở Page_Load có thể tóm được dữ liệu để xử lý mượt mà.
            Response.Redirect($"Default.aspx?kw={Server.UrlEncode(keyword)}&cat={categoryId}");
        }

        // SỰ KIỆN: Thêm vào giỏ hàng
        protected void rptProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "AddToCart")
            {
                int productId = int.Parse(e.CommandArgument.ToString());

                using (var db = new ComputerStoreDBEntities())
                {
                    var product = db.Products.FirstOrDefault(p => p.ProductId == productId);
                    if (product != null)
                    {
                        List<CartItem> cart = Session["Cart"] as List<CartItem>;
                        if (cart == null)
                        {
                            cart = new List<CartItem>();
                        }

                        var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);
                        if (existingItem != null)
                        {
                            existingItem.Quantity += 1;
                        }
                        else
                        {
                            cart.Add(new CartItem
                            {
                                ProductId = product.ProductId,
                                ProductName = product.ProductName,
                                ImageUrl = product.ImageUrl,
                                Price = product.Price,
                                Quantity = 1
                            });
                        }

                        Session["Cart"] = cart;

                        string script = $"alert('🎉 Đã thêm {product.ProductName} vào giỏ hàng thành công!');";
                        ClientScript.RegisterStartupScript(this.GetType(), "AddToCartSuccess", script, true);
                    }
                }
            }
        }
    }
}