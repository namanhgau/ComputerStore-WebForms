<%@ Page Title="Thanh toán đơn hàng" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="ComputerStore.WebForms.Checkout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4 mb-5">
        <h2 class="fw-bold mb-4">💳 Thanh toán đơn hàng</h2>

        <div class="row">
            <div class="col-md-7">
                <div class="card shadow-sm border-0 mb-4">
                    <div class="card-header bg-primary text-white fw-bold">
                        📍 Thông tin nhận hàng
                    </div>
                    <div class="card-body">
                        <div class="mb-3">
                            <label class="form-label">Họ và tên người nhận</label>
                            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" required="true"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label class="form-label">Số điện thoại liên hệ</label>
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" required="true"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label class="form-label">Địa chỉ giao hàng chi tiết</label>
                            <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" required="true"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label class="form-label">Ghi chú thêm (Tùy chọn)</label>
                            <asp:TextBox ID="txtNote" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-5">
                <div class="card shadow-sm border-0 bg-light">
                    <div class="card-header bg-dark text-white fw-bold">
                        🛍️ Tóm tắt đơn hàng
                    </div>
                    <div class="card-body">
                        <asp:Repeater ID="rptCheckoutItems" runat="server">
                            <ItemTemplate>
                                <div class="d-flex justify-content-between border-bottom pb-2 mb-2 align-items-center">
                                    <div class="d-flex align-items-center">
                                        <img src='<%# ResolveUrl(Eval("ImageUrl").ToString()) %>' style="width: 40px; height: 40px; object-fit: contain;" class="me-2 border bg-white rounded" />
                                        <div>
                                            <span class="d-block fw-bold" style="font-size: 0.9rem;"><%# Eval("ProductName") %></span>
                                            <small class="text-muted">SL: <%# Eval("Quantity") %></small>
                                        </div>
                                    </div>
                                    <span class="text-danger fw-bold"><%# string.Format("{0:N0}", Eval("Total")) %>đ</span>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>

                        <div class="input-group mb-3 mt-4 border-top pt-3">
                            <asp:TextBox ID="txtPromoCode" runat="server" CssClass="form-control" Placeholder="Nhập mã giảm giá..."></asp:TextBox>
                            <asp:Button ID="btnApplyPromo" runat="server" Text="Áp dụng" CssClass="btn btn-secondary fw-bold" OnClick="btnApplyPromo_Click" CausesValidation="false" />
                        </div>
                        <asp:Label ID="lblPromoMsg" runat="server" CssClass="d-block mb-3 text-center"></asp:Label>

                        <div class="d-flex justify-content-between mb-2">
                            <span class="text-muted">Tạm tính:</span>
                            <span class="fw-bold"><asp:Label ID="lblSubTotal" runat="server"></asp:Label> VNĐ</span>
                        </div>
                        <div class="d-flex justify-content-between mb-2 text-success">
                            <span>Giảm giá khuyến mãi:</span>
                            <span class="fw-bold">- <asp:Label ID="lblDiscountAmount" runat="server" Text="0"></asp:Label> VNĐ</span>
                        </div>
                        
                        <div class="d-flex justify-content-between mt-2 pt-2 border-top">
                            <span class="fw-bold fs-5">Tổng thanh toán:</span>
                            <span class="fw-bold text-danger fs-4"><asp:Label ID="lblTotalAmount" runat="server"></asp:Label> VNĐ</span>
                        </div>
                        
                        <asp:Label ID="lblMessage" runat="server" CssClass="d-block mt-2 fw-bold text-center"></asp:Label>
                        <asp:Button ID="btnPlaceOrder" runat="server" Text="Chốt Đơn Đặt Hàng 🚀" CssClass="btn btn-danger w-100 fw-bold py-3 mt-3 fs-5" OnClick="btnPlaceOrder_Click" />
                        
                        <a id="btnInHoaDon" runat="server" visible="false" target="_blank" class="btn btn-outline-primary w-100 fw-bold py-2 mt-2">
                            🖨️ Xem / In Hóa Đơn
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>