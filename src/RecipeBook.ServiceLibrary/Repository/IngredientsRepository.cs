using Dapper;
using RecipeBook.ServiceLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.ServiceLibrary.Repository
{

    public interface IIngredientsRepository
    {

        public Task<int> InsertAsync(SqlConnection connection, DbTransaction transaction, IEnumerable<IngredientEntity> ingrediantEntity);

        public Task<int> DeleteAsync(SqlConnection connection, DbTransaction transaction, Guid recipeId);
        public Task<int> UpdateAsync(SqlConnection connection, DbTransaction transaction, IEnumerable<IngredientEntity> ingrediantEntity);
        public Task<IList<IngredientEntity>> GetByIdAsync(SqlConnection connection, DbTransaction transaction, Guid recipeId);
        public Task<int> DeleteOneAsync(SqlConnection connection, DbTransaction transaction, Guid recipeId, int ordinalPosition);
    }

    public class IngredientsRepository : IIngredientsRepository
    {


        public async Task<IList<IngredientEntity>> GetByIdAsync(SqlConnection connection, DbTransaction transaction, Guid recipeId)
        {
            IList<IngredientEntity> ingredients = new List<IngredientEntity>();
            try
            {
                var reader = await connection.ExecuteReaderAsync(@"SELECT * FROM [dbo].[Ingredients] WHERE RecipeId = @RecipeId", new { RecipeId = recipeId }, transaction: transaction);

                var parser = reader.GetRowParser<IngredientEntity>(typeof(IngredientEntity));

                while (reader.Read())
                {
                    ingredients.Add(parser(reader));
                }
            }
            catch
            {
                return ingredients;
            }
            

            return ingredients;
        }

        public async Task<int> InsertAsync(SqlConnection connection, DbTransaction transaction, IEnumerable<IngredientEntity> ingrediantEntity)
        {
            var rowsAffected = 0;

            foreach (var entity in ingrediantEntity)
            {
                rowsAffected += await connection.ExecuteAsync
                (
                    @"INSERT INTO [dbo].[Ingredients]
                    (
                         [RecipeId],
                         [OrdinalPosition],
                         [Unit],
                         [Quantity],
                         [Ingredient]
                    )
                    Values
                    (
                         @RecipeId,
                         @OrdinalPosition,
                         @Unit,
                         @Quantity,
                         @Ingredient
                    )", 
                    new 
                    { 
                        entity.RecipeId, 
                        entity.OrdinalPosition, 
                        entity.Unit, 
                        entity.Quantity, 
                        entity.Ingredient, 
                    },
                    transaction: transaction
                    
                );
            }
            return rowsAffected;
        }

        public async Task<int> UpdateAsync(SqlConnection connection, DbTransaction transaction, IEnumerable<IngredientEntity> ingrediantEntity)
        {
            var rowsAffected = 0;

            IngredientEntity lastIngredientEntity = null;

            IList<IngredientEntity> remainingEntities = new List<IngredientEntity>();

            foreach (var entity in ingrediantEntity)
            {
                var rowAffected = await connection.ExecuteAsync
                (
                    @"UPDATE [dbo].[Ingredients]
                    SET Unit = @Unit, Quantity = @Quantity, Ingredient = @Ingredient WHERE RecipeId = @RecipeId AND OrdinalPosition = @OrdinalPosition",
                    new
                    {
                        entity.Unit,
                        entity.Quantity,
                        entity.Ingredient,
                        entity.RecipeId,
                        entity.OrdinalPosition
                    },
                    transaction: transaction
                );

                if (rowAffected == 0)
                {
                    remainingEntities.Add(entity);
                }



                rowsAffected += rowsAffected;
                

                lastIngredientEntity = entity;

            }

            if (remainingEntities.Count > 0)
            {
                rowsAffected += await InsertAsync(connection, transaction, remainingEntities);
            }
            else
            {
                try
                {
                    rowsAffected += await connection.ExecuteAsync(@"DELETE FROM [dbo].[Ingredients] WHERE recipeId = @recipeId AND OrdinalPosition > @OrdinalPosition", new { recipeId = lastIngredientEntity.RecipeId, OrdinalPosition = lastIngredientEntity.OrdinalPosition }, transaction: transaction);

                }
                catch (SystemException error)
                {
                    Console.WriteLine(error);
                }
             }




            return rowsAffected;
        }


        public async Task<int> DeleteOneAsync(SqlConnection connection, DbTransaction transaction, Guid recipeId, int ordinalPosition)
        {
            var rowsAffected = await connection.ExecuteAsync(@"DELETE FROM [dbo].[Ingredients] WHERE recipeId = @recipeId AND OrdinalPosition = @OrdinalPosition", new { recipeId = recipeId, OrdinalPosition = ordinalPosition }, transaction: transaction);

            return rowsAffected;
        }

        public async Task<int> DeleteAsync(SqlConnection connection, DbTransaction transaction, Guid recipeId)
        {
            var rowsAffected = await connection.ExecuteAsync(@"DELETE FROM [dbo].[Ingredients] WHERE recipeId = @recipeId", new { recipeId }, transaction: transaction);

            return rowsAffected;
        }

    }
}

