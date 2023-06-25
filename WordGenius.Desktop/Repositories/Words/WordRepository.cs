using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using WordGenius.Desktop.Constants;
using WordGenius.Desktop.Entities.Words;
using WordGenius.Desktop.Interfaces.Words;
using WordGenius.Desktop.Utils;
using WordGenius.Repositories;

namespace WordGenius.Desktop.Repository.Words;

class WordRepository : IWordsRepository 
{
    private readonly NpgsqlConnection _connection;
    public WordRepository()
    {
        _connection = new NpgsqlConnection(DbConstans.DB_CONNECTIONSTRING);
    }

    public async Task<int> CountAsync()
    {
        try
        {
            await _connection.OpenAsync();
            int n = 0;
            string query = "Select count(id) from words";
            await using (var command = new NpgsqlCommand(query, _connection))
            {
                await using (var reader = await command.ExecuteReaderAsync())
                {
                    if(await reader.ReadAsync())
                    {
                        n=reader.GetInt32(0);
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

    public async Task<int> CreateAsync(Word Obj)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "INSERT INTO public.words(" +
                "word, tranlate, discription,  is_remember, created_at, update_at) " +
                "VALUES(@name, @translate, @discription,  @isremember, @created_at, @updated_at);";

            await using (var command = new NpgsqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("name", Obj.Text);
                command.Parameters.AddWithValue("translate", Obj.Translate);
                command.Parameters.AddWithValue("discription", Obj.Discription);
                command.Parameters.AddWithValue("isremember", Obj.Is_Remember);
                command.Parameters.AddWithValue("created_at", Obj.CreateAt);
                command.Parameters.AddWithValue("updated_at", Obj.UpdateAt);

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

            string query = $"DELETE FROM words  WHERE  id = {Id};";

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


    public async Task<IList<Word>> GetAllAsync(PagenationParams @params)
    {
        try
        {
            await _connection.OpenAsync();
            var list = new List<Word>();
            string query = $"select * from words order by id " +
                $"offset {(@params.PageNumber - 1) * @params.PageSize} " +
                $"limit {@params.PageSize}";
            await using (var command = new NpgsqlCommand(query, _connection))
            {
                await using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var word = new Word();
                        word.Id = reader.GetInt64(0);
                        word.Text = reader.GetString(1);
                        word.Translate = reader.GetString(2);
                        word.Discription = reader.GetString(3);
                        word.Is_Remember = reader.GetBoolean(4);
                        word.CreateAt = reader.GetDateTime(5);
                        word.UpdateAt = reader.GetDateTime(6);
                        list.Add(word);
                    }
                }
            }
            return list;
        }
        catch
        {
            return new List<Word>();
        }
        finally
        {
            await _connection.CloseAsync();
        }

    }


    public async Task<Word> GetAsync(long Id)
    {
        Word word = new Word();
        try
        {
            await _connection.OpenAsync();
            string query = $"select * from where id = {Id}";
            await using (var command = new NpgsqlCommand(query, _connection))
            {
                await using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        word = new Word();
                        word.Id = reader.GetInt64(0);
                        word.Text = reader.GetString(1);
                        word.Translate = reader.GetString(2);
                        word.Discription = reader.GetString(3);
                        word.Is_Remember = reader.GetBoolean(4);
                        word.CreateAt = reader.GetDateTime(5);
                        word.UpdateAt = reader.GetDateTime(6);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            await _connection.CloseAsync();

        }
        return word;

    }

    public Task<int> UpdateAsync(long Id, Word EditedObj)
    {
        throw new System.NotImplementedException();
    }

    public async Task<IList<Word>> GetAllNMemoAsync(int offset)
    {
        try
        {
            await _connection.OpenAsync();
            var list = new List<Word>();
            string query = $"select * from words where is_remember = false  order by id desc offset {offset} limit 30 ";
            await using (var command = new NpgsqlCommand(query, _connection))
            {
                await using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var word = new Word();
                        word.Id = reader.GetInt64(0);
                        word.Text = reader.GetString(1);
                        word.Translate = reader.GetString(2);
                        word.Discription = reader.GetString(3);
                        word.Is_Remember = reader.GetBoolean(4);
                        word.CreateAt = reader.GetDateTime(5);
                        word.UpdateAt = reader.GetDateTime(6);
                        list.Add(word);
                    }
                }
            }
            return list;
        }
        catch
        {
            return new List<Word>();
        }
        finally
        {
            await _connection.CloseAsync();
        }

    }
}
