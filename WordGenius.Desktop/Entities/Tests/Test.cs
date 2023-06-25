using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordGenius.Desktop.Entities.Words;

namespace WordGenius.Desktop.Entities.Test;

 public class Test : BaseEntity
{
    public int maxcount { get; set; }

    public int count { get; set; }

    public Word word { get; set; }

    public string errorAns1 { get; set; }

    public string errorAns2 { get; set; }

    public string errorAns3 { get; set; }

}
