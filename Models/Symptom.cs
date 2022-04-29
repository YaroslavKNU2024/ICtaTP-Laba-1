using System;
using System.Collections.Generic;

namespace VirusAppFinal
{
    public partial class Symptom
    {
        public Symptom()
        {
            //SymptomsVariants = new List<SymptomsVariant>();
        }

        public int Id { get; set; }
        public string? SymptomName { get; set; }

        //public virtual ICollection<SymptomsVariant> SymptomsVariants { get; set; } = new HashSet<SymptomsVariant>();
        public virtual ICollection<Variant> Variants { get; set; } = new List<Variant>();
    }
}
