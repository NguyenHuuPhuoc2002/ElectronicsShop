﻿namespace EcommerceWeb.Helpers
{
    public class MySetting
    {
        public static string CART_KEY = "MYCART";
        public static string ROLE_ADMIN = "Admin";
        public static string ROLE_CUSTOMER = "Customer";
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
        public static double SHIPPING_FEE = 30;
        public static string CLAIM_CUSTOMER_ID = "CustomerID";
        public static string CLAIM_EMPLOYEE_ID = "EmployeeID";

        public static string DEPARTMENT = "Permission";
        public static string ROLE_DIRECTORS = "BGD";
        public static string ROLE_ACCOUNTING = "PKTo";
        public static string ROLE_TECHNIQUE = "PKT";
        public static string ROLE_BUSINESS = "PKD";
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
