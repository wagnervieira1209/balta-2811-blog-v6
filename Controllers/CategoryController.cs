using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogV6.Data;
using BlogV6.Extensions;
using BlogV6.Models;
using BlogV6.ViewModels;
using BlogV6.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BlogV6.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync(
            [FromServices] IMemoryCache cache,
            [FromServices] BlogDataContext context
        )
        {
            var categories = await cache.GetOrCreate(
                key: "CategoriesCache",
                factory: entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                    return GetCategories(context);
                }
            );

            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

            try
            {
                //var categories = await context.Categories.ToListAsync(); // Comentando porque estamos usando do cache
                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch
            {
                return Ok(new ResultViewModel<List<Category>>("05EX05 - Falha interna no servidor"));
            }
        }

        // Método apenas para testes, pode ser usado direto no cache
        private async Task<List<Category>> GetCategories(BlogDataContext context)
        {
            return await context.Categories.ToListAsync();
        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromRoute] int id,
            [FromServices] BlogDataContext context
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                return Ok(new ResultViewModel<Category>(category));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna do servidor"));
            }
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync(
            [FromBody] EditorCategoryViewModel model,
            [FromServices] BlogDataContext context
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

            if (!ModelState.IsValid)
            {
                var errorsList = new List<string>();
                foreach (var item in ModelState.Values)
                {
                    errorsList.AddRange(item.Errors.Select(x => x.ErrorMessage));
                }

                return BadRequest(new ResultViewModel<Category>(errorsList));
            }

            try
            {
                var category = new Category()
                {
                    Name = model.Name,
                    Slug = model.Slug.ToLower()
                };

                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}", model);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>($"05EX09 - Não foi possível cadastrar uma categoria na base de dados. Message {ex.Message}"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("05EX10 - Falha interna no servidor"));
            }
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] EditorCategoryViewModel model,
            [FromServices] BlogDataContext context
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Categoria não encontrada"));

                category.Name = model.Name;
                category.Slug = model.Slug;

                context.Categories.Update(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>($"05EX08 - Não foi possível alterar uma categoria na base de dados. Message {ex.Message}"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("05EX11 - Falha interna no servidor"));
            }
        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] int id,
            [FromServices] BlogDataContext context
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Categoria não encontrada"));

                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>($"05EX07 - Não foi possível remover uma categoria na base de dados. Message {ex.Message}"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("05EX12 - Falha interna no servidor"));
            }
        }

    }
}