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
        public Task<int> InsertAsync(SqlConnection connection, DbTransaction transaction, IEnumerable<InstructionEntity> instructionsEntity);

        public Task<int> DeleteAsync(SqlConnection connection, DbTransaction transaction, Guid recipeId);
        public Task<IList<InstructionEntity>> GetByIdAsync(SqlConnection connection, DbTransaction transaction, Guid recipeId);
        public Task<int> UpdateAsync(SqlConnection connection, DbTransaction transaction, IEnumerable<InstructionEntity> ingrediantEntity);
    }

    public class InstructionsRepository : IInstructionsRepository
    {

        public async Task<IList<InstructionEntity>> GetByIdAsync(SqlConnection connection, DbTransaction transaction, Guid recipeId)
        {

            IList<InstructionEntity> instructions = new List<InstructionEntity>();

            try
            {
                var reader = await connection.ExecuteReaderAsync(@"SELECT * FROM [dbo].[Instructions] WHERE RecipeId = @RecipeId", new { RecipeId = recipeId }, transaction: transaction);

                var parser = reader.GetRowParser<InstructionEntity>(typeof(InstructionEntity));

                while (reader.Read())
                {
                    instructions.Add(parser(reader));
                }
            }
            catch
            {
                return instructions;
            }
            

            return instructions;
        }

        public async Task<int> InsertAsync(SqlConnection connection, DbTransaction transaction, IEnumerable<InstructionEntity> instructionsEntity)
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

        public async Task<int> UpdateAsync(SqlConnection connection, DbTransaction transaction, IEnumerable<InstructionEntity> ingrediantEntity)
        {
            var rowsAffected = 0;

            InstructionEntity lastInstructionEntity = null;

            IList<InstructionEntity> remainingEntities = new List<InstructionEntity>();

            foreach (var entity in ingrediantEntity)
            {

                var rowAffected = await connection.ExecuteAsync
                (
                    @"UPDATE [dbo].[Instructions]
                    SET Instruction = @Instruction WHERE RecipeId = @RecipeId AND OrdinalPosition = @OrdinalPosition",
                    new
                    {
                        entity.Instruction,
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


                lastInstructionEntity = entity;
            }

            if (remainingEntities.Count > 0)
            {
                rowsAffected += await InsertAsync(connection, transaction, remainingEntities);
            }
            else
            {
                rowsAffected = await connection.ExecuteAsync(@"DELETE FROM [dbo].[Instructions] WHERE recipeId = @recipeId AND OrdinalPosition > @OrdinalPosition", new { recipeId = lastInstructionEntity.RecipeId, OrdinalPosition = lastInstructionEntity.OrdinalPosition }, transaction: transaction);
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
