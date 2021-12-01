using System;
using System.Collections.Generic;
using System.Text;

using Domain;

namespace Application.DtoModels
{
    public class ParkSpotsDto
    {
        public virtual Spot Spot { get; set; }
        public bool Status { get; set; }
    }
}
