using FinanceTrackerAPI.Model;

namespace FinanceTrackerAPI.DTOs
{
    public class TransactionNoCategoriesDTO
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

    }
}
