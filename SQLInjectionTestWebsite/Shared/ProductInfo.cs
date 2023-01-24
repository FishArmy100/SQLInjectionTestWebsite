
namespace SQLInjectionTestWebsite.Shared
{
    public class ProductInfo
    {
        public readonly string Name;
        public readonly float Cost;
        public readonly string Description;

        public ProductInfo(string name, float cost, string description)
        {
            Name = name;
            Cost = cost;
            Description = description;
        }
    }
}
