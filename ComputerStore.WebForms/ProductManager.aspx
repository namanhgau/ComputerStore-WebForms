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
                    <asp:TemplateField HeaderText="Hình ảnh">
                        <ItemTemplate>
                            <asp:Image ID="imgProduct" runat="server" ImageUrl='<%# Eval("ImageUrl") %>' Width="60" Height="60" style="object-fit:contain;" />
                        </ItemTemplate>
    
                        <EditItemTemplate>
                            <asp:HiddenField ID="hdnOldImage" runat="server" Value='<%# Eval("ImageUrl") %>' />
        
                            <asp:Image ID="imgEditPreview" runat="server" ImageUrl='<%# Eval("ImageUrl") %>' Width="40" CssClass="d-block mb-1 border" />
        
                            <asp:FileUpload ID="fileUploadImage" runat="server" CssClass="form-control form-control-sm" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                </div>
                <asp:Button ID="btnAdd" runat="server" Text="Lưu sản phẩm" CssClass="btn btn-success w-100 mt-2" OnClick="btnAdd_Click" />
            </div>
        </div>

        <div class="col-md-8">
            <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" DataKeyNames="ProductId" 
                CssClass="table table-bordered table-hover text-center align-middle"
                OnRowDeleting="gvProducts_RowDeleting" 
                OnRowEditing="gvProducts_RowEditing" 
                OnRowCancelingEdit="gvProducts_RowCancelingEdit" 
                AllowPaging="True"
                PageSize="5"
                OnPageIndexChanging="gvProducts_PageIndexChanging"
                OnRowUpdating="gvProducts_RowUpdating">

                <Columns>
                    <asp:BoundField DataField="ProductName" HeaderText="Tên máy" ControlStyle-CssClass="form-control" />
        
                    <asp:BoundField DataField="Price" HeaderText="Giá bán (VNĐ)" ControlStyle-CssClass="form-control" />

                    <asp:TemplateField HeaderText="Hình ảnh">
                        <ItemTemplate>
                            <asp:Image ID="imgProduct" runat="server" ImageUrl='<%# Eval("ImageUrl") %>' Width="50" Height="50" style="object-fit:contain;" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:HiddenField ID="hdnOldImage" runat="server" Value='<%# Eval("ImageUrl") %>' />
                            <asp:FileUpload ID="fuEditImage" runat="server" CssClass="form-control form-control-sm" />
                        </EditItemTemplate> 
                    </asp:TemplateField>

                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" ControlStyle-CssClass="btn btn-sm btn-outline-primary" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
</asp:Content>
