public class ProductGenerator
{
    public List<StoreProduct> Generate()
    {
        Random random = new Random(123);

        List<Product> products = new List<Product>();

        products.Add(CreateProduct(
            "Rolled oats", 1000, random));

        products.Add(CreateProduct(
            "Brown rice", 1000, random));

        products.Add(CreateProduct(
            "Pasta", 500, random));

        products.Add(CreateProduct(
            "Cheddar cheese", 500, random));

        products.Add(CreateProduct(
            "Protein powder", 500, random));

        products.Add(CreateProduct(
            "Chicken breast", 1000, random));

        products.Add(CreateProduct(
            "Greek yoghurt", 500, random));

        products.Add(CreateProduct(
            "Canned tuna", 185, random));

        products.Add(CreateProduct(
            "Peanut butter", 375, random));

        products.Add(CreateProduct(
            "Red lentils", 500, random));

        products.Add(CreateProduct(
            "Wholemeal bread", 700, random));

        products.Add(CreateProduct(
            "Tofu", 450, random));

        List<string> stores = new List<string>();

        stores.Add("Safepath Supermarket");
        stores.Add("Coals & Co");

        List<StoreProduct> catalogue =
            new List<StoreProduct>();

        int listingId = 1;

        foreach (Product product in products)
        {
            foreach (string store in stores)
            {
                // Gen prices between 1.50 and 8.00.
                decimal price =
                    random.Next(150, 801) / 100m;

                StoreProduct listing = new StoreProduct(
                    listingId,
                    product,
                    store,
                    price);

                catalogue.Add(listing);

                listingId++;
            }
        }

        return catalogue;
    }

    private Product CreateProduct(
        string name,
        int packSizeGrams,
        Random random)
    {
        Product product = new Product();

        product.Name = name;
        product.PackSizeGrams = packSizeGrams;

        // Generate random nutrition values for energy and protein.
        product.EnergyKjPer100g =
            random.Next(1000, 2001);

        product.ProteinGramsPer100g =
            random.Next(10, 451) / 10m;

        return product;
    }
}