namespace SQLInjectionTestWebsite.Shared
{
	public static class StoreRegistrySearcher
	{
		public static List<StoreProduct> SearchForProducts(StoreProductRegistry registry, string searched)
		{
            string[] searchTerms = GetSearchTerms(searched);
            return Search(registry.GetProducts(), searchTerms);
		}

        private struct SearchedProduct
        {
            public readonly StoreProduct Product;
            public readonly uint Weight;

            public SearchedProduct(StoreProduct product, uint weight)
            {
                Product = product;
                Weight = weight;
            }
        }

        private static List<StoreProduct> Search(List<StoreProduct> products, string[] searchTerms)
        {
            List<SearchedProduct> searchedProducts = new List<SearchedProduct>();

            foreach (StoreProduct product in products)
            {
                string[] itemTerms = GetSearchTerms(product.Name);
                uint weight = GetWeight(searchTerms, itemTerms);
                if (weight > 0)
                    searchedProducts.Add(new SearchedProduct(product, weight));
            }

            return searchedProducts.OrderBy(x => x.Weight).Select(p => p.Product).ToList();
        }

        private static uint GetWeight(string[] searchedTerms, string[] itemTerms)
        {
            uint weight = 0;
            foreach(string searched in searchedTerms)
            {
                if (itemTerms.Contains(searched))
                    weight++;
            }

            return weight;
        }

        private static string[] GetSearchTerms(string searched) => searched.Split(' ', ',', '-', '_');
    }
}
