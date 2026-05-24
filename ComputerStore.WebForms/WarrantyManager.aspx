<%@ Page Title="Quản lý Bảo hành" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WarrantyManager.aspx.cs" Inherits="ComputerStore.WebForms.WarrantyManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4 mb-5">
        <h2 class="fw-bold mb-4">🔧 Quản lý Bảo hành & Sửa chữa</h2>

        <div class="row">
            <div class="col-md-4 mb-4">
                <div class="card shadow-sm border-0">
                    <div class="card-header bg-primary text-white fw-bold">
                        📝 Tiếp nhận máy bảo hành
                    </div>
                    <div class="card-body bg-light">
                        <div class="mb-2">
                            <label class="form-label fw-bold">Số Serial / Service Tag</label>
                            <asp:TextBox ID="txtSerial" runat="server" CssClass="form-control text-uppercase" placeholder="VD: PF2VXYZ"></asp:TextBox>
                        </div>
                        <div class="mb-2">
                            <label class="form-label fw-bold">Tên khách hàng</label>
                            <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="mb-2">
                            <label class="form-label fw-bold">Số điện thoại</label>
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label class="form-label fw-bold">Tình trạng máy / Lỗi báo</label>
                            <asp:TextBox ID="txtIssue" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" placeholder="Mô tả lỗi..."></asp:TextBox>
                        </div>
                        <asp:Button ID="btnAddWarranty" runat="server" Text="Lưu Phiếu Nhận Máy" CssClass="btn btn-primary w-100 fw-bold" OnClick="btnAddWarranty_Click" />
                        <asp:Label ID="lblMsg" runat="server" CssClass="d-block mt-2 text-center fw-bold"></asp:Label>
                    </div>
                </div>
            </div>

            <div class="col-md-8">
                <div class="card shadow-sm border-0">
                    <div class="card-body p-0">
                        <table class="table table-hover align-middle mb-0">
                            <thead class="table-dark">
                                <tr>
                                    <th class="ps-3">Mã phiếu</th>
                                    <th>Serial</th>
                                    <th>Khách hàng</th>
                                    <th>Trạng thái hiện tại</th>
                                    <th class="text-center">Cập nhật</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptWarranties" runat="server" OnItemCommand="rptWarranties_ItemCommand">
                                    <ItemTemplate>
                                        <tr>
                                            <asp:HiddenField ID="hdnWarrantyId" runat="server" Value='<%# Eval("WarrantyId") %>' />
                                            <td class="ps-3 fw-bold">#BH<%# Eval("WarrantyId") %></td>
                                            <td class="text-danger fw-bold"><%# Eval("SerialNumber") %></td>
                                            <td>
                                                <span class="d-block fw-bold"><%# Eval("CustomerName") %></span>
                                                <small class="text-muted">📞 <%# Eval("CustomerPhone") %></small>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select form-select-sm fw-bold text-primary" SelectedValue='<%# Eval("Status") %>'>
                                                    <asp:ListItem Text="Đã tiếp nhận" Value="Đã tiếp nhận"></asp:ListItem>
                                                    <asp:ListItem Text="Đang kiểm tra" Value="Đang kiểm tra"></asp:ListItem>
                                                    <asp:ListItem Text="Chờ linh kiện hãng" Value="Chờ linh kiện hãng"></asp:ListItem>
                                                    <asp:ListItem Text="Đã sửa xong" Value="Đã sửa xong"></asp:ListItem>
                                                    <asp:ListItem Text="Đã trả khách" Value="Đã trả khách"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td class="text-center">
                                                <asp:Button ID="btnUpdate" runat="server" CommandName="UpdateStatus" Text="Lưu" CssClass="btn btn-sm btn-success fw-bold" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                        <asp:Panel ID="pnlNoData" runat="server" Visible="false" CssClass="text-center p-4 text-muted">
                            Chưa có máy nào đang bảo hành.
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>