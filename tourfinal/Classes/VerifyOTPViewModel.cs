using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tourfinal.Models;
namespace tourfinal.Classes
{
    public class VerifyOTPViewModel
    {
        public int UserId { get; set; }
        public string OTP { get; set; }
    }
}