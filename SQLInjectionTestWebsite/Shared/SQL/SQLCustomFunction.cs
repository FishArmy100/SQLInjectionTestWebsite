using System.Data.SQLite;

namespace SQLInjectionTestWebsite.Shared.SQL
{
	[AttributeUsage(AttributeTargets.Method)]
	public class SQLCustomFunction : Attribute
	{
		public readonly string FunctionName;	
		public readonly int ArgumentCount;
		public readonly FunctionType FunctionType;

		public SQLCustomFunction(string functionName, int argumentCount, FunctionType functionType)
		{
			ArgumentCount = argumentCount;
			FunctionType = functionType;
			FunctionName = functionName;
		}
	}
}
