namespace Sitecore.Feature.Cart.Repositories
{
    using System.Collections.Generic;
    using Sitecore.Feature.Base.Models.JsonResults;
    using Sitecore.Feature.Cart.Models.InputModels;
    using Sitecore.Feature.Cart.Models.JsonResults;
    using Sitecore.Foundation.CommerceServer.Models.InputModels;

    public interface ICartRepository
    {
        BaseJsonResult AddCartLine(AddCartLineInputModel inputModel);

        BaseJsonResult AddCartLines(IEnumerable<AddCartLineInputModel> inputModels);

        CSCartBaseJsonResult ApplyDiscount(DiscountInputModel model);

        CSCartBaseJsonResult DeleteLineItem(DeleteCartLineInputModel model);

        CSCartBaseJsonResult GetCurrentCart();

        CSCartBaseJsonResult RemoveDiscount(DiscountInputModel model);

        MiniCartBaseJsonResult UpdateMiniCart(bool updateCart);

        CSCartBaseJsonResult UpdateLineItem(UpdateCartLineInputModel inputModel);

    }
}