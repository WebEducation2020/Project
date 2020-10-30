using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppEducation.Models
{
    public class Classes
    {
        public string ClassID { get; set; }
        public string ClassName { get; set; }
        public string Topic { get; set; }
    }
    public class Room
    {
        public Classes RoomIF { get; set; }
        public List<UserCall> UserCalls { get; set; }
    }
    public class UserCall
    {
        public string FullName { get; set; }
        public string ConnectionID { get; set; }
        public bool InCall { get; set; }
        public bool IsCaller { get; set; }
    }
}
