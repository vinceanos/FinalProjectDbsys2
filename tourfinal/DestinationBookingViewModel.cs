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
        public tourfinal.Models.Booking1 Booking { get; set; }

    }
}