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

		public static event EventHandler? ProductsUpdated;

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
				$"SET {nameof(AccountInfo.CurrentBalance)} = '{newBalance}' " +
				$"WHERE " +
					$"{nameof(AccountInfo.UserName)} = '{accountName}' AND " +
					$"{nameof(AccountInfo.Password)} = '{password}' " +
					$"LIMIT 1";

			return s_Database.ExecuteCommand(commandString) > 0;
		}

		public static bool UpdateAccountCart(string accountName, string password, string newCart)
		{
			string commandString =
				$"UPDATE {AccountsTableName} " +
				$"SET {nameof(AccountInfo.CartItems)} = '{newCart}' " +
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
			ProductsUpdated?.Invoke(null, EventArgs.Empty);
			return true;
		}

		public static List<ProductInfo> SearchProducts(string searchTerm)
		{
			string termVarName = "@searchTerm";
			List<(string, object)> parameters = new List<(string, object)> { (termVarName,  searchTerm) };

			string command = $"SELECT * FROM {ProductsTableName} " +
				$"WHERE KEYWORD_SEARCH({termVarName}, {nameof(ProductInfo.Name)}) > 0 " +
				$"ORDER BY KEYWORD_SEARCH({termVarName}, {nameof(ProductInfo.Name)}) DESC";
			
			return s_Database.DeserializeObjects<ProductInfo>(ProductsTableName, command, parameters);
		}

		public static List<ProductInfo> GetAllProducts()
		{
			return s_Database.DeserializeObjects<ProductInfo>(ProductsTableName, $"SELECT * FROM {ProductsTableName}");
		}

		public static bool UpdateProductCount(string productId, int newCount)
		{
			string commandString =
				$"UPDATE {ProductsTableName} " +
				$"SET {nameof(ProductInfo.Count)} = '{newCount}' " +
				$"WHERE " +
					$"{nameof(ProductInfo.ID)} = '{productId}'" +
					$"LIMIT 1";

			bool updated = s_Database.ExecuteCommand(commandString) > 0;
			ProductsUpdated?.Invoke(null, EventArgs.Empty);
			return updated;
		}

		public static bool DeleteProduct(string productId)
		{
			string command =
				$"DELETE FROM {ProductsTableName} WHERE " +
					$"{nameof(ProductInfo.ID)} = '{productId}'" +
					$"LIMIT 1;";

			bool removed = s_Database.ExecuteCommand(command) > 0;
			if (removed)
				ProductsUpdated?.Invoke(null, EventArgs.Empty);

			return removed;
		}
	}
}
