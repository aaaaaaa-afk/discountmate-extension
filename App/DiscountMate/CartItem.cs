public class CartItem
{
    public StoreProduct Listing { get; private set; }
    public int Quantity { get; private set; }

    public CartItem(StoreProduct listing)
    {
        Listing = listing;
        Quantity = 1;
    }

    public void IncreaseQuantity()
    {
        Quantity++;
    }

    public void DecreaseQuantity()
    {
        if (Quantity > 0)
        {
            Quantity--;
        }
    }

    public decimal CalculateLineTotal()
    {
        return Listing.Price * Quantity;
    }
}