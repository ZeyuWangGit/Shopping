using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Shopping.Common;
using Shopping.Models;
using Shopping.Models.Trolley;
using Shopping.Services;
using Shouldly;

namespace Shopping.UnitTests
{
    [TestClass]
    public class ProductServiceTests
    {
        private readonly Mock<IResourceHttpClientService> _shoppingResourceService = new Mock<IResourceHttpClientService>();
        private IProductService _mockProductService;

        public ProductServiceTests()
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
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _mockProductService = new ProductService(_shoppingResourceService.Object);
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

            var actual = _mockProductService.SortProducts(ProductSortOption.Low).Result;
            
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

            var actual = _mockProductService.SortProducts(ProductSortOption.High).Result;

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

            var actual = _mockProductService.SortProducts(ProductSortOption.Ascending).Result;

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

            var actual = _mockProductService.SortProducts(ProductSortOption.Descending).Result;

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
                }
            };

            var actual = _mockProductService.SortProducts(ProductSortOption.Recommended).Result;

            // Act
            var expectedJson = JsonConvert.SerializeObject(expected);
            var actualJson = JsonConvert.SerializeObject(actual);

            // Assert
            actualJson.ShouldBe(expectedJson);
        }
    }
}
