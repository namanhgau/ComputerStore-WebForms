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
            // Kiểm tra phân quyền hiện nút Admin (giữ nguyên code cũ của bạn)
            if (Session["Role"] != null && Session["Role"].ToString() == "Manager")
            {
                pnlManager.Visible = true;
            }

            // CHỈ CHẠY 1 LẦN KHI MỚI VÀO TRANG
            if (!IsPostBack)
            {
                LoadCategories(); // Tải danh sách hãng (Asus, Dell) đổ vào hộp Dropdown
                LoadProducts("", 0); // Mới vào trang -> Tải toàn bộ máy tính
            }


        }

        // HÀM 1: Lấy Danh mục từ CSDL đổ vào DropDownList
        private void LoadCategories()
        {
            using (var db = new ComputerStoreDBEntities())
            {
                var categories = db.Categories.ToList();
                ddlCategory.DataSource = categories;
                ddlCategory.DataTextField = "CategoryName"; // Chữ hiển thị
                ddlCategory.DataValueField = "CategoryId";  // ID ngầm
                ddlCategory.DataBind();

                // Chèn thêm mục "Tất cả" lên dòng đầu tiên
                ddlCategory.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Tất cả danh mục --", "0"));
            }
        }

        // HÀM 2: Lọc sản phẩm cực mạnh bằng LINQ
        private void LoadProducts(string keyword, int categoryId)
        {
            using (var db = new ComputerStoreDBEntities())
            {
                // Bắt đầu với câu lệnh lấy hết sản phẩm
                var query = db.Products.AsQueryable();

                // Nếu người dùng có gõ chữ -> Lọc tên có chứa chữ đó (tương đương LIKE trong SQL)
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(p => p.ProductName.Contains(keyword));
                }

                // Nếu người dùng chọn 1 hãng cụ thể (ID > 0) -> Lọc theo ID hãng
                if (categoryId > 0)
                {
                    query = query.Where(p => p.CategoryId == categoryId);
                }

                // Đổ kết quả cuối cùng ra Repeater
                rptProducts.DataSource = query.ToList();
                rptProducts.DataBind();
            }
        }

        // SỰ KIỆN NÚT BẤM: Khi người dùng nhấn nút Tìm kiếm
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim(); // Lấy chữ người dùng gõ
            int categoryId = int.Parse(ddlCategory.SelectedValue); // Lấy ID danh mục đang chọn

            // Gọi hàm Lọc
            LoadProducts(keyword, categoryId);
        }

        protected void rptProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // Kiểm tra xem nút vừa bấm có tên là "AddToCart" không
            if (e.CommandName == "AddToCart")
            {
                // Lấy ID sản phẩm đã được giấu trong CommandArgument
                int productId = int.Parse(e.CommandArgument.ToString());

                using (var db = new ComputerStoreDBEntities()) // Đổi ComputerStoreDBEntities thành tên đúng trong project của bạn
                {
                    var product = db.Products.FirstOrDefault(p => p.ProductId == productId);
                    if (product != null)
                    {
                        // Lấy giỏ hàng từ Session ra
                        List<CartItem> cart = Session["Cart"] as List<CartItem>;
                        if (cart == null)
                        {
                            cart = new List<CartItem>();
                        }

                        // Kiểm tra xem món đồ này đã có trong giỏ chưa
                        var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);
                        if (existingItem != null)
                        {
                            existingItem.Quantity += 1; // Có rồi thì tăng số lượng
                        }
                        else
                        {
                            // Chưa có thì tạo món mới
                            cart.Add(new CartItem
                            {
                                ProductId = product.ProductId,
                                ProductName = product.ProductName,
                                ImageUrl = product.ImageUrl,
                                Price = product.Price,
                                Quantity = 1
                            });
                        }

                        // Cất lại vào Session
                        Session["Cart"] = cart;

                        // Bắn ra một thông báo Pop-up nhỏ (Alert) để khách biết đã mua thành công
                        string script = $"alert('🎉 Đã thêm {product.ProductName} vào giỏ hàng thành công!');";
                        ClientScript.RegisterStartupScript(this.GetType(), "AddToCartSuccess", script, true);
                    }
                }
            }
        }
    }
}