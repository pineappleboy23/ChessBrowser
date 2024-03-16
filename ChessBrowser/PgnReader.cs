using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChessBrowser
{ 
    public class PgnReader
    {
        public List<ChessGame> ReadGames(string filePath)
        { 
            List<ChessGame> games = new List<ChessGame>();
            ChessGame currentGame = null;

            foreach (var line in File.ReadLines(filePath))
            {
                if (line.StartsWith("[Event"))
                {
                    // Start of a new game
                    if (currentGame != null)
                    {
                        games.Add(currentGame);
                    }
                    currentGame = new ChessGame();
                }
                else if (currentGame != null && line != "")
                {
                    // Extract game information
                    if (line.StartsWith("["))
                    {
                        string[] parts = ExtractContentInQuotes(line);
                        switch (parts[0])
                        {
                            case "Event": currentGame.Event = parts[1]; break;
                            case "Site": currentGame.Site = parts[1]; break;
                            case "Date": currentGame.Date = parts[1]; break;
                            case "Round": currentGame.Round = parts[1]; break;
                            case "White": currentGame.White = parts[1]; break;
                            case "Black": currentGame.Black = parts[1]; break;
                            case "Result": currentGame.Result = parts[1]; break;
                            case "WhiteElo": currentGame.WhiteElo = parts[1]; break;
                            case "BlackElo": currentGame.BlackElo = parts[1]; break;
                            case "ECO": currentGame.ECO = parts[1]; break;
                            case "EventDate": currentGame.EventDate = parts[1]; break;
                        }
                    }
                    else
                    {
                        // Moves
                        currentGame.Moves.Add(line);
                    }
                }
            }

            // Add the last game
            if (currentGame != null)
            {
                games.Add(currentGame);
            }

            return games;
        }
        private string[] ExtractContentInQuotes(string line)
        {
            List<string> result = new List<string>();
            Regex regex = new Regex("\"(.*?)\"");
            MatchCollection matches = regex.Matches(line);
            foreach (Match match in matches)
            {
                result.Add(match.Groups[1].Value);
            }
            return result.ToArray();
        }
    }
}
