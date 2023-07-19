using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WordGenius.Desktop.Entities;
using WordGenius.Desktop.Entities.Words;

namespace WordGenius.Desktop.ViewModels;

public class SentenceViewModel : BaseEntity
{
    public string  word { get; set; }

    public string sentence { get; set; }
}
