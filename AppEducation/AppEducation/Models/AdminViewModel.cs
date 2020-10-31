using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppEducation.Models.Users;
namespace AppEducation.Models
{
  public class TotalInformation {
      public List<Room> Rooms {get; set;}
    
      public IEnumerable<AppUser> Users {get; set;}
  }
}
