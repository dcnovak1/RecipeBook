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



    public interface IInstructionsRepository
    {
        public Task<int> InsertAsync(SqlConnection connection, DbTransaction transaction, IEnumerable<InstructionsEntity> instructionsEntity);

        public Task<int> DeleteAsync(SqlConnection connection, DbTransaction transaction, Guid recipeId);
    }

    public class InstructionsRepository : IInstructionsRepository
    {

        public async Task<int> InsertAsync(SqlConnection connection, DbTransaction transaction, IEnumerable<InstructionsEntity> instructionsEntity)
        {
            var rowsAffected = 0;

            foreach (var entity in instructionsEntity)
            {
                rowsAffected += await connection.ExecuteAsync
                (
                    @"INSERT INTO [dbo].[Instructions]
                    (
                         [RecipeId],
                         [OrdinalPosition],
                         [Instruction]
                    )
                    Values
                    (
                         @RecipeId,
                         @OrdinalPosition,
                         @Instruction
                    )",
                    new
                    {
                        entity.RecipeId,
                        entity.OrdinalPosition,
                        entity.Instruction
                    },
                    transaction: transaction

                );
            }
            return rowsAffected;
        }

        public async Task<int> DeleteAsync(SqlConnection connection, DbTransaction transaction, Guid recipeId)
        {
            var rowsAffected = await connection.ExecuteAsync(@"DELETE FROM [dbo].[Instructions] WHERE recipeId = @recipeId", new { recipeId }, transaction: transaction);

            return rowsAffected;
        }

    }
}
