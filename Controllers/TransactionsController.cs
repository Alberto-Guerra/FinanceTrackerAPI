using FinanceTrackerAPI.Data;
using FinanceTrackerAPI.Model;
using FinanceTrackerAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using FinanceTrackerAPI.Helper;

namespace FinanceTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;


        //create constructor
        public TransactionsController(DataContext context, IConfiguration configuration)

        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<List<TransactionDTO>>> GetTransactions()
        {
            var id = checkToken();

            if(id == null)
            {
                return Unauthorized();
            }

            return Ok(await PrivateGetTransactions(id));
        }

        private async Task<List<Transaction>> PrivateGetTransactions(string id)
        {
            return await _context.Transactions.Include(t => t.Categories).Where(t => t.UserId == int.Parse(id)).ToListAsync();
        }

        

        [HttpGet("{categoryID}")]
        public async Task<ActionResult<List<TransactionDTO>>> GetTransactionsByCategory(int categoryID)
        {
            var id = checkToken();

            if (id == null)
            {
                return Unauthorized();
            }

            var dbCategory = await _context.Categories.FindAsync(categoryID);

            if (dbCategory == null)
            {
                return BadRequest("Category not found");
            }
     

            return Ok(await _context.Transactions.Where(t => t.Categories.Contains(dbCategory)).Where(t => t.UserId == int.Parse(id)).ToListAsync());
        }


        [HttpPost]
        public async Task<ActionResult<List<TransactionDTO>>> CreateTransaction(TransactionDTO transaction)
        {
            var id = checkToken();

            if(id == null)
            {
                return Unauthorized();
            }

            var dbTransaction = dtoToTransaction(transaction, id);

            _context.Transactions.Add(dbTransaction);
            await _context.SaveChangesAsync();

            return Ok(await PrivateGetTransactions(id));
        }

        private Transaction dtoToTransaction(TransactionDTO transaction, string userID)
        {
            
            var dbTransaction = new Transaction(transaction);

            dbTransaction.UserId = int.Parse(userID);

            foreach (var category in transaction.Categories)
            {
                var dbCategory = _context.Categories.Find(category.Id);
                if(dbCategory != null)
                    dbTransaction.Categories.Add(dbCategory);
            }

            return dbTransaction;
        }

        [HttpPut]
        public async Task<ActionResult<List<TransactionDTO>>> UpdateTransaction(TransactionDTO transaction)
        {
            var id = checkToken();

            if (id == null)
            {
                return Unauthorized();
            }

            var dbTransaction = await _context.Transactions.Include(t => t.Categories).Where(t => t.UserId == int.Parse(id)).FirstOrDefaultAsync(t => t.Id == transaction.Id);


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
                var dbCategory = _context.Categories.Find(category.Id);
                if (dbCategory != null) { 
                    dbTransaction.Categories.Add(dbCategory);
                    if(!dbCategory.Transactions.Contains(dbTransaction))
                        dbCategory.Transactions.Add(dbTransaction);
                }
            }

            await _context.SaveChangesAsync();

            return Ok(await PrivateGetTransactions(id));
        }

        [HttpPut("addCategory/{transactionId}")]
        public async Task<ActionResult<List<TransactionDTO>>> AddCategoryToTransaction(int transactionId, CategoryNoTransactionDTO category)
        {

            var id = checkToken();

            if (id == null)
            {
                return Unauthorized();
            }


            var dbTransaction = await _context.Transactions.Include(t => t.Categories).Where(t => t.UserId == int.Parse(id)).FirstOrDefaultAsync(t => t.Id == transactionId);

            if (dbTransaction == null)
            {
                return BadRequest("Transaction not found");
            }
      

            var dbCategory = await _context.Categories.Include(c => c.Transactions).Where(t => t.UserId == int.Parse(id)).FirstOrDefaultAsync(c => c.Id == category.Id);

            if (dbCategory == null)
            {
                return BadRequest("Category not found");
            }

            dbTransaction.Categories.Add(dbCategory);
            dbCategory.Transactions.Add(dbTransaction);

            await _context.SaveChangesAsync();

            return Ok(await PrivateGetTransactions(id));
        }


        [HttpPut("removeCategory/{transactionId}")]
        public async Task<ActionResult<List<TransactionDTO>>> RemoveCategoryFromTransaction(int transactionId, CategoryNoTransactionDTO category)
        {
            var id = checkToken();

            if (id == null)
            {
                return Unauthorized();
            }

            var dbTransaction = await _context.Transactions.Include(t => t.Categories).Where(t => t.UserId == int.Parse(id)).FirstOrDefaultAsync(t => t.Id == transactionId);

            if (dbTransaction == null)
            {
                return BadRequest("Transaction not found");
            }

            var dbCategory = await _context.Categories.Include(c => c.Transactions).Where(t => t.UserId == int.Parse(id)).FirstOrDefaultAsync(c => c.Id == category.Id);

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

            return Ok(await PrivateGetTransactions(id));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<TransactionDTO>>> DeleteTransaction(int id)
        {
            var userId = checkToken();

            if (userId == null)
            {
                return Unauthorized();
            }

            var dbTransaction = await _context.Transactions.Where(t => t.UserId == int.Parse(userId)).FirstOrDefaultAsync(c => c.Id == id);

            if (dbTransaction == null)
            {
                return BadRequest("Transaction not found");
            }

            _context.Transactions.Remove(dbTransaction);
            await _context.SaveChangesAsync();

            return Ok(await PrivateGetTransactions(userId));
        }


        private string checkToken()
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            //Console.WriteLine(token);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false, // You might want to set these to your actual issuer details
                ValidateAudience = false, // You might want to set these to your actual audience details
                ClockSkew = TimeSpan.Zero // Adjust as needed
            };
            ClaimsPrincipal claimsPrincipal;
            try
            {
                SecurityToken validatedToken;
                claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch (Exception ex)
            {
                return null;
            }


            // Now you can access the claims, including the NameIdentifier
            return claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
        }


    }
}
