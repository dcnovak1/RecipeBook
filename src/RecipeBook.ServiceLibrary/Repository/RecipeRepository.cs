using Dapper;
using RecipeBook.ServiceLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.ServiceLibrary.Repository
{
    public interface IRecipeRepository
    {
        Task<int> InsertAsync(RecipeEntity entity);
        Task<int> DeleteAsync(Guid recipeId);
        Task<int> UpdateAsync(RecipeEntity entity);
    }

    public class RecipeRepository : IRecipeRepository
    {

        private readonly IIngredientsRepository _ingredientsRepository;
        private readonly IInstructionsRepository _instructionsRepository;


        public RecipeRepository(IIngredientsRepository ingredientsRepository, IInstructionsRepository instructionsRepository)
        {
            _ingredientsRepository = ingredientsRepository;
            _instructionsRepository = instructionsRepository;
        }

        public async Task<int> InsertAsync(RecipeEntity entity)
        {
            using (var connection = new SqlConnection("Data Source=host.docker.internal,5050; Initial Catalog=RecipeBook;User Id=SA;Password=P@ssword123;MultipleActiveResultSets=true"))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var rowsAffected = await connection.ExecuteAsync
                    (
                        @"INSERT INTO [dbo].[Recipes]
                        (
                            [Id],
                            [Title],
                            [Description],
                            [Logo],
                            [CreatedDate]
                        )
                        Values
                        (
                            @Id,
                            @Title,
                            @Description,
                            @Logo,
                            @CreatedDate
                        )", 
                        new 
                        { 
                            entity.Id, 
                            entity.Title, 
                            entity.Description, 
                            entity.Logo, 
                            entity.CreatedDate
                        },
                        transaction: transaction
                    );

                    rowsAffected += await _ingredientsRepository.InsertAsync(connection, transaction, entity.Ingredients);
                    rowsAffected += await _instructionsRepository.InsertAsync(connection, transaction, entity.Instructions);

                    transaction.Commit();

                    return rowsAffected;
                }
            }
        }

        public async Task<int> UpdateAsync(RecipeEntity entity)
        {
            using (var connection = new SqlConnection("Data Source=host.docker.internal,5050; Initial Catalog=RecipeBook;User Id=SA;Password=P@ssword123;MultipleActiveResultSets=true"))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var rowsAffected = await connection.ExecuteAsync
                    (
                        @"UPDATE [dbo].[Recipes]
                        SET Title = @Title, Description = @Description, Logo = @Logo, CreatedDate = @CreatedDate WHERE Id = @Id",
                        new
                        {
                            entity.Title,
                            entity.Description,
                            entity.Logo,
                            entity.CreatedDate,
                            entity.Id
                        },
                        transaction: transaction
                    );

                    rowsAffected += await _ingredientsRepository.UpdateAsync(connection, transaction, entity.Ingredients);
                    //rowsAffected += await _instructionsRepository.UpdateAsync(connection, transaction, entity.Instructions);

                    transaction.Commit();

                    return rowsAffected;
                }
            }
        }

        public async Task<int> DeleteAsync(Guid recipeId)
        {
            using (var connection = new SqlConnection("Data Source=host.docker.internal,5050; Initial Catalog=RecipeBook;User Id=SA;Password=P@ssword123;MultipleActiveResultSets=true"))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var rowsAffected = await _ingredientsRepository.DeleteAsync(connection, transaction, recipeId);
                    rowsAffected += await _instructionsRepository.DeleteAsync(connection, transaction, recipeId);

                    rowsAffected += await connection.ExecuteAsync(@"DELETE FROM [dbo].[Recipes] WHERE Id = @Id", new { Id = recipeId }, transaction: transaction);
    
                    transaction.Commit();

                    return rowsAffected;
                }
            }
        }
    }
}
