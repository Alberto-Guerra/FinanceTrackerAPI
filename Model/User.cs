namespace FinanceTrackerAPI.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = new byte[0];
        public byte[] PasswordSalt { get; set; } = new byte[0];

        List<Transaction> Transactions { get; set; } = new List<Transaction>();
        List<Category> Categories { get; set; } = new List<Category>();
    }
}
