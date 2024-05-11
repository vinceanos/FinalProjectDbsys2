using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tourfinal.Models;

namespace tourfinal
{
    public class DestinationBookingViewModel
    {
        public DestinationDetail Destination { get; set; }
        public Booking Booking { get; set; }
        public string UserName { get; set; } 
        public string Email { get; set; }
    }
}