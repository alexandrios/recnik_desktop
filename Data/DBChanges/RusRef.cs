using System;
using System.Collections.Generic;
using System.Text;

namespace SRWords
{
    public class RusRef
    {
        public string name;
        public string stress;
        public string srbname;
        public string kw;

        public RusRef(string name, string stress, string srbname, string kw)
        {
            this.name = name;
            this.stress = stress;
            this.srbname = srbname;
            this.kw = kw;
        }
    }
}
