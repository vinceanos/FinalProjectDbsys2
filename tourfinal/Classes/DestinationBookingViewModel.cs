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
        public Booking1 Booking { get; set; }
        public DestinationDetail ImageURL { get; set; }
        public DestinationDetail Descriptipn { get; set; }
        public DestinationDetail DestinationID { get; set;}
        public DestinationDetail DestinationName { get; set; }
        public DestinationDetail Location { get; set; }
        public Booking1 BookingID { get; set; }
        

      
        public DateTime Booking_Date { get; set; }
        public string Booking_Number { get; set; }
        public int Number_of_Travellers { get; set; }
        public string Type_of_Booking { get; set; }
        public string Package_Booked { get; set; }
        public decimal Price { get; set; }
    }
}

    
