using System;
using System.Collections.Generic;
using System.Text;

namespace SRWords
{
    public class UserDonation
    {
        public string date;
        public string donate;
        public string note;

        public UserDonation(string date, string donate, string note)
        {
            this.date = date;
            this.donate = donate;
            this.note = note;
        }
    }
}
