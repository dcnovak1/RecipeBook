using RecipeBook.ServiceLibrary.Entities;
using RecipeBook.ServiceLibrary.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RecipeBook.ServiceLibrary.Domains
{
    public class Recipe
    {

        public async Task<RecipeEntity> GetRecipeByIdAsync(Guid recipeId)
        {
            var recipeRepository = new RecipeRepository(new IngredientsRepository(), new InstructionsRepository());

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                RecipeEntity recipeEntity = await recipeRepository.GetByIdAsync(recipeId);
                return recipeEntity;

            }
        }

        public async Task<IEnumerable<RecipeEntity>> GetRecipesAsync(int pageSize, int pageNumber)
        {
            var recipeRepository = new RecipeRepository(new IngredientsRepository(), new InstructionsRepository());

            IEnumerable<RecipeEntity> recipeEntity;


            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                recipeEntity = await recipeRepository.GetAsync(pageSize, pageNumber);

            }

            return recipeEntity;
        }

        public async Task<int> UpdateRecipeAsync(RecipeEntity recipeEntity)
        {
            var recipeRepository = new RecipeRepository(new IngredientsRepository(), new InstructionsRepository());

            var rowsAffected = 0;

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                rowsAffected += await recipeRepository.UpdateAsync(recipeEntity);

                if (rowsAffected > 0)
                {
                    scope.Complete();
                }

            }

            

            return rowsAffected;
        }

        public async Task<int> CreateRecipeAsync(RecipeEntity recipeEntity)
        {
            var recipeRepository = new RecipeRepository(new IngredientsRepository(), new InstructionsRepository());

            var rowsAffected = 0;

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                rowsAffected += await recipeRepository.InsertAsync(recipeEntity);

                if (rowsAffected > 0)
                {
                    scope.Complete();
                }

            }

            return rowsAffected;
        }

        public async Task<int> DeleteRecipeAsync(Guid recipeId)
        {

            var recipeRepository = new RecipeRepository(new IngredientsRepository(), new InstructionsRepository());

            var rowsAffected = 0;

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                rowsAffected = await recipeRepository.DeleteAsync(recipeId);

                if (rowsAffected > 0)
                {
                    scope.Complete();
                }

            }

            return rowsAffected;


        }
    }
}
