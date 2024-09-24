using Api.src.User.domain.enums;
using backend.src.User.application.mappers;
using backend.src.User.domain.dto;
using backend.src.User.domain.entity;
using backend.src.User.domain.enums;
using FluentAssertions;


namespace Test.User.UnitTests.application.UnitTests.mappers.UnitTests
{
    public class UserMappersTests
    {
        [Fact]
        public void Given_UserEntity_When_AllFieldsMatch_Then_UserDto()
        {
            //Arrange
            UserEntity userEntity = new() { Id = Guid.NewGuid(), Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Employee, Status = UserStatus.Created };

            //ACT
            var result = UserMappers.ToUserDto(userEntity);
            var expected = new UserDto() { Name = userEntity.Name, Email = userEntity.Email, UserName = userEntity.UserName, Role = userEntity.Role };

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UserDto>();
            result.Should().BeEquivalentTo(expected);

        }
        [Fact]
        public void Given_UserEntity_When_NotAllFieldsMatch_Then_UserDto()
        {
            //Arrange
            UserEntity userEntity = new() { Id = Guid.NewGuid(),  Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Employee, Status = UserStatus.Created };

            //ACT
            var result = UserMappers.ToUserDto(userEntity);
            var expected = new UserDto() {  Email = userEntity.Email, UserName = userEntity.UserName, Role = userEntity.Role };

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UserDto>();
            result.Should().BeEquivalentTo(expected);

        }
        [Fact]
        public void Given_UserEntity_When_NoAllFieldsMatch_Then_UserDto()
        {
            //Arrange
            UserEntity userEntity = new() { Id = Guid.NewGuid()};

            //ACT
            var result = UserMappers.ToUserDto(userEntity);
            var expected = new UserDto();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UserDto>();
            result.Should().BeEquivalentTo(expected);

        }

    }
}