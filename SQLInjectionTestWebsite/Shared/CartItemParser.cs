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
	}
}
