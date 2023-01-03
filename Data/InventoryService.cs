using System.Text.Json;

namespace BikeService.Data;

public static class InventoryService
{
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
    public static List<Inventory> Delete(Guid userId , string ItemName, int quantitytaken, string TakenBy)
    {  // withdraw service
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
        itemToRemove.TakenQuantity = quantitytaken;
        itemToRemove.LastTaken = DateTime.Now;
        itemToRemove.TakenBy = TakenBy;
        itemToRemove.ApprovedBy = userId;
        /*items.Remove(item);*/
        SaveAll(items);
        return items;
    }
    public static List<Inventory> Update( string ItemName, int Quantity) 
    {
        // taken, bool isApproved , string TakenBy Guid userId, Guid id,bool isApproved
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
