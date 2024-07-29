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
        public Task<int> InsertAsync(SqlConnection connection, DbTransaction transaction, IEnumerable<IngredientsEntity> ingrediantEntity);

        public Task<int> DeleteAsync(SqlConnection connection, DbTransaction transaction, Guid recipeId);
        public Task<int> UpdateAsync(SqlConnection connection, DbTransaction transaction, IEnumerable<IngredientsEntity> ingrediantEntity);
    }

    public class IngredientsRepository : IIngredientsRepository
    {

        public async Task<int> InsertAsync(SqlConnection connection, DbTransaction transaction, IEnumerable<IngredientsEntity> ingrediantEntity)
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

        public async Task<int> UpdateAsync(SqlConnection connection, DbTransaction transaction, IEnumerable<IngredientsEntity> ingrediantEntity)
        {
            var rowsAffected = 0;

            if (ingrediantEntity.AsList<IngredientsEntity>().Count == 0)
            {
                return 0;
            }

            Guid recipeId = ingrediantEntity.AsList<IngredientsEntity>()[0].RecipeId;
            int greatestOrdingalPosition = -1;
            foreach (var entity in ingrediantEntity)
            {
                if (entity.OrdinalPosition > greatestOrdingalPosition)
                {
                    greatestOrdingalPosition = entity.OrdinalPosition;
                }

                rowsAffected = await connection.ExecuteAsync
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

            }

            if (greatestOrdingalPosition != -1)
            {
                rowsAffected = await connection.ExecuteAsync(@"DELETE FROM [dbo].[Ingredients] WHERE RecipeId = @RecipeId AND OrdinalPosition > @OrdinalPosition", new { recipeId, OrdinalPosition = greatestOrdingalPosition }, transaction: transaction);
            }
            

            return rowsAffected;
        }

        public async Task<int> DeleteAsync(SqlConnection connection, DbTransaction transaction, Guid recipeId)
        {
            var rowsAffected = await connection.ExecuteAsync(@"DELETE FROM [dbo].[Ingredients] WHERE recipeId = @recipeId", new { recipeId }, transaction: transaction);

            return rowsAffected;
        }

    }
}

