namespace Dolphin_Book.ViewModels
{
    public class PaymentConfirmVM
    {
        public string? Email { get; set; }
        public DateTime? DateTime { get; set; }
        public string? OrderNumber { get; set; }
        public double? totalPrice { get; set; }
    }
}
