protected void lstProducts_SelectedIndexChanged(object sender, EventArgs e)
{
    // Fetch the record from the application data based on the selected item...
    var product = findProduct(Convert.ToInt32(lstProducts.SelectedItem.Value));
    if (product == null)
    {
        lblError.Text = "No Product found to display!";
        return;
    }
    
    txtId.Text = product.Id.ToString();
    txtName.Text = product.Name;
    txtPrice.Text = product.Price.ToString();
    imgProduct.ImageUrl = product.Image;

    // Filling the Recent List...
    var recentList = Session["recentList"] as Queue<Product> ?? new Queue<Product>();

    // Check if the product already exists in the recent list
    if (recentList.Contains(product))
    {
        // Create a temporary list to hold the items
        var tempList = new Queue<Product>(recentList.Where(p => p.Id != product.Id));
        recentList = tempList; // Update recentList to exclude the existing product
    }

    // Add the new product to the front of the queue
    recentList.Enqueue(product);

    // Limit the size of the recent list to 5 items
    if (recentList.Count > 5)
    {
        recentList.Dequeue(); // Remove the oldest item
    }

    // Update the session with the new recent list
    Session["recentList"] = recentList;

    // Bind the recent list to the data table
    dtRecentList.DataSource = recentList.Reverse(); // Reverse to show most recent first
    dtRecentList.DataBind();
}
