using SQLInjectionTestWebsite.Shared.SQL;
using System.Data.SQLite;
using ProductData = System.Tuple<string, float, string, string, int>;
using TestProductData = System.Tuple<string, string>;

namespace SQLInjectionTestWebsite.Shared.SQL
{
	[SQLFunctionLibrary]
	public static class SQLiteFunctions
	{
		[SQLCustomFunction("KEYWORD_SEARCH", 2, FunctionType.Scalar)]
		public static object KeywordSearch(object[] args)
		{
			return 0;
		}
	}
}
