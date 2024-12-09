using Logistics.Data.Account.Models;
using Logistics.Data.Requests.Models;
using System;
using System.ComponentModel;

namespace Logistics.Data.Transportations.Models
{
    public class Transportation
    {
        public Guid id { get; set; }

        public Guid requestId { get; set; }

        public Request request { get; set; }

        public Transporter transporter { get; set; }

        public TransportationStatus status { get; set; }

        public Transportation(){}

        public Transportation(Transporter transporter)
        {
            id = Guid.NewGuid();
            this.transporter = transporter;
            status = TransportationStatus.WaitingForStart;
        }
    }

    public enum TransportationStatus
    {
        WaitingForStart,
        OnWayToLoading,
        Loading,
        OnWayToUnloading,
        Repairing,
        Unloading,
        Finished
    }
}
