using Xunit;

public class CatalogueServiceTests
{
    // set nutrition values to test, at and above the 30g boundary
    private List<StoreProduct> CreateTestCatalogue()
    {
        Product rice = new Product();
        rice.Name = "Brown rice";
        rice.PackSizeGrams = 1000;
        rice.ProteinGramsPer100g = 29m;

        Product pasta = new Product();
        pasta.Name = "Pasta";
        pasta.PackSizeGrams = 500;
        pasta.ProteinGramsPer100g = 30m;

        Product oats = new Product();
        oats.Name = "Rolled oats";
        oats.PackSizeGrams = 1000;
        oats.ProteinGramsPer100g = 31m;

        List<StoreProduct> catalogue = new List<StoreProduct>();

        catalogue.Add(new StoreProduct(
            1, rice, "Safepath Supermarket", 5.96m));

        catalogue.Add(new StoreProduct(
            2, rice, "Coals & Co", 7.35m));

        catalogue.Add(new StoreProduct(
            3, pasta, "Coals & Co", 2.41m));

        catalogue.Add(new StoreProduct(
            4, oats, "Coals & Co", 5.65m));

        return catalogue;
    }

    [Fact]
    public void SearchIgnoresCapitalisationAndOutsideSpaces()
    {
        List<StoreProduct> catalogue = CreateTestCatalogue();
        CatalogueService service = new CatalogueService();

        List<StoreProduct> results =
            service.SearchByName(catalogue, "  RICE  ");

        // rice listing should match both stores, so two results are expected
        Assert.Equal(2, results.Count);

        foreach (StoreProduct listing in results)
        {
            Assert.Equal("Brown rice", listing.Product.Name);
        }
    }

    [Fact]
    public void UnknownProductReturnsNoMatches()
    {
        List<StoreProduct> catalogue = CreateTestCatalogue();
        CatalogueService service = new CatalogueService();

        List<StoreProduct> results =
            service.SearchByName(catalogue, "Chocolate");

        Assert.Empty(results);
    }

    [Fact]
    public void EmptySearchReturnsEveryListing()
    {
        List<StoreProduct> catalogue = CreateTestCatalogue();
        CatalogueService service = new CatalogueService();

        List<StoreProduct> results =
            service.SearchByName(catalogue, "");

        Assert.Equal(catalogue.Count, results.Count);
    }

    [Fact]
    public void ProteinFilterExcludesThirtyAndIncludesThirtyOne()
    {
        List<StoreProduct> catalogue = CreateTestCatalogue();
        CatalogueService service = new CatalogueService();

        List<StoreProduct> results =
            service.FilterByProtein(catalogue, 30m);

        // rice has 29, pasta has 30, and oats has 31.
        // only oats should appear
        Assert.Single(results);
        Assert.Equal("Rolled oats", results[0].Product.Name);
        Assert.Equal(31m, results[0].Product.ProteinGramsPer100g);
    }

    [Fact]
    public void SearchAndProteinFilterMustBothMatch()
    {
        List<StoreProduct> catalogue = CreateTestCatalogue();
        CatalogueService service = new CatalogueService();

        // rice matches the name but not the protein threshold
        List<StoreProduct> results =
            service.SearchByName(catalogue, "rice");

        results = service.FilterByProtein(results, 30m);

        Assert.Empty(results);
    }
}