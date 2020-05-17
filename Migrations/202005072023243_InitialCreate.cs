namespace Calories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ChatId = c.String(),
                        Name = c.String(),
                        Male = c.Boolean(nullable: false),
                        Age = c.Int(nullable: false),
                        Weight = c.Single(nullable: false),
                        Height = c.Single(nullable: false),
                        Activity = c.Double(nullable: false),
                        Calories = c.Int(nullable: false),
                        Proteins = c.Int(nullable: false),
                        Fats = c.Int(nullable: false),
                        Carbohydrates = c.Int(nullable: false),
                        Goal = c.String(),
                        Language = c.String(),
                        DayProteins = c.Int(nullable: false),
                        DayFats = c.Int(nullable: false),
                        DayCarbohydrates = c.Int(nullable: false),
                        DayCalories = c.Int(nullable: false),
                        Command = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
