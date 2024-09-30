using Api.src.Cart.application.service;
using Api.src.Cart.domain.dto;
using Api.src.Cart.domain.entity;
using Api.src.Cart.domain.enums;
using Api.src.Category.domain.enums;
using Api.src.Common.exceptions;
using Api.src.User.domain.enums;
using backend.Data;
using backend.src.User.domain.entity;
using backend.src.User.domain.enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Test.Common;


namespace Test.src.Cart.UnitTests.service.UnitTest
{
    public class CreateCartTests
    {

        [Fact]
        public async void Given_ANewCart_When_UserHasOneWithPendingStatus_Then_ConflictException()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Shopper, Status = UserStatus.Created };
            CartEntity cart = new() { User = user, State = CartState.Pending};
            dbContext.User.Add(user);
            dbContext.Cart.Add(cart);
            await dbContext.SaveChangesAsync();

             CreateCart createCart = new CreateCart(dbContext);

            //ACT
            Func<Task> act = () => createCart.Run(user.Id);

            //Assert
            await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("The cart already exists");

        }

        [Fact]
        public async void Given_ANewCart_When_UserDoesNotHaveOneOrApprovedStatus_Then_CartResponseDto()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Shopper, Status = UserStatus.Created };
            CartEntity cart = new() { User = user, State = CartState.Approved };
            dbContext.User.Add(user);
            dbContext.Cart.Add(cart);
            await dbContext.SaveChangesAsync();
            CartResponseDto cartResponse = new CartResponseDto() { UserId = user.Id, ShoppingCart = new List<Guid>(), ShippingCost = 0, FinalTotal = 0 };

            CreateCart createCart = new CreateCart(dbContext);

            //ACT
            var result = await createCart.Run(user.Id);

            //Assert
            result.UserId.Should().Be(user.Id);
            result.ShoppingCart.Should().BeEmpty();
            result.Should().NotBeNull();
            result.FinalTotal.Should().Be(cartResponse.FinalTotal);

        }

    }


}
