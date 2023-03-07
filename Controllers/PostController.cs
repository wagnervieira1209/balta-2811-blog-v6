using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogV6.Data;
using BlogV6.Models;
using BlogV6.ViewModels;
using BlogV6.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogV6.Controllers
{
    [ApiController]
    public class PostController : ControllerBase
    {
        [HttpGet("v1/post")]
        public async Task<IActionResult> Get(
            [FromServices] BlogDataContext context,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25
        )
        {
            try
            {
                /*var posts = context
                            .Posts
                            .AsNoTracking()
                            .Select(x => 
                            new {
                                x.Id,
                                x.Title
                            })
                            .ToListAsync();*/

                // Forçar erro de referência ciclica quando não tiver ReferenceHandler no startup
                /*var posts = await context
                            .Posts
                            .AsNoTracking()
                            .Include(x => x.Category)
                            .Include(x => x.Author)
                            .ToListAsync();*/

                var count = await context.Posts.AsNoTracking().CountAsync();

                var posts = await context
                            .Posts
                            .AsNoTracking()
                            .Include(x => x.Category)
                            .Include(x => x.Author)
                            .Select(x =>
                            new ListPostsViewModel
                            {
                                Id = x.Id,
                                Title = x.Title,
                                Slug = x.Slug,
                                LastUpdateDate = x.LastUpdateDate,
                                Category = x.Category.Name,
                                Author = $"{x.Author.Name} - {x.Author.Email}"
                            })
                            .Skip(page * pageSize)
                            .Take(pageSize)
                            .OrderByDescending(x => x.LastUpdateDate)
                            .ToListAsync();

                //ListPostsViewModel

                return Ok(new ResultViewModel<dynamic>(
                    new
                    {
                        total = count,
                        page,
                        pageSize,
                        posts
                    }));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("Erro X0A2 - Falha interna do servidor"));
            }
        }

        [HttpGet("v1/post/{id:int}")]
        public async Task<IActionResult> GetById(
            [FromServices] BlogDataContext context,
            [FromRoute] int id
        )
        {
            try
            {
                var post = await context
                            .Posts
                            .AsNoTracking()
                            .Include(x => x.Category)
                            .Include(x => x.Author)
                            .ThenInclude(x => x.Roles)
                            .FirstOrDefaultAsync(x => x.Id == id);

                if (post == null)
                    return NotFound("Post não encontrado");

                return Ok(new ResultViewModel<Post>(post));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("Erro X0A2 - Falha interna do servidor"));
            }
        }

        [HttpGet("v1/post/category/{category}")]
        public async Task<IActionResult> GetByCategory(
            [FromServices] BlogDataContext context,
            [FromRoute] string category,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25
        )
        {
            try
            {
                var count = await context.Posts.AsNoTracking().CountAsync();

                var posts = await context
                            .Posts
                            .AsNoTracking()
                            .Include(x => x.Category)
                            .Include(x => x.Author)
                            .Where(x => x.Category.Slug == category)
                            .Select(x =>
                            new ListPostsViewModel
                            {
                                Id = x.Id,
                                Title = x.Title,
                                Slug = x.Slug,
                                LastUpdateDate = x.LastUpdateDate,
                                Category = x.Category.Name,
                                Author = $"{x.Author.Name} - {x.Author.Email}"
                            })
                            .Skip(page * pageSize)
                            .Take(pageSize)
                            .OrderByDescending(x => x.LastUpdateDate)
                            .ToListAsync();

                //ListPostsViewModel

                return Ok(new ResultViewModel<dynamic>(
                    new
                    {
                        total = count,
                        page,
                        pageSize,
                        posts
                    }));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("Erro X0A2 - Falha interna do servidor"));
            }
        }
    }
}