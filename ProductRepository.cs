using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;   // вместо Newtonsoft.Json

namespace ShopLib
{
    public class ProductRepository
    {
        private readonly string _filePath;
        private List<Product> _products;

        public event Action DataChanged;

        public ProductRepository(string filePath)
        {
            _filePath = filePath;
            _products = LoadAll();
        }

        private List<Product> LoadAll()
        {
            if (!File.Exists(_filePath))
                return new List<Product>();

            string json = File.ReadAllText(_filePath);
            // Заменили JsonConvert.DeserializeObject на JsonSerializer.Deserialize
            return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
        }

        private void Save()
        {
            // Опции для читаемого форматирования (аналог Formatting.Indented)
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(_products, options);
            File.WriteAllText(_filePath, json);
            DataChanged?.Invoke();   // уведомление подписчиков
        }

        public List<Product> GetAll()
        {
            return new List<Product>(_products);
        }

        public void Add(Product product)
        {
            if (_products.Any(p => p.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Товар с названием \"" + product.Name + "\" уже существует.");

            _products.Add(product);
            Save();
        }

        public void Update(string name, Product updatedProduct)
        {
            int index = _products.FindIndex(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (index == -1)
                throw new InvalidOperationException("Товар с названием \"" + name + "\" не найден.");

            _products[index] = updatedProduct;
            Save();
        }

        public List<Product> FindByField(string fieldName, string value)
        {
            var all = GetAll();
            switch (fieldName)
            {
                case "Name":
                    // Поиск без учёта регистра с помощью IndexOf
                    return all.Where(p => p.Name != null && p.Name.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                case "Price":
                    return all.Where(p => p.Price.ToString().Contains(value)).ToList();
                case "Quantity":
                    return all.Where(p => p.Quantity.ToString().Contains(value)).ToList();
                case "ExpiryDate":
                    return all.Where(p => p.ExpiryDate.ToString("dd.MM.yyyy").Contains(value)).ToList();
                default:
                    return new List<Product>();
            }
        }
    }
}