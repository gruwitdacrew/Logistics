using Logistics.Data.Transportations.Models;
using Logistics.Services.Utils;
using System;

namespace Logistics.Data.Transportations.DTOs.Responses
{
    public class TransportationStatusChangeResponseDTO
    {
        public string status { get; set; }

        public string time { get; set; }

        public TransportationStatusChangeResponseDTO(TransportationStatusChange statusChange)
        {
            status = EnumToStringMapper.map(statusChange.status);
            time = statusChange.time.ToString("HH:mm, dd MMMM yyyy", new System.Globalization.CultureInfo("ru-RU"));
        }
    }

}
