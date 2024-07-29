using Microsoft.AspNetCore.Mvc;
using RecipeBook.ServiceLibrary.Entities;
using RecipeBook.ServiceLibrary.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeBook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        [HttpGet("{recipeId}")] //api/recipe/{recipe_id}
        public async Task<IActionResult> GetOneRecipeAsync([FromRoute]Guid recipeId)
        {
            return Ok(recipeId);
        }

        [HttpGet] //api/recipe?pagesize=10&pagenumber=1
        public async Task<IActionResult> GetListAsync([FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            return Ok(pageSize + " " + pageNumber);
        }

        [HttpPost]
        public async Task<IActionResult> PostNewRecipeAsync([FromBody] RecipeEntity recipeEntity)
        {
            return Ok(recipeEntity);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] RecipeEntity recipeEnitity)
        {
            return Ok(recipeEnitity);
        }

        [HttpDelete("{recipeId}")]
        public async Task<IActionResult> DeleteAsync(Guid recipeId)
        {
            return Ok(recipeId);
        }

    }

    //get one recipe

    //get many recipes

    //add new recipe

    //update recipe

    //delete recipe

}
