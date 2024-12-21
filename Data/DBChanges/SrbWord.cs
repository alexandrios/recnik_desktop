using System;
using System.Collections.Generic;
using System.Text;

namespace SRWords
{
    public class SrbWord
    {
        public int id;
        public string name;
        public string stress;
        public string kw;
        public string xml;

        public SrbWord(string name, string kw, string xml) 
        {
            this.name = name;
            this.kw = kw;
            this.xml = xml;
        }

        public SrbWord(string name, string stress, string kw, string xml)
        {
            this.name = name;
            this.stress = stress;
            this.kw = kw;
            this.xml = xml;
        }
    }
}
