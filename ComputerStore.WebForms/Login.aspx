<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ComputerStore.WebForms.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-4">
            <div class="card shadow">
                <div class="card-header bg-primary text-white text-center">
                    <h4>Đăng nhập hệ thống</h4>
                </div>
                <div class="card-body">
                    <div class="form-group mb-3">
                        <label>Tên đăng nhập</label>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Nhập tài khoản"></asp:TextBox>
                    </div>
                    <div class="form-group mb-3">
                        <label>Mật khẩu</label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Nhập mật khẩu"></asp:TextBox>
                    </div>
                    <asp:Label ID="lblError" runat="server" CssClass="text-danger mb-3 d-block"></asp:Label>
                    
                    <asp:Button ID="btnLogin" runat="server" Text="Đăng nhập" CssClass="btn btn-primary w-100" OnClick="btnLogin_Click" />
                    
                    <div class="text-center mt-3">
                        <a href="Register.aspx">Chưa có tài khoản? Đăng ký ngay</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
</asp:Content>
