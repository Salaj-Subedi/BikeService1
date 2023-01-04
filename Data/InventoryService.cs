using System.Text.Json;

namespace BikeService.Data;

public static class InventoryService
{
    //method to save inventory values 
    private static void SaveAll( List<Inventory> items) 
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string itemsFilePath = Utils.GetItemsFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }
        
        var json = JsonSerializer.Serialize(items);
        File.WriteAllText(itemsFilePath, json);
    }
    //method to get all inventory 
    public static List<Inventory> GetAll()
    {
        string itemsFilePath = Utils.GetItemsFilePath();
        if (!File.Exists(itemsFilePath))
        {
            return new List<Inventory>();
        }
        var json = File.ReadAllText(itemsFilePath);

        return JsonSerializer.Deserialize<List<Inventory>>(json);
    }
    public static List<Inventory> Create( string ItemName, int Quantity) 
    {
        List<Inventory> items = GetAll();
        items.Add(
            new Inventory{
            ItemName=ItemName,
            Quantity=Quantity, 
            LastTaken=null,
           
        });
        SaveAll(items);
        return items;
    }

    //method to withdraw items from the stock 
    public static List<Inventory> Delete(string Username , string ItemName, int quantitytaken, string TakenBy , bool IsApproved)
    {  
        List<Inventory> items = GetAll();
        Inventory itemToRemove = items.FirstOrDefault(x => x.ItemName == ItemName);
        if (itemToRemove == null)
        {
            throw new Exception("Stock not found.");
        }
        if (itemToRemove.Quantity < quantitytaken)
        {
            throw new Exception("cannot take more than the stock quantity choose a less number");
        }
        itemToRemove.Quantity -= quantitytaken;
        itemToRemove.TotalTaken += quantitytaken;
        itemToRemove.TakenQuantity = quantitytaken;
        itemToRemove.LastTaken = DateTime.Now;
        itemToRemove.TakenBy = TakenBy;
        itemToRemove.ApprovedBy = Username;
        /*items.Remove(item);*/
        SaveAll(items);
        return items;
    }

    //method to update item quantity or name in the stock 
    public static List<Inventory> Update( string ItemName, int Quantity) 
    {
        List<Inventory> items = GetAll();
        Inventory itemToUpdate = items.FirstOrDefault(x => x.ItemName == ItemName);
        if (itemToUpdate == null)
        {
            throw new Exception("Stock not found.");
        }
        itemToUpdate.ItemName = ItemName;
        itemToUpdate.Quantity   = Quantity;
        SaveAll(items);
        return items;
    }

    //method to force remove an item from the list of inventories
    public static List<Inventory> Remove(string ItemName)
    {
        List<Inventory> items = GetAll();
        Inventory item = items.FirstOrDefault(x => x.ItemName == ItemName);

        if (item == null)
        {
            throw new Exception("Item not found.");
        }

        items.Remove(item);
        SaveAll(items);
        return items;
    }
}
