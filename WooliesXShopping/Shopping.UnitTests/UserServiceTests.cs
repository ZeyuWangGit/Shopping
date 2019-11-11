using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shopping.Models;
using Shopping.Services;
using Shouldly;

namespace Shopping.UnitTests
{
    [TestClass]
    public class UserServiceTests
    {
        private readonly Mock<IOptions<Resource>> _resource = new Mock<IOptions<Resource>>();
        private IUserService _userService;

        public UserServiceTests()
        {
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
            _userService = new UserService(_resource.Object);
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

            var actual = _userService.GetUserInfo();

            // Assert
            actual.Name.ShouldBe(expected.Name);
            actual.Token.ShouldBe(expected.Token);
        }
    }
}
