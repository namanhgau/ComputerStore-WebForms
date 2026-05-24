<%@ Page Title="Giỏ hàng của bạn" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="ComputerStore.WebForms.Cart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4 mb-5">
        <h2 class="fw-bold mb-4">🛒 Giỏ hàng của bạn</h2>

        <div class="row">
            <div class="col-md-8">
                <div class="card shadow-sm border-0 mb-3">
                    <div class="card-body p-0">
                        <table class="table table-hover align-middle mb-0">
                            <thead class="table-light">
                                <tr>
                                    <th class="ps-3" style="width: 50px;">Chọn</th>
                                    <th>Sản phẩm</th>
                                    <th>Đơn giá</th>
                                    <th class="text-center" style="width: 120px;">Số lượng</th>
                                    <th>Thành tiền</th>
                                    <th class="text-center">Xóa</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptCart" runat="server" OnItemCommand="rptCart_ItemCommand">
                                    <ItemTemplate>
                                        <tr>
                                            <asp:HiddenField ID="hdnProductId" runat="server" Value='<%# Eval("ProductId") %>' />
                                            
                                            <td class="ps-3 text-center">
                                                <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Eval("IsSelected") %>' 
                                                    AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" CssClass="form-check-input-custom" />
                                            </td>

                                            <td>
                                                <div class="d-flex align-items-center">
                                                    <img src='<%# ResolveUrl(Eval("ImageUrl").ToString()) %>' alt="Ảnh" style="width: 60px; height: 60px; object-fit: contain;" class="rounded me-3 border" />
                                                    <span class="fw-bold"><%# Eval("ProductName") %></span>
                                                </div>
                                            </td>

                                            <td class="text-danger fw-bold"><%# string.Format("{0:N0}", Eval("Price")) %>đ</td>
                                            
                                            <td class="text-center">
                                                <asp:TextBox ID="txtQuantity" runat="server" Text='<%# Eval("Quantity") %>' 
                                                    TextMode="Number" min="1" max="99" CssClass="form-control form-control-sm text-center d-inline-block" 
                                                    AutoPostBack="true" OnTextChanged="txtQuantity_TextChanged"></asp:TextBox>
                                            </td>

                                            <td class="text-danger fw-bold"><%# string.Format("{0:N0}", Eval("Total")) %>đ</td>
                                            
                                            <td class="text-center">
                                                <asp:LinkButton ID="btnRemove" runat="server" CommandName="RemoveItem" CommandArgument='<%# Eval("ProductId") %>' CssClass="text-danger fs-5 text-decoration-none" OnClientClick="return confirm('Bạn có chắc muốn bỏ sản phẩm này khỏi giỏ?');">
                                                    🗑️
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                        
                        <asp:Panel ID="pnlEmptyCart" runat="server" Visible="false" CssClass="text-center p-5">
                            <h5 class="text-muted">Giỏ hàng của bạn đang trống!</h5>
                            <a href="Default.aspx" class="btn btn-primary mt-2">← Tiếp tục mua sắm</a>
                        </asp:Panel>
                    </div>
                </div>
            </div>

            <div class="col-md-4">
                <div class="card shadow-sm border-0 bg-light">
                    <div class="card-body">
                        <h4 class="fw-bold border-bottom pb-2 mb-3">Tổng cộng</h4>
                        <div class="d-flex justify-content-between mb-3">
                            <span class="text-muted">Tiền thanh toán:</span>
                            <span class="fw-bold text-danger fs-4"><asp:Label ID="lblTotalPrice" runat="server" Text="0"></asp:Label> VNĐ</span>
                        </div>
                        <asp:Button ID="btnCheckout" runat="server" Text="Tiến hành Thanh toán 💳" CssClass="btn btn-success w-100 fw-bold py-2" OnClick="btnCheckout_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>