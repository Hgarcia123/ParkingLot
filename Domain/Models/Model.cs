using System;
using System.Collections.Generic;

using System.Text;
using System.Text.Json.Serialization;
using MediatR;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Park : ModelAbstract
    {
        [Required]
        [MaxLength (50)]
        public string Name { get; set; }

        [Required]
        [MaxLength (50)]
        public string Location { get; set; }

        [Required]
        [Range(1, 999999, ErrorMessage = "A park must have more than 0 spots")]
        public int MaxSpots { get; set; }

        //List of Spots in Park
        [JsonIgnore]
        public virtual ICollection<ParkSpots> ListSpots{ get; set; }

    }

    public class ParkSpots : ModelAbstract
    {

        [JsonIgnore]
        public int SpotId { get; set; }

        [Required]
        public int ParkId { get; set; }

        //[JsonIgnore]
        public virtual Spot Spot { get; set; }

        [JsonIgnore]
        public virtual Park Parks { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "Error: Status of Spot is required")]
        public bool Status { get; set; }

    }

    public class Spot : ModelAbstract
    {
        public string SpotMark { get; set; }

        [JsonIgnore]
        public virtual ICollection<ParkSpots> ListParks { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<Entry> ListEntries { get; set; }

    }

    public class Entry : ModelAbstract
    {
        [Required]
        public int SpotId { get; set; }

        [Required]
        public string Plate { get; set; }

        public DateTimeOffset? Arrival { get; set; }

        public DateTimeOffset? Departure { get; set; }

        [JsonIgnore]
        public float TotalPay { get; set; }

        [Required]
        public float PricePerHour { get; set; }

        [JsonIgnore]
        public virtual Spot Spots { get; set; }

    }
}
