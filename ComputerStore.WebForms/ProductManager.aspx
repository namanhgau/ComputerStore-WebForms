<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductManager.aspx.cs" Inherits="ComputerStore.WebForms.ProductManager" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
    <h2 class="mb-4">Quản lý Kho Sản phẩm</h2>
    <asp:Label ID="lblMessage" runat="server" CssClass="text-success fw-bold d-block mb-3"></asp:Label>

    <div class="row">
        <div class="col-md-4">
            <div class="card shadow-sm p-3 mb-4">
                <h4>Thêm máy tính mới</h4>
                <div class="mb-2">
                    <label>Tên sản phẩm</label>
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="mb-2">
                    <label>Giá tiền</label>
                    <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                </div>
                <div class="mb-2">
                    <label>Hình ảnh</label>
                    <asp:FileUpload ID="fileUploadImage" runat="server" CssClass="form-control" />
                </div>
                <asp:Button ID="btnAdd" runat="server" Text="Lưu sản phẩm" CssClass="btn btn-success w-100 mt-2" OnClick="btnAdd_Click" />
            </div>
        </div>

        <div class="col-md-8">
            <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-bordered table-striped text-center align-middle" DataKeyNames="ProductId" 
                OnRowDeleting="gvProducts_RowDeleting"
                OnRowEditing="gvProducts_RowEditing"
                OnRowCancelingEdit="gvProducts_RowCancelingEdit"
                OnRowUpdating="gvProducts_RowUpdating">
                <Columns>
                    <asp:BoundField DataField="ProductName" HeaderText="Tên máy" />
                    <asp:BoundField DataField="Price" HeaderText="Giá bán (VNĐ)" />
                    <asp:ImageField DataImageUrlField="ImageUrl" HeaderText="Hình ảnh" ControlStyle-Width="80" ReadOnly="True" />
        
                    <%-- Bật cả nút Sửa và nút Xóa tự động --%>
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" HeaderText="Thao tác" ControlStyle-CssClass="btn btn-sm btn-outline-primary m-1" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
</asp:Content>
