<%@ Page Title="Quản lý Đơn hàng" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrderManager.aspx.cs" Inherits="ComputerStore.WebForms.OrderManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4 mb-5">
        <h2 class="fw-bold mb-4">📦 Quản lý Đơn đặt hàng</h2>

        <div class="card shadow-sm border-0">
            <div class="card-body p-0">
                <table class="table table-hover align-middle mb-0">
                    <thead class="table-dark">
                        <tr>
                            <th class="ps-3">Mã ĐH</th>
                            <th>Ngày đặt</th>
                            <th>Khách hàng</th>
                            <th>Thông tin nhận hàng</th>
                            <th>Tổng tiền</th>
                            <th>Trạng thái</th>
                            <th class="text-center">Hành động</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptOrders" runat="server" OnItemCommand="rptOrders_ItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <asp:HiddenField ID="hdnOrderId" runat="server" Value='<%# Eval("OrderId") %>' />
                                    
                                    <td class="ps-3 fw-bold">#<%# Eval("OrderId") %></td>
                                    <td><%# Eval("OrderDate", "{0:dd/MM/yyyy HH:mm}") %></td>
                                    <td class="fw-bold text-primary"><%# Eval("FullName") %></td>
                                    <td>
                                        <small class="d-block">📞 <%# Eval("Phone") %></small>
                                        <small class="d-block text-muted">📍 <%# Eval("ShippingAddress") %></small>
                                    </td>
                                    <td class="text-danger fw-bold"><%# string.Format("{0:N0}", Eval("TotalAmount")) %>đ</td>
                                    
                                    <td>
                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select form-select-sm fw-bold" SelectedValue='<%# Eval("Status") %>'>
                                            <asp:ListItem Text="Chờ duyệt" Value="Chờ duyệt"></asp:ListItem>
                                            <asp:ListItem Text="Đang giao" Value="Đang giao"></asp:ListItem>
                                            <asp:ListItem Text="Hoàn thành" Value="Hoàn thành"></asp:ListItem>
                                            <asp:ListItem Text="Đã hủy" Value="Đã hủy"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    
                                    <td class="text-center">
                                        <asp:Button ID="btnUpdate" runat="server" CommandName="UpdateStatus" Text="Lưu" CssClass="btn btn-sm btn-success fw-bold" />

                                        <asp:Button ID="Button1" runat="server" CommandName="UpdateStatus" Text="Lưu" CssClass="btn btn-sm btn-success fw-bold me-2" />

                                        <a href='Invoice.aspx?id=<%# Eval("OrderId") %>' target="_blank" class="btn btn-sm btn-outline-secondary fw-bold">
                                            📄 In hóa đơn
                                        </a>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
                
                <asp:Panel ID="pnlNoOrders" runat="server" Visible="false" CssClass="text-center p-5">
                    <h5 class="text-muted">Chưa có đơn hàng nào trong hệ thống!</h5>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>