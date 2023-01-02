using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeService.Data
{
    public class TimeSlot {
    public bool CanWithdraw()
    {
        // Get the current date and time
        DateTime now = DateTime.Now;

        // Check if the current day is Monday to Friday
        if (now.DayOfWeek >= DayOfWeek.Monday && now.DayOfWeek <= DayOfWeek.Friday)
        {
            // Check if the current time is between 9 AM and 5 PM
            if (now.TimeOfDay.TotalHours >= 9 && now.TimeOfDay.TotalHours < 17)
            {
                                   return true;
             
            }
        }

        return false;
    } 
}

}
