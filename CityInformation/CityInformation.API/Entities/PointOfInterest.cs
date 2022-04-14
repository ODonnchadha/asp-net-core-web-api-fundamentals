﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInformation.API.Entities
{
    public class PointOfInterest
    {
        [Key()]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(), MaxLength(60)]
        public string Name { get; private set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        [ForeignKey("CityId")]
        public City? City { get; set; }
        public int CityId { get; set; }
        public PointOfInterest(string name) => Name = name;
    }
}