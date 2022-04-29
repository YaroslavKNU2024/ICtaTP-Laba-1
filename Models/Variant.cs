using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VirusAppFinal
{
    public partial class Variant
    {
        public Variant()
        {
            //CountriesVariants = new HashSet<CountriesVariant>();
            //SymptomsVariants = new HashSet<SymptomsVariant>();
        }

        public int Id { get; set; }
        public string? VariantName { get; set; }
        public string? VariantOrigin { get; set; }
        [DataType(DataType.Date)]
        public DateTime? VariantDateDiscovered { get; set; }
        public int? VirusId { get; set; }

        public virtual Virus? Virus { get; set; }
        public virtual ICollection<Country> Countries { get; set; } = new List<Country>();
        public virtual ICollection<Symptom> Symptoms { get; set; } = new List<Symptom>();
    }
}
