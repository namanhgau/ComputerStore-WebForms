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

            Page.Form.Attributes.Add("enctype", "multipart/form-data");
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

        // 2. Chức năng CREATE & UPLOAD ẢNH MỚI
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
        protected void gvProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Lấy ID của sản phẩm tại dòng bị click
            int id = (int)gvProducts.DataKeys[e.RowIndex].Value;

            using (var db = new ComputerStoreDBEntities())
            {
                var sp = db.Products.FirstOrDefault(p => p.ProductId == id);
                if (sp != null)
                {
                    try
                    {
                        // Thử thực hiện lệnh xóa
                        db.Products.Remove(sp);
                        db.SaveChanges();

                        // Nếu xóa thành công thì hiện thông báo và tải lại bảng
                        ClientScript.RegisterStartupScript(this.GetType(), "DeleteSuccess", "alert('✅ Xóa sản phẩm thành công!');", true);
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        // BẮT LỖI TẠI ĐÂY: Nếu dính khóa ngoại, báo lỗi lịch sự thay vì sập web
                        string msg = "❌ Không thể xóa! Sản phẩm này đã nằm trong Đơn hàng hoặc có người Đánh giá.\\n\\nGợi ý: Hãy cập nhật số lượng Kho về 0 thay vì xóa nó đi.";
                        ClientScript.RegisterStartupScript(this.GetType(), "DeleteFail", $"alert('{msg}');", true);
                    }
                }
            }
        }

        // 4. Khi bấm nút "Sửa" -> Chuyển dòng đó thành các ô Textbox để gõ chữ
        protected void gvProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvProducts.EditIndex = e.NewEditIndex;
            LoadData(); // Tải lại bảng
        }

        // 5. Khi đang sửa mà đổi ý bấm nút "Hủy" -> Quay lại như cũ
        protected void gvProducts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvProducts.EditIndex = -1; // -1 nghĩa là tắt chế độ sửa
            LoadData();
        }

        // 6. Khi sửa xong và bấm nút "Cập nhật" -> Lưu vào Database
        protected void gvProducts_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Lấy ID của máy tính đang sửa
            int id = (int)gvProducts.DataKeys[e.RowIndex].Value;

            // Lấy dòng hiện tại trên bảng
            GridViewRow row = gvProducts.Rows[e.RowIndex];

            // Trích xuất chữ mà bạn vừa gõ vào các ô (Ô 0 là Tên, Ô 1 là Giá)
            string newName = (row.Cells[0].Controls[0] as TextBox).Text;
            string newPriceStr = (row.Cells[1].Controls[0] as TextBox).Text;

            // --- ĐOẠN NÂNG CẤP XỬ LÝ HÌNH ẢNH CỦA BẠN TẠI ĐÂY ---
            FileUpload fuEditImage = (FileUpload)row.FindControl("fuEditImage");
            HiddenField hdnOldImage = (HiddenField)row.FindControl("hdnOldImage");

            // Mặc định lấy lại đường dẫn ảnh cũ từ thẻ ẩn phòng trường hợp không đổi ảnh mới
            string finalImagePath = hdnOldImage != null ? hdnOldImage.Value : "";

            // Nếu người dùng chọn file ảnh mới để thay thế
            if (fuEditImage != null && fuEditImage.HasFile)
            {
                try
                {
                    string fileName = Path.GetFileName(fuEditImage.FileName);
                    // Thêm Ticks thời gian để tên file không bao giờ bị trùng lặp trên máy chủ
                    string uniqueFileName = DateTime.Now.Ticks + "_" + fileName;
                    string savePath = Server.MapPath("~/Images/") + uniqueFileName;

                    fuEditImage.SaveAs(savePath);
                    finalImagePath = "~/Images/" + uniqueFileName; // Lưu đường dẫn ảnh mới
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "UploadError", "alert('❌ Có lỗi xảy ra khi tải ảnh mới lên!');", true);
                    return;
                }
            }
            // ----------------------------------------------------

            using (var db = new ComputerStoreDBEntities())
            {
                var sp = db.Products.FirstOrDefault(p => p.ProductId == id);
                if (sp != null)
                {
                    sp.ProductName = newName;
                    sp.Price = decimal.Parse(newPriceStr);
                    sp.ImageUrl = finalImagePath; // Cập nhật hình ảnh (Mới hoặc giữ Cũ)
                    db.SaveChanges(); // Lưu vào SQL
                }
            }

            // Tắt chế độ sửa và tải lại bảng
            gvProducts.EditIndex = -1;
            LoadData();
            lblMessage.Text = "Cập nhật sản phẩm thành công!";
        }

        // 7. SỰ KIỆN: Khi người dùng bấm chuyển trang trên bảng GridView
        protected void gvProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Báo cho bảng biết nó phải chuyển sang trang số mấy
            gvProducts.PageIndex = e.NewPageIndex;

            // Gọi lại hàm LoadData để đổ dữ liệu mới vào bảng
            LoadData();
        }
    }
}