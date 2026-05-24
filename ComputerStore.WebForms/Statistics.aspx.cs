using System;
using System.Linq;

namespace ComputerStore.WebForms
{
    public partial class Statistics : System.Web.UI.Page
    {
        // Khai báo 2 biến toàn cục để truyền dữ liệu sang JavaScript
        public string ChartLabels = "";
        public string ChartData = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            // Bảo mật: Chỉ tài khoản Manager mới được xem doanh số
            if (Session["Role"] == null || Session["Role"].ToString() != "Manager")
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CalculateStatistics();
            }
        }

        private void CalculateStatistics()
        {
            using (var db = new ComputerStoreDBEntities())
            {
                // 1. Tính toán số liệu cho các thẻ Dashboard
                // Lấy toàn bộ danh sách đơn hàng ra bộ nhớ để tính toán cho mượt
                var allOrders = db.Orders.ToList();

                // Đơn hàng thành công là đơn có trạng thái "Hoàn thành"
                var successOrdersList = allOrders.Where(o => o.Status == "Hoàn thành").ToList();
                decimal totalRevenue = successOrdersList.Sum(o => o.TotalAmount);
                int successCount = successOrdersList.Count;

                // Đơn hàng chờ xử lý
                int pendingCount = allOrders.Count(o => o.Status == "Chờ duyệt");

                // Đổ số liệu ra giao diện thẻ
                lblTotalRevenue.Text = string.Format("{0:N0}", totalRevenue);
                lblSuccessOrders.Text = successCount.ToString();
                lblPendingOrders.Text = pendingCount.ToString();

                // 2. Gom nhóm dữ liệu để tính Doanh thu theo từng tháng (Dùng LINQ GroupBy)
                var monthlyData = successOrdersList
                    .Where(o => o.OrderDate.HasValue) // Đảm bảo đơn hàng có ghi nhận ngày đặt
                    .GroupBy(o => new { o.OrderDate.Value.Year, o.OrderDate.Value.Month })
                    .Select(g => new
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        OrderCount = g.Count(),
                        Revenue = g.Sum(o => o.TotalAmount)
                    })
                    .OrderByDescending(g => g.Year)
                    .ThenByDescending(g => g.Month)
                    .ToList();

                // Đổ dữ liệu vào bảng thống kê
                rptMonthlyRevenue.DataSource = monthlyData;
                rptMonthlyRevenue.DataBind();

                // 3. XỬ LÝ CHUỖI ĐỂ VẼ BIỂU ĐỒ
                if (monthlyData != null && monthlyData.Count > 0)
                {
                    // Đảo ngược danh sách (Reverse) để biểu đồ hiển thị tháng cũ trước, tháng mới sau theo đúng thứ tự thời gian
                    var chartList = monthlyData.AsEnumerable().Reverse().ToList();

                    // Tạo chuỗi nhãn dạng: 'Tháng 04/2026', 'Tháng 05/2026'
                    var labels = chartList.Select(g => $"'{g.Month}/{g.Year}'");
                    ChartLabels = string.Join(",", labels);

                    // Tạo chuỗi số liệu dạng: 15000000, 45000000
                    var dataValues = chartList.Select(g => g.Revenue.ToString());
                    ChartData = string.Join(",", dataValues);
                }
            }

        }
    }
}