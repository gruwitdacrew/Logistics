﻿using Logistics.Data.Account.AccountDTOs.Requests;
using Logistics.Data.Documents.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Logistics.Data.Account.Models
{
    public class Transporter : User
    {
        public string permanentResidence { get; set; }

        public Truck truck { get; set; }

        public Transporter() { }

        public Transporter(RegisterRequestDTO registerRequest) : base(registerRequest)
        {
            role = Role.Transporter;
        }
    }
}
