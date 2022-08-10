using Identity.Service.Core.Model;
using NUnit.Framework;

namespace Identity.Service.Core.Tests
{
    public class AuthRequestTest
    {

        [Test]
        public void CreateNew_InvalidUsername_Expect_false()
        {
            //Arrange
            AuthRequest request = new AuthRequest();
            request.UserName = "";
            request.Password = "mypass";
            //Act
            AuthRequest.AuthRequestValidator validator = new AuthRequest.AuthRequestValidator();
            var results = validator.Validate(request);
            //Assert
            Assert.IsFalse(results.IsValid);
        }

        [Test]
        public void CreateNew_InvalidPassword_Expect_false()
        {
            //Arrange
            AuthRequest request = new AuthRequest();
            request.UserName = "myuser";
            request.Password = "";
            //Act
            AuthRequest.AuthRequestValidator validator = new AuthRequest.AuthRequestValidator();
            var results = validator.Validate(request);
            //Assert
            Assert.IsFalse(results.IsValid);
        }

        [Test]
        public void CreateNew_ValidUserPass_Expect_true()
        {
            //Arrange
            AuthRequest request = new AuthRequest();
            request.UserName = "myuser";
            request.Password = "mypass";
            //Act
            AuthRequest.AuthRequestValidator validator = new AuthRequest.AuthRequestValidator();
            var results = validator.Validate(request);
            //Assert
            Assert.True(results.IsValid);
        }
    }
}