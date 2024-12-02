using Logistics.Data.Account.Models;
using Logistics.Data.Requests.Models;
using Logistics.Data.Transportations.Models;
using System;

namespace Logistics.Services.Utils
{
    public class EnumToStringMapper
    {
        private static Dictionary<TransportationStatus, string> transportationStatusMapper = new Dictionary<TransportationStatus, string>
        {
            { TransportationStatus.WaitingForStart, "Ожидает старта" },
            { TransportationStatus.OnWayToLoading, "Активно (В пути до места загрузки)" },
            { TransportationStatus.Loading, "Активно (Загрузка)" },
            { TransportationStatus.OnWayToUnloading, "Активно (В пути)" },
            { TransportationStatus.Repairing, "Активно (Поломка ТС)" },
            { TransportationStatus.Unloading, "Активно (Выгрузка)" }
        };

        private static Dictionary<RequestStatus, string> requestStatusMapper = new Dictionary<RequestStatus, string>
        {
            { RequestStatus.Active, "Активно" },
            { RequestStatus.Delayed, "Отложено" },
            { RequestStatus.Accepted, "Принято" },
            { RequestStatus.ArchivedTransportationFinished, "Архив (Перевозка завершена)" },
            { RequestStatus.ArchivedNotAccepted, "Архив (Заявку не приняли)" }
        };

        private static Dictionary<ShipmentType, string> shipmentTypeMapper = new Dictionary<ShipmentType, string>
        {
            { ShipmentType.Perishable, "Скоропортящийся" },
            { ShipmentType.Bulk, "Насыпной" },
            { ShipmentType.Piece, "Штучный" },
            { ShipmentType.Oversized, "Негабаритный" },
            { ShipmentType.Gaseous, "Газообразный" },
            { ShipmentType.Dusty, "Пылевидный" },
            { ShipmentType.Liquid, "Жидкий" },
            { ShipmentType.Dangerous, "Опасный" },
        };

        private static Dictionary<TruckType, string> truckTypeMapper = new Dictionary<TruckType, string>
        {
            { TruckType.Tented, "Тентованный" }
        };

        public static string map(TransportationStatus status)
        {
            return transportationStatusMapper[status];
        }

        public static string map(RequestStatus status)
        {
            return requestStatusMapper[status];
        }

        public static string map(ShipmentType type)
        {
            return shipmentTypeMapper[type];
        }

        public static string map(TruckType type)
        {
            return truckTypeMapper[type];
        }
    }
}
