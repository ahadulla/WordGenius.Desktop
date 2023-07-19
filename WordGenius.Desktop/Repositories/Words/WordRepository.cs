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
using WordGenius.Desktop.ViewModels.Words;
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

    public async Task<int> CountTodayAsync()
    {
        try
        {
            await _connection.OpenAsync();
            int n = 0;
            string query = $"select count(*) from words where created_at >= current_date ";
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

    public async Task<int> CreateAsync(Word Obj)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "INSERT INTO public.words(" +
                "word, tranlate, discription,  is_remember, created_at, update_at, correct_count) " +
                "VALUES(@name, @translate, @discription,  @isremember, @created_at, @updated_at, @correct_count);";

            await using (var command = new NpgsqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("name", Obj.Text);
                command.Parameters.AddWithValue("translate", Obj.Translate);
                command.Parameters.AddWithValue("discription", Obj.Discription);
                command.Parameters.AddWithValue("isremember", Obj.Is_Remember);
                command.Parameters.AddWithValue("created_at", Obj.CreateAt);
                command.Parameters.AddWithValue("updated_at", Obj.UpdateAt);
                command.Parameters.AddWithValue("correct_count", Obj.Correct_count);

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
            string query = $"select * from words order by id desc " +
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

    public async Task<int> UpdateAsync(long Id, Word EditedObj)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "UPDATE words SET word = @word, " +
                $"tranlate = @tranlate , discription = @discription," +
                $"update_at = @updated_at  WHERE   id = @Id;";

            await using (var command = new NpgsqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("Id", Id);
                command.Parameters.AddWithValue("word", EditedObj.Text);
                command.Parameters.AddWithValue("tranlate", EditedObj.Translate);
                command.Parameters.AddWithValue("discription", EditedObj.Discription);
                command.Parameters.AddWithValue("updated_at", EditedObj.UpdateAt);

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

    public async Task<IList<Word>> GetAllTodayAsync()
    {
        try
        {
            await _connection.OpenAsync();
            var list = new List<Word>();
            string query = $"select * from words where is_remember = false and created_at >= current_date order by id desc ";
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

    public async Task<IList<Word>> SearchAsync(string search)
    {
        try
        {
            await _connection.OpenAsync();
            var list = new List<Word>();
            string query = "select *from words where word ilike @search or tranlate ilike @search";
            await using (var command = new NpgsqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("search", $"%{search}%");

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

    public async Task<IList<ByDayView>> GetAllCreatedDayAsync()
    {
        try
        {
            await _connection.OpenAsync();
            var list = new List<ByDayView>();
            string query = $"SELECT DATE(created_at) AS date, COUNT(*) AS count FROM words " +
                $"GROUP BY DATE(created_at) ORDER BY DATE(created_at) DESC LIMIT 10;";
            await using (var command = new NpgsqlCommand(query, _connection))
            {
                await using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var days = new ByDayView();
                        days.date = reader.GetFieldValue<DateOnly>(0);
                        days.Ids = reader.GetInt64(1);
                        list.Add(days);
                    }
                }
            }
            return list;
        }
        catch
        {
            return new List<ByDayView>();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<int> UpdateCorrectCountAsync(long Id)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "UPDATE words SET correct_count = correct_count+1  WHERE   id = @Id;";

            await using (var command = new NpgsqlCommand(query, _connection))
            {

                command.Parameters.AddWithValue("Id", Id);

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

    public async Task<int> UpdateIsRememberAsync(long Id)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "UPDATE words SET is_remember = true, correct_count=0  WHERE correct_count = 10 and id = @Id;";

            await using (var command = new NpgsqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("Id", Id);

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

    public async Task<int> UpdateIsRememberAllAsync(IList<long> Idlist)
    {
        try
        {
            await _connection.OpenAsync();

            int n = 0;
            foreach (var Id in Idlist)
            {
                string query = "UPDATE words SET is_remember = false, correct_count=0  WHERE  id = @Id;";

                await using (var command = new NpgsqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("Id", Id);

                    var dbResult = await command.ExecuteNonQueryAsync();
                    n += dbResult;
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
}
