﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMqUtils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReservationProcessor
{
    public class ReservationListener : RabbitListener
    {

        private readonly ILogger<ReservationListener> Logger;
        private readonly ReservationsHttpService Service;

        public ReservationListener(ReservationsHttpService service, ILogger<ReservationListener> logger, IOptionsMonitor<RabbitOptions> options) : base(options)
        {

            Logger = logger;
            base.QueueName = "reservationQueue";
            base.ExchangeName = "";
            Service = service;

        }

        public override async Task<bool> Process(string message)
        {
            var request = JsonSerializer.Deserialize<ReservationRequest>(message);
            Logger.LogInformation($"Got a Reservation! {Environment.NewLine} \t {request.For}");
            return await Service.MarkReservationAccepted(request); // Task.FromResult(true);
        }
    }


    public class ReservationRequest
    {
        public int Id { get; set; }
        public string For { get; set; }
        public string Status { get; set; }
        public DateTime ReservationCreated { get; set; }
        public string[] books { get; set; }
    }

}