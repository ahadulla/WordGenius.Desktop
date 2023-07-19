using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordGenius.Desktop.Entities.Results;
using WordGenius.Desktop.Entities.Words;
using WordGenius.Desktop.Interfaces.Results;
using WordGenius.Desktop.Utils;
using WordGenius.Repositories;

namespace WordGenius.Desktop.Repositories.Results;

class ResultRepository : BaseRepository, IResultRepository
{
    public async Task<int> CreateAsync(Result Obj)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "INSERT INTO results( words_id, step_1) VALUES( @WordId, @step1);";

            await using (var command = new NpgsqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("WordId", Obj.WordsId);
                command.Parameters.AddWithValue("step1", Obj.Step1);

                var dbResult = await command.ExecuteNonQueryAsync();
                return dbResult;
            }
        }
        catch
        {
            return 0;
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public Task<int> DeleteAsync(long Id)
    {
        throw new NotImplementedException();
    }

    public Task<IList<Result>> GetAllAsync(PagenationParams @params)
    {
        throw new NotImplementedException();
    }

    public Task<Result> GetAsync(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<int> UpdateAsync(long Id, Result EditedObj)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "UPDATE results SET step_2 = @Step2, " +
                "WHERE id = @Id and step_1 is not null and step_3 is null;";

            await using (var command = new NpgsqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("Id", Id);
                command.Parameters.AddWithValue("Step2", EditedObj.Step2);

                var dbResult = await command.ExecuteNonQueryAsync();
                return dbResult;
            }
        }
        catch
        {
            return 0;
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<IList<long>> GetAllFinishedStep1Async()
    {
        try
        {
            await _connection.OpenAsync();
            var list = new List<long>()!;
            string query = $"Select words_id from" +
                $" results where date(current_date)-date(step_1)>7 and step_2 is null and step_3 is null;";
            await using (var command = new NpgsqlCommand(query, _connection))
            {
                await using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        long n = reader.GetInt64(0);
                        list.Add(n);
                    }
                }
            }
            return list;
        }
        catch
        {
            return new List<long>()!;
        }
        finally
        {
            await _connection.CloseAsync();
        }

    }

    public async Task<IList<long>> GetAllFinishedStep2Async()
    {
        try
        {
            await _connection.OpenAsync();
            var list = new List<long>()!;
            string query = $"Select words_id from" +
                $" results where date(current_date)-date(step_2)>30 and step_1 is not null and step_3 is null;";
            await using (var command = new NpgsqlCommand(query, _connection))
            {
                await using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        long n = reader.GetInt64(0);
                        list.Add(n);
                    }
                }
            }
            return list;
        }
        catch
        {
            return new List<long>()!;
        }
        finally
        {
            await _connection.CloseAsync();
        }

    }

    public async Task<IList<long>> GetAllFinishedStep3Async()
    {
        try
        {
            await _connection.OpenAsync();
            var list = new List<long>()!;
            string query = $"Select words_id from" +
                $" results where date(current_date)-date(step_3)>90 and step_1 is not null and step_2 is not null;";
            await using (var command = new NpgsqlCommand(query, _connection))
            {
                await using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        long n = reader.GetInt64(0);
                        list.Add(n);
                    }
                }
            }
            return list;
        }
        catch
        {
            return new List<long>()!;
        }
        finally
        {
            await _connection.CloseAsync();
        }

    }


}
