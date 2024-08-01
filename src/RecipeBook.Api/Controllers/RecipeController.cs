using Microsoft.AspNetCore.Mvc;
using RecipeBook.ServiceLibrary.Entities;
using RecipeBook.ServiceLibrary.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeBook.ServiceLibrary.Repository;
using System.Transactions;
using Microsoft.AspNetCore.Cors;

namespace RecipeBook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        [EnableCors]
        [HttpGet("{recipeId}")] //api/recipe/{recipe_id}
        public async Task<IActionResult> GetOneRecipeAsync([FromRoute]Guid recipeId)
        {


            Recipe recipe = new Recipe();

            RecipeEntity recipeEntity = await recipe.GetRecipeByIdAsync(recipeId);


            if (recipeEntity == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(recipeEntity);
            }

        }

        [EnableCors]
        [HttpGet] //api/recipe?pagesize=10&pagenumber=1
        public async Task<IActionResult> GetListAsync([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 0)
        {

            if (pageSize <= 0 || pageNumber < 0)
            {
                return BadRequest();
            }


            Recipe recipe = new Recipe();

            IEnumerable<RecipeEntity> recipes = await recipe.GetRecipesAsync(pageSize, pageNumber);

            if (recipes == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(recipes);
            }

        }

        [EnableCors]
        [HttpPost]
        public async Task<IActionResult> PostNewRecipeAsync([FromBody] RecipeEntity recipeEntity)
        {

            Recipe recipe = new Recipe();

            int rowsAffected = await recipe.CreateRecipeAsync(recipeEntity);


            if (rowsAffected > 0)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }

        }

        [EnableCors]
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] RecipeEntity recipeEnitity)
        {

            Recipe recipe = new Recipe();

            int rowsAffected = await recipe.UpdateRecipeAsync(recipeEnitity);


            if (rowsAffected > 0)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
        }

        [EnableCors]
        [HttpDelete("{recipeId}")]
        public async Task<IActionResult> DeleteAsync(Guid recipeId)
        {
            Recipe recipe = new Recipe();

            int rowsAffected = await recipe.DeleteRecipeAsync(recipeId);

            if (rowsAffected > 0)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }

        }

    }

    //get one recipe

    //get many recipes

    //add new recipe

    //update recipe

    //delete recipe

}
