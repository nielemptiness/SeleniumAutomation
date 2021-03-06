using System;
using Core.Base.Startup;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using Tests.PageObjects;

namespace Tests.Tests
{
    [TestFixture(DRIVERS.ChromeDriver)]
    [Parallelizable(ParallelScope.Self | ParallelScope.Children)]
    public class CheckoutTests : BaseTest
    {
        
        public CheckoutTests(DRIVERS driverType) : base(driverType)
        {
        }
        
        [Test]
        public void Can_GoToCheckOutPage_WhenAddedALaptopToCart()
        {
            //Act
            #region Explicit variant:
                // var mainPage = new Navigation(driver).GoToMainPage();
                // var laptopPage = mainPage.OpenLaptopsSection();
                // var laptops = laptopPage.OpenLaptopSelectionPage();
                // var cart = laptops.AddFirstToCart();
                // var checkOut = cart.GoToCheckOut();
                // var result = checkOut.IsCheckoutPage();
            #endregion
            
            //chaining variant
            var result = new Navigation(driver)
                .GoToMainPage()
                .OpenLaptopsSection()
                .OpenLaptopSelectionPage()
                .AddFirstToCart()
                .GoToCheckOut()
                .IsCheckoutPage();
            
            Assert.True(result, "The page was wrong!");
        }

        [Test]
        public void CanNot_GoToCheckout_WhenNoAddedItems()
        {
            //Act
            new Navigation(driver).GoToMainPage(); 
            Func<CheckOutPage> checkout = () => new CartPage(driver).GoToCheckOut();
            //Assert
            checkout.Should().Throw<WebDriverTimeoutException>();
        }
    }
}