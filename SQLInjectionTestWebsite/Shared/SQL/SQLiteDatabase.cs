using System.Data.SQLite;
using System.Reflection;

namespace SQLInjectionTestWebsite.Shared.SQL
{
    public class SQLiteDatabase
    {
		public readonly string Name;
		public SQLiteConnection GetConnection() => new SQLiteConnection(Name);

        public SQLiteDatabase(string name)
        {
			string cs = "Data Source=:memory:";
			string stm = "SELECT SQLITE_VERSION()";

			using var con = new SQLiteConnection(cs);
			con.Open();

			using var cmd = new SQLiteCommand(stm, con);
			string? version = cmd.ExecuteScalar()?.ToString();

			Console.WriteLine($"SQLite version: {version}");
		}

		public void ConstructTable<T>(string tableName, IEnumerable<T> items)
		{
			if(typeof(T).GetCustomAttribute<SQLSerializeableObject>() == null)
				throw new ArgumentException("Type " + typeof(T).Name + " must have the attribute SQLSerializableObject");

			var connection = GetConnection();

		}
    }
}
