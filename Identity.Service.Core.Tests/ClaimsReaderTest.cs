using Identity.Service.Core.Model;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Identity.Service.Core.Tests
{
    public class ClaimsReaderTest
    {
        [Test]
        public void Getting_Claim_InvalidToken_ExpectException()
        {
            //Arrange, act, Assert
            Assert.Throws<ArgumentException>(() => new ClaimsReader("invalid token"));
        }

        [Test]
        public void Getting_Claim_ValidToken_InvalidKey_ExpectTrue()
        {
            //Arrange
            CustomClaimDictionary customClaimDictionary = new CustomClaimDictionary();
            customClaimDictionary.Add(AuthClaims.Role, "Normal");
            TokenGenerator generator = new TokenGenerator(getConfiguration(), null, customClaimDictionary);
            var tokenResult = generator.GenerateToken("brhv@ucn.dk");
            ClaimsReader reader = new ClaimsReader(tokenResult.Value);
            //Act
            var claimResult = reader.ReadClaim(AuthClaims.Role);
            //Assert
            Assert.IsTrue(claimResult.Success);
        }

        private IConfiguration getConfiguration(int limit = 0)
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"Auth:SecretKey", "1q2w3e4r5t6y7u8i9o"},
                 {"Auth:Issuer", "auth.ucn.dk"},
                  {"Auth:Audience", "students.ucn.dk"}
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            return config;
        }
    }
}