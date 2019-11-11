using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shopping.Models.Trolley;
using Shopping.Services;
using Shouldly;

namespace Shopping.UnitTests
{
    [TestClass]
    public class TrolleyServiceTests
    {
        private readonly Mock<IResourceHttpClientService> _shoppingResourceService = new Mock<IResourceHttpClientService>();
        private readonly ITrolleyService _mockTrolleyService;

        public TrolleyServiceTests()
        {
            _mockTrolleyService = new TrolleyService(_shoppingResourceService.Object);
        }

        [TestMethod]
        public void CalculateTrolley_ShouldReturnCorrectTotal()
        {
            // Arrange
            var trolley = new Trolley
            {
                Products = new List<TrolleyProduct>
                {
                    new TrolleyProduct {Name = "string", Price = 5},
                    new TrolleyProduct {Name = "A", Price = 10}
                },
                Quantities = new List<ProductQuantity>
                {
                    new ProductQuantity { Name = "string", Quantity = 5 },
                    new ProductQuantity { Name = "A", Quantity = 3 }
                },
                Specials = new List<Special>
                {
                    new Special
                    {
                        Quantities = new List<ProductQuantity>
                        {
                            new ProductQuantity{ Name = "string", Quantity = 3},
                            new ProductQuantity{ Name = "A", Quantity = 3},
                        },
                        Total = 20
                    }
                }
            };

            // Act
            decimal expected = 30;
            var actual = _mockTrolleyService.CalculateTrolley(trolley);

            // Assert
            actual.ShouldBe(expected);

        }
    }
}
