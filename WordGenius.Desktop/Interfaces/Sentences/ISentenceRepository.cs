using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordGenius.Desktop.Entities.Sentences;

namespace WordGenius.Desktop.Interfaces.Sentences;

public interface ISentenceRepository : IRepository<Sentence, Sentence>
{
    public Task<int> CountAsync();
}


