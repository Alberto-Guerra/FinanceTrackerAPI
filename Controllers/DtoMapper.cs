using FinanceTrackerAPI.Data;
using FinanceTrackerAPI.DTOs;
using FinanceTrackerAPI.Model;

namespace FinanceTrackerAPI.Controllers
{
    public static class DtoMapper

        
    {
      
        internal static List<CategoryDTO> ToCategoryDtoList(List<Category> categories)
        {
            //generate method

            List<CategoryDTO> dtoCategories = new List<CategoryDTO>();

            foreach(Category category in categories)
            {
                dtoCategories.Add(ToCategoryDto(category));
            }

            return dtoCategories;
            
        }


        private static CategoryDTO ToCategoryDto(Category category)
        {
            var dto = new CategoryDTO();

            dto.Id = category.Id;
            dto.Name = category.Name;
            dto.Description = category.Description;
            dto.Transactions = new List<TransactionNoCategoriesDTO>();
            dto.Color = category.Color;
            dto.Budget = category.Budget;
            foreach(Transaction transaction in category.Transactions)
            {
                dto.Transactions.Add(ToTransactionNoCategoriesDto(transaction));
            }

            return dto;
            
        }

        private static TransactionNoCategoriesDTO ToTransactionNoCategoriesDto(Transaction transaction)
        {
            var dto = new TransactionNoCategoriesDTO();
            dto.Id = transaction.Id;
            dto.Name = transaction.Name;
            dto.Amount = transaction.Amount;
            dto.Date = transaction.Date;
            dto.Description = transaction.Description;

            return dto;
        }
    }
}
