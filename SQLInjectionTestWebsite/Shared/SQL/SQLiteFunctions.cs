using SQLInjectionTestWebsite.Shared.SQL;
using System.Data.SQLite;
using ProductData = System.Tuple<string, float, string, string, int>;

namespace SQLInjectionTestWebsite.Shared.SQL
{
	[SQLFunctionLibrary]
	public static class SQLiteFunctions
	{
		[SQLCustomFunction("STORESEARCH", 2, FunctionType.Scalar)]
		public static object StoreSearch(object[] args)
		{
			string searchTerm = (string)args[0];
			ProductData product = (ProductData)args[1];

			if (searchTerm != "Worked")
				throw new Exception("Did not work");

			return true;
		}
	}
}
