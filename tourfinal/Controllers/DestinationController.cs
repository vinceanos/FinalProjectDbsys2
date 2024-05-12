using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tourfinal.Models;
namespace tourfinal.Controllers
{
    public class DestinationController : Controller
    {
        public readonly TsuperVansEntities _context;

        public DestinationController()
        {
            _context = new TsuperVansEntities();
        }

        public DbSet<DestinationDetail> DestinationDetails { get; set; }
        public DbSet<Booking1> Bookings { get; set; }
       
       

    }
}

