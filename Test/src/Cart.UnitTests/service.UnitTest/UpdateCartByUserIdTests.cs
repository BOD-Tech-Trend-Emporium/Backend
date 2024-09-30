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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Common;

namespace Test.src.Cart.UnitTests.service.UnitTest
{
    public class UpdateCartByUserIdTests
    {

        [Fact]
        public async void Given_UpdateCartDto_When_APendingCartDoesNotExist_Then_NotFoundException()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Shopper, Status = UserStatus.Created };
            CouponEntity couponEntity = new CouponEntity() { Code = "BLASH", Discount = 10, Status = CouponStatus.Active };

            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            UpdateCartDto updateCartDto = new UpdateCartDto() {CouponCode=couponEntity.Code };

            UpdateCartByUserId updateCartByUserId = new UpdateCartByUserId(dbContext);

            //ACT
            Func<Task> act = () => updateCartByUserId.Run(updateCartDto, user.Id);

            //Assert
            await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Cart does not exist");

        }

        [Fact]
        public async void Given_UpdateCartDto_When_APendingCartExists_Then_CartResponseDto()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Shopper, Status = UserStatus.Created };
            CouponEntity couponEntity = new CouponEntity() { Code = "BLASH", Discount = 10, Status = CouponStatus.Active };
            CartEntity cart = new() { User = user, State = CartState.Pending, Coupon = couponEntity };
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
            CouponDto couponDto = new CouponDto() { Coupon_code = couponEntity.Code, Discount_percentage = couponEntity.Discount };
            CartWithCouponResponseDto expectedCartResponse = new CartWithCouponResponseDto() { UserId = user.Id, ShoppingCart = new List<Guid> { product.Id }, ShippingCost = 0, TotalBeforeDiscount = total, TotalAfterDiscount = total - (total * couponEntity.Discount) / 100, FinalTotal = (total - (total * couponEntity.Discount) / 100) + cart.ShippingCost, CouponApplied = couponDto }; 

            UpdateCartDto updateCartDto = new UpdateCartDto() { CouponCode = couponEntity.Code };

            UpdateCartByUserId updateCartByUserId = new UpdateCartByUserId(dbContext);

            //ACT
            var result = await updateCartByUserId.Run(updateCartDto, user.Id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CartWithCouponResponseDto>();
            result.Should().BeEquivalentTo(expectedCartResponse);

        }

    }
}
