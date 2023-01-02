using System.Text.Json;

namespace BikeService.Data;
// IF YOU WANT EACH USER SPECIFIC  DATA THEN PASS IN USER ID AS PARAMETER  i.e. Guid userId
public static class InventoryService
{
    private static void SaveAll( List<Inventory> items) //Guid userId,
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
    public static List<Inventory> GetAll()//Guid userId
    {
        string itemsFilePath = Utils.GetItemsFilePath();
        if (!File.Exists(itemsFilePath))
        {
            return new List<Inventory>();
        }
        var json = File.ReadAllText(itemsFilePath);

        return JsonSerializer.Deserialize<List<Inventory>>(json);
    }
    public static List<Inventory> Create(Guid userId, string ItemName, int Quantity) 
    {
        List<Inventory> items = GetAll();//userId
        items.Add(new Inventory
        {
            ItemName=ItemName,
            Quantity=Quantity,  
           
        });
        SaveAll(items);
        return items;
    }
    
    public static List<Inventory> Delete( Guid id)
    {
        List<Inventory> items = GetAll();
        Inventory item = items.FirstOrDefault(x => x.Id == id);

        if (item == null)
        {
            throw new Exception("Stock not found.");
        }

        items.Remove(item);
        SaveAll(items);
        return items;
    }
       public static List<Inventory> Update(Guid id, string ItemName, int quantitytaken, bool isApproved , string TakenBy) //Guid userId,bool isApproved
    {
        List<Inventory> items = GetAll();
        Inventory itemToUpdate = items.FirstOrDefault(x => x.Id == id);

        if (itemToUpdate == null)
        {
            throw new Exception("Stock not found.");
        }
        if (itemToUpdate.Quantity < quantitytaken)
        {
            throw new Exception("cannot take more than the stock quantity choose a less number");
        }

        itemToUpdate.ItemName = ItemName;
        itemToUpdate.Quantity -= quantitytaken;
        itemToUpdate.LastTaken = DateTime.Now;
        itemToUpdate.IsApproved = isApproved;
        itemToUpdate.TakenBy = TakenBy;
        itemToUpdate.ApprovedBy = userId;
        SaveAll(items);
        return items;
    }
}
