using System;
using System.Collections.Generic;
using System.Text;

namespace SRWords
{
    public class RusWord
    {
        public string name;
        public string stress;
        public string srbname;

        public RusWord(string name, string srbname) 
        {
            this.name = name;
            this.srbname = srbname;
        }

        public RusWord(string name, string stress, string srbname)
        {
            this.name = name;
            this.stress = stress;
            this.srbname = srbname;
        }

    }
}
