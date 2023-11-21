using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankHelper
{
    internal struct FloorSizeEstimates
    {
        public FloorSizeEstimates()
        {
        }

        public int floor1 { get; set; } = 0;
        public int floor2 { get; set; } = 0;
        public int floor3 { get; set; } = 0;
        public int floor4 { get; set; } = 0;
        public int floor5 { get; set; } = 0;

        public string GetReadout()
        {
            return $"Floor 1: {floor1} ({Math.Sqrt(floor1)}x{Math.Sqrt(floor1)})\nFloor 2: {floor2} ({Math.Sqrt(floor2)}x{Math.Sqrt(floor2)})\nFloor 3: {floor3} ({Math.Sqrt(floor3)}x{Math.Sqrt(floor3)})\nFloor 4: {floor4} ({Math.Sqrt(floor4)}x{Math.Sqrt(floor4)})\nFloor 5: {floor5} ({Math.Sqrt(floor5)}x{Math.Sqrt(floor5)})";
        }
    }
}
