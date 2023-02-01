using System.Data.SQLite;

namespace SQLInjectionTestWebsite.Shared.SQLUtils
{
    public class SqliteDatabase
    {
        public SqliteDatabase(string name)
        {
			string cs = "Data Source=:memory:";
			string stm = "SELECT SQLITE_VERSION()";

			using var con = new SQLiteConnection(cs);
			con.Open();

			using var cmd = new SQLiteCommand(stm, con);
			string? version = cmd.ExecuteScalar()?.ToString();

			Console.WriteLine($"SQLite version: {version}");
		}
    }
}
