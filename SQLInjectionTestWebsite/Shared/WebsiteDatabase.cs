using SQLInjectionTestWebsite.Shared.SQL;
using SQLInjectionTestWebsite.Shared.Utils;

namespace SQLInjectionTestWebsite.Shared
{
	public static class WebsiteDatabase
	{
		public const string DatabaseName = "StoreDB";
		public const string AccountsTableName = "Accounts";
		public const string ProductsTableName = "Products";

		private static readonly SQLiteDatabase s_Database = new SQLiteDatabase(DatabaseName);

		public static bool TryCreateAccount(AccountInfo account)
		{
			var accounts = s_Database.DeserializeObjects<AccountInfo>(AccountsTableName, $"SELECT * FROM {AccountsTableName} WHERE {nameof(AccountInfo.UserName)} = '{account.UserName}'");
			if (accounts.Count != 0)
				return false;

			s_Database.SerializeObjects(AccountsTableName, new AccountInfo[] { account});
			return true;
		}

		public static Option<AccountInfo> TryGetAccount(string accountName, string password)
		{
			var accounts = s_Database.DeserializeObjects<AccountInfo>(AccountsTableName, $"SELECT * FROM {AccountsTableName} WHERE UserName = '{accountName}' AND Password = '{password}' LIMIT 1");
			if(accounts.Count > 0)
				return new Option<AccountInfo>(accounts[0]);

			return new Option<AccountInfo>();
		}

		public static void RemoveAccount(string accountName, string password)
		{
			string command = 
				$"DELETE FROM {AccountsTableName} WHERE " +
					$"{nameof(AccountInfo.UserName)} = '{accountName}' AND " +
					$"{nameof(AccountInfo.Password)} = '{password}' " +
					$"LIMIT 1;";

			int affectedRows = s_Database.ExecuteCommand(command);
			if (affectedRows < 1)
				throw new Exception("Tried to delete an account that does not exist");
		}

		public static bool UpdateAccountBallance(string accountName, string password, double newBalance) 
		{
			string commandString =
				$"UPDATE {AccountsTableName} " +
				$"SET CurrentBalance = '{newBalance}' " +
				$"WHERE " +
					$"{nameof(AccountInfo.UserName)} = '{accountName}' AND " +
					$"{nameof(AccountInfo.Password)} = '{password}' " +
					$"LIMIT 1";

			return s_Database.ExecuteCommand(commandString) > 0;
		}

		public static bool AddProduct(ProductInfo product)
		{
			var products = s_Database.DeserializeObjects<ProductInfo>(ProductsTableName, $"SELECT * FROM {ProductsTableName} WHERE {nameof(ProductInfo.ID)} = '{product.ID}'");
			if (products.Count >= 1)
				return false;

			s_Database.SerializeObjects(ProductsTableName, new ProductInfo[] { product });
			return true;
		}

		public static List<ProductInfo> GetProducts(string searchTerm)
		{
			return new List<ProductInfo>();
		}

		public static void UpdateProductCount(string productId, int newCount)
		{

		}
	}
}
