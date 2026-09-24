using Xunit;

public class ShoppingCartTests
{
    // Reuse this setup so every test does not repeat it.
    private StoreProduct CreatePasta()
    {
        Product pasta = new Product();
        pasta.Name = "Pasta";
        pasta.PackSizeGrams = 500;

        return new StoreProduct(
            1,
            pasta,
            "Coals & Co",
            2.41m);
    }

    [Fact]
    public void EmptyCartHasZeroTotal()
    {
        ShoppingCart cart = new ShoppingCart();

        Assert.Empty(cart.GetItems());
        Assert.Equal(0m, cart.CalculateTotal());
    }

    [Fact]
    public void AddingTwoPastaPacksGivesCorrectTotal()
    {
        StoreProduct pasta = CreatePasta();
        ShoppingCart cart = new ShoppingCart();

        cart.AddItem(pasta);
        cart.AddItem(pasta);

        Assert.Equal(4.82m, cart.CalculateTotal());
    }

    [Fact]
    public void AddingSameListingTwiceIncreasesQuantity()
    {
        StoreProduct pasta = CreatePasta();
        ShoppingCart cart = new ShoppingCart();

        cart.AddItem(pasta);
        cart.AddItem(pasta);

        // 1 row having 2 packs
        Assert.Single(cart.GetItems());
        Assert.Equal(2, cart.GetItems()[0].Quantity);
    }

    [Fact]
    public void RemovingOnePackReducesQuantityAndTotal()
    {
        StoreProduct pasta = CreatePasta();
        ShoppingCart cart = new ShoppingCart();

        cart.AddItem(pasta);
        cart.AddItem(pasta);

        cart.RemoveOne(pasta.Id);

        Assert.Single(cart.GetItems());
        Assert.Equal(1, cart.GetItems()[0].Quantity);
        Assert.Equal(2.41m, cart.CalculateTotal());
    }

    [Fact]
    public void RemovingPastZeroLeavesAnEmptyCart()
    {
        StoreProduct pasta = CreatePasta();
        ShoppingCart cart = new ShoppingCart();

        cart.AddItem(pasta);

        //remove last pack then remove again to test removing past zero
        cart.RemoveOne(pasta.Id);
        cart.RemoveOne(pasta.Id);

        Assert.Empty(cart.GetItems());
        Assert.Equal(0m, cart.CalculateTotal());
    }

    [Fact]
    public void SameProductFromDifferentStoresStaysSeparate()
    {
        StoreProduct coalsPasta = CreatePasta();

        // test different store with id and price
        StoreProduct safepathPasta = new StoreProduct(
            2,
            coalsPasta.Product,
            "Safepath Supermarket",
            4.89m);

        ShoppingCart cart = new ShoppingCart();

        cart.AddItem(coalsPasta);
        cart.AddItem(safepathPasta);

        Assert.Equal(2, cart.GetItems().Count);
        Assert.Equal(1, cart.GetItems()[0].Quantity);
        Assert.Equal(1, cart.GetItems()[1].Quantity);

        // 2.41 + 4.89 = 7.30.
        Assert.Equal(7.30m, cart.CalculateTotal());
    }

    [Fact]
    public void RemovingEntireEntryKeepsOtherEntries()
    {
        StoreProduct coalsPasta = CreatePasta();

        StoreProduct safepathPasta = new StoreProduct(
            2,
            coalsPasta.Product,
            "Safepath Supermarket",
            4.89m);

        ShoppingCart cart = new ShoppingCart();

        cart.AddItem(coalsPasta);
        cart.AddItem(coalsPasta);
        cart.AddItem(safepathPasta);

        // remove both Coals packs in one go, leaving the Safepath pack.
        cart.RemoveItem(coalsPasta.Id);

        // safepath should remain with 1
        Assert.Single(cart.GetItems());
        Assert.Equal(safepathPasta.Id, cart.GetItems()[0].Listing.Id);
        Assert.Equal(4.89m, cart.CalculateTotal());
    }
}