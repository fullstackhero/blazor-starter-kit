using System.ComponentModel.DataAnnotations;

namespace CleanBlazor.Client.Models;

public class AddEditBrandModel
{
    public int Id { get; set; }

    [Required] public string Name { get; set; }

    [Required] public string Description { get; set; }

    [Required] public decimal Tax { get; set; }
}
