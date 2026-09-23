var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Generate the catalogue once at startup.
ProductGenerator generator = new ProductGenerator();
List<StoreProduct> catalogue = generator.Generate();

CatalogueService catalogueService = new CatalogueService();

// One shared cart for this demonstration application.
ShoppingCart cart = new ShoppingCart();

// Prevent simultaneous requests from changing the cart together.
object cartLock = new object();

app.MapGet("/products", ShowProducts);

app.MapGet("/cart", ShowCart);
app.MapPost("/cart/{id:int}/add", AddToCart);
app.MapPost("/cart/{id:int}/decrease", DecreaseCartItem);
app.MapDelete("/cart/{id:int}", RemoveCartItem);

app.Run();

List<StoreProduct> ShowProducts(
    string? search,
    string? sort,
    bool? highProtein)
{
    List<StoreProduct> results = catalogue;

    if (!string.IsNullOrWhiteSpace(search))
    {
        results = catalogueService.SearchByName(results, search);
    }

    if (highProtein == true)
    {
        results = catalogueService.FilterByProtein(results, 30m);
    }

    if (sort == "price-ascending")
    {
        results = catalogueService.SortByPrice(results, true);
    }
    else if (sort == "price-descending")
    {
        results = catalogueService.SortByPrice(results, false);
    }

    return results;
}

IResult ShowCart()
{
    lock (cartLock)
    {
        // Copy the current values into a response.
        // This avoids exposing the changing cart list directly.
        List<object> responseItems = new List<object>();

        foreach (CartItem item in cart.GetItems())
        {
            responseItems.Add(new
            {
                listingId = item.Listing.Id,
                name = item.Listing.Product.Name,
                storeName = item.Listing.StoreName,
                packSizeGrams = item.Listing.Product.PackSizeGrams,
                price = item.Listing.Price,
                quantity = item.Quantity,
                lineTotal = item.CalculateLineTotal()
            });
        }

        return Results.Ok(new
        {
            items = responseItems,
            total = cart.CalculateTotal()
        });
    }
}

IResult AddToCart(int id)
{
    StoreProduct? selectedListing = FindListing(id);

    if (selectedListing == null)
    {
        return Results.NotFound("Product listing not found.");
    }

    lock (cartLock)
    {
        cart.AddItem(selectedListing);
    }

    return Results.NoContent();
}

IResult DecreaseCartItem(int id)
{
    lock (cartLock)
    {
        cart.RemoveOne(id);
    }

    return Results.NoContent();
}

IResult RemoveCartItem(int id)
{
    lock (cartLock)
    {
        cart.RemoveItem(id);
    }

    return Results.NoContent();
}

StoreProduct? FindListing(int id)
{
    foreach (StoreProduct listing in catalogue)
    {
        if (listing.Id == id)
        {
            return listing;
        }
    }

    return null;
}