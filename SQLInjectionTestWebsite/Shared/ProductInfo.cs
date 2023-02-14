
namespace SQLInjectionTestWebsite.Shared
{
    public class ProductInfo
    {
        public readonly string Name;
        public readonly float Cost;
        public readonly string Description;
        public readonly string ID;

        public ProductInfo(string name, float cost, string description, string id)
		{
			Name = name;
			Cost = cost;
			Description = description;
			ID = id;
		}
	}
}
