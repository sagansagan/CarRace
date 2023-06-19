using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRace
{
    public class Car
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Speed { get; set; }
        public double RaceTime { get; set; }
        public double DistanceLeft { get; set; }

        public double DistanceDriven { get; set; }
        public double Penalty { get; set; }
        public double TimeLeft { get; set; }

        public Car()
        {
            DistanceLeft = 10000;
            Speed = 120;
            DistanceDriven= 0;  
            RaceTime = 0;
            Penalty = 0;
            TimeLeft = DistanceLeft / (Speed / 3.6) + Penalty;

        }
    }
}
