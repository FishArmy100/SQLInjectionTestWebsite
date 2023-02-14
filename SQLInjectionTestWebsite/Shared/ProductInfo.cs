
namespace SQLInjectionTestWebsite.Shared
{
    public class ProductInfo
    {
        public readonly string Name;
        public readonly float Cost;
        public readonly string Description;
        public readonly string ID;
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
