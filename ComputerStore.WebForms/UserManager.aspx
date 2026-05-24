<%@ Page Title="Quản lý Người dùng" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserManager.aspx.cs" Inherits="ComputerStore.WebForms.UserManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4 mb-5">
        <h2 class="fw-bold mb-4">👥 Quản lý Người dùng & Phân quyền</h2>

        <div class="card shadow-sm border-0">
            <div class="card-body p-0">
                <table class="table table-hover align-middle mb-0">
                    <thead class="table-dark">
                        <tr>
                            <th class="ps-3">ID</th>
                            <th>Tên đăng nhập</th>
                            <th>Họ và tên</th>
                            <th>Quyền hạn (Role)</th>
                            <th class="text-center">Trạng thái</th>
                            <th class="text-center">Hành động</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptUsers" runat="server" OnItemCommand="rptUsers_ItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <asp:HiddenField ID="hdnUserId" runat="server" Value='<%# Eval("UserId") %>' />
                                    <td class="ps-3 fw-bold"><%# Eval("UserId") %></td>
                                    <td><strong><%# Eval("Username") %></strong></td>
                                    <td><%# Eval("FullName") %></td>
                                    
                                    <td>
                                        <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-select form-select-sm w-auto d-inline-block fw-bold text-primary" SelectedValue='<%# GetSafeRole(Eval("Role")) %>'>
                                            <asp:ListItem Text="Thành viên" Value="Member"></asp:ListItem>
                                            <asp:ListItem Text="Quản lý" Value="Manager"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Button ID="btnSaveRole" runat="server" CommandName="ChangeRole" Text="Sửa" CssClass="btn btn-sm btn-outline-primary ms-1 py-0 px-2" />
                                    </td>
                                    
                                    <td class="text-center">
                                        <%# Convert.ToBoolean(Eval("IsActive")) ? "<span class='badge bg-success'>Đang hoạt động</span>" : "<span class='badge bg-danger'>Đang bị khóa</span>" %>
                                    </td>
                                    
                                    <td class="text-center">
                                        <asp:LinkButton ID="btnToggleLock" runat="server" 
                                            CommandName="ToggleLock" 
                                            CommandArgument='<%# Eval("UserId") %>'
                                            CssClass='<%# Convert.ToBoolean(Eval("IsActive")) ? "btn btn-sm btn-danger fw-bold" : "btn btn-sm btn-success fw-bold" %>'
                                            Text='<%# Convert.ToBoolean(Eval("IsActive")) ? "🔒 Khóa" : "🔓 Mở khóa" %>'
                                            OnClientClick="return confirm('Bạn có chắc muốn thực hiện thao tác này?');">
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</asp:Content>