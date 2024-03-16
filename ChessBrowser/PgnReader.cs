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
        public List<ChessGame> ParsePgn(string filePath)
        {
            List<ChessGame> games = new List<ChessGame>();
            ChessGame currentGame = null;

            List<string> moves = new List<string>();
            foreach (var line in File.ReadLines(filePath))
            {
                if (line.StartsWith("[Event "))
                {
                    // Start of a new game
                    if (currentGame != null)
                    {

                        currentGame.Moves = string.Join(" ", moves);
                        games.Add(currentGame);

                        moves = new List<string>();
                    }
                    currentGame = new ChessGame();
                }
                if (!string.IsNullOrWhiteSpace(line))
                {
                    // Extract game information

                    if (line.StartsWith("[EventD"))
                    {
                        currentGame.EventDate = DateTime.Parse(ExtractContentInQuotes(line));
                    }
                    else if (line.StartsWith("[Event"))
                    {
                        currentGame.Event = ExtractContentInQuotes(line);
                    }
                    else if (line.StartsWith("[Site"))
                    {
                        currentGame.Site = ExtractContentInQuotes(line);
                    }
                    else if (line.StartsWith("[Date"))
                    {
                        currentGame.Date = DateTime.Parse(ExtractContentInQuotes(line));
                    }
                    else if (line.StartsWith("[Round"))
                    {
                        currentGame.Round = ExtractContentInQuotes(line);
                    }
                    else if (line.StartsWith("[WhiteElo"))
                    {
                        currentGame.WhiteElo = int.Parse(ExtractContentInQuotes(line));
                    }
                    else if (line.StartsWith("[BlackElo"))
                    {
                        currentGame.BlackElo = int.Parse(ExtractContentInQuotes(line));
                    }
                    else if (line.StartsWith("[White"))
                    {
                        currentGame.White = ExtractContentInQuotes(line);
                    }
                    else if (line.StartsWith("[Black"))
                    {
                        currentGame.Black = ExtractContentInQuotes(line);
                    }
                    else if (line.StartsWith("[Result"))
                    {
                        string result = ExtractContentInQuotes(line);
                        if (result == "0-1")
                        {
                            currentGame.Result = "B";
                        }
                        else if(result == "1-0")
                        {
                            currentGame.Result = "W";
                        }
                        else
                        {
                            currentGame.Result = "D";
                        }
                    }
                    
                    else if (line.StartsWith("[ECO"))
                    {
                        currentGame.ECO = ExtractContentInQuotes(line);
                    }
                    else
                    {
                        // Assume it's a move
                        moves.Add(line);
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

        private string ExtractContentInQuotes(string line)
        {
            int startIndex = line.IndexOf('"') + 1;
            int endIndex = line.LastIndexOf('"');
            return line.Substring(startIndex, endIndex - startIndex);
        }
    }

    
}
