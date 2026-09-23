public class CatalogueService
{
    public List<StoreProduct> SearchByName(
        List<StoreProduct> catalogue,
        string searchText)
    {
        List<StoreProduct> results =
            new List<StoreProduct>();

        string search = searchText.Trim();

        foreach (StoreProduct listing in catalogue)
        {
            bool matches = listing.Product.Name.Contains(
                search,
                StringComparison.OrdinalIgnoreCase);

            if (matches)
            {
                results.Add(listing);
            }
        }

        return results;
    }

    public List<StoreProduct> FilterByProtein(
        List<StoreProduct> catalogue,
        decimal threshold)
    {
        List<StoreProduct> results =
            new List<StoreProduct>();

        foreach (StoreProduct listing in catalogue)
        {
            if (listing.Product.ProteinGramsPer100g > threshold)
            {
                results.Add(listing);
            }
        }

        return results;
    }

    public List<StoreProduct> FilterByEnergy(
        List<StoreProduct> catalogue,
        int threshold)
    {
        List<StoreProduct> results =
            new List<StoreProduct>();

        foreach (StoreProduct listing in catalogue)
        {
            if (listing.Product.EnergyKjPer100g > threshold)
            {
                results.Add(listing);
            }
        }

        return results;
    }

    public List<StoreProduct> SortByPrice(
        List<StoreProduct> catalogue,
        bool lowToHigh)
    {
        // copy list so the OG order stays same
        List<StoreProduct> sorted =
            new List<StoreProduct>(catalogue);

        if (lowToHigh)
        {
            sorted.Sort(ComparePriceAscending);
        }
        else
        {
            sorted.Sort(ComparePriceDescending);
        }

        return sorted;
    }

    private int ComparePriceAscending(
        StoreProduct first,
        StoreProduct second)
    {
        return first.Price.CompareTo(second.Price);
    }

    private int ComparePriceDescending(
        StoreProduct first,
        StoreProduct second)
    {
        return second.Price.CompareTo(first.Price);
    }
}