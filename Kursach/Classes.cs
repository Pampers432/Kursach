using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursach
{
    public class Village
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Location")]
        public int Location_Id { get; set; }
        public string Name { get; set; }
        public string PopulationBefore { get; set; }
        public DateTime DateDestroyed { get; set; }
        public string Cause { get; set; }
        public bool MemorialExists { get; set; }
        public string? ImagePath { get; set; }
        public string Description { get; set; }
        public Village() { }
    }

    public class Location
    {
        [Key]
        public int Id { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public Location() { }
    }

    public class Monument
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Location")]
        public int Location_Id { get; set; }
        public string Name { get; set; }
        public DateTime InstallationDate { get; set; }
        public string Cause { get; set; }
        public string? ImagePath { get; set; }
        public string Description { get; set; }

        public Monument() { }
    }

    public class Ghetto
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Location")]
        public int Location_Id { get; set; }
        public string Name { get; set; }
        public DateTime EstablishedDate { get; set; }
        public DateTime LiquidationDate { get; set; }
        public int VictimsCount { get; set; }
        public int Population { get; set; }
        public string? ImagePath { get; set; }
        public string Description { get; set; }

        public Ghetto() { }
    }

    public class MassGrave
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Location")]
        public int Location_Id { get; set; }
        public string Name { get; set; }
        public DateTime InstallationDate { get; set; }
        public string Cause { get; set; }
        public int VictimsCount { get; set; }
        public string? ImagePath { get; set; }
        public string Description { get; set; }

        public MassGrave() { }
    }

    public class Victim
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("MassGrave")]
        public int MassGrave_Id { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime DeathDate { get; set; }
        public string? ImagePath { get; set; }
        public string Description { get; set; }

        public Victim() { }
    }
}
