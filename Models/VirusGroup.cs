using System;
using System.Collections.Generic;

namespace VirusAppFinal
{
    public partial class VirusGroup
    {
        public VirusGroup()
        {
            Viruses = new List<Virus>();
        }

        public int Id { get; set; }
        public string? GroupName { get; set; }
        public string? GroupInfo { get; set; }
        public DateTime? DateDiscovered { get; set; }

        public virtual ICollection<Virus> Viruses { get; set; } = new List<Virus>();
    }
}
