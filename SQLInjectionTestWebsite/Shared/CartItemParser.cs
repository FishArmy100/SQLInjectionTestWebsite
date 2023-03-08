namespace SQLInjectionTestWebsite.Shared
{
	public static class CartItemParser
	{
		public const string Seperator = ";";

		public static List<ProductInfo> ParseProducts(string cart)
		{
			string[] productIds = cart.Split(Seperator);
			List<ProductInfo> products = WebsiteDatabase.GetAllProducts()
				.Where(p => productIds.Contains(p.ID))
				.ToList();
			return products;
		}

		public static string EncodeProducts(IEnumerable<ProductInfo> products)
		{
			return string.Join(Seperator, products.Select(p => p.ID));
		}

		public static AccountInfo AddToCart(ProductInfo product, AccountInfo account)
		{
			string cartItems = account.CartItems;
			if(!cartItems.Split(Seperator).Contains(product.ID))
			{
				cartItems += Seperator + product.ID;
			}

			return new AccountInfo(account.UserName, account.Password, account.Email, account.CreditCardNumber, account.ID, account.CurrentBalance, account.IsAdmin, cartItems);
		}

		public static AccountInfo RemoveFromCart(ProductInfo product, AccountInfo account)
		{
			var products = ParseProducts(account.CartItems);
			var modifiedProducts = products.Where(p => p.ID != product.ID).ToList();
			string encodedProducts = EncodeProducts(modifiedProducts);
			return new AccountInfo(account.UserName, account.Password, account.Email, account.CreditCardNumber, account.ID, account.CurrentBalance, account.IsAdmin, encodedProducts);
		}

		public static bool IsInCart(ProductInfo product, AccountInfo account)
		{
			return account.CartItems
				.Split(Seperator)
				.Contains(product.ID);
		}
	}
}
