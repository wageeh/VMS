using FleetManagement.API.BAL;
using FleetManagement.Entities;
using FleetManagement.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FleetManagement.API.UnitTests
{
    /// <summary>
    /// Unit Test for Fleet Management project's Web Api Controller's Repository methods using MoQ
    /// </summary>
    [TestClass]
    public class FleetControllerTests
    {
        List<Customer> expectedCustomers;
        Mock<IDocumentDBRepository<Customer>> mockDocumentDBRepository;
        CustomerManager customerManager;
        public string searchname = "";

        [TestInitialize]
        public void InitializeTestData()
        {
            //Setup test data
            expectedCustomers = GetAllExpectedCustomers();
            //Arrange
            mockDocumentDBRepository = new Mock<IDocumentDBRepository<Customer>>();
            customerManager = new CustomerManager(mockDocumentDBRepository.Object);

            //Setup
            mockDocumentDBRepository.Setup(m => m.GetAllItemsAsync()).ReturnsAsync(expectedCustomers);

            mockDocumentDBRepository.Setup(m => m.GetItemsAsync(It.IsAny<Expression<Func<Customer, bool>>>())).ReturnsAsync(expectedCustomers);

            
        }

        [TestMethod]
        public void Get_Filtered_CustomersAsync()
        {
            //Act
            List<Customer> actualCustomers = customerManager.FilterByNameAsync(searchname).Result;

            //Assert
            Assert.IsTrue(expectedCustomers.Count().Equals(actualCustomers.Count()));

        }

        [TestMethod]
        public void Get_All_CustomersAsync()
        {
            //Act
            List<Customer> actualCustomers = customerManager.ListAsync().Result;

            //Assert
            Assert.IsTrue(expectedCustomers.Count().Equals(actualCustomers.Count()));

        }
        /*
        [TestMethod]
        public void Add_Product()
        {
            //int productCount = mockProductRepository.Object.GetProducts().Count;
            int productCount = productController.GetProducts().Count;

            Assert.AreEqual(2, productCount);

            //Prepare
            ProductJSON newProduct = GetNewProduct("N3", "C3", 33.55M);
            //Act
            //mockProductRepository.Object.AddProduct(newProduct);
            productController.AddProduct(newProduct);

            //productCount = mockProductRepository.Object.GetProducts().Count;
            productCount = productController.GetProducts().Count;
            //Assert
            Assert.AreEqual(3, productCount);
        }
        [TestMethod]
        public void Update_Product()
        {
            ProductJSON product = new ProductJSON()
            {
                Id = 2,
                Name = "N22",//Changed Name
                Category = "P2",
                Price = 22
            };

            //mockProductRepository.Object.UpdateProduct(product);
            productController.UpdateProduct(product);

            // Verify the change
            //Assert.AreEqual("N22", mockProductRepository.Object.GetProducts()[1].Name);
            Assert.AreEqual("N22", productController.GetProducts()[1].Name);
        }
        [TestMethod]
        public void Delete_Product()
        {
            //Assert.AreEqual(2, mockProductRepository.Object.GetProducts().Count);
            Assert.AreEqual(2, productController.GetProducts().Count);

            //mockProductRepository.Object.Delete(1);
            productController.Delete(1);

            // Verify the change
            //Assert.AreEqual(1, mockProductRepository.Object.GetProducts().Count);
            Assert.AreEqual(1, productController.GetProducts().Count);
        }
        */
        [TestCleanup]
        public void CleanUpTestData()
        {
            expectedCustomers = null;
            mockDocumentDBRepository = null;
        }

        #region HelperMethods
        private static List<Customer> GetAllExpectedCustomers()
        {
            return new List<Customer>()
            {
                new Customer()
                {
                    Id = "1",
                    Name = "c1",
                },
                new Customer()
                {
                    Id = "2",
                    Name = "c2",
                }
            };
        }
        private static Customer GetNewCustomer(string name, string city, string details)
        {
            return new Customer()
            {
                Name = name,
                Address = new Address { City=city,Details=details}
            };
        }
        #endregion
    }
}
