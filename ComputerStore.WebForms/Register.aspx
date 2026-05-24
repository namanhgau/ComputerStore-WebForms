<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="ComputerStore.WebForms.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5 mb-5">
    <div class="row justify-content-center">
        <div class="col-md-5">
            <div class="card shadow">
                <div class="card-header bg-success text-white text-center">
                    <h4>Đăng ký tài khoản mới</h4>
                </div>
                <div class="card-body">
                    <div class="form-group mb-3">
                        <label>Họ và tên</label>
                        <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" placeholder="Nhập họ tên của bạn" required="required"></asp:TextBox>
                    </div>
                    <div class="form-group mb-3">
                        <label>Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="Nhập địa chỉ email" required="required"></asp:TextBox>
                    </div>
                    <div class="form-group mb-3">
                        <label>Tên đăng nhập</label>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Chọn một tên đăng nhập" required="required"></asp:TextBox>
                    </div>
                    <div class="form-group mb-3">
                        <label>Mật khẩu</label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Nhập mật khẩu" required="required"></asp:TextBox>
                    </div>
                    <div class="form-group mb-3">
                        <label>Xác nhận mật khẩu</label>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Nhập lại mật khẩu" required="required"></asp:TextBox>
                    </div>
                    
                    <asp:Label ID="lblMessage" runat="server" CssClass="fw-bold mb-3 d-block"></asp:Label>
                    
                    <asp:Button ID="btnRegister" runat="server" Text="Đăng ký ngay" CssClass="btn btn-success w-100" OnClick="btnRegister_Click" />
                    
                    <div class="text-center mt-3">
                        <a href="Login.aspx">Đã có tài khoản? Quay lại đăng nhập</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
</asp:Content>
