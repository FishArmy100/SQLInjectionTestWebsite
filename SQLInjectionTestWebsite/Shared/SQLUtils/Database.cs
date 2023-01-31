using Microsoft.Data.Sqlite;

namespace SQLInjectionTestWebsite.Shared.SQLUtils
{
    public class SqliteDatabase
    {
        public SqliteDatabase(string name)
        {
			string cs = "Data Source=:memory:";
			string stm = "SELECT SQLITE_VERSION()";

			using var con = new SqliteConnection(cs);
			con.Open();

			using var cmd = new SqliteCommand(stm, con);
			string? version = cmd.ExecuteScalar()?.ToString();

			Console.WriteLine($"SQLite version: {version}");
		}
    }
}
