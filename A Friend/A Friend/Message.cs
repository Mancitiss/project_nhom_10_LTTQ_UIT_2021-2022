using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_Friend
{
    public class Message
    {
        public string text;
        public string author;
        public DateTime time;

        // author = "" -> current user
        public Message(string author, string text, DateTime time)
        {
            this.author = author;
            this.text = text;
            this.time = time;
        }
    }
}
