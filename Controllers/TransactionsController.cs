using FinanceTrackerAPI.Data;
using FinanceTrackerAPI.Model;
using FinanceTrackerAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace FinanceTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly DataContext _context;


        //create constructor
        public TransactionsController(DataContext context)

        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<TransactionDTO>>> GetTransactions()
        {
            return Ok(await _context.transactions.Include(t => t.Categories).ToListAsync());
        }


        [HttpGet("{categoryID}")]
        public async Task<ActionResult<List<TransactionDTO>>> GetTransactionsByCategory(int categoryID)
        {
            var dbCategory = await _context.categories.FindAsync(categoryID);

            if (dbCategory == null)
            {
                return BadRequest("Category not found");
            }
     

            return Ok(await _context.transactions.Where(t => t.Categories.Contains(dbCategory)).ToListAsync());
        }


        [HttpPost]
        public async Task<ActionResult<List<TransactionDTO>>> CreateTransaction(TransactionDTO transaction)
        {
            var dbTransaction = dtoToTransaction(transaction);


            _context.transactions.Add(dbTransaction);
            await _context.SaveChangesAsync();

            return Ok(await _context.transactions.Include(t => t.Categories).ToListAsync());
        }

        private Transaction dtoToTransaction(TransactionDTO transaction)
        {
            var dbTransaction = new Transaction(transaction);

            foreach (var category in transaction.Categories)
            {
                var dbCategory = _context.categories.Find(category.Id);
                if(dbCategory != null)
                    dbTransaction.Categories.Add(dbCategory);
            }

            return dbTransaction;
        }

        [HttpPut]
        public async Task<ActionResult<List<TransactionDTO>>> UpdateTransaction(TransactionDTO transaction)
        {
            var dbTransaction = await _context.transactions.Include(t => t.Categories).FirstOrDefaultAsync(t => t.Id == transaction.Id);


            if (dbTransaction == null)
            {
                return BadRequest("Transaction not found");
            }

            dbTransaction.Name = transaction.Name;
            dbTransaction.Amount = transaction.Amount;
            dbTransaction.Date = transaction.Date;
            dbTransaction.Description = transaction.Description;

            foreach (var category in dbTransaction.Categories)
            {
                if (category != null)
                {
                    if (category.Transactions.Contains(dbTransaction))
                        category.Transactions.Remove(dbTransaction);
                }
            }


            dbTransaction.Categories = new List<Category>();

            foreach (var category in transaction.Categories)
            {
                var dbCategory = _context.categories.Find(category.Id);
                if (dbCategory != null) { 
                    dbTransaction.Categories.Add(dbCategory);
                    if(!dbCategory.Transactions.Contains(dbTransaction))
                        dbCategory.Transactions.Add(dbTransaction);
                }
            }

            await _context.SaveChangesAsync();

            return Ok(await _context.transactions.Include(t => t.Categories).ToListAsync());
        }

        [HttpPut("addCategory/{transactionId}")]
        public async Task<ActionResult<List<TransactionDTO>>> AddCategoryToTransaction(int transactionId, CategoryNoTransactionDTO category)
        {
        
            var dbTransaction = await _context.transactions.Include(t => t.Categories).FirstOrDefaultAsync(t => t.Id == transactionId);

            if (dbTransaction == null)
            {
                return BadRequest("Transaction not found");
            }
      

            var dbCategory = await _context.categories.Include(c => c.Transactions).FirstOrDefaultAsync(c => c.Id == category.Id);

            if (dbCategory == null)
            {
                return BadRequest("Category not found");
            }

            dbTransaction.Categories.Add(dbCategory);
            dbCategory.Transactions.Add(dbTransaction);

            await _context.SaveChangesAsync();

            return Ok(await _context.transactions.Include(t => t.Categories).ToListAsync());
        }


        [HttpPut("removeCategory/{transactionId}")]
        public async Task<ActionResult<List<TransactionDTO>>> RemoveCategoryFromTransaction(int transactionId, CategoryNoTransactionDTO category)
        {
            var dbTransaction = await _context.transactions.Include(t => t.Categories).FirstOrDefaultAsync(t => t.Id == transactionId);

            if (dbTransaction == null)
            {
                return BadRequest("Transaction not found");
            }

            var dbCategory = await _context.categories.Include(c => c.Transactions).FirstOrDefaultAsync(c => c.Id == category.Id);

            if (dbCategory == null)
            {
                return BadRequest("Category not found");
            }

            if (! dbTransaction.Categories.Contains(dbCategory))
            {
                return BadRequest("Transaction does not contain this category");
            }

            dbCategory.Transactions.Remove(dbTransaction);
            dbTransaction.Categories.Remove(dbCategory);
           

            await _context.SaveChangesAsync();

            return Ok(await _context.transactions.Include(t => t.Categories).ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<TransactionDTO>>> DeleteTransaction(int id)
        {
            var dbTransaction = await _context.transactions.FindAsync(id);

            if(dbTransaction == null)
            {
                return BadRequest("Transaction not found");
            }

            _context.transactions.Remove(dbTransaction);
            await _context.SaveChangesAsync();

            return Ok(await _context.transactions.Include(t => t.Categories).ToListAsync());
        }


    }
}
