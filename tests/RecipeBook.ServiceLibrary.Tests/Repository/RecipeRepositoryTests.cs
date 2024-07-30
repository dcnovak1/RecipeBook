using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using RecipeBook.ServiceLibrary.Entities;
using RecipeBook.ServiceLibrary.Repository;
using Xunit;

namespace RecipeBook.ServiceLibrary.Tests.Repository
{
    public class RecipeRepositoryTests
    {
        private bool _commitToDatabase = true;

        [Fact]
        public async Task GetOneAsync_Success()
        {
            var recipeRepository = new RecipeRepository(new IngredientsRepository(), new InstructionsRepository());

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                Guid recipeId = Guid.Parse("845598f5-7fee-4a96-aab3-f4d553fb6063");

                RecipeEntity recipeEntity = await recipeRepository.GetByIdAsync(recipeId);

                Assert.Equal(recipeId, recipeEntity.Id);
            }
        }

        public async Task GetManyAsync_Success()
        {

        }

        [Fact]
        public async Task InsertAsync_Success()
        {
            var recipeRepository = new RecipeRepository(new IngredientsRepository(), new InstructionsRepository());

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                Guid recipeId = Guid.Parse("845598f5-7fee-4a96-aab3-f4d553fb6063");

                var rowsAffected = await recipeRepository.InsertAsync(new RecipeEntity()
                {
                    Id = recipeId,
                    Title = "Chicken Parm Unit Test",
                    Description = "Chicken Parm Description",
                    Logo = null,
                    CreatedDate = DateTimeOffset.UtcNow,
                    Ingredients = new List<IngredientEntity>() { new IngredientEntity() { RecipeId = recipeId, Ingredient = "Chicken", OrdinalPosition = 0, Quantity = 2, Unit = "Breasts"} },
                    Instructions = new List<InstructionEntity>() { new InstructionEntity() { RecipeId = recipeId, OrdinalPosition = 0, Instruction = "Get chicken breast dry them really good" } }
                });

                if (_commitToDatabase) {
                    scope.Complete();
                }

                Assert.Equal(3, rowsAffected);
            }
        }

        [Fact]
        public async Task UpdateAsync_Success()
        {
            var recipeRepository = new RecipeRepository(new IngredientsRepository(), new InstructionsRepository());

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                Guid recipeId = Guid.Parse("845598f5-7fee-4a96-aab3-f4d553fb6063");

                var rowsAffected = await recipeRepository.UpdateAsync(new RecipeEntity()
                {
                    Id = recipeId,
                    Title = "New Title Chicken Parm Unit Test",
                    Description = "Chicken Parm Description",
                    Logo = null,
                    CreatedDate = DateTimeOffset.UtcNow,
                    Ingredients = new List<IngredientEntity>() { new IngredientEntity() { RecipeId = recipeId, Ingredient = "Not Chicken Beef", OrdinalPosition = 0, Quantity = 2, Unit = "Breasts" }, new IngredientEntity() { RecipeId = recipeId, Ingredient = "Noodles", OrdinalPosition = 1, Quantity = 2, Unit = "lbs" } },
                    Instructions = new List<InstructionEntity>() { new InstructionEntity() { RecipeId = recipeId, OrdinalPosition = 0, Instruction = "Soak them in watter" } }
                });

                if (_commitToDatabase)
                {
                    scope.Complete();
                }

                Assert.Equal(2, rowsAffected);
            }
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            var recipeRepository = new RecipeRepository(new IngredientsRepository(), new InstructionsRepository());

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                Guid recipeId = Guid.Parse("845598f5-7fee-4a96-aab3-f4d553fb6063");

                var rowsAffected = await recipeRepository.DeleteAsync(recipeId);

                if (_commitToDatabase)
                {
                    scope.Complete();
                }

                Assert.Equal(3, rowsAffected);
            }
        }

    }
}
