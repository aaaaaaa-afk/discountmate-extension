public class StoreProduct
{
    public int Id { get; private set; }
    public Product Product { get; private set; }
    public string StoreName { get; private set; }
    public decimal Price { get; private set; }

    public StoreProduct(
        int id,
        Product product,
        string storeName,
        decimal price)
    {
        if (price <= 0)
        {
            throw new ArgumentException(
                "Price must be greater than zero.");
        }

        Id = id;
        Product = product;
        StoreName = storeName;
        Price = price;
    }
}