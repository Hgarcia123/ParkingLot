using System;
using System.Collections.Generic;
using System.Text;

using Domain;


namespace Application.DtoModels
{
    public class EntryDto : ModelAbstract
    {
        public DateTimeOffset? Arrival { get; set; }
        public DateTimeOffset? Departure { get; set; }
        public float TotalPay { get; set; }
        public float PricePerHour { get; set; }
    }
}
