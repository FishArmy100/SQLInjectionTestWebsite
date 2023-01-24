﻿
namespace SQLInjectionTestWebsite.Shared
{
    public class ProductInfo
    {
        public readonly string Name;
        public readonly uint Cost;
        public readonly string Description;

        public ProductInfo(string name, uint cost, string description)
        {
            Name = name;
            Cost = cost;
            Description = description;
        }
    }
}
