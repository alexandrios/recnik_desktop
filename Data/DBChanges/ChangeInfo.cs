using System;
using System.Collections.Generic;
using System.Text;

namespace SRWords
{
    public class ChangeInfo
    {
        public int id;
        public string type;
        public string name;
        public string stress;
        public string kw;
        public string xml;

        public ChangeInfo(int id, string type, string name, string kw, string xml) 
        {
            this.id = id;
            this.type = type;
            this.name = name;
            this.kw = kw;
            this.xml = xml;
        }

        public SrbWord ToWord()
        {
            return new SrbWord(Utils.LatToCyr(this.name), this.kw, this.xml);  // в ADS name - кириллица
        }

        public SrbWord ToWord2()
        {
            return new SrbWord(this.name, this.stress, this.kw, this.xml);  // в новом формате (2) ADS name - латиница
        }

        public RusRef ToRusRef()
        {
            return new RusRef(this.name, this.stress, this.xml, this.kw);
        }


        public override string ToString()
        {
            string t = id.ToString() + " " + type + " " + name;
            return t;
        }
    }
}
