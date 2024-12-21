using System;
using System.Collections.Generic;
using System.Text;

namespace SRWords
{
    public interface IRepository
    {
        int IsExistsWord(string wordName);

        void InsertWord(SrbWord word);

        void UpdateWord(SrbWord word);

        void DeleteWord(SrbWord word);

        int IsExistsRusWord(string wordName);

        RusWord GetRusWord(string name);

        void InsertRusWord(RusWord rusWord);

        void UpdateRusWord(RusWord rusWord);

        void DeleteRusWord(RusWord rusWord);

        int IsExistsRusRef(RusRef rusRef);

        void InsertRusRef(RusRef rusRef);

        void DeleteRusRef(RusRef rusRef);

        void StartTransaction();

        void CommitTransaction();

        void RollbackTransaction();

        void UpdateChanges(int changeId);

        /// <summary>
        /// Поиск слова в столбце таблицы LETTERS.letter
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool LetterFind(string name);

        Letters GetLetterValue(string letter);

        void InsertLetters(Letters letters);

        void UpdateLetters(Letters letters);

        void DeleteLetters(Letters letters);
    }
}
