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
        public DateTime Date { get; set; }
        public string Round { get; set; }
        public string White { get; set; }
        public string Black { get; set; }
        public int WhiteElo { get; set; }
        public int BlackElo { get; set; }
        public string Result { get; set; }
        public DateTime EventDate { get; set; }
        public string Moves { get; set; }
        public string ECO { get; set; }
        public int eid { get; set; }
        public int wpID { get; set; }
        public int bpID { get; set; }
        public ChessGame()
        {
            Moves = new List<string>();
        }

    }
}
