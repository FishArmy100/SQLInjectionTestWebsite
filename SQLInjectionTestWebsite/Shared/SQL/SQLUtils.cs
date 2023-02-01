using System.Data.SQLite;
using System.Reflection;

namespace SQLInjectionTestWebsite.Shared.SQL
{
	public static class SQLUtils
	{
		public static void ExecuteCommand(string databaseName, string commandString)
		{
			using SQLiteConnection connection = GetConnection(databaseName);
			connection.Open();
			using SQLiteCommand command = connection.CreateCommand();
			command.CommandText = commandString;
			command.ExecuteNonQuery();
			connection.Close();
		}

		public static List<T> ExecuteReadCommand<T>(string databaseName, string commandString, Func<SQLiteDataReader, T> valueConverter)
		{
			using SQLiteConnection connection = GetConnection(databaseName);
			connection.Open();
			using SQLiteCommand command = connection.CreateCommand();
			command.CommandText = commandString;
			using SQLiteDataReader reader = command.ExecuteReader();

			List<T> result = new List<T>();
			while(reader.Read())
				result.Add(valueConverter(reader));

			connection.Close();
			return result;
		}

		private static SQLiteConnection GetConnection(string databaseName)
		{
			string location = Assembly.GetExecutingAssembly().Location;
			string? folder = Path.GetDirectoryName(location);
			string uri = $"{folder}\\{databaseName}.db";
			return new SQLiteConnection($"URI=file:{uri}");
		}
	}
}
