using Api.src.Cart.application.service;
using Api.src.Cart.domain.dto;
using Api.src.Cart.domain.entity;
using Api.src.Cart.domain.enums;
using Api.src.CartToProduct.domain.entity;
using Api.src.Category.domain.entity;
using Api.src.Category.domain.enums;
using Api.src.Common.exceptions;
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
    public class CreatePurchaseTests
    {
        [Fact]
        public async void Given_APurchase_When_TheCartDoesNotExist_Then_NotFoundException()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Shopper, Status = UserStatus.Created };
            dbContext.User.Add(user);
            await dbContext.SaveChangesAsync();

            CreatePurchase createPurchase = new CreatePurchase(dbContext);

            //ACT
            Func<Task> act = () => createPurchase.Run(user.Id);

            //Assert
            await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Cart does not exist");

        }

        [Fact]
        public async void Given_APurchase_When_InsufficientProductInInventory_Then_ConflictException()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Shopper, Status = UserStatus.Created };
            CartEntity cart = new() { User = user, State = CartState.Pending };
            CategoryEntity categoryEntity = new CategoryEntity() { Name = "Music", Status = CategoryStatus.Created};
            ProductEntity product = new ProductEntity() { Category = categoryEntity, Title = "Pink Floyd album",Description="The best one",Image="www.pinkfloyd.com" ,Stock = 0,Status= ProductStatus.Created };
            PriceEntity priceEntity = new PriceEntity() { Product=product,Current = true, Price = 120 };
            CartToProductEntity cartToProduct = new CartToProductEntity() {Cart=cart, Price=priceEntity,Quantity=10 };
            dbContext.User.Add(user);
            dbContext.Cart.Add(cart);
            dbContext.Category.Add(categoryEntity);
            dbContext.Price.Add(priceEntity);
            dbContext.CartToProduct.Add(cartToProduct);
            dbContext.Product.Add(product);

            await dbContext.SaveChangesAsync();

            CreatePurchase createPurchase = new CreatePurchase(dbContext);

            //ACT
            Func<Task> act = () => createPurchase.Run(user.Id);

            //Assert
            await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("Insufficient product in inventory");

        }
        [Fact]
        public async void Given_APurchase_When_TheProductIsEnough_Then_PurchaseResponseDto()
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
            var expected = new PurchaseResponseDto() { Message = "Successful purchase" };
            //ACT
            CreatePurchase createPurchase = new CreatePurchase(dbContext);
            var result =await createPurchase.Run(user.Id);
            //Assert
            result.Message.Should().Be(expected.Message);

        }
    }
}
