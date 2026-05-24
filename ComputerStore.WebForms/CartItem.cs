using System;

namespace ComputerStore
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        // Cột này tự động tính Thành tiền = Giá x Số lượng
        public bool IsSelected { get; set; } = true;

        public decimal Total => Price * Quantity;
    }
}