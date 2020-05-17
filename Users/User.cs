using System.Collections.Generic;

namespace Calories
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

        public string Language { get; set; }
        public int DayProteins { get; set; }
        public int DayFats { get; set; }
        public int DayCarbohydrates { get; set; }
        public int DayCalories { get; set; }
        public string foodname = string.Empty;
        public double weightoffood = 0;
        public Food food;
        public List<Food> DayFoodList = new List<Food>();
        public string Command { get; set; }
        public bool edit = false;

        public double BodyMassIndex;
        public void MinusOfDay(Food food)
        {
            DayCalories -= (int)food.nutrients.ENERC_KCAL;
            DayFats -= (int)food.nutrients.FAT;
            DayProteins -= (int)food.nutrients.PROCNT;
            DayCarbohydrates -= (int)food.nutrients.CHOCDF;
        }
        public void PlusToDay(Food food)
        {
            DayCalories += (int)food.nutrients.ENERC_KCAL;
            DayFats += (int)food.nutrients.FAT;
            DayProteins += (int)food.nutrients.PROCNT;
            DayCarbohydrates += (int)food.nutrients.CHOCDF;
        }
        public void ToEmptyFood()
        {
            food = new Food();
            foodname = string.Empty;
            weightoffood = 0;
        }
        public void DeleteUser()
        {
            Name = string.Empty;
            Age = 0;
            Weight = 0;
            Height = 0;
            Activity = 0;
            Goal = string.Empty;
            Calories = 0;
            Proteins = 0;
            Fats = 0;
            Carbohydrates = 0;
            DayProteins = 0;
            DayFats = 0;
            DayCarbohydrates = 0;
            DayCalories = 0;
            Command = string.Empty;
        }
        public void SetCalories()
        {
            DayCalories = Calories;
            DayFats = Fats;
            DayProteins = Proteins;
            DayCarbohydrates = Carbohydrates;
        }
        public void Multiply(double weight)
        {
            food.nutrients.CHOCDF *= weight;
            food.nutrients.PROCNT *= weight;
            food.nutrients.FAT *= weight;
            food.nutrients.ENERC_KCAL *= weight;
        }
        public void UpdateUser()
        {
            DayProteins = Proteins;
            DayFats = Fats;
            DayCarbohydrates = Carbohydrates;
            DayCalories = Calories;
            DayFoodList = new List<Food>();
        }
    }
}
