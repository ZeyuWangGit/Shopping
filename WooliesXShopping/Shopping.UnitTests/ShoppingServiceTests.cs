using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Shopping.Models;
using Shopping.Models.Trolley;
using Shopping.Services;
using Shouldly;

namespace Shopping.UnitTests
{
    [TestClass]
    public class ShoppingServiceTests
    {
        private Mock<IOptions<Resource>> _resource = new Mock<IOptions<Resource>>();
        private Mock<IShoppingResourceService> _shoppingResourceService = new Mock<IShoppingResourceService>();
        private IShoppingService _mockShoppingService;

        public ShoppingServiceTests()
        {
            _shoppingResourceService.Setup(t => t.GetProducts())
                .ReturnsAsync(new List<Product>
                {
                    new Product
                    {
                        Name = "Test Product A",
                        Price = (decimal) 5.0,
                        Quantity = 0
                    },
                    new Product
                    {
                        Name = "Test Product C",
                        Price = (decimal) 10.99,
                        Quantity = 0
                    },
                    new Product
                    {
                        Name = "Test Product D",
                        Price = (decimal) 99.99,
                        Quantity = 0
                    },
                    new Product
                    {
                        Name = "Test Product B",
                        Price = (decimal) 101.99,
                        Quantity = 0
                    },
                    new Product
                    {
                        Name = "Test Product F",
                        Price = (decimal) 999999999999.0,
                        Quantity = 0
                    }
                });

            _shoppingResourceService.Setup(t => t.GetShoppingHistory())
                .ReturnsAsync(new List<ShopperHistory>
                {
                    new ShopperHistory
                    {
                        CustomerId = 123,
                        Products = new List<Product>
                        {
                            new Product
                            {
                                Name = "Test Product A",
                                Price = (decimal)99.99,
                                Quantity = 3
                            },
                            new Product
                            {
                                Name = "Test Product B",
                                Price = (decimal)101.99,
                                Quantity = 1
                            },
                            new Product
                            {
                                Name = "Test Product F",
                                Price = (decimal)999999999999,
                                Quantity = 1
                            }
                        }
                    },
                    new ShopperHistory
                    {
                        CustomerId = 23,
                        Products = new List<Product>
                        {
                            new Product
                            {
                                Name = "Test Product A",
                                Price = (decimal)99.99,
                                Quantity = 2
                            },
                            new Product
                            {
                                Name = "Test Product B",
                                Price = (decimal)101.99,
                                Quantity = 3
                            },
                            new Product
                            {
                                Name = "Test Product F",
                                Price = (decimal)999999999999,
                                Quantity = 1
                            }
                        }
                    },
                    new ShopperHistory
                    {
                        CustomerId = 23,
                        Products = new List<Product>
                        {
                            new Product
                            {
                                Name = "Test Product C",
                                Price = (decimal)10.99,
                                Quantity = 2
                            },
                            new Product
                            {
                                Name = "Test Product F",
                                Price = (decimal)999999999999,
                                Quantity = 2
                            }
                        }
                    },
                    new ShopperHistory
                    {
                        CustomerId = 23,
                        Products = new List<Product>
                        {
                            new Product
                            {
                                Name = "Test Product A",
                                Price = (decimal)99.99,
                                Quantity = 1
                            },
                            new Product
                            {
                                Name = "Test Product B",
                                Price = (decimal)101.99,
                                Quantity = 1
                            },
                            new Product
                            {
                                Name = "Test Product C",
                                Price = (decimal)10.99,
                                Quantity = 1
                            }
                        }
                    }
                });

            _resource.Setup(t => t.Value).Returns(new Resource
            {
                Name = "TestName",
                ResourceUrl = "testUrl",
                Token = "TestToken"
            });
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _mockShoppingService = new ShoppingService(_resource.Object, _shoppingResourceService.Object);
        }

        [TestMethod]
        public void GetUserInfo_ShouldReturnCorrectUserInfo()
        {
            // Arrange
            var expected = new UserInfo
            {
                Name = "TestName",
                Token = "TestToken"
            };

            var actual = _mockShoppingService.GetUserInfo();

            // Assert
            actual.Name.ShouldBe(expected.Name);
            actual.Token.ShouldBe(expected.Token);
        }

        [TestMethod]
        public void SortProducts_ShouldReturnCorrectSortedProducts_WhenSortOptionIsLow()
        {
            // Arrange
            var expected = new List<Product>
            {
                new Product
                {
                    Name = "Test Product A",
                    Price = (decimal) 5.0,
                    Quantity = 0
                },
                new Product
                {
                    Name = "Test Product C",
                    Price = (decimal) 10.99,
                    Quantity = 0
                },
                new Product
                {
                    Name = "Test Product D",
                    Price = (decimal) 99.99,
                    Quantity = 0
                },
                new Product
                {
                    Name = "Test Product B",
                    Price = (decimal) 101.99,
                    Quantity = 0
                },
                new Product
                {
                    Name = "Test Product F",
                    Price = (decimal) 999999999999.0,
                    Quantity = 0
                }
            };

            var actual = _mockShoppingService.SortProducts("Low").Result;
            
            // Act
            var expectedJson = JsonConvert.SerializeObject(expected);
            var actualJson = JsonConvert.SerializeObject(actual);

            // Assert
            actualJson.ShouldBe(expectedJson);
        }

        [TestMethod]
        public void SortProducts_ShouldReturnCorrectSortedProducts_WhenSortOptionIsHigh()
        {
            // Arrange
            var expected = new List<Product>
            {
                new Product
                {
                    Name = "Test Product F",
                    Price = (decimal) 999999999999.0,
                    Quantity = 0
                },
                new Product
                {
                    Name = "Test Product B",
                    Price = (decimal) 101.99,
                    Quantity = 0
                },
                new Product
                {
                    Name = "Test Product D",
                    Price = (decimal) 99.99,
                    Quantity = 0
                },
                new Product
                {
                    Name = "Test Product C",
                    Price = (decimal) 10.99,
                    Quantity = 0
                },
                new Product
                {
                    Name = "Test Product A",
                    Price = (decimal) 5.0,
                    Quantity = 0
                }

            };

            var actual = _mockShoppingService.SortProducts("High").Result;

            // Act
            var expectedJson = JsonConvert.SerializeObject(expected);
            var actualJson = JsonConvert.SerializeObject(actual);

            // Assert
            actualJson.ShouldBe(expectedJson);
        }

        [TestMethod]
        public void SortProducts_ShouldReturnCorrectSortedProducts_WhenSortOptionIsAscending()
        {
            // Arrange
            var expected = new List<Product>
            {
                new Product
                {
                    Name = "Test Product A",
                    Price = (decimal) 5.0,
                    Quantity = 0
                },
                new Product
                {
                    Name = "Test Product B",
                    Price = (decimal) 101.99,
                    Quantity = 0
                },
                new Product
                {
                    Name = "Test Product C",
                    Price = (decimal) 10.99,
                    Quantity = 0
                },
                new Product
                {
                    Name = "Test Product D",
                    Price = (decimal) 99.99,
                    Quantity = 0
                },
                new Product
                {
                    Name = "Test Product F",
                    Price = (decimal) 999999999999.0,
                    Quantity = 0
                }
            };

            var actual = _mockShoppingService.SortProducts("Ascending").Result;

            // Act
            var expectedJson = JsonConvert.SerializeObject(expected);
            var actualJson = JsonConvert.SerializeObject(actual);

            // Assert
            actualJson.ShouldBe(expectedJson);
        }

        [TestMethod]
        public void SortProducts_ShouldReturnCorrectSortedProducts_WhenSortOptionIsDescending()
        {
            // Arrange
            var expected = new List<Product>
            {
                new Product
                {
                    Name = "Test Product F",
                    Price = (decimal) 999999999999.0,
                    Quantity = 0
                },
                new Product
                {
                    Name = "Test Product D",
                    Price = (decimal) 99.99,
                    Quantity = 0
                },
                new Product
                {
                    Name = "Test Product C",
                    Price = (decimal) 10.99,
                    Quantity = 0
                },
                new Product
                {
                    Name = "Test Product B",
                    Price = (decimal) 101.99,
                    Quantity = 0
                },
                new Product
                {
                    Name = "Test Product A",
                    Price = (decimal) 5.0,
                    Quantity = 0
                }
            };

            var actual = _mockShoppingService.SortProducts("Descending").Result;

            // Act
            var expectedJson = JsonConvert.SerializeObject(expected);
            var actualJson = JsonConvert.SerializeObject(actual);

            // Assert
            actualJson.ShouldBe(expectedJson);
        }

        [TestMethod]
        public void SortProducts_ShouldReturnCorrectSortedProducts_WhenSortOptionIsRecommended()
        {
            // Arrange
            var expected = new List<Product>
            {
                new Product
                {
                    Name = "Test Product D",
                    Price = (decimal) 99.99,
                    Quantity = 0
                },
                new Product
                {
                    Name = "Test Product C",
                    Price = (decimal) 10.99,
                    Quantity = 0
                },
                new Product
                {
                    Name = "Test Product A",
                    Price = (decimal) 5.0,
                    Quantity = 0
                },
                new Product
                {
                    Name = "Test Product B",
                    Price = (decimal) 101.99,
                    Quantity = 0
                },
                new Product
                {
                    Name = "Test Product F",
                    Price = (decimal) 999999999999.0,
                    Quantity = 0
                }
            };

            var actual = _mockShoppingService.SortProducts("Recommended").Result;

            // Act
            var expectedJson = JsonConvert.SerializeObject(expected);
            var actualJson = JsonConvert.SerializeObject(actual);

            // Assert
            actualJson.ShouldBe(expectedJson);
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
            var actual = _mockShoppingService.CalculateTrolley(trolley);

            // Assert
            actual.ShouldBe(expected);

        }
    }
}
