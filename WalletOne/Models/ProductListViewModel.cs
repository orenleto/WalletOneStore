using System.Collections.Generic;
using WalletOne.Domain.Entities;

namespace WalletOne.Models {
    public class ProductsListViewModel {
        public IEnumerable<Product> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}