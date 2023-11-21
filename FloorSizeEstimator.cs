using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankHelper
{
    internal static class FloorSizeEstimator
    {
        public static FloorSizeEstimates Estimate(Room room)
        {
            var estimate = new FloorSizeEstimates();
            return Estimate(room, estimate);
        }

        private static FloorSizeEstimates Estimate(Room room, FloorSizeEstimates estimate)
        {
            switch (room.path.Count)
            {
                case 1:
                    estimate.floor1 += room.area;
                    break;
                case 2:
                    estimate.floor2 += room.area;
                    break;
                case 3:
                    estimate.floor3 += room.area;
                    break;
                case 4:
                    estimate.floor4 += room.area;
                    break;
                case 5:
                    estimate.floor5 += room.area;
                    break;
            }
            
            foreach (var subroom in room.subrooms)
            {
                estimate = Estimate(subroom, estimate);
            }

            return estimate;
        }
    }
}
