using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBrowser
{
    public class ChessGame
    {
        public string Event { get; set; }
        public string Site { get; set; }
        public string Date { get; set; }
        public string Round { get; set; }
        public string White { get; set; }
        public string Black { get; set; }
        public string WhiteElo { get; set; }
        public string BlackElo { get; set; }
        public string Result { get; set; }
        public string EventDate { get; set; }
        public string ECO { get; set; }

        public List<string> Moves { get; set; }
        public ChessGame()
        {
            Moves = new List<string>();
        }

    }
}
