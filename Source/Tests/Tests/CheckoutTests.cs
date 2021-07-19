using Core.Base.Startup;
using NUnit.Framework;
using Tests.PageObjects;

namespace Tests.Tests
{
    [TestFixture(DRIVERS.ChromeDriver)]
    [Parallelizable(ParallelScope.Self)]
    public class CheckoutTests : BaseTest
    {
        
        public CheckoutTests(DRIVERS driverType) : base(driverType)
        {
        }
        
        [Test]
        public void Can_GoToCheckOutPage_WhenAddedALaptop()
        {
            //Act
            var mainPage = new Navigation(driver).GoToMainPage();
            var laptopPage = mainPage.OpenLaptopsSection();
            var laptops = laptopPage.OpenLaptopSection();
            var cart = laptops.AddFirstToCart();
            var checkOut = cart.GoToCheckOut();
            var result = checkOut.IsCheckoutPage();
            
            Assert.True(result, "The page was wrong!");
        }
    }
}