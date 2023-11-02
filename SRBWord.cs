using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ScanWord;

namespace SRWords
{
    // “екущее слово, показываемое в браузере
    public class SRBWord : Word
    {
        public SRBWord() : base()
        {
            Id = 0;
        }

        public SRBWord(DataRowView dr) : base (dr)
        {
            //Id = (int)dr["ID"];
            /*
            string ids = dr["ID"].ToString();
            int value = 0;
            if (int.TryParse(ids, out value))
                Id = value;
            */
        }

        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}
