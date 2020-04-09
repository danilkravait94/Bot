using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloryBot
{
    public class User
    {
        public long Id { get; set; }
        public string ChatId { get; set; }
        public string Name { get; set; }
        public bool Male { get; set; }
        public int Age { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public double Activity { get; set; }
        public int Calories { get; set; }
        public int Proteins { get; set; }
        public int Fats { get; set; }
        public int Carbohydrates { get; set; }
        public string Goal { get; set; }
        public string Language;
        public int DayProteins;
        public int DayFats;
        public int DayCarbohydrates;
        public int DayCalories;
        public string Command;
        public double BodyMassIndex;

    }
}