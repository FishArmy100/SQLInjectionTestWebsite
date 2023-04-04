using System.Data.SQLite;
using System.Reflection;

namespace SQLInjectionTestWebsite.Shared.SQL
{
    public class SQLiteDatabase
    {
		public readonly string Name;
		private readonly SQLiteConnection m_Connection;

		public SQLiteDatabase(string name)
		{
			Name = name;
			m_Connection = GetConnection(name);
		}

		public int ExecuteCommand(string commandString, List<(string, object)>? extraParameters = null)
		{
			m_Connection.Open();
			SQLiteFunctionGenerator.BindAllCustomFunctions(m_Connection);

			using SQLiteCommand command = m_Connection.CreateCommand();

			command.CommandText = commandString;

            if (extraParameters != null)
            {
                foreach (var (name, value) in extraParameters)
                    command.Parameters.AddWithValue(name, value);
            }

            int changedRows = command.ExecuteNonQuery();
			m_Connection.Close();

			return changedRows;
		}

		public void DropTable(string name)
		{
			ExecuteCommand($"DROP TABLE IF EXISTS {name};");
		}

		public void SerializeObjects<T>(string tableName, IEnumerable<T> objects)
		{
			string commands = SQLSerializer.GetSerializeCommands(tableName, objects);
			ExecuteCommand(commands);
		}

		public List<T> DeserializeObjects<T>(string tableName, string selector, List<(string, object)>? extraParameters = null)
		{
			extraParameters ??= new List<(string, object)>();
			return SQLSerializer.Deserialize<T>(tableName, selector, this, extraParameters);
		}

		public List<T> ExecuteReadCommand<T>(string commandString, Func<SQLiteDataReader, T> valueConverter, List<(string, object)>? extraParameters = null)
		{
			m_Connection.Open();
			SQLiteFunctionGenerator.BindAllCustomFunctions(m_Connection);
			using SQLiteCommand command = m_Connection.CreateCommand();
			command.CommandText = commandString;

			if(extraParameters != null)
			{
				foreach(var (name, value) in extraParameters)
					command.Parameters.AddWithValue(name, value);
			}

            Console.WriteLine(commandString);

            using SQLiteDataReader reader = command.ExecuteReader();


			List<T> result = new List<T>();
			while (reader.Read())
				result.Add(valueConverter(reader));

			m_Connection.Close();
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
