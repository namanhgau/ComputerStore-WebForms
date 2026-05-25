<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ComputerStore.WebForms._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

   <div class="container mt-4">
    <div class="card shadow-sm border-0 bg-light p-3 mb-4">
        <div class="row g-2">
            <div class="col-md-5">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" Placeholder="Nhập tên máy tính cần tìm (VD: Asus, Dell...)"></asp:TextBox>
            </div>
        
            <div class="col-md-4">
                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control">
                    <asp:ListItem Text="-- Tất cả danh mục --" Value="0"></asp:ListItem>
                </asp:DropDownList>
            </div>
        
            <div class="col-md-3">
                <asp:Button ID="btnSearch" runat="server" Text="🔍 Tìm kiếm" CssClass="btn btn-primary w-100 fw-bold" OnClick="btnSearch_Click" />
            </div>
        </div>
    </div>
    <h2 class="mb-4">Danh sách Laptop</h2>
    <asp:Panel ID="pnlManager" runat="server" Visible="false" CssClass="mb-4">
        <div class="alert alert-warning d-flex justify-content-between align-items-center shadow-sm">
            <span class="fw-bold text-dark">Chào mừng Quản lý! Bạn có quyền chỉnh sửa hệ thống.</span>
            <a href="ProductManager.aspx" class="btn btn-danger fw-bold">
                ⚙️ Quản lý Kho Sản phẩm
            </a>
        </div>
    </asp:Panel>
    
    <div class="row">
        <asp:Repeater ID="rptProducts" runat="server" OnItemCommand="rptProducts_ItemCommand">
            <ItemTemplate>
                <div class="col-md-3 mb-4">
                    <div class="card h-100 shadow-sm border-0">
                        <div class="text-center p-3">
                            <img src='<%# ResolveUrl(Eval("ImageUrl").ToString()) %>' 
                                 alt='<%# Eval("ProductName") %>' 
                                 class="img-fluid rounded" 
                                 style="max-height: 180px; object-fit: contain;" />
                        </div>

                        <div class="card-body d-flex flex-column">
                            <h6 class="fw-bold"><%# Eval("ProductName") %></h6>
                            <p class="text-danger fw-bold mb-1"><%# string.Format("{0:N0} VNĐ", Eval("Price")) %></p>
    
                            <small class="d-block mb-3">
                                <%# Convert.ToInt32(Eval("StockQuantity")) > 0 
                                    ? "<span class='text-muted'>Số lượng: " + Eval("StockQuantity") + "</span>" 
                                    : "<span class='text-danger fw-bold'>❌ Hết hàng</span>" %>
                            </small>

                            <div class="mt-auto">
                                <a href='Details.aspx?id=<%# Eval("ProductId") %>' class="btn btn-sm btn-outline-primary w-100 mb-2">Xem chi tiết</a>
        
                                <asp:LinkButton ID="btnAddToCartHome" runat="server" CssClass="btn btn-sm btn-danger w-100 fw-bold" 
                                    CommandName="AddToCart" CommandArgument='<%# Eval("ProductId") %>'
                                    Visible='<%# Convert.ToInt32(Eval("StockQuantity")) > 0 %>'>
                                    🛒 Thêm vào giỏ
                                </asp:LinkButton>
        
                                <button type="button" class="btn btn-sm btn-secondary w-100 fw-bold" disabled="disabled"
                                    style='<%# Convert.ToInt32(Eval("StockQuantity")) <= 0 ? "display:block;" : "display:none;" %>'>
                                    🚫 Đã bán hết
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <div class="row mt-5 mb-4">
            <div class="col-12 d-flex justify-content-center">
                <nav aria-label="Page navigation">
                    <ul class="pagination pagination-lg shadow-sm">
                        <asp:Literal ID="litPagination" runat="server"></asp:Literal>
                    </ul>
                </nav>
            </div>
        </div>
    </div>
</div>

</asp:Content>