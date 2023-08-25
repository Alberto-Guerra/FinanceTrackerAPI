using FinanceTrackerAPI.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FinanceTrackerAPI.Model
{
    public class Category
    {
        public int Id { get; set; }
        [Column(TypeName = "VARCHAR")]
        public string Name { get; set; } = "";
        [Column(TypeName = "VARCHAR")]
        public string Description { get; set; } = "";
        [Column(TypeName = "VARCHAR")]
        public string Color { get; set; } = "transparent";
        public int Budget { get; set; } = 0;
        

        [JsonIgnore]
        public List<Transaction> Transactions { get; set; }

        public Category(CategoryNoTransactionDTO category)
        {
            Name = category.Name;
            Description = category.Description;
            Color = category.Color;
            Budget = category.Budget;
            Transactions = new List<Transaction>();
        }

        public Category(string name, string description, string color, int budget)
        {
            Name = name;
            Description = description;
            Transactions = new List<Transaction>();
            Color = color;
            Budget = budget;
        }
    }   
}
