using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinanceTrackerAPI.Data;
using FinanceTrackerAPI.Model;
using FinanceTrackerAPI.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinanceTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public CategoriesController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        //create get post put and delete with same format as ExpenseItemsController
        [HttpGet]
        public async Task<ActionResult<List<CategoryDTO>>> GetCategories()
        {
            var id = checkToken();

            if (id == null)
            {
                return Unauthorized();
            }

            List<Category> categories = await _context.Categories.Include(e => e.Transactions).Where(t => t.UserId == int.Parse(id)).ToListAsync();

            List<CategoryDTO> dtoCategories = DtoMapper.ToCategoryDtoList(categories);

            return dtoCategories;
        }




        [HttpPost]

        public async Task<ActionResult<List<CategoryDTO>>> PostCategory(CategoryNoTransactionDTO category)
        {
            var id = checkToken();

            if (id == null)
            {
                return Unauthorized();
            }

            var dbCategory = new Category(category.Name, category.Description, category.Color, category.Budget);
            dbCategory.UserId = int.Parse(id);

            _context.Categories.Add(dbCategory);
            await _context.SaveChangesAsync();

            return Ok(await getDtoList(id));
        }

        [HttpPut]
        public async Task<ActionResult<List<CategoryDTO>>> UpdateCategory(CategoryNoTransactionDTO category)
        {
            var id = checkToken();

            if (id == null)
            {
                return Unauthorized();
            }

            var dbCategory = await _context.Categories.Where(t => t.UserId == int.Parse(id)).FirstOrDefaultAsync(c => c.Id == category.Id);

            if (dbCategory == null)
            {
                return BadRequest("Category not found");
            }

            dbCategory.Name = category.Name;
            dbCategory.Description = category.Description;
            dbCategory.Color = category.Color;
            dbCategory.Budget = category.Budget;

            await _context.SaveChangesAsync();

            return Ok(await getDtoList(id));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<CategoryDTO>>> DeleteCategory(int id)
        {

            var userId = checkToken();

            if (userId == null)
            {
                return Unauthorized();
            }

            var dbCategory = await _context.Categories.FindAsync(id);

            if (dbCategory == null)
            {
                return BadRequest("Category not found");
            }

            _context.Categories.Remove(dbCategory);
            await _context.SaveChangesAsync();

            return Ok(await getDtoList(userId));
        }


        private async Task<List<CategoryDTO>> getDtoList(string id)
        {
            List<Category> categories = await _context.Categories.Include(e => e.Transactions).Where(t => t.UserId == int.Parse(id)).ToListAsync();

            List<CategoryDTO> dtoCategories = DtoMapper.ToCategoryDtoList(categories);

            return dtoCategories;
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
                Console.WriteLine(ex.Message);
                return null;
            }


            // Now you can access the claims, including the NameIdentifier
            return claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
