using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirusAppFinal.Models;

public class CountriesEdit {
    public int Id { get; set; }
    public string? CountryName { get; set; }

    public List<int> VariantsIds { get; set; } = new List<int>();
}
