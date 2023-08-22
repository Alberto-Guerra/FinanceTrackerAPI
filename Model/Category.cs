using FinanceTrackerAPI.Data;
using System.Text.Json.Serialization;

namespace FinanceTrackerAPI.Model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";

        [JsonIgnore]
        public List<Transaction> Transactions { get; set; }

        public Category(CategoryNoTransactionDTO category)
        {
            Name = category.Name;
            Description = category.Description;
            Transactions = new List<Transaction>();
        }

        public Category(string name, string description)
        {
            Name = name;
            Description = description;
            Transactions = new List<Transaction>(); ;
        }
    }   
}
