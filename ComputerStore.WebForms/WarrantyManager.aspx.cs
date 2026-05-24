using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace ComputerStore.WebForms // Nhớ giữ nguyên namespace của bạn
{
    public partial class WarrantyManager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Bảo mật: Chỉ Admin/Manager mới được vào
            if (Session["Role"] == null || Session["Role"].ToString() != "Manager")
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadWarrantyList();
            }
        }

        private void LoadWarrantyList()
        {
            using (var db = new ComputerStoreDBEntities())
            {
                // Load các máy chưa trả khách lên đầu
                var list = db.Warranties.OrderByDescending(w => w.ReceiveDate).ToList();

                if (list.Any())
                {
                    rptWarranties.DataSource = list;
                    rptWarranties.DataBind();
                    pnlNoData.Visible = false;
                }
                else
                {
                    rptWarranties.DataSource = null;
                    rptWarranties.DataBind();
                    pnlNoData.Visible = true;
                }
            }
        }

        // Sự kiện: TIẾP NHẬN MÁY MỚI
        protected void btnAddWarranty_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSerial.Text) || string.IsNullOrEmpty(txtCustomerName.Text))
            {
                lblMsg.Text = "Vui lòng nhập số Serial và Tên khách!";
                lblMsg.CssClass = "d-block mt-2 text-center fw-bold text-danger";
                return;
            }

            using (var db = new ComputerStoreDBEntities())
            {
                var newWarranty = new Warranty
                {
                    SerialNumber = txtSerial.Text.Trim().ToUpper(),
                    CustomerName = txtCustomerName.Text.Trim(),
                    CustomerPhone = txtPhone.Text.Trim(),
                    IssueDescription = txtIssue.Text.Trim(),
                    ReceiveDate = DateTime.Now,
                    Status = "Đã tiếp nhận"
                };

                db.Warranties.Add(newWarranty);
                db.SaveChanges();
            }

            // Xóa trắng form sau khi nhập xong
            txtSerial.Text = ""; txtCustomerName.Text = "";
            txtPhone.Text = ""; txtIssue.Text = "";
            lblMsg.Text = "✅ Tạo phiếu tiếp nhận thành công!";
            lblMsg.CssClass = "d-block mt-2 text-center fw-bold text-success";

            LoadWarrantyList(); // Cập nhật lại bảng
        }

        // Sự kiện: LƯU TRẠNG THÁI TIẾN ĐỘ SỬA
        protected void rptWarranties_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "UpdateStatus")
            {
                HiddenField hdnId = (HiddenField)e.Item.FindControl("hdnWarrantyId");
                DropDownList ddlStatus = (DropDownList)e.Item.FindControl("ddlStatus");

                int warrantyId = int.Parse(hdnId.Value);
                string newStatus = ddlStatus.SelectedValue;

                using (var db = new ComputerStoreDBEntities())
                {
                    var item = db.Warranties.FirstOrDefault(w => w.WarrantyId == warrantyId);
                    if (item != null)
                    {
                        item.Status = newStatus;
                        db.SaveChanges();
                    }
                }

                LoadWarrantyList();
                string script = "alert('✅ Đã cập nhật tiến độ sửa chữa cho máy!');";
                ClientScript.RegisterStartupScript(this.GetType(), "UpdateSuccess", script, true);
            }
        }
    }
}