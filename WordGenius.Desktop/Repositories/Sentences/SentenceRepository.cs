using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using WordGenius.Desktop.Constants;
using WordGenius.Desktop.Entities.Sentences;
using WordGenius.Desktop.Entities.Words;
using WordGenius.Desktop.Interfaces;
using WordGenius.Desktop.Interfaces.Sentences;
using WordGenius.Desktop.Utils;
using WordGenius.Desktop.ViewModels;

namespace WordGenius.Repositories.Sentences;

public class SentenceRepository : ISentenceRepository
{
    private readonly NpgsqlConnection _connection;


    public SentenceRepository()
    {
        _connection = new NpgsqlConnection(DbConstans.DB_CONNECTIONSTRING);
    }

    public async Task<int> CountAsync()
    {
        try
        {
            await _connection.OpenAsync();
            int n = 0;
            string query = "Select count(id) from sentences";
            await using (var command = new NpgsqlCommand(query, _connection))
            {
                await using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        n = reader.GetInt32(0);
                    }
                }
            }
            return n;
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

    public async Task<int> CreateAsync(Sentence Obj)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "INSERT INTO sentences (words_id, sentence) VALUES ( @word_id, @sentence);";

            await using (var command = new NpgsqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("word_id", Obj.WordsId);
                command.Parameters.AddWithValue("sentence", Obj.SentenceText);

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

    public async Task<int> DeleteAsync(long Id)
    {
        try
        {
            await _connection.OpenAsync();

            string query = $"DELETE FROM sentences  WHERE  id = {Id};";

            await using (var command = new NpgsqlCommand(query, _connection))
            {
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

    public async Task<IList<SentenceViewModel>> GetAllAsync(PagenationParams @params)
    {
        try
        {
            await _connection.OpenAsync();
            var list = new List<SentenceViewModel>();
            string query = $"select * from Sentence_view_model order by id " +
                $"offset {(@params.PageNumber - 1) * @params.PageSize} " +
                $"limit {@params.PageSize}";
            await using (var command = new NpgsqlCommand(query, _connection))
            {
                await using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var sentence = new SentenceViewModel();
                        sentence.Id = reader.GetInt64(0);
                        sentence.word = reader.GetString(1);
                        sentence.sentence = reader.GetString(2);
                        list.Add(sentence);
                    }
                }
            }
            return list;
        }
        catch
        {
            return new List<SentenceViewModel>();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<SentenceViewModel> GetAsync(long id)
    {
        try
        {
            await _connection.OpenAsync();
            var sentence = new SentenceViewModel();

            string query = $"select sentences.id, words.tranlate, sentences.sentence from sentences " +
                $"left join words on words.id = sentences.words_id " +
                $"where sentences.id = {id};";
            await using (var command = new NpgsqlCommand(query, _connection))
            {
                await using (var reader = await command.ExecuteReaderAsync())
                {
                    if(await reader.ReadAsync())
                    {
                        sentence.Id = reader.GetInt64(0);
                        sentence.word = reader.GetString(1);
                        sentence.sentence = reader.GetString(2);
                    }
                }
            }
            return sentence;
        }
        catch
        {
            return new SentenceViewModel();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<int> UpdateAsync(long Id, Sentence EditedObj)
    {
        try
        {
            await _connection.OpenAsync();

            string query = $"UPDATE sentences SET words_id = @words_id, " +
                $"sentence = @sentence  WHERE   id = {Id};";

            await using (var command = new NpgsqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("words_id", EditedObj.WordsId);
                command.Parameters.AddWithValue("sentence", EditedObj.SentenceText);

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

    public async Task<IList<SentenceViewModel>> GetAllWordsSentenceAsync(long id)
    {
        try
        {
            await _connection.OpenAsync();
            var list = new List<SentenceViewModel>();
            string query = $"select sentences.id, words.tranlate, sentences.sentence from sentences " +
                $"left join words on words.id = sentences.words_id " +
                $"where sentences.words_id = {id};";
            await using (var command = new NpgsqlCommand(query, _connection))
            {
                await using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var sentence = new SentenceViewModel();
                        sentence.Id = reader.GetInt64(0);
                        sentence.word = reader.GetString(1);
                        sentence.sentence = reader.GetString(2);
                        list.Add(sentence);
                    }
                }
            }
            return list;
        }
        catch
        {
            return new List<SentenceViewModel>();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

}


