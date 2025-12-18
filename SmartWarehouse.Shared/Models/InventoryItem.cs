using System.ComponentModel.DataAnnotations;

namespace SmartWarehouse.Shared.Models;

[ValidatableType]
public class InventoryItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "Name is required")]
    public string? Name
    {
        get;
        set => field = value?.Trim();
    }

    [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000")]
    public int Quantity { get; set; }

    [Required] public string Sku { get; set; } = string.Empty;
}