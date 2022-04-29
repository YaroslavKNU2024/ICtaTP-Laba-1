using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VirusAppFinal
{
    public partial class Country
    {
        public Country()
        {
            //CountriesVariants = new HashSet<CountriesVariant>();
        }

        public int Id { get; set; }
        [Display(Name = "Назва")]
        [StringLength(255, ErrorMessage = "Занадто коротке або занадто довге.")]
        public string? CountryName { get; set; }

        public virtual ICollection<Variant> Variants { get; set; } = new List<Variant>();
    }
}
