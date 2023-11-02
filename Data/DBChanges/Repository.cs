using System;
using System.Collections.Generic;
using System.Text;
using ScanWord;

namespace SRWords
{
    public class Repository : IRepository
    {
        public int IsExistsWord(string wordName)
        {
#if SQLITE
            Int64 result = SQLiteData.RunCommandScalarInt("select count(*) cnt from words where name = '" + wordName + "'");
#else
            Int64 result = ADSData.RunCommandScalarInt("select count(*) cnt from words where name = '" + wordName + "'");
#endif
            if (result == 1)
            {
                return 1;
            }
            return 0;
        }

        public void InsertWord(SrbWord word)
        {
            string cmd = "insert into words (name, kw, xml, name_lat) values ('" + word.name + "','" + word.kw + "','" + word.xml + "','" +
                Utils.CyrToLat(word.name) + "')";
#if SQLITE
            SQLiteData.RunCommand(cmd);
#else
            ADSData.RunCommand(cmd);
#endif
        }

        public void UpdateWord(SrbWord word)
        {
            string cmd = "update words set xml='" + word.xml + "', kw='" + word.kw + "' where name='" + word.name + "'";
#if SQLITE
            SQLiteData.RunCommand(cmd);
#else
            ADSData.RunCommand(cmd);
#endif
        }

        public void DeleteWord(SrbWord word)
        {
            string cmd = "delete from words where name='" + word.name + "'";
#if SQLITE
            SQLiteData.RunCommand(cmd);
#else
            ADSData.RunCommand(cmd);
#endif
        }

        public int IsExistsRusWord(string wordName)
        {
#if SQLITE
            Int64 result = SQLiteData.RunCommandScalarInt("select count(*) cnt from ruswords where name = '" + wordName + "'");
#else
            Int64 result = ADSData.RunCommandScalarInt("select count(*) cnt from ruswords where name = '" + wordName + "'");
#endif
            if (result == 1)
            {
                return 1;
            }
            return 0;
        }

        public RusWord GetRusWord(string name)
        {
#if SQLITE
            string srbname = SQLiteData.RunCommandString("select srbname from ruswords where name = '" + name + "'");
#else
            string srbname = ADSData.RunCommandString("select srbname from ruswords where name = '" + name + "'");
#endif
            return new RusWord(name, srbname);
        }

        public void InsertRusWord(RusWord rusWord)
        {
            string cmd = "insert into ruswords (name, srbname) values ('" + rusWord.name + "', '" + rusWord.srbname + "')";
#if SQLITE
            SQLiteData.RunCommand(cmd);
#else
            ADSData.RunCommand(cmd);
#endif
        }

        public void UpdateRusWord(RusWord rusWord)
        {
            string cmd = "update ruswords set srbname='" + rusWord.srbname + "' where name='" + rusWord.name + "'";
#if SQLITE
            SQLiteData.RunCommand(cmd);
#else
            ADSData.RunCommand(cmd);
#endif
        }

        public void DeleteRusWord(RusWord rusWord)
        {
            string cmd = "delete from ruswords where name='" + rusWord.name + "'";
#if SQLITE
            SQLiteData.RunCommand(cmd);
#else
            ADSData.RunCommand(cmd);
#endif
        }

        public void StartTransaction()
        {
#if SQLITE
            SQLiteData.StartTransaction();
#else
            ADSData.StartTransaction();
#endif
        }

        public void CommitTransaction()
        {
#if SQLITE
            SQLiteData.CommitTransaction();
#else
            ADSData.CommitTransaction();
#endif
        }

        public void RollbackTransaction()
        {
#if SQLITE
            SQLiteData.RollbackTransaction();
#else
            ADSData.RollbackTransaction();
#endif
        }

        public void UpdateChanges(int changeId)
        {
            string cmd = "update changes set change_id=" + changeId.ToString() + " where id=0";
#if SQLITE
            SQLiteData.RunCommand(cmd);
#else
            ADSData.RunCommand(cmd);
#endif
        }

        public bool LetterFind(string name)
        {
#if SQLITE
            Int64 result = SQLiteData.RunCommandScalarInt("select count(*) cnt from letters where letter = '" + name + "'");
#else
            Int64 result = ADSData.RunCommandScalarInt("select count(*) cnt from letters where letter = '" + name + "'");
#endif
            if (result == 1)
            {
                return true;
            }
            return false;
        }

        public Letters GetLetterValue(string letter)
        {
#if SQLITE
            string value = SQLiteData.RunCommandString("select value from letters where letter = '" + letter + "'");
#else
            string value = ADSData.RunCommandString("select value from letters where letter = '" + letter + "'");
#endif
            return new Letters(letter, value);
        }

        public void InsertLetters(Letters letters)
        {
            string cmd = "insert into letters (letter, value) values ('" + letters.letter + "', '" + letters.value + "')";
#if SQLITE
            SQLiteData.RunCommand(cmd);
#else
            ADSData.RunCommand(cmd);
#endif
        }

        public void UpdateLetters(Letters letters)
        {
            string cmd = "update letters set value='" + letters.value + "' where letter='" + letters.letter + "'";
#if SQLITE
            SQLiteData.RunCommand(cmd);
#else
            ADSData.RunCommand(cmd);
#endif
        }

        public void DeleteLetters(Letters letters)
        {
            string cmd = "delete from letters where letter='" + letters.letter + "'";
#if SQLITE
            SQLiteData.RunCommand(cmd);
#else
            ADSData.RunCommand(cmd);
#endif
        }
    }
}
