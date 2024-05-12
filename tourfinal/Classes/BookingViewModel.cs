using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using tourfinal.Models;

namespace tourfinal.Classes
{
    public class BookingViewModel
    {
       

        public int DestinationID { get; set; }
        public string DestinationName { get; set; }
        public string ImageURL { get; set; }
   
        [Required(ErrorMessage = "Please enter the booking date.")]
        public DateTime Booking_Date { get; set; }

        [Required(ErrorMessage = "Please enter the booking number.")]
        [StringLength(20, ErrorMessage = "The booking number must be at most 20 characters long.")]
        public string Booking_Number { get; set; }

        [Required(ErrorMessage = "Please enter the number of travelers.")]
        public int Number_of_Travellers { get; set; }

        [Required(ErrorMessage = "Please enter the type of booking.")]
        [StringLength(50, ErrorMessage = "The type of booking must be at most 50 characters long.")]
        public string Type_of_Booking { get; set; }

        [Required(ErrorMessage = "Please enter the package booked.")]
        [StringLength(100, ErrorMessage = "The package booked must be at most 100 characters long.")]
        public string Package_Booked { get; set; }

        [Required(ErrorMessage = "Please enter the price.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a valid price.")]


        public decimal Price { get; set; }
    }
}