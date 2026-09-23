public class ShoppingCart
{
    private List<CartItem> items = new List<CartItem>();

    public void AddItem(StoreProduct listing)
    {
        foreach (CartItem item in items)
        {
            if (item.Listing.Id == listing.Id)
            {
                item.IncreaseQuantity();
                return;
            }
        }

        CartItem newItem = new CartItem(listing);

        items.Add(newItem);
    }

    public void RemoveOne(int listingId)
    {
        for (int index = 0; index < items.Count; index++)
        {
            CartItem item = items[index];

            if (item.Listing.Id == listingId)
            {
                item.DecreaseQuantity();

                if (item.Quantity == 0)
                {
                    items.RemoveAt(index);
                }

                return;
            }
        }
    }

    public void RemoveItem(int listingId)
    {
        for (int index = 0; index < items.Count; index++)
        {
            if (items[index].Listing.Id == listingId)
            {
                items.RemoveAt(index);
                return;
            }
        }
    }

    public decimal CalculateTotal()
    {
        decimal total = 0;

        foreach (CartItem item in items)
        {
            total += item.CalculateLineTotal();
        }

        return total;
    }

    public IReadOnlyList<CartItem> GetItems()
    {
        return items.AsReadOnly();
    }
}