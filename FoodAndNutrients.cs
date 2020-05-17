namespace Calories
{
    public class Food
    {
        public string label { get; set; }
        public Nutrients nutrients { get; set; }
        public override string ToString()
        {
            return string.Format($"{label} - {nutrients.ENERC_KCAL:0.0}/" +
                $"{nutrients.PROCNT:0.0}/{nutrients.FAT:0.0}/{nutrients.CHOCDF:0.0}");
        }
    }
    public class Nutrients
    {
        public double ENERC_KCAL { get; set; }
        public double PROCNT { get; set; }
        public double FAT { get; set; }
        public double CHOCDF { get; set; }
    }
}
