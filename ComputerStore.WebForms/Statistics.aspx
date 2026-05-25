<%@ Page Title="Thống kê doanh thu" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Statistics.aspx.cs" Inherits="ComputerStore.WebForms.Statistics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4 mb-5">
        <h2 class="fw-bold mb-4">📊 Thống kê doanh thu & Báo cáo số liệu</h2>

        <div class="row mb-4">
            <div class="col-md-4">
                <div class="card bg-success text-white border-0 shadow-sm">
                    <div class="card-body p-4">
                        <h6 class="text-uppercase fw-bold text-white-50">💰 Tổng Doanh Thu (Thực thu)</h6>
                        <h2 class="fw-bold m-0"><asp:Label ID="lblTotalRevenue" runat="server" Text="0"></asp:Label> đ</h2>
                        <small class="text-white-50">Tính từ các đơn hàng "Hoàn thành"</small>
                    </div>
                </div>
            </div>
            
            <div class="col-md-4">
                <div class="card bg-primary text-white border-0 shadow-sm">
                    <div class="card-body p-4">
                        <h6 class="text-uppercase fw-bold text-white-50">📦 Đơn hàng thành công</h6>
                        <h2 class="fw-bold m-0"><asp:Label ID="lblSuccessOrders" runat="server" Text="0"></asp:Label> đơn</h2>
                        <small class="text-white-50">Đã giao máy tính và thu tiền</small>
                    </div>
                </div>
            </div>

            <div class="col-md-4">
                <div class="card bg-warning text-dark border-0 shadow-sm">
                    <div class="card-body p-4">
                        <h6 class="text-uppercase fw-bold text-black-50">⏳ Đơn hàng chờ duyệt</h6>
                        <h2 class="fw-bold m-0"><asp:Label ID="lblPendingOrders" runat="server" Text="0"></asp:Label> đơn</h2>
                        <small class="text-black-50">Cần liên hệ khách để xác nhận</small>
                    </div>
                </div>
            </div>
        </div>

        <div class="card shadow-sm border-0">
            <div class="card-header bg-dark text-white fw-bold py-3">
                📈 Doanh thu tổng hợp theo từng tháng
            </div>
            <div class="card-body p-0">
                <table class="table table-hover table-striped align-middle mb-0">
                    <thead class="table-light">
                        <tr>
                            <th class="ps-4">Tháng / Năm</th>
                            <th class="text-center">Số đơn giao dịch</th>
                            <th class="text-end pe-4">Tổng doanh thu thu về</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptMonthlyRevenue" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td class="ps-4 fw-bold">Tháng <%# Eval("Month") %> / <%# Eval("Year") %></td>
                                    <td class="text-center"><span class="badge bg-secondary"><%# Eval("OrderCount") %> đơn</span></td>
                                    <td class="text-end text-danger fw-bold pe-4"><%# string.Format("{0:N0}", Eval("Revenue")) %> VNĐ</td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="card shadow-sm border-0 mb-4">
            <div class="card-header bg-dark text-white fw-bold py-3">
                📊 Biểu đồ tăng trưởng doanh thu theo tháng
            </div>
            <div class="card-body p-4">
                <div style="position: relative; height:300px; width:100%">
                    <canvas id="revenueChart"></canvas>
                </div>
            </div>
        </div>
        <div class="card shadow-sm border-0 mt-4 mb-4">
            <div class="card-header bg-info text-white fw-bold py-3">
                📅 Doanh thu chi tiết theo từng ngày (30 ngày gần nhất)
            </div>
            <div class="card-body p-0">
                <table class="table table-hover table-striped align-middle mb-0">
                    <thead class="table-light">
                        <tr>
                            <th class="ps-4">Ngày giao dịch</th>
                            <th class="text-center">Số đơn thành công</th>
                            <th class="text-end pe-4">Doanh thu trong ngày</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptDailyRevenue" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td class="ps-4 fw-bold"><%# Convert.ToDateTime(Eval("Date")).ToString("dd/MM/yyyy") %></td>
                                    <td class="text-center"><span class="badge bg-secondary"><%# Eval("OrderCount") %> đơn</span></td>
                                    <td class="text-end text-danger fw-bold pe-4"><%# string.Format("{0:N0}", Eval("Revenue")) %> VNĐ</td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="card shadow-sm border-0 mb-4">
            <div class="card-header bg-primary text-white fw-bold py-3">
                📈 Biểu đồ xu hướng doanh thu 7 ngày gần nhất
            </div>
            <div class="card-body p-4">
                <div style="position: relative; height:300px; width:100%">
                    <canvas id="dailyRevenueChart"></canvas>
                </div>
            </div>
        </div>
    </div>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    
    <script>
        document.addEventListener("DOMContentLoaded", function () {
            const ctx = document.getElementById('revenueChart').getContext('2d');
            
            // Khởi tạo biểu đồ Chart.js
            new Chart(ctx, {
                type: 'bar', // Bạn có thể đổi thành 'line' nếu muốn biểu đồ đường
                data: {
                    // Nhận danh sách nhãn (Tháng/Năm) truyền từ C# sang
                    labels: [<%= ChartLabels %>], 
                    datasets: [{
                        label: 'Doanh thu thực thu (VNĐ)',
                        // Nhận số liệu doanh thu tương ứng truyền từ C# sang
                        data: [<%= ChartData %>], 
                        backgroundColor: 'rgba(40, 167, 69, 0.7)', // Màu xanh lá đổ bóng
                        borderColor: 'rgba(40, 167, 69, 1)',
                        borderWidth: 2,
                        borderRadius: 5
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                        y: {
                            beginAtZero: true,
                            ticks: {
                                // Định dạng hiển thị tiền tệ có dấu phẩy cho trục Y
                                callback: function(value) {
                                    return value.toLocaleString('vi-VN') + ' đ';
                                }
                            }
                        }
                    },
                    plugins: {
                        legend: {
                            display: true,
                            position: 'top'
                        }
                    }
                }
            });
            // Biểu đồ doanh thu hàng ngày
            const ctxDaily = document.getElementById('dailyRevenueChart').getContext('2d');
            new Chart(ctxDaily, {
                type: 'line',
                data: {
                    labels: [<%= DailyLabels %>],
                    datasets: [{
                        label: 'Doanh thu (VNĐ)',
                        data: [<%= DailyData %>],
                        borderColor: '#0d6efd',
                        backgroundColor: 'rgba(13, 110, 253, 0.1)',
                        fill: true,
                        tension: 0.4, // Tạo độ cong cho đường
                        pointRadius: 5,
                        pointBackgroundColor: '#0d6efd'
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                        y: { beginAtZero: true, ticks: { callback: v => v.toLocaleString() + ' đ' } }
                    }
                }
            });
        });

    </script>
</asp:Content>