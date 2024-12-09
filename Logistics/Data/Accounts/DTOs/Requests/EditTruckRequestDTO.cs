﻿using Logistics.Data.Account.Models;
using Logistics.Data.Common;
using static Logistics.Data.Account.Models.Truck;

namespace Logistics.Data.Accounts.DTOs.Requests
{
    public class EditTruckRequestDTO
    {
        public TruckBrand truckBrand { get; set; }

        public string? model { get; set; }

        public TruckType? truckType { get; set; }

        public int? loadCapacityInTons { get; set; }

        public int? yearOfProduction { get; set; }

        [Annotations.CarNumber]
        public string? number { get; set; }

        public int? regionCode { get; set; }

        public float? lengthInMeters { get; set; }

        public float? widthInMeters { get; set; }

        public float? heightInMeters { get; set; }
    }
}
