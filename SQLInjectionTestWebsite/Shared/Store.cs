using SQLInjectionTestWebsite.Shared.Utils;

namespace SQLInjectionTestWebsite.Shared
{
	public static class Store
	{
		public static readonly StoreProductRegistry Registry = new StoreProductRegistry(new () 
		{ 
			GenProduct("Bull Wip", 20.0f, 10),
			GenProduct("Leather Jeans", 50.0f, 30),
			GenProduct("Cowboy Hat", 30.0f, 20),
			GenProduct("Leather Boots", 50.0f, 15),
			GenProduct("Leather High Heeled Boots", 45.0f, 10),
			GenProduct("Indiana Jones Action Figure", 1000.0f, 5),
			GenProduct("Giant Rolling Stone Trap", 5000.0f, 1),
			GenProduct("Steel Sword", 300.0f, 5),
			GenProduct("Steel Knife", 150.0f, 10),
			GenProduct("Steel Spike Trap", 6000.0f, 2),
			GenProduct("Fire Pit Trap", 1000.0f, 3),
			GenProduct("Snake Pit Trap", 2000.0f, 5),
			GenProduct("Torch", 30.0f, 30),
			GenProduct("Flash-Light", 25.0f, 25),
			GenProduct("Model Ark of the Covenent", 40000.0f, 1)
		});

		public static Option<AccountInfo> CurrentUser { get; set; } = AccountInfo.GenAccount("Bob", "1234567", "bob@gmail.com", "0123456789");

		private static StoreProduct GenProduct(string name, float price, uint count) => new StoreProduct(new ProductInfo(name, price, ""), count);
	}
}
