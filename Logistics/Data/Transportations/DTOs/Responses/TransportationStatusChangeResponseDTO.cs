using Logistics.Data.Transportations.Models;
using Logistics.Services.Utils;
using System;

namespace Logistics.Data.Transportations.DTOs.Responses
{
    public class TransportationStatusChangeResponseDTO
    {
        public TransportationStatus status { get; set; }

        public string time { get; set; }

        public TransportationStatusChangeResponseDTO(TransportationStatusChange statusChange)
        {
            status = statusChange.status;
            time = statusChange.time.ToString("HH:mm, dd MMMM yyyy", new System.Globalization.CultureInfo("ru-RU"));
        }
    }

}
