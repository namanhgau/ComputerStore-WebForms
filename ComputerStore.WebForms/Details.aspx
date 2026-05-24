<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="ComputerStore.WebForms.Details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4 mb-5">
    <div class="row">
        <div class="col-md-5">
            <asp:Image ID="imgProduct" runat="server" CssClass="img-fluid rounded shadow-sm" AlternateText="Hình ảnh sản phẩm" />
        </div>
        
        <div class="col-md-7">
            <h2 class="fw-bold"><asp:Label ID="lblName" runat="server"></asp:Label></h2>
            <h4 class="text-danger fw-bold mt-3"><asp:Label ID="lblPrice" runat="server"></asp:Label></h4>
            
            <p class="mt-3"><strong>Mô tả:</strong></p>
            <p><asp:Label ID="lblDescription" runat="server"></asp:Label></p>
            <p><strong>Số lượng trong kho:</strong> <asp:Label ID="lblStock" runat="server"></asp:Label></p>
            
            <div class="mt-4 mb-4 pt-3 border-top">
                <asp:Button ID="btnAddToCart" runat="server" Text="🛒 Thêm vào giỏ hàng" CssClass="btn btn-danger btn-lg fw-bold px-5 shadow-sm" OnClick="btnAddToCart_Click" />
                <br />
                <asp:Label ID="lblCartMessage" runat="server" CssClass="text-success fw-bold mt-2 d-block"></asp:Label>
            </div>
            <hr class="my-4" />
                <h4 class="fw-bold mb-3">Đánh giá & Bình luận</h4>

                <asp:Repeater ID="rptReviews" runat="server">
                    <ItemTemplate>
                        <div class="card mb-2 shadow-sm border-0 bg-light">
                            <div class="card-body py-2">
                                <div class="d-flex justify-content-between">
                                    <strong><%# Eval("AppUser.FullName") ?? "Thành viên" %></strong>
                                    <span class="text-warning fw-bold"><%# Eval("Rating") %> ⭐</span>
                                </div>
                                <p class="mb-0 mt-1"><%# Eval("Comment") %></p>
                                <small class="text-muted"><%# Eval("CreatedAt", "{0:dd/MM/yyyy HH:mm}") %></small>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <asp:Label ID="lblNoReview" runat="server" Text="Chưa có đánh giá nào. Hãy là người đầu tiên!" CssClass="text-muted fst-italic" Visible="false"></asp:Label>

                <asp:Panel ID="pnlReviewForm" runat="server" CssClass="mt-4 p-3 border rounded shadow-sm">
                    <h5>Viết đánh giá của bạn</h5>
                    <div class="mb-2">
                        <label>Chấm điểm:</label>
                        <asp:DropDownList ID="ddlRating" runat="server" CssClass="form-select w-auto d-inline-block ms-2">
                            <asp:ListItem Value="5">5 Sao (Tuyệt vời)</asp:ListItem>
                            <asp:ListItem Value="4">4 Sao (Tốt)</asp:ListItem>
                            <asp:ListItem Value="3">3 Sao (Bình thường)</asp:ListItem>
                            <asp:ListItem Value="2">2 Sao (Kém)</asp:ListItem>
                            <asp:ListItem Value="1">1 Sao (Tệ)</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="mb-2">
                        <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" Placeholder="Chia sẻ cảm nhận của bạn về sản phẩm..."></asp:TextBox>
                    </div>
                    <asp:Button ID="btnSubmitReview" runat="server" Text="Gửi đánh giá" CssClass="btn btn-primary fw-bold" OnClick="btnSubmitReview_Click" />
                    <asp:Label ID="lblReviewMsg" runat="server" CssClass="ms-2 fw-bold"></asp:Label>
                </asp:Panel>

                <asp:Panel ID="pnlRequireLogin" runat="server" CssClass="mt-4 alert alert-warning text-center shadow-sm">
                    Vui lòng <a href="Login.aspx" class="fw-bold text-danger text-decoration-none">Đăng nhập</a> để lại bình luận và đánh giá sản phẩm.
                </asp:Panel>
        </div>
    </div>
</div>
</asp:Content>