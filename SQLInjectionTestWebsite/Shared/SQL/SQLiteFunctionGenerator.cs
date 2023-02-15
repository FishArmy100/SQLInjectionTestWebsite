using System.Data.SQLite;

namespace SQLInjectionTestWebsite.Shared.SQL
{
	public static class SQLiteFunctionGenerator
	{
		public static void BindFunction(SQLiteConnection connection, SQLiteFunctionData data)
		{
			SQLiteFunctionAttribute attribute = data.ToAttribute();
			SQLiteFunctionWrapper wrapper = new SQLiteFunctionWrapper(data.Function, data.FunctionName, data.ArgumentCount);
			connection.Open();
			connection.BindFunction(attribute, wrapper);
			bool removed = connection.UnbindFunction(data.ToAttribute());
			connection.Close();
			Console.WriteLine($"Function removed: {removed}");
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
