using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordGenius.Desktop.Entities.Sentences;
using WordGenius.Desktop.Utils;
using WordGenius.Desktop.ViewModels;

namespace WordGenius.Desktop.Interfaces.Sentences;

public interface ISentenceRepository : IRepository<Sentence, SentenceViewModel>
{
    public Task<int> CountAsync();
}


