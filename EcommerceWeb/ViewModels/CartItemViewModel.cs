namespace EcommerceWeb.ViewModels
{
    public class CartItemViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public CheckoutVM CheckoutVM { get; set; }
        public double ThanhTien { get; set; }
    }
}
