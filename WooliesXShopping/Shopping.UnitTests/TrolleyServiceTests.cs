using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
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
            var trolleyTestCase1 = JsonConvert.DeserializeObject<Trolley>(@"{""products"":[{""name"":""Product 0"",""price"":1.70380992428577}],""specials"":[],""quantities"":[{""name"":""Product 0"",""quantity"":3}]}");
            var trolleyTestCase2 = JsonConvert.DeserializeObject<Trolley>(@"{""products"":[{""name"":""Product 0"",""price"":3.63491612888636},{""name"":""Product 1"",""price"":11.7149544911063},{""name"":""Product 2"",""price"":11.0674842661561},{""name"":""Product 3"",""price"":11.2949710531602},{""name"":""Product 4"",""price"":7.11063366714429},{""name"":""Product 5"",""price"":13.2273932375141},{""name"":""Product 6"",""price"":6.93450286376034},{""name"":""Product 7"",""price"":6.35481837501508}],""specials"":[{""quantities"":[{""name"":""Product 0"",""quantity"":2},{""name"":""Product 1"",""quantity"":4},{""name"":""Product 2"",""quantity"":6},{""name"":""Product 3"",""quantity"":8},{""name"":""Product 4"",""quantity"":1},{""name"":""Product 5"",""quantity"":4},{""name"":""Product 6"",""quantity"":5},{""name"":""Product 7"",""quantity"":6}],""total"":14.8261315251573},{""quantities"":[{""name"":""Product 0"",""quantity"":5},{""name"":""Product 1"",""quantity"":6},{""name"":""Product 2"",""quantity"":9},{""name"":""Product 3"",""quantity"":3},{""name"":""Product 4"",""quantity"":3},{""name"":""Product 5"",""quantity"":7},{""name"":""Product 6"",""quantity"":2},{""name"":""Product 7"",""quantity"":5}],""total"":21.7281220168362},{""quantities"":[{""name"":""Product 0"",""quantity"":8},{""name"":""Product 1"",""quantity"":0},{""name"":""Product 2"",""quantity"":3},{""name"":""Product 3"",""quantity"":0},{""name"":""Product 4"",""quantity"":9},{""name"":""Product 5"",""quantity"":8},{""name"":""Product 6"",""quantity"":3},{""name"":""Product 7"",""quantity"":8}],""total"":18.7842904552888},{""quantities"":[{""name"":""Product 0"",""quantity"":5},{""name"":""Product 1"",""quantity"":4},{""name"":""Product 2"",""quantity"":4},{""name"":""Product 3"",""quantity"":6},{""name"":""Product 4"",""quantity"":4},{""name"":""Product 5"",""quantity"":2},{""name"":""Product 6"",""quantity"":2},{""name"":""Product 7"",""quantity"":3}],""total"":64.5759010016626},{""quantities"":[{""name"":""Product 0"",""quantity"":0},{""name"":""Product 1"",""quantity"":2},{""name"":""Product 2"",""quantity"":1},{""name"":""Product 3"",""quantity"":3},{""name"":""Product 4"",""quantity"":1},{""name"":""Product 5"",""quantity"":9},{""name"":""Product 6"",""quantity"":2},{""name"":""Product 7"",""quantity"":1}],""total"":39.2972200859245},{""quantities"":[{""name"":""Product 0"",""quantity"":6},{""name"":""Product 1"",""quantity"":6},{""name"":""Product 2"",""quantity"":1},{""name"":""Product 3"",""quantity"":6},{""name"":""Product 4"",""quantity"":2},{""name"":""Product 5"",""quantity"":6},{""name"":""Product 6"",""quantity"":3},{""name"":""Product 7"",""quantity"":3}],""total"":4.44149390218065},{""quantities"":[{""name"":""Product 0"",""quantity"":2},{""name"":""Product 1"",""quantity"":9},{""name"":""Product 2"",""quantity"":2},{""name"":""Product 3"",""quantity"":5},{""name"":""Product 4"",""quantity"":2},{""name"":""Product 5"",""quantity"":0},{""name"":""Product 6"",""quantity"":9},{""name"":""Product 7"",""quantity"":7}],""total"":65.8853098798582},{""quantities"":[{""name"":""Product 0"",""quantity"":4},{""name"":""Product 1"",""quantity"":6},{""name"":""Product 2"",""quantity"":8},{""name"":""Product 3"",""quantity"":4},{""name"":""Product 4"",""quantity"":8},{""name"":""Product 5"",""quantity"":7},{""name"":""Product 6"",""quantity"":6},{""name"":""Product 7"",""quantity"":9}],""total"":4.58695622328803}],""quantities"":[{""name"":""Product 0"",""quantity"":9},{""name"":""Product 1"",""quantity"":1},{""name"":""Product 2"",""quantity"":6},{""name"":""Product 3"",""quantity"":1},{""name"":""Product 4"",""quantity"":5},{""name"":""Product 5"",""quantity"":5},{""name"":""Product 6"",""quantity"":0},{""name"":""Product 7"",""quantity"":4}]}");
            var trolleyTestCase3 = JsonConvert.DeserializeObject<Trolley>(@"{""products"":[{""name"":""1"",""price"":2.0},{""name"":""2"",""price"":5.0}],""specials"":[{""quantities"":[{""name"":""1"",""quantity"":3},{""name"":""2"",""quantity"":0}],""total"":5.0},{""quantities"":[{""name"":""1"",""quantity"":1},{""name"":""2"",""quantity"":2}],""total"":10.0}],""quantities"":[{""name"":""1"",""quantity"":3},{""name"":""2"",""quantity"":2}]}");
            var trolleyTestCase4 = JsonConvert.DeserializeObject<Trolley>(@"{""products"":[{""name"":""Product 0"",""price"":4.69747858806396},{""name"":""Product 1"",""price"":9.20291770212488},{""name"":""Product 2"",""price"":10.6104442596484},{""name"":""Product 3"",""price"":14.7151796751261},{""name"":""Product 4"",""price"":5.30681912335885}],""specials"":[{""quantities"":[{""name"":""Product 0"",""quantity"":1},{""name"":""Product 1"",""quantity"":8},{""name"":""Product 2"",""quantity"":0},{""name"":""Product 3"",""quantity"":3},{""name"":""Product 4"",""quantity"":2}],""total"":39.8820224233705},{""quantities"":[{""name"":""Product 0"",""quantity"":5},{""name"":""Product 1"",""quantity"":5},{""name"":""Product 2"",""quantity"":1},{""name"":""Product 3"",""quantity"":0},{""name"":""Product 4"",""quantity"":3}],""total"":3.23310332333999},{""quantities"":[{""name"":""Product 0"",""quantity"":3},{""name"":""Product 1"",""quantity"":9},{""name"":""Product 2"",""quantity"":4},{""name"":""Product 3"",""quantity"":9},{""name"":""Product 4"",""quantity"":4}],""total"":11.8196681651239},{""quantities"":[{""name"":""Product 0"",""quantity"":2},{""name"":""Product 1"",""quantity"":0},{""name"":""Product 2"",""quantity"":4},{""name"":""Product 3"",""quantity"":5},{""name"":""Product 4"",""quantity"":2}],""total"":23.5799786943425},{""quantities"":[{""name"":""Product 0"",""quantity"":5},{""name"":""Product 1"",""quantity"":6},{""name"":""Product 2"",""quantity"":4},{""name"":""Product 3"",""quantity"":7},{""name"":""Product 4"",""quantity"":3}],""total"":34.8986721384895},{""quantities"":[{""name"":""Product 0"",""quantity"":9},{""name"":""Product 1"",""quantity"":1},{""name"":""Product 2"",""quantity"":5},{""name"":""Product 3"",""quantity"":9},{""name"":""Product 4"",""quantity"":3}],""total"":23.8529347490023}],""quantities"":[{""name"":""Product 0"",""quantity"":7},{""name"":""Product 1"",""quantity"":0},{""name"":""Product 2"",""quantity"":3},{""name"":""Product 3"",""quantity"":8},{""name"":""Product 4"",""quantity"":2}]}");

            // Act
            decimal expectedCase1 = 5.11142977285731m;
            var actualCase1 = _mockTrolleyService.CalculateTrolley(trolleyTestCase1);

            var expectedCase2 = 249.23848432453261m;
            var actualCase2 = _mockTrolleyService.CalculateTrolley(trolleyTestCase2);

            var expectedCase3 = 14m;
            var actualCase3 = _mockTrolleyService.CalculateTrolley(trolleyTestCase3);

            var expectedCase4 = 193.04875854311942m;
            var actualCase4 = _mockTrolleyService.CalculateTrolley(trolleyTestCase4);

            // Assert
            actualCase1.ShouldBe(expectedCase1);
            actualCase2.ShouldBe(expectedCase2);
            actualCase3.ShouldBe(expectedCase3);
            actualCase4.ShouldBe(expectedCase4);
        }
    }
}
