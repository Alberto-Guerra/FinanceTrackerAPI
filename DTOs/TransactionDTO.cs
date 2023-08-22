namespace FinanceTrackerAPI.Data
{
    public class TransactionDTO    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }

        public List<CategoryNoTransactionDTO> Categories { get; set; }
        public string Description { get; set; }

    }
}
