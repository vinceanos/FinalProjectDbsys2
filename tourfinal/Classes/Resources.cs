using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tourfinal.Classes
{
    public enum ErrorCode
    {
        Success,
        Error
    }
    public enum Status
    {
        InActive,
        Active
    }

    public enum RoleType
    {
        Admin,
        Customer
    }
    public class Resources
    {
        public static String gUid
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }
        // Return random number for OTP
        public static int code
        {
            get
            {
                Random r = new Random();
                return r.Next(100000, 999999);
            }
        }
    }
}