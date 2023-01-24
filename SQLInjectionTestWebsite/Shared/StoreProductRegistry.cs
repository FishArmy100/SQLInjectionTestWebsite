namespace SQLInjectionTestWebsite.Shared
{
	public class StoreProductRegistry
	{
		private readonly Dictionary<string, StoreProduct> m_Products = new Dictionary<string, StoreProduct>();

		public StoreProductRegistry(List<StoreProduct> products)
		{
			foreach (var product in products)
				AddProductType(product.Info, product.Count);
		}

		public StoreProductRegistry() { }

		public void AddProductType(ProductInfo info, uint count)
		{
			if(!m_Products.ContainsKey(info.Name))
			{
				m_Products.Add(info.Name, new StoreProduct(info, count));
			}
			else
			{
				throw new ArgumentException("Product with name: " + info.Name + ", already exists.");
			}
		}

		public List<StoreProduct> GetProducts() => m_Products.Values.ToList();

		public bool TryGetProduct(string name, out StoreProduct? product)
		{
			return m_Products.TryGetValue(name, out product);
		}

		public void RemoveProducts(string name, uint count, Action<uint> onNotEnouphProducts)
		{
			if(m_Products.TryGetValue(name, out StoreProduct? info))
			{
				if(info.Count < count)
				{
					onNotEnouphProducts(info.Count);
				}
				else
				{
					info.Count -= count;
				}
			}
			else
			{
				onNotEnouphProducts(0);
			}
		}
	}
}
