using System;
using Sitecore.Data.Items;
using Sitecore.Foundation.CommerceServer.Models.SitecoreItemModels;
using Sitecore.Globalization;
using Sitecore.Mvc.Presentation;
using System.Web;
using FluentAssertions;
using NSubstitute;
using Sitecore.Foundation.Testing.Mocks.Contexts;
using Sitecore.Data;
using Sitecore.FakeDb.Sites;
using Sitecore.Feature.GiftCard.Repositories;
using Sitecore.Foundation.CommerceServer.Interfaces;
using Sitecore.Foundation.CommerceServer.Models;
using Sitecore.Foundation.Testing.Attributes;
using Xunit;

namespace Sitecore.Feature.GiftCard.Tests.Repositories
{

    /* This section must be in config file
      <sitecore>
         <settings>
           <setting name="LicenseFile" value="c:\Websites\license.xml" />
         </settings>
         <commerceServer>
           <types>
             <type name="ISiteContext" type="Sitecore.Foundation.CommerceServer.Infrastructure.Contexts.SiteContext, Sitecore.Foundation.CommerceServer" lifetime="Singleton" />
             <type name="CommerceStorefront" type="Sitecore.Foundation.CommerceServer.Models.CommerceServerStorefront, Sitecore.Foundation.CommerceServer" lifetime="PerCall" />
           </types>
         </commerceServer>
       </sitecore>
   */

    public class GiftCardRepositoryTests
    {

        private readonly IAccountManager _accountManagerMock;
        private readonly ICatalogManager _catalogManagerMock;
        private readonly IContactFactory _contactFactoryMock;
        

        public GiftCardRepositoryTests()
        {
            _accountManagerMock = Substitute.For<IAccountManager>();
            _catalogManagerMock = Substitute.For<ICatalogManager>();

            _contactFactoryMock = Substitute.For<IContactFactory>();
        }

        [Theory]
        [AutoDbData]
        public void GetGiftCardViewModel_should_return_ProductViewModel_from_SiteContext_IfAvailable(Database db)
        {
            //arrange
            ProductViewModel testProduct = new ProductViewModel
            {
                ProductName = "testProductName",
                Description = "testProductDescription"
            };

            MockContexts(db);
            var rep = new GiftCardRepository(_accountManagerMock, _contactFactoryMock, _catalogManagerMock);
            rep.CurrentSiteContext.Items["CurrentProductViewModel"] = testProduct;

            // act    
            var result = rep.GetGiftCardViewModel(BuildFakeItem(db), new Rendering());

            //assert
            result.ProductName.Should().Be(testProduct.ProductName);
            result.Description.Should().Be(testProduct.Description);
        }

        [Theory]
        [AutoDbData]
        public void GetGiftCardViewModel_should_take_ProductName_from_ItemsDisplayName(Database db)
        {
            //arrange
            string displayNameTest = "DisplayNameTest";
            MockContexts(db);
            
            var rep = new GiftCardRepository(_accountManagerMock, _contactFactoryMock, _catalogManagerMock);

            // act
            var result = rep.GetGiftCardViewModel(BuildFakeItem(db, displayNameTest), new Rendering());

            // assert
            result.ProductName.Should().Be(displayNameTest);
        }


        [Theory]
        [AutoDbData]
        public void GetGiftCardViewModel_should_set_ParentCategoryId_and_ParentCategoryId_if_UrlContainsCategory(Database db)
        {
            //arrange
            string categoryIdTest = "categoryIdTest";
            string categoryNameTest = "parentCategoryNameTest";

            Item fakeItem = BuildFakeItem(db, categoryNameTest);

            Category categoryTest = new Category(fakeItem);

            _catalogManagerMock.GetCategory(categoryIdTest).Returns(categoryTest);

            MockContexts(db, $"http://local/{categoryIdTest}/Id");

            var rep = new GiftCardRepository(_accountManagerMock, _contactFactoryMock, _catalogManagerMock);
            rep.CurrentSiteContext.UrlContainsCategory = true;

            // act
            var product = rep.GetGiftCardViewModel(BuildFakeItem(db), new Rendering());

            // assert
            product.ParentCategoryId.Should().Be(categoryIdTest);
            product.ParentCategoryName.Should().Be(categoryNameTest);
        }

        [Theory]
        [ItemWithChildrenDbData]
        public void GetGiftCardViewModel_should_return_ModelWithVarians_if_ItemHasChildren(Database db)
        {
            //arrange
            Item fakeItem = GetFakeItemWithChildren(db);
            MockContexts(db);

            var rep = new GiftCardRepository(_accountManagerMock, _contactFactoryMock, _catalogManagerMock);

            // act
            var product = rep.GetGiftCardViewModel(fakeItem, new Rendering());

            // assert
            product.Variants.Count.Should().Be(GiftCardTestsConstants.ItemWithChildrenNumberOfChildren);
            product.Variants[0].Id.Should().Be(GiftCardTestsConstants.ItemWithChildrenFirstChildItemName);
            product.Variants[1].Id.Should().Be(GiftCardTestsConstants.ItemWithChildrenSecondChildItemName);
        }


        [Theory]
        [AutoDbData]
        public void GetGiftCardViewModel_should_apply_GiftCardSpecialHandling_For_NonGiftCardItems(Database db)
        {
            //arrange
            Item fakeItem = BuildFakeItem(db);
            MockContexts(db);
            decimal testRating = new decimal(5.0);

            _catalogManagerMock.GetProductRating(fakeItem).Returns(testRating);
            var rep = new GiftCardRepository(_accountManagerMock, _contactFactoryMock, _catalogManagerMock);

            // act
            var product = rep.GetGiftCardViewModel(fakeItem, new Rendering());

            // assert
            product.CustomerAverageRating.Should().Be(testRating);
        }

        [Theory]
        [ItemWithChildrenDbData("22565422120")] // Gift Card ProductId
        public void GetGiftCardViewModel_should_apply_GiftCardSpecialHandling_For_GiftCardItem(Database db)
        {
            //arrange
            Item fakeGiftCardItem = GetFakeItemWithChildren(db);
            MockContexts(db);

            decimal adjustedPriceTest = new decimal(1.0);
            _catalogManagerMock.GetProductPrice(Arg.Any<IVisitorContext>(), Arg.Do<ProductViewModel>(p => p.Variants.ForEach(v=>v.AdjustedPrice = adjustedPriceTest)));


            var rep = new GiftCardRepository(_accountManagerMock, _contactFactoryMock, _catalogManagerMock);

            // act
            var product = rep.GetGiftCardViewModel(fakeGiftCardItem, new Rendering());

            // assert
            product.GiftCardAmountOptions.Count.Should().Be(GiftCardTestsConstants.ItemWithChildrenNumberOfChildren);
            product.GiftCardAmountOptions[0].Key.Should().Be(GiftCardTestsConstants.ItemWithChildrenFirstChildItemName);
            product.GiftCardAmountOptions[0].Value.Should().Be(adjustedPriceTest);
            product.GiftCardAmountOptions[1].Key.Should().Be(GiftCardTestsConstants.ItemWithChildrenSecondChildItemName);
            product.GiftCardAmountOptions[1].Value.Should().Be(adjustedPriceTest);
        }

        [Theory]
        [AutoDbData]
        public void GetGiftCardViewModel_should_set_CurrentProductItem_in_SiteContext(Database db)
        {
            //arrange
            Item fakeItem = BuildFakeItem(db);
            MockContexts(db);

            var rep = new GiftCardRepository(_accountManagerMock, _contactFactoryMock, _catalogManagerMock);
            
            // act
            var product = rep.GetGiftCardViewModel(fakeItem, new Rendering());
            
            // assert
            rep.CurrentSiteContext.Items["CurrentProductViewModel"].Should().Be(product);
        }

        private Item GetFakeItemWithChildren(Database db)
        {
            return db.Items[GiftCardTestsConstants.ItemWithChildrenParentItemId];
        }

        private Item BuildFakeItem(Database db, string displayName = "DisplayName1")
        {
            var fakeFieldSet = new FieldList
            {
                {FieldIDs.DisplayName, displayName},
                {new ID(Guid.NewGuid()), "Value2"},
                {new ID(Guid.NewGuid()), "Value3"}
            };

            var fakeProduct = new Item(new ID(Guid.NewGuid()),
                new ItemData(new ItemDefinition(
                    new ID(Guid.NewGuid()), "ItemName",
                    new ID(Guid.NewGuid()),
                    new ID(Guid.NewGuid())), Language.Current, Data.Version.Parse(1), fakeFieldSet),
                db);
            
            return fakeProduct;
        }

        private void MockContexts(Database db, string httpRequestUrl = "http://local/")
        {
            var fakeSiteContext = new FakeSiteContext("fake");
            HttpContext.Current = new HttpContext(new HttpRequest(null, httpRequestUrl, null), new HttpResponse(null));
            fakeSiteContext.Database = db;
            Context.Site = fakeSiteContext;
            Context.Items["__visitorContext"] = new MockVisitorContext("1", "fake", "1");
        }
    }
}
