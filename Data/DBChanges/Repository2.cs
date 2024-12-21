using System;
using System.Collections.Generic;
using System.Text;

namespace SRWords
{
    public class Repository2 : IRepository
    {
        public int IsExistsWord(string wordName)
        {
            Int64 result = ADSData.RunCommandScalarInt("select count(*) cnt from words where name = '" + wordName + "'");

            if (result == 1)
            {
                return 1;
            }
            return 0;
        }

        public void InsertWord(SrbWord word)
        {
            string cmd = "insert into words (name, stress, kw, xml, name_cyr) values ('" + word.name + "','" + word.stress + "','" + word.kw + "','" + word.xml + "','" +
                Utils.LatToCyr(word.name) + "')";
            ADSData.RunCommand(cmd);
        }

        public void UpdateWord(SrbWord word)
        {
            string cmd = "update words set xml='" + word.xml + "', kw='" + word.kw + "', stress='" + word.stress + "', name_cyr='" + Utils.LatToCyr(word.name) + "' where name='" + word.name + "'";
            ADSData.RunCommand(cmd);
        }

        public void DeleteWord(SrbWord word)
        {
            string cmd = "delete from words where name='" + word.name + "'";
            ADSData.RunCommand(cmd);
        }

        public int IsExistsRusWord(string wordName)
        {
            Int64 result = ADSData.RunCommandScalarInt("select count(*) cnt from ruswords where name = '" + wordName + "'");
            if (result == 1)
            {
                return 1;
            }
            return 0;
        }

        public RusWord GetRusWord(string name)
        {
            string srbname = ADSData.RunCommandString("select srbname from ruswords where name = '" + name + "'");
            return new RusWord(name, srbname);
        }

        public void InsertRusWord(RusWord rusWord)
        {
            string cmd = "insert into ruswords (name, stress, srbname) values ('" + rusWord.name + "', '" + rusWord.stress + "', '" + rusWord.srbname + "')";
            ADSData.RunCommand(cmd);
        }

        public void UpdateRusWord(RusWord rusWord)
        {
            string cmd = "update ruswords set srbname='" + rusWord.srbname + "' where name='" + rusWord.name + "'";
            ADSData.RunCommand(cmd);
        }

        public void DeleteRusWord(RusWord rusWord)
        {
            string cmd = "delete from ruswords where name='" + rusWord.name + "'";
            ADSData.RunCommand(cmd);
        }

        public int IsExistsRusRef(RusRef rusRef)
        {
            Int64 result = ADSData.RunCommandScalarInt("select count(*) cnt from rusref where name='" + rusRef.name + "' and stress='" + rusRef.stress + "' and srbname='" + rusRef.srbname + "'");

            if (result == 1)
            {
                return 1;
            }
            return 0;
        }

        public void InsertRusRef(RusRef rusRef)
        {
            string cmd = "insert into rusref (name, stress, srbname, kw) values ('" + rusRef.name + "', '" + rusRef.stress + "', '" + rusRef.srbname + "', '" + rusRef.kw + "')";
            ADSData.RunCommand(cmd);
        }

        public void DeleteRusRef(RusRef rusRef)
        {
            string cmd = "delete from rusref where name='" + rusRef.name + "' and stress='" + rusRef.stress + "' and srbname='" + rusRef.srbname + "'";
            ADSData.RunCommand(cmd);
        }

        public void StartTransaction()
        {
            ADSData.StartTransaction();
        }

        public void CommitTransaction()
        {
            ADSData.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            ADSData.RollbackTransaction();
        }

        public void UpdateChanges(int changeId)
        {
            string cmd = "update changes set change_id=" + changeId.ToString() + " where id=0";
            ADSData.RunCommand(cmd);
        }

        public bool LetterFind(string name)
        {
            Int64 result = ADSData.RunCommandScalarInt("select count(*) cnt from letters where letter = '" + name + "'");

            if (result == 1)
            {
                return true;
            }
            return false;
        }

        public Letters GetLetterValue(string letter)
        {
            string value = ADSData.RunCommandString("select value from letters where letter = '" + letter + "'");
            return new Letters(letter, value);
        }

        public void InsertLetters(Letters letters)
        {
            string cmd = "insert into letters (letter, value) values ('" + letters.letter + "', '" + letters.value + "')";
            ADSData.RunCommand(cmd);
        }

        public void UpdateLetters(Letters letters)
        {
            string cmd = "update letters set value='" + letters.value + "' where letter='" + letters.letter + "'";
            ADSData.RunCommand(cmd);
        }

        public void DeleteLetters(Letters letters)
        {
            string cmd = "delete from letters where letter='" + letters.letter + "'";
            ADSData.RunCommand(cmd);
        }
    }
}
