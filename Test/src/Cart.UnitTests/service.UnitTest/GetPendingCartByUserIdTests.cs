using Api.src.Cart.application.service;
using Api.src.Cart.domain.dto;
using Api.src.Cart.domain.entity;
using Api.src.Cart.domain.enums;
using Api.src.CartToProduct.domain.entity;
using Api.src.Category.domain.entity;
using Api.src.Category.domain.enums;
using Api.src.Common.exceptions;
using Api.src.Coupon.domain.entity;
using Api.src.Coupon.domain.enums;
using Api.src.Price.domain.entity;
using Api.src.Product.domain.entity;
using Api.src.Product.domain.enums;
using Api.src.User.domain.enums;
using backend.Data;
using backend.src.User.domain.entity;
using backend.src.User.domain.enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Common;


namespace Test.src.Cart.UnitTests.service.UnitTest
{
    public class GetPendingCartByUserIdTests
    {

        [Fact]
        public async void Given_APendingCart_When_ItIsEmpty_Then_NotFoundException()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Shopper, Status = UserStatus.Created };

            dbContext.Add(user);
            await dbContext.SaveChangesAsync();

            GetPendingCartByUserId getPendingCartByUserId = new GetPendingCartByUserId(dbContext);

            //ACT
            Func<Task> act = () => getPendingCartByUserId.Run(user.Id);

            //Assert
            await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Cart does not have products");

        }
        [Fact]
        public async void Given_APendingCart_When_ItDoesNotHaveCoupon_Then_CartResponseDto()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Shopper, Status = UserStatus.Created };
            CartEntity cart = new() { User = user, State = CartState.Pending };
            CategoryEntity categoryEntity = new CategoryEntity() { Name = "Music", Status = CategoryStatus.Created };
            ProductEntity product = new ProductEntity() { Category = categoryEntity, Title = "Pink Floyd album", Description = "The best one", Image = "www.pinkfloyd.com", Stock = 20, Status = ProductStatus.Created };
            PriceEntity priceEntity = new PriceEntity() { Product = product, Current = true, Price = 120 };
            CartToProductEntity cartToProduct = new CartToProductEntity() { Cart = cart, Price = priceEntity, Quantity = 10 };
            dbContext.User.Add(user);
            dbContext.Cart.Add(cart);
            dbContext.Category.Add(categoryEntity);
            dbContext.Price.Add(priceEntity);
            dbContext.CartToProduct.Add(cartToProduct);
            dbContext.Product.Add(product);
            await dbContext.SaveChangesAsync();
            CartResponseDto expectedCartResponse = new CartResponseDto() { UserId = user.Id, ShoppingCart = new List<Guid> { product.Id}, ShippingCost=0, FinalTotal=cartToProduct.Quantity*priceEntity.Price};

            GetPendingCartByUserId getPendingCartByUserId = new GetPendingCartByUserId(dbContext);

            //ACT
            var result = await getPendingCartByUserId.Run(user.Id);

            //Assert

            result.Should().BeOfType<CartResponseDto>();
            result.FinalTotal.Should().Be(expectedCartResponse.FinalTotal);
            result.UserId.Should().Be(expectedCartResponse.UserId);
            result.ShoppingCart.Should().NotBeEmpty();
            result.ShoppingCart.Should().BeEquivalentTo(expectedCartResponse.ShoppingCart);
        }

        [Fact]
        public async void Given_APendingCart_When_ItHasCoupon_Then_CartWithCouponResponseDto()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Shopper, Status = UserStatus.Created };
            CouponEntity couponEntity = new CouponEntity() { Code = "BLASH", Discount = 10, Status = CouponStatus.Active };
            CartEntity cart = new() { User = user, State = CartState.Pending, Coupon=couponEntity};
            CategoryEntity categoryEntity = new CategoryEntity() { Name = "Music", Status = CategoryStatus.Created };
            ProductEntity product = new ProductEntity() { Category = categoryEntity, Title = "Pink Floyd album", Description = "The best one", Image = "www.pinkfloyd.com", Stock = 20, Status = ProductStatus.Created };
            PriceEntity priceEntity = new PriceEntity() { Product = product, Current = true, Price = 120 };
            CartToProductEntity cartToProduct = new CartToProductEntity() { Cart = cart, Price = priceEntity, Quantity = 10 };
            dbContext.User.Add(user);
            dbContext.Cart.Add(cart);
            dbContext.Category.Add(categoryEntity);
            dbContext.Price.Add(priceEntity);
            dbContext.CartToProduct.Add(cartToProduct);
            dbContext.Product.Add(product);
            dbContext.Coupon.Add(couponEntity);
            await dbContext.SaveChangesAsync();
            var total = cartToProduct.Quantity * priceEntity.Price;
            CouponDto couponDto = new CouponDto() {Coupon_code= couponEntity.Code, Discount_percentage=couponEntity.Discount };
            CartWithCouponResponseDto expectedCartResponse = new CartWithCouponResponseDto() { UserId = user.Id, ShoppingCart = new List<Guid> { product.Id }, ShippingCost = 0, TotalBeforeDiscount= total, TotalAfterDiscount = total - (total * couponEntity.Discount)/100, FinalTotal = (total - (total * couponEntity.Discount) / 100)+cart.ShippingCost, CouponApplied = couponDto };

            GetPendingCartByUserId getPendingCartByUserId = new GetPendingCartByUserId(dbContext);

            //ACT
            var result = await getPendingCartByUserId.Run(user.Id);

            //Assert

            result.Should().BeOfType<CartWithCouponResponseDto>();
            result.FinalTotal.Should().Be(expectedCartResponse.FinalTotal);
            result.UserId.Should().Be(expectedCartResponse.UserId);
            result.ShoppingCart.Should().NotBeEmpty();
            result.ShoppingCart.Should().BeEquivalentTo(expectedCartResponse.ShoppingCart);
            result.Should().BeEquivalentTo(result);
        }
    }
}
