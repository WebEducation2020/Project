using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppEducation.Models.Users;
namespace AppEducation.Models
{
  public class JoinClassInfor {
      public IEnumerable<Classes> AvailableClasses {get; set;}
      public Classes NewClass {get; set;}
      public int PageIndex { get;  set; }
      public int TotalPages { get; set; }
        public bool PreviousPage
        {
            get
            {
                return (PageIndex >= 1);
            }
        }
        public bool NextPage
        {
            get
            {
                return (PageIndex <= TotalPages);
            }
        }
    }

}
