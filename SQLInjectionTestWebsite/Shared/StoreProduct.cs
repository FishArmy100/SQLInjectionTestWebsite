namespace SQLInjectionTestWebsite.Shared
{
	public class StoreProduct
	{
		public readonly ProductInfo Info;

		public StoreProduct(ProductInfo info, uint count)
		{
			Info = info;
			Count = count;
		}

		public string Name => Info.Name;
		public uint Count { get; set; }
	}
}
