using Xunit;

public class ShoppingCartTests
{
    [Fact]
    public void AddingTwoPastaPacksGivesCorrectTotal()
    {
        Product pasta = new Product();
        pasta.Name = "Pasta";
        pasta.PackSizeGrams = 500;

        StoreProduct storePasta = new StoreProduct(
            1,
            pasta,
            "Coals & Co",
            2.41m);

        ShoppingCart cart = new ShoppingCart();

        cart.AddItem(storePasta);
        cart.AddItem(storePasta);

        decimal actualTotal = cart.CalculateTotal();

        Assert.Equal(4.82m, actualTotal);
    }
}