using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WordGenius.Desktop.Entities.Sentences;
using WordGenius.Desktop.Interfaces.Sentences;
using WordGenius.Desktop.Utils;

namespace WordGenius.Repositories.Sentences;

internal class SentencesRepository : ISentenceRepository
{
    public Task<int> CountAsync()
    {
        throw new NotImplementedException();
    }

    public Task<int> CreateAsync(Sentence Obj)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteAsync(long Id)
    {
        throw new NotImplementedException();
    }

    public Task<IList<Sentence>> GetAllAsync(PagenationParams @params)
    {
        throw new NotImplementedException();
    }

    public Task<Sentence> GetAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateAsync(long Id, Sentence EditedObj)
    {
        throw new NotImplementedException();
    }
}


