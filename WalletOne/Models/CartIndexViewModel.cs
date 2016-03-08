using WalletOne.Domain.Entities;

namespace WalletOne.Models {
    public class CartIndexViewModel {
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
    }
}