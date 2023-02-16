using System.Data.SQLite;
using System.Data;
using System.Reflection;

namespace SQLInjectionTestWebsite.Shared.SQL
{
	public static class SQLiteFunctionGenerator
	{
		private static readonly SQLFunctionFinder s_Finder = new SQLFunctionFinder();

		public static void BindAllCustomFunctions(SQLiteConnection connection)
		{
			if (connection.State == ConnectionState.Closed)
				throw new ArgumentException("Connection must be open");

			foreach(SQLiteFunctionData data in s_Finder.FunctionDatas)
			{
				BindFunction(connection, data);
			}
		}

		public static void BindFunction(SQLiteConnection connection, SQLiteFunctionData data)
		{
			if (connection.State == ConnectionState.Closed)
				throw new ArgumentException("Connection must be open");

			SQLiteFunctionAttribute attribute = data.ToAttribute();
			SQLiteFunctionWrapper wrapper = new SQLiteFunctionWrapper(data.Function, data.FunctionName, data.ArgumentCount);

			connection.BindFunction(attribute, wrapper);
		}

		public static void Test(SQLiteConnection connection)
		{
			connection.Open();

			BindAllCustomFunctions(connection);

			//SQLiteDatabase database = new SQLiteDatabase(WebsiteDatabase.DatabaseName);
			//database.SerializeObjects(WebsiteDatabase.ProductsTableName, new List<ProductInfo> 
			//{ 
			//	new ProductInfo("Test", 4.0f, "111", "A test product", 200000) 
			//});

			SQLiteCommand command = connection.CreateCommand();
			string tupleText = $"{nameof(ProductInfo.Name)}, {nameof(ProductInfo.Cost)}, {nameof(ProductInfo.Description)}, {nameof(ProductInfo.ID)}, {nameof(ProductInfo.Count)}";
			command.CommandText = $"SELECT {tupleText} FROM {WebsiteDatabase.ProductsTableName} WHERE STORESEARCH('Worked', ({tupleText}))";
			SQLiteDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				Console.WriteLine(reader.GetString(0));
			}

			connection.Close();
		}

		private class SQLFunctionFinder
		{
			public readonly List<SQLiteFunctionData> FunctionDatas;

			public SQLFunctionFinder() 
			{
				FunctionDatas = AppDomain.CurrentDomain.GetAssemblies()
					.Select(a => a.DefinedTypes)
					.SelectMany(ts => ts)
					.Where(t => t.GetCustomAttribute<SQLFunctionLibrary>() != null)
					.Select(t => t.GetMethods()
						.Where(m => m.GetCustomAttribute<SQLCustomFunction>() != null && 
							   m.ReturnType == typeof(object) && 
							   m.GetParameters().Length == 1 &&
							   m.GetParameters()[0].ParameterType == typeof(object[]) &&
							   m.IsStatic))
					.SelectMany(ms => ms)
					.Select(m => 
					{
						SQLCustomFunction f = m.GetCustomAttribute<SQLCustomFunction>()!;
						return new SQLiteFunctionData(f.FunctionName, f.ArgumentCount, f.FunctionType, (args) =>
						{
							return m.Invoke(null, new object[] { args })!;
						});
					}).ToList();
			}
		}

		private class SQLiteFunctionWrapper : SQLiteFunction
		{
			private readonly Func<object[], object> m_Function;
			private readonly string m_FunctionName;
			private readonly int m_ArgCount;
			public SQLiteFunctionWrapper(Func<object[], object> function, string functionName, int argCount)
			{
				m_Function = function;
				m_FunctionName = functionName;
				m_ArgCount = argCount;
			}

			public override object Invoke(object[] args)
			{
				if(m_ArgCount != args.Length)
					throw new ArgumentException($"Function {m_FunctionName} requires {m_ArgCount} arguments, but was passed {args.Length} arguments.");

				return m_Function(args);
			}
		}
	}
}
