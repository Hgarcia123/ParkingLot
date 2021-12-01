using System;
using System.Collections.Generic;
using System.Text;

using Domain;

namespace Application.DtoModels
{
    public class ParkDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; } 
        public int MaxSpots { get; set; }
    }
}
