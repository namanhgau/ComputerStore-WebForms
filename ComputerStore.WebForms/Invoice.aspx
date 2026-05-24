<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Invoice.aspx.cs" Inherits="ComputerStore.WebForms.Invoice" %>

<!DOCTYPE html>
<html lang="vi">
<head runat="server">
    <meta charset="utf-8" />
    <title>Hóa đơn mua hàng</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body { background-color: #fff; font-family: 'Times New Roman', Times, serif; }
        .invoice-box { max-width: 800px; margin: auto; padding: 30px; border: 1px solid #eee; box-shadow: 0 0 10px rgba(0, 0, 0, 0.15); }
        
        /* CSS đặc biệt phục vụ cho việc IN ẤN hoặc xuất PDF */
        @media print {
            .no-print { display: none; } /* Ẩn các nút bấm khi in */
            .invoice-box { border: none; box-shadow: none; padding: 0; }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-4 mb-4 text-center no-print">
            <button type="button" class="btn btn-danger fw-bold px-4 me-2" onclick="window.print();">🖨️ Tiến hành In / Xuất PDF</button>
            <button type="button" class="btn btn-secondary fw-bold" onclick="window.close();">Đóng cửa sổ</button>
        </div>

        <div class="invoice-box">
            <div class="row mb-4">
                <div class="col-6">
                    <h4 class="fw-bold text-uppercase m-0">💻 Cửa hàng "Hết lước chấm"</h4>
                    <small class="text-muted">Địa chỉ: 123 Đường Cầu Giấy, Hà Nội</small><br />
                    <small class="text-muted">Hotline: 0987.654.321</small>
                </div>
                <div class="col-6 text-end">
                    <h2 class="fw-bold text-danger m-0">HÓA ĐƠN MUA HÀNG</h2>
                    <small class="fw-bold text-dark">Mã đơn hàng: #<asp:Label ID="lblOrderId" runat="server"></asp:Label></small><br />
                    <small class="text-muted">Ngày lập: <asp:Label ID="lblOrderDate" runat="server"></asp:Label></small>
                </div>
            </div>

            <hr />

            <div class="row mb-4 mt-4">
                <div class="col-10">
                    <h6 class="fw-bold text-secondary text-uppercase mb-2">📍 Thông tin giao hàng</h6>
                    <table class="table table-sm table-borderless m-0">
                        <tr>
                            <td style="width: 150px;"><strong>Người nhận:</strong></td>
                            <td><asp:Label ID="lblCustomerName" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td><strong>Số điện thoại:</strong></td>
                            <td><asp:Label ID="lblPhone" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td><strong>Địa chỉ ship:</strong></td>
                            <td><asp:Label ID="lblAddress" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td><strong>Ghi chú:</strong></td>
                            <td><asp:Label ID="lblNote" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                </div>
            </div>

            <table class="table table-bordered align-middle">
                <thead class="table-light">
                    <tr class="text-center">
                        <th style="width: 50px;">STT</th>
                        <th>Tên sản phẩm (Laptop)</th>
                        <th style="width: 100px;">Số lượng</th>
                        <th style="width: 150px;">Đơn giá</th>
                        <th style="width: 150px;">Thành tiền</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptInvoiceDetails" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td class="text-center"><%# Container.ItemIndex + 1 %></td>
                                <td class="fw-bold"><%# Eval("ProductName") %></td>
                                <td class="text-center"><%# Eval("Quantity") %></td>
                                <td class="text-end"><%# string.Format("{0:N0}", Eval("UnitPrice")) %>đ</td>
                                <td class="text-end text-danger fw-bold"><%# string.Format("{0:N0}", Convert.ToDecimal(Eval("Quantity")) * Convert.ToDecimal(Eval("UnitPrice"))) %>đ</td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>

            <div class="row justify-content-end mt-4">
                <div class="col-5">
                    <div class="d-flex justify-content-between mb-2">
                        <span class="fw-bold">Mã giảm giá áp dụng:</span>
                        <span class="text-success fw-bold"><asp:Label ID="lblPromoCode" runat="server" Text="Không có"></asp:Label></span>
                    </div>
                    <div class="d-flex justify-content-between border-top pt-2">
                        <h5 class="fw-bold m-0">Tổng thanh toán:</h5>
                        <h5 class="fw-bold text-danger m-0"><asp:Label ID="lblTotalAmount" runat="server"></asp:Label> VNĐ</h5>
                    </div>
                </div>
            </div>

            <div class="row text-center mt-5 pt-3">
                <div class="col-6">
                    <span class="fst-italic">Khách hàng nhận máy</span><br />
                    <small class="text-muted">(Ký và ghi rõ họ tên)</small>
                </div>
                <div class="col-6">
                    <span class="fw-bold">Người lập hóa đơn</span><br />
                    <small class="text-muted">(Ký, đóng dấu cửa hàng)</small>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
