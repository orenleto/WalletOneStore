using WalletOne.Domain.Entities;

namespace WalletOne.Models
{
    public class OrderViewModel
    {
        public CartIndexViewModel CartIndexViewModel { get; set; }
        public ShippingDetails ShippingDetails { get; set; }
    }
}