using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShopLib.Tests
{
    [TestClass]
    public class ProductRepositoryTests
    {
        private const string TestFile = "test_products.json";
        private ProductRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            if (File.Exists(TestFile))
                File.Delete(TestFile);

            _repository = new ProductRepository(TestFile);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(TestFile))
                File.Delete(TestFile);
        }

        [TestMethod]
        public void Add_ShouldStoreProductInFile()
        {
            var product = new Product
            {
                Name = "Молоко",
                Price = 85.50m,
                Quantity = 20,
                ExpiryDate = new DateTime(2026, 1, 15)
            };

            _repository.Add(product);
            var all = _repository.GetAll();

            Assert.AreEqual(1, all.Count);
            Assert.AreEqual(product, all[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Add_DuplicateName_ShouldThrowException()
        {
            _repository.Add(new Product { Name = "Хлеб", Price = 30, Quantity = 10, ExpiryDate = DateTime.Now });
            _repository.Add(new Product { Name = "Хлеб", Price = 35, Quantity = 5, ExpiryDate = DateTime.Now });
        }

        [TestMethod]
        public void Update_ExistingProduct_ShouldModifyData()
        {
            var original = new Product
            {
                Name = "Сыр",
                Price = 200,
                Quantity = 15,
                ExpiryDate = new DateTime(2025, 6, 1)
            };
            _repository.Add(original);

            var updated = new Product
            {
                Name = "Сыр",
                Price = 210,
                Quantity = 12,
                ExpiryDate = new DateTime(2025, 6, 10)
            };

            _repository.Update("Сыр", updated);
            var result = _repository.GetAll().First(p => p.Name == "Сыр");

            Assert.AreEqual(updated, result);
        }

        [TestMethod]
        public void FindByField_ShouldReturnFilteredList()
        {
            _repository.Add(new Product { Name = "Масло", Price = 150, Quantity = 5, ExpiryDate = DateTime.Today.AddDays(10) });
            _repository.Add(new Product { Name = "Масло сливочное", Price = 180, Quantity = 2, ExpiryDate = DateTime.Today.AddDays(5) });
            _repository.Add(new Product { Name = "Маргарин", Price = 90, Quantity = 8, ExpiryDate = DateTime.Today.AddDays(20) });

            var result = _repository.FindByField("Name", "Масло");

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(p => p.Name.Contains("Масло")));
        }

        [TestMethod]
        public void Equals_ShouldReturnTrueForIdenticalProducts()
        {
            var p1 = new Product { Name = "А", Price = 1, Quantity = 1, ExpiryDate = DateTime.Today };
            var p2 = new Product { Name = "А", Price = 1, Quantity = 1, ExpiryDate = DateTime.Today };

            Assert.IsTrue(p1.Equals(p2));
        }

        [TestMethod]
        public void CompareTo_ShouldSortByName()
        {
            var list = new List<Product>
            {
                new Product { Name = "Яблоки" },
                new Product { Name = "Бананы" },
                new Product { Name = "Виноград" }
            };
            list.Sort();

            Assert.AreEqual("Бананы", list[0].Name);
            Assert.AreEqual("Виноград", list[1].Name);
            Assert.AreEqual("Яблоки", list[2].Name);
        }
    }
}