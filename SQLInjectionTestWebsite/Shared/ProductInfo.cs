using SQLInjectionTestWebsite.Shared.SQL;

namespace SQLInjectionTestWebsite.Shared
{
	[SQLSerializeableObject]
    public class ProductInfo
    {
		[SQLSerializeableField]
        public readonly string Name;
		[SQLSerializeableField]
		public readonly float Cost;
		[SQLSerializeableField]
		public readonly string Description;
		[SQLSerializeableField]
		public readonly string ID;
		[SQLSerializeableField]
		public readonly int Count;

        public ProductInfo(string name, float cost, string description, string id, int count)
		{
			Name = name;
			Cost = cost;
			Description = description;
			ID = id;
			Count = count;
		}
	}
}
