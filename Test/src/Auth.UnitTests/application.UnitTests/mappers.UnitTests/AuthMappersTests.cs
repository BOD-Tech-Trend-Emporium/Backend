using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Auth.application.mappers;
using Api.src.Auth.domain.dto;
using Api.src.User.domain.enums;
using backend.src.User.application.mappers;
using backend.src.User.domain.entity;
using backend.src.User.domain.enums;
using FluentAssertions;

namespace Test.src.Auth.UnitTests.application.UnitTests.mappers.UnitTests
{
    public class AuthMappersTests
    {
        [Fact]
        public void Given_UserEntityAndToken_When_AllFieldsMatch_Then_LoggedUserDto()
        {
            //Arrange
            string token =  "token";
            UserEntity userEntity = new () {
                Name = "user",
                Email = "user@gmail.com",
                UserName = "user",
                Password = "user",
                Role = UserRole.Shopper,
                Status = UserStatus.Created,
            };

            //ACT
            var result = AuthMappers.ToLoggedUser(userEntity, token);
            var expected = new LoggedUserDto() {Token = token, Email = "user@gmail.com", UserName = "user"};

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<LoggedUserDto>();
            result.Should().BeEquivalentTo(expected);
        }
    }
}