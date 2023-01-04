using System;
using System.ComponentModel.DataAnnotations;

namespace BikeService.Data;
//inventory class for all inventory attributes
public class Inventory
{
    public Guid Id { get; set; } = Guid.NewGuid();
   

    [Required(ErrorMessage = "Please provide the Item name.")]
    public string ItemName { get; set; }
    public bool IsApproved { get; set; }

    [Required(ErrorMessage = "Please provide the Quantity")]
    public int TakenQuantity { get; set; }
    public int TotalTaken { get; set; } 
    public int Quantity { get; set; }
    public string ApprovedBy { get; set; }
    public string TakenBy { get; set; }
    public DateTime? LastTaken { get; set; } = DateTime.Now;
}
   
