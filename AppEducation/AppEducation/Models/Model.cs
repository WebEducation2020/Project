﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppEducation.Models
{
    public class _Class
    {
        public string ClassID { get; set; }
        public string ClassName { get; set; }
        public string Topic { get; set; }
    }
    public class Room
    {
        public _Class RoomIF { get; set; }
        public List<User> UserCall { get; set; }
    }
    public class User
    {
        public string UserName { get; set; }
        public string ConnectionID { get; set; }
        public bool InCall { get; set; }
        public bool IsCaller { get; set; }
    }
}
