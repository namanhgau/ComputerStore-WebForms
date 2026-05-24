using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace ComputerStore.WebForms
{
    public partial class ProductManager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 1. Phân quyền: Chặn người lạ hoặc Member thường
            if (Session["Role"] == null || Session["Role"].ToString() != "Manager")
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        // Hàm lấy dữ liệu đổ vào Bảng
        private void LoadData()
        {
            using (var db = new ComputerStoreDBEntities())
            {
                gvProducts.DataSource = db.Products.ToList();
                gvProducts.DataBind();
            }
        }

        // 2. Chức năng CREATE & UPLOAD ẢNH
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string imgPath = "";

            // Kiểm tra xem người dùng có chọn file ảnh chưa
            if (fileUploadImage.HasFile)
            {
                // Lấy tên file gốc (VD: laptop-dell.jpg)
                string fileName = Path.GetFileName(fileUploadImage.FileName);
                // Tạo đường dẫn lưu vào thư mục Images trong máy chủ
                string savePath = Server.MapPath("~/Images/") + fileName;
                // Lưu file vật lý
                fileUploadImage.SaveAs(savePath);
                // Tạo đường dẫn ảo để lưu vào Database
                imgPath = "~/Images/" + fileName;
            }

            using (var db = new ComputerStoreDBEntities())
            {
                var newProduct = new Product
                {
                    ProductName = txtName.Text,
                    Price = decimal.Parse(txtPrice.Text),
                    ImageUrl = imgPath,
                    StockQuantity = 10, // Tạm fix cứng hoặc bạn có thể thêm ô nhập Tồn kho
                    CategoryId = 1 // Tạm gán vào danh mục 1
                };

                db.Products.Add(newProduct);
                db.SaveChanges();
            }

            lblMessage.Text = "Thêm sản phẩm thành công!";
            LoadData(); // Tải lại bảng để thấy sản phẩm mới
        }

        // 3. Chức năng DELETE (Khi bấm nút Xóa trên bảng)
        protected void gvProducts_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            // Lấy ID của sản phẩm tại dòng bị click
            int id = (int)gvProducts.DataKeys[e.RowIndex].Value;

            using (var db = new ComputerStoreDBEntities())
            {
                var sp = db.Products.FirstOrDefault(p => p.ProductId == id);
                if (sp != null)
                {
                    db.Products.Remove(sp);
                    db.SaveChanges();
                }
            }
            lblMessage.Text = "Đã xóa sản phẩm!";
            LoadData();
        }

        // 1. Khi bấm nút "Sửa" -> Chuyển dòng đó thành các ô Textbox để gõ chữ
        protected void gvProducts_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvProducts.EditIndex = e.NewEditIndex;
            LoadData(); // Tải lại bảng
        }

        // 2. Khi đang sửa mà đổi ý bấm nút "Hủy" -> Quay lại như cũ
        protected void gvProducts_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvProducts.EditIndex = -1; // -1 nghĩa là tắt chế độ sửa
            LoadData();
        }

        // 3. Khi sửa xong và bấm nút "Cập nhật" -> Lưu vào Database
        protected void gvProducts_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            // Lấy ID của máy tính đang sửa
            int id = (int)gvProducts.DataKeys[e.RowIndex].Value;

            // Lấy dòng hiện tại trên bảng
            System.Web.UI.WebControls.GridViewRow row = gvProducts.Rows[e.RowIndex];

            // Trích xuất chữ mà bạn vừa gõ vào các ô (Ô 0 là Tên, Ô 1 là Giá)
            string newName = (row.Cells[0].Controls[0] as System.Web.UI.WebControls.TextBox).Text;
            string newPriceStr = (row.Cells[1].Controls[0] as System.Web.UI.WebControls.TextBox).Text;

            using (var db = new ComputerStoreDBEntities())
            {
                var sp = db.Products.FirstOrDefault(p => p.ProductId == id);
                if (sp != null)
                {
                    sp.ProductName = newName;
                    sp.Price = decimal.Parse(newPriceStr);
                    db.SaveChanges(); // Lưu vào SQL
                }
            }

            // Tắt chế độ sửa và tải lại bảng
            gvProducts.EditIndex = -1;
            LoadData();
            lblMessage.Text = "Cập nhật sản phẩm thành công!";
        }
    }
}