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
			string searchTerm = (string)args[0];
			string productName = (string)args[1];

			int score = 0;
			foreach(string term in searchTerm.SplitSearch())
			{
				foreach(string nameTerm in productName.SplitSearch())
				{
					if(term.ToLower() == nameTerm.ToLower()) 
						score++;
				}
			}

			return score;
		}

		private static string[] SplitSearch(this string self)
		{
			return self.Split(',', ' ', '\t', '-', '_');
		}
	}
}
