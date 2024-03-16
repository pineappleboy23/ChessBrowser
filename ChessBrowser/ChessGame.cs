using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBrowser
{
    public class ChessGame
    {
        string Event { get; set; }
        string Site { get; set; }
        string Round { get; set; }
        string White { get; set; }
        string Black { get; set; }
        string WhiteElo { get; set; }
        string BlackElo { get; set; }
        string Result { get; set; }
        string EventDate { get; set; }

        public ChessGame(string event_i, string site_i, string round_i, string white_i, string black_i, string white_elo, string black_elo, string result_i, string event_date)
        {
            Event = event_i;
            Site = site_i;
            Round = round_i;
            White = white_i;
            Black = black_i;
            WhiteElo = white_elo;
            BlackElo = black_elo;
            Result = result_i;
            EventDate = event_date;
        }

    }
}
