using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordGenius.Desktop.Helpers;

class Randomm
{
    public List<int> GenerateRandom(int start, int stop, int bloc)
    {
        List<int> list = new List<int>();

        int n = 0;
        Random random = new Random();
        while (true)
        {
            bool s = false;
            n = random.Next(start, stop);
            foreach(var i in list)
            {
                if (n == i && n == bloc)
                {
                     s= true; break;
                }
            }
            if(!s)
            {
                list.Add(n);
            }
            if(list.Count == 3)
            {
                break;
            }

        }


        return list;
    }
}
