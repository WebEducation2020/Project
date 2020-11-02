using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppEducation.Models.Users;
namespace AppEducation.Models
{
  public class JoinClassInfor {
      public List<Room> AvailableRooms {get; set;}
      public Classes NewClass {get; set;}
  }
}
