using FinanceTrackerAPI.Data;

namespace FinanceTrackerAPI.DTOs
{
    public class CategoryDTO
    {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public string Description { get; set; } = "";
            public string Color { get; set; } = "transparent";
            public int Budget { get; set; } = 0;

        public List<TransactionNoCategoriesDTO> Transactions { get; set; }

            
        
    }
}
