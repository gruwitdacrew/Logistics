﻿using Logistics.Data.Common;
using Logistics.Data.Requests.Models;
using System.ComponentModel.DataAnnotations;

namespace Logistics.Data.Requests.DTOs.Requests
{
    public class ShipmentDTO
    {
        public ShipmentType type { get; set; }

        public float lengthInMeters { get; set; }

        public float widthInMeters { get; set; }

        public float heightInMeters { get; set; }

        [Annotations.WeightInTons]
        public float weightInTons { get; set; }
    }
}
