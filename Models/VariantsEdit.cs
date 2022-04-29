using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirusAppFinal.Models;

public class VariantsEdit
{
    public int Id { get; set; }
    public string? VariantName { get; set; }
    public string? VariantOrigin { get; set; }
    [DataType(DataType.Date)]
    public DateTime? VariantDateDiscovered { get; set; }
    public int? VirusId { get; set; }

    public List<int> CountriesIds { get; set; } = new List<int>();
    public List<int> SymptomsIds { get; set; } = new List<int>();
}
