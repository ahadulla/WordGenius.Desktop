using System.Threading.Tasks;
using WordGenius.Desktop.Entities.Words;
namespace WordGenius.Desktop.Interfaces.Words;

public interface IWordsRepository : IRepository<Word, Word>
{
    public Task<int> CountAsync();
}
