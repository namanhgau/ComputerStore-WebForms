using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace ComputerStore.WebForms
{
    public partial class Cart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCartData();
            }
        }

        private void LoadCartData()
        {
            List<CartItem> cart = Session["Cart"] as List<CartItem>;

            if (cart != null && cart.Count > 0)
            {
                rptCart.DataSource = cart;
                rptCart.DataBind();
                pnlEmptyCart.Visible = false;

                // THAY ĐỔI: Chỉ tính tổng tiền của những món có tích chọn (IsSelected == true)
                decimal total = cart.Where(item => item.IsSelected).Sum(item => item.Total);
                lblTotalPrice.Text = string.Format("{0:N0}", total);

                // Nếu không tích món nào thì khóa nút thanh toán lại
                btnCheckout.Enabled = cart.Any(item => item.IsSelected);
            }
            else
            {
                rptCart.DataSource = null;
                rptCart.DataBind();
                pnlEmptyCart.Visible = true;
                lblTotalPrice.Text = "0";
                btnCheckout.Enabled = false;
            }
        }

        // SỰ KIỆN 1: KHI THAY ĐỔI Ô SỐ LƯỢNG
        protected void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            TextBox txtQty = (TextBox)sender;
            // Dùng NamingContainer để mò ngược lại xem ô TextBox này nằm ở hàng nào trong bảng
            RepeaterItem row = (RepeaterItem)txtQty.NamingContainer;
            HiddenField hdnId = (HiddenField)row.FindControl("hdnProductId");

            int productId = int.Parse(hdnId.Value);
            int newQty = int.Parse(txtQty.Text);

            List<CartItem> cart = Session["Cart"] as List<CartItem>;
            if (cart != null)
            {
                var item = cart.FirstOrDefault(c => c.ProductId == productId);
                if (item != null && newQty > 0)
                {
                    item.Quantity = newQty; // Cập nhật số lượng mới
                    Session["Cart"] = cart;
                }
            }
            LoadCartData(); // Tải lại bảng để nhảy số tiền mới
        }

        // SỰ KIỆN 2: KHI TÍCH HOẶC BỎ TÍCH CHỌN SẢN PHẨM
        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            RepeaterItem row = (RepeaterItem)chk.NamingContainer;
            HiddenField hdnId = (HiddenField)row.FindControl("hdnProductId");

            int productId = int.Parse(hdnId.Value);

            List<CartItem> cart = Session["Cart"] as List<CartItem>;
            if (cart != null)
            {
                var item = cart.FirstOrDefault(c => c.ProductId == productId);
                if (item != null)
                {
                    item.IsSelected = chk.Checked; // Cập nhật trạng thái Tích chọn
                    Session["Cart"] = cart;
                }
            }
            LoadCartData(); // Tính lại tổng tiền theo các món được chọn
        }

        protected void rptCart_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "RemoveItem")
            {
                int productId = int.Parse(e.CommandArgument.ToString());
                List<CartItem> cart = Session["Cart"] as List<CartItem>;

                if (cart != null)
                {
                    var itemToRemove = cart.FirstOrDefault(c => c.ProductId == productId);
                    if (itemToRemove != null)
                    {
                        cart.Remove(itemToRemove);
                        Session["Cart"] = cart;
                    }
                }
                LoadCartData();
            }
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                // Cho sang trang thanh toán
                Response.Redirect("~/Checkout.aspx");
            }
        }
    }
}