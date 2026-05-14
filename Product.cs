using System;
using System.Collections.Generic;

namespace ShopLib
{
    public class Product : IComparable<Product>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Product other = (Product)obj;
            return Name == other.Name &&
                   Price == other.Price &&
                   Quantity == other.Quantity &&
                   ExpiryDate.Date == other.ExpiryDate.Date;
        }

        public override int GetHashCode()
        {
            // Комбинация хэшей для .NET Framework
            int hash = 17;
            hash = hash * 31 + (Name != null ? Name.GetHashCode() : 0);
            hash = hash * 31 + Price.GetHashCode();
            hash = hash * 31 + Quantity.GetHashCode();
            hash = hash * 31 + ExpiryDate.Date.GetHashCode();
            return hash;
        }

        public int CompareTo(Product other)
        {
            if (other == null) return 1;
            return string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }
    }

    public class SortByPrice : IComparer<Product>
    {
        public int Compare(Product x, Product y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            return x.Price.CompareTo(y.Price);
        }
    }

    public class SortByQuantity : IComparer<Product>
    {
        public int Compare(Product x, Product y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            return x.Quantity.CompareTo(y.Quantity);
        }
    }

    public class SortByExpiryDate : IComparer<Product>
    {
        public int Compare(Product x, Product y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            return x.ExpiryDate.Date.CompareTo(y.ExpiryDate.Date);
        }
    }
}