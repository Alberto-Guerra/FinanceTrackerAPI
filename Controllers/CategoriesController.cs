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

namespace FinanceTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext _context;

        public CategoriesController(DataContext context)
        {
            _context = context;
        }

        //create get post put and delete with same format as ExpenseItemsController
        [HttpGet]
        public async Task<ActionResult<List<CategoryDTO>>> GetCategories()
        {
            List<Category> categories = await _context.categories.Include(e => e.Transactions).ToListAsync();

            List<CategoryDTO> dtoCategories = DtoMapper.ToCategoryDtoList(categories);

            return dtoCategories;
        }




        [HttpPost]

        public async Task<ActionResult<List<CategoryDTO>>> PostCategory(CategoryNoTransactionDTO category)
        {

            var dbCategory = new Category(category.Name, category.Description, category.Color, category.Budget);

            _context.categories.Add(dbCategory);
            await _context.SaveChangesAsync();

            return Ok(await getDtoList());
        }

        [HttpPut]
        public async Task<ActionResult<List<CategoryDTO>>> UpdateCategory(CategoryNoTransactionDTO category)
        {
            var dbCategory = await _context.categories.FindAsync(category.Id);

            if (dbCategory == null)
            {
                return BadRequest("Category not found");
            }

            dbCategory.Name = category.Name;
            dbCategory.Description = category.Description;
            dbCategory.Color = category.Color;
            dbCategory.Budget = category.Budget;

            await _context.SaveChangesAsync();

            return Ok(await getDtoList());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<CategoryDTO>>> DeleteCategory(int id)
        {
            var dbCategory = await _context.categories.FindAsync(id);

            if (dbCategory == null)
            {
                return BadRequest("Category not found");
            }

            _context.categories.Remove(dbCategory);
            await _context.SaveChangesAsync();

            return Ok(await getDtoList());
        }


        private async Task<List<CategoryDTO>> getDtoList()
        {
            List<Category> categories = await _context.categories.Include(e => e.Transactions).ToListAsync();

            List<CategoryDTO> dtoCategories = DtoMapper.ToCategoryDtoList(categories);

            return dtoCategories;
        }
    }
}
