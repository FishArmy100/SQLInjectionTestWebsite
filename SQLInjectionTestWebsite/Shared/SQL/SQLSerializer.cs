using System.Data.SQLite;
using System.Reflection;
using System.Runtime.Serialization;

namespace SQLInjectionTestWebsite.Shared.SQL
{
	public static class SQLSerializer
	{
		private enum PrimativeType
		{
			Int32,
			Float,
			String,
			Bool,
		}

		private class FieldData
		{
			public readonly string Name;
			public readonly PrimativeType Type;
			public readonly object Value;

			public FieldData(string name, PrimativeType type, object value)
			{
				Name = name;
				Type = type;
				Value = value;
			}

			public static FieldData FromInfo(FieldInfo info, object parent)
			{
				var name = info.Name;
				var type = GetPrimativeFromType(info.FieldType);
				var value = info.GetValue(parent)!;
				return new FieldData(name, type, value);
			}
		}

		public static string GetSerializeCommands<T>(string tableName, IEnumerable<T> data)
		{
			CheckType<T>();

			List<List<FieldData>> dataFeilds = GetTableFields(data);
			string[] tableColumns = GetTableColumns<T>();

			string commands = $"CREATE TABLE IF NOT EXISTS {tableName} ({string.Join(", ", tableColumns)});\n";
			commands += string.Join("\n", dataFeilds.Select(d => CreateCommandString(tableName, d)));

			return commands;
		}

		public static List<T> Deserialize<T>(string tableName, string selectorCommand, SQLiteDatabase database)
		{
			CheckType<T>();

			string[] tableColumns = GetTableColumns<T>();
			string commands = $"CREATE TABLE IF NOT EXISTS {tableName} ({string.Join(", ", tableColumns)});\n";

			commands += selectorCommand;

			return database.ExecuteReadCommand(commands, r =>
			{
				return ReadObject<T>(r);
			});

		}

		private static T ReadObject<T>(SQLiteDataReader reader)
		{
			object instance = FormatterServices.GetUninitializedObject(typeof(T));

			typeof(T).GetFields()
				.Where(f => f.GetCustomAttribute<SQLSerializeableField>() != null)
				.ForEach(f =>
				{
					PrimativeType t = GetPrimativeFromType(f.FieldType);
					int index = reader.GetOrdinal(f.Name);
					switch (t)
					{
						case PrimativeType.Int32:
							f.SetValue(instance, reader.GetInt32(index));
							break;
						case PrimativeType.Float:
							f.SetValue(instance, reader.GetFloat(index));
							break;
						case PrimativeType.String:
							f.SetValue(instance, reader.GetString(index));
							break;
						case PrimativeType.Bool:
							f.SetValue(instance, reader.GetInt32(index) == 1);	
							break;
					}
				});

			return (T)instance;
		}

		private static string[] GetTableColumns<T>()
		{
			return typeof(T).GetFields()
					.Where(f => f.GetCustomAttribute<SQLSerializeableField>() != null)
					.Select(f => $"{f.Name} {CreateValueTypeString(GetPrimativeFromType(f.FieldType))}")
					.ToArray();
		}

		private static List<List<FieldData>> GetTableFields<T>(IEnumerable<T> data)
		{
			return data.Select(obj =>
							obj!.GetType()
							.GetFields()
							.Where(f => f.GetCustomAttribute<SQLSerializeableField>() != null)
							.Select(f => FieldData.FromInfo(f, obj)).ToList())
							.ToList();
		}

		private static string CreateCommandString(string tableName, IEnumerable<FieldData> fields)
		{
			string[] valueNames = fields.Select(f => f.Name).ToArray();
			string[] values = fields.Select(f => CreateValueString(f.Value, f.Type)).ToArray();

			string command = $"INSERT INTO {tableName} ({string.Join(", ", valueNames)}) VALUES ({string.Join(", ", values)});";
			return command;
		}

		private static string CreateValueTypeString(PrimativeType type)
		{
			return type switch
			{
				PrimativeType.Int32 => "int",
				PrimativeType.Float => "FLOAT",
				PrimativeType.String => "TEXT",
				PrimativeType.Bool => "int",
				_ => throw new NotImplementedException(),
			};
		}

		private static string CreateValueString(object value, PrimativeType type)
		{
			return type switch
			{
				PrimativeType.Int32 => $"'{(int)value}'",
				PrimativeType.Float => $"'{(float)value}'",
				PrimativeType.String => $"'{(string)value}'",
				PrimativeType.Bool => (((bool)value) ? "1" : "0"),
				_ => throw new NotImplementedException(),
			};
		}

		private static PrimativeType GetPrimativeFromType(Type type)
		{
			if (type == typeof(int))
				return PrimativeType.Int32;
			else if (type == typeof(float))
				return PrimativeType.Float;
			else if (type == typeof(string))
				return PrimativeType.String;
			else if(type == typeof(bool))
				 return PrimativeType.Bool;

			throw new ArgumentException($"Type '{type.Name}' is not a supported primative type.");
		}

		private static void CheckType<T>()
		{
			if (typeof(T).GetCustomAttribute<SQLSerializeableObject>() == null)
				throw new ArgumentException("Type '" + typeof(T).Name + "', must have the SQLSerializeableObject attribute.");
		}
		private static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
		{
			foreach (T item in enumeration)
			{
				action(item);
			}
		}
	}
}
