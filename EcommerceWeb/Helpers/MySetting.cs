namespace EcommerceWeb.Helpers
{
    public class MySetting
    {
        public static string CART_KEY = "MYCART";
        public static string NAME_SORT_DESCENDING = "Giảm dần theo tên";
        public static string NAME_SORT_ASCENDING = "Tăng dần theo tên";
        public static string PRICE_SORT_DESCENDING = "Giảm dần theo giá";
        public static string PRICE_SORT_ASCENDING = "Tăng dần theo giá";
        public static string COD = "COD";
        public static string PAYPAL = "Paypal";
        public static string VNPAY = "VnPay";
        public static string STATE = "Chờ xác nhận";
        public static string SHIPPING_COD = "Grab";
        public static string SHIPPING_PAYPAL = "Online";
        public static string SHIPPING_VNPAY = "Online";
        public static double SHIPPING_FEE = 30000;
        public static string CLAIM_CUSTOMER_ID = "CustomerID";
    }

    public static class PaymentType
    {
        public static string COD = "COD";
        public static string PAYPAL = "Paypal";
        public static string VNPAY = "VnPay";
        public static string MOMO = "MoMo";
        public static string STRIPE = "Stripe";
    }
}
