using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tekeg
{
    public class User
    {
        public long Id { get; set; }
        public string ChatId { get; set; }
        public string Name { get; set; }
        public int DayCalories { get; set; }
        public bool Male { get; set; }
        public int Age { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public double Activity { get; set; }
        public int Calories { get; set; }
        public string Goal { get; set; }
        public string Language { get; set; }
        public string Command { get; set; }
        public double BodyMassIndex { get; set; }

    }
}