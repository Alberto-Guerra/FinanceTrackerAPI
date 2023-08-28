using FinanceTrackerAPI.Controllers;
using FinanceTrackerAPI.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTrackerAPI.Model
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string Name { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public List<Category> Categories { get; set; }
        [Column(TypeName = "VARCHAR")]
        public string Description { get; set; }

        public Transaction(TransactionDTO transaction)
        {
            Name = transaction.Name;
            Amount = transaction.Amount;
            Date = transaction.Date;
            Description = transaction.Description;
            Categories = new List<Category>();
 
        }

        public Transaction(int id, string name, double amount, DateTime date, List<Category> categories, string description)
        {
            Id = id;
            Name = name;
            Amount = amount;
            Date = date;
            Categories = categories;
            Description = description;
        }

        public Transaction()
        {

        }
    }
}
