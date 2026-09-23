async function loadProducts() {
    const status = document.getElementById("status");
    const tableBody = document.getElementById("product-rows");

    const searchText =
        document.getElementById("search-input").value.trim();

    const sortOrder =
        document.getElementById("sort-order").value;

    const highProtein =
        document.getElementById("high-protein").checked;

    const url =
        "/products?search=" + encodeURIComponent(searchText) +
        "&highProtein=" + highProtein;

    status.textContent = "Loading products...";

    try {
        const response = await fetch(url);

        if (!response.ok) {
            throw new Error("The server could not return the products.");
        }

        const listings = await response.json();

        const products = groupProducts(listings);

        if (sortOrder === "price-ascending") {
            products.sort(compareCheapestAscending);
        }
        else if (sortOrder === "price-descending") {
            products.sort(compareCheapestDescending);
        }

        tableBody.replaceChildren();

        for (const productGroup of products) {
            const row = document.createElement("tr");
            const product = productGroup.product;

            addCell(row, product.name);
            addCell(row, product.packSizeGrams + " g");
            addCell(row, product.energyKjPer100g + " kJ");
            addCell(row, product.proteinGramsPer100g + " g");

            addStoreCell(row, productGroup, "Safepath Supermarket");
            addStoreCell(row, productGroup, "Coals & Co");

            tableBody.appendChild(row);
        }

        if (products.length === 0) {
            status.textContent =
                "No products matched your search and filters.";
        }
        else {
            status.textContent =
                products.length + " products found.";
        }
    }
    catch (error) {
        tableBody.replaceChildren();

        status.textContent =
            "Unable to load products. Please try again.";

        console.error(error);
    }
}

function groupProducts(listings) {
    const groups = [];

    for (const listing of listings) {
        let matchingGroup = null;

        for (const group of groups) {
            const sameName =
                group.product.name === listing.product.name;

            const samePackSize =
                group.product.packSizeGrams ===
                listing.product.packSizeGrams;

            if (sameName && samePackSize) {
                matchingGroup = group;
                break;
            }
        }

        if (matchingGroup === null) {
            matchingGroup =
            {
                product: listing.product,
                listings: [],
                cheapestPrice: listing.price
            };

            groups.push(matchingGroup);
        }

        matchingGroup.listings.push(listing);

        if (listing.price < matchingGroup.cheapestPrice) {
            matchingGroup.cheapestPrice = listing.price;
        }
    }

    return groups;
}

function compareCheapestAscending(first, second) {
    return first.cheapestPrice - second.cheapestPrice;
}

function compareCheapestDescending(first, second) {
    return second.cheapestPrice - first.cheapestPrice;
}

function addStoreCell(row, productGroup, storeName) {
    const cell = document.createElement("td");

    let storeListing = null;

    for (const listing of productGroup.listings) {
        if (listing.storeName === storeName) {
            storeListing = listing;
            break;
        }
    }

    if (storeListing === null) {
        cell.textContent = "Unavailable";
    }
    else {
        const price = document.createElement("div");
        price.textContent = formatMoney(storeListing.price);

        const button = createCartButton(
            "Add",
            storeListing.id,
            "add");

        button.setAttribute(
            "aria-label",
            "Add " + productGroup.product.name +
            " from " + storeName + " to cart");

        cell.appendChild(price);
        cell.appendChild(button);
    }

    row.appendChild(cell);
}

async function loadCart() {
    const status = document.getElementById("cart-status");
    const tableBody = document.getElementById("cart-rows");
    const totalDisplay = document.getElementById("cart-total");

    try {
        const response = await fetch("/cart");

        if (!response.ok) {
            throw new Error("The server could not return the cart.");
        }

        const cart = await response.json();

        tableBody.replaceChildren();

        for (const item of cart.items) {
            const row = document.createElement("tr");

            addCell(row, item.name);
            addCell(row, item.storeName);
            addCell(row, item.packSizeGrams + " g");
            addCell(row, formatMoney(item.price));
            addCell(row, item.quantity);
            addCell(row, formatMoney(item.lineTotal));

            const actionCell = document.createElement("td");

            actionCell.appendChild(
                createCartButton("+1", item.listingId, "add"));

            actionCell.appendChild(
                createCartButton("-1", item.listingId, "decrease"));

            actionCell.appendChild(
                createCartButton("Remove", item.listingId, "remove"));

            row.appendChild(actionCell);
            tableBody.appendChild(row);
        }

        totalDisplay.textContent = formatMoney(cart.total);

        if (cart.items.length === 0) {
            status.textContent = "Your cart is empty.";
        }
        else {
            status.textContent =
                cart.items.length +
                " different store listings in your cart.";
        }
    }
    catch (error) {
        tableBody.replaceChildren();
        totalDisplay.textContent = "—";

        status.textContent =
            "Unable to load the cart. Refresh the page to try again.";

        console.error(error);
    }
}

function createCartButton(label, listingId, action) {
    const button = document.createElement("button");

    button.type = "button";
    button.textContent = label;

    button.dataset.listingId = listingId;
    button.dataset.action = action;

    button.addEventListener("click", handleCartAction);

    return button;
}

async function handleCartAction(event) {
    const button = event.currentTarget;

    const listingId = button.dataset.listingId;
    const action = button.dataset.action;

    let url = "/cart/" + listingId;
    let method = "POST";

    if (action === "add") {
        url += "/add";
    }
    else if (action === "decrease") {
        url += "/decrease";
    }
    else if (action === "remove") {
        method = "DELETE";
    }
    else {
        return;
    }

    button.disabled = true;

    try {
        const response = await fetch(url, { method: method });

        if (!response.ok) {
            throw new Error("The cart could not be updated.");
        }

        await loadCart();
    }
    catch (error) {
        document.getElementById("cart-status").textContent =
            "Could not confirm the cart update. " +
            "Refresh to check before retrying.";

        console.error(error);
    }
    finally {
        button.disabled = false;
    }
}

function addCell(row, value) {
    const cell = document.createElement("td");

    cell.textContent = value;

    row.appendChild(cell);
}

function formatMoney(value) {
    return "$" + value.toFixed(2);
}

function searchProducts(event) {
    event.preventDefault();

    loadProducts();
}

function clearSearch() {
    document.getElementById("search-input").value = "";
    document.getElementById("sort-order").value = "";
    document.getElementById("high-protein").checked = false;

    loadProducts();
}

document.getElementById("search-form")
    .addEventListener("submit", searchProducts);

document.getElementById("clear-search")
    .addEventListener("click", clearSearch);

loadProducts();
loadCart();