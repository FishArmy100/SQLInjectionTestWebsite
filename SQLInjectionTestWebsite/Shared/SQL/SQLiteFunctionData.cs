using System.Data.SQLite;

namespace SQLInjectionTestWebsite.Shared.SQL
{
	public readonly struct SQLiteFunctionData
	{
		public readonly string FunctionName;
		public readonly int ArgumentCount;
		public readonly FunctionType FunctionType;

		public readonly Func<object[], object> Function;

		public SQLiteFunctionData(string functionName, int argumentCount, FunctionType functionType, Func<object[], object> function)
		{
			FunctionName = functionName;
			ArgumentCount = argumentCount;
			FunctionType = functionType;
			Function = function;
		}

		public SQLiteFunctionAttribute ToAttribute()
		{
			return new SQLiteFunctionAttribute(FunctionName, ArgumentCount, FunctionType);
		}
	}
}
