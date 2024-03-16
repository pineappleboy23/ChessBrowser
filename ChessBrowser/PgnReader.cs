using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static ObjCRuntime.Dlfcn;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChessBrowser
{ 
    public class PgnReader
    {
        public List<ChessGame> ReadGames(string filePath)
        {
            List<ChessGame> games = new List<ChessGame>();

            try
            {
                string[] lines = File.ReadAllLines(filePath);
                List<List<string>> blocks = new List<List<string>>();
                List<string> currentBlock = new List<string>();

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        if (currentBlock.Count > 0)
                        {
                            blocks.Add(currentBlock);
                            currentBlock = new List<string>();
                        }
                    }
                    else
                    {
                        currentBlock.Add(line);
                    }
                }

                if (currentBlock.Count > 0)
                {
                    blocks.Add(currentBlock);
                }

                foreach (List<string> block in blocks)
                {
                    ChessGame game = new ChessGame();

                    // Extract data from the block
                    foreach (string line in block)
                    {
                        if (line.StartsWith("[Event "))
                            game.Event = GetValueFromTag(line);
                        else if (line.StartsWith("[Site "))
                            game.Site = GetValueFromTag(line);
                        else if (line.StartsWith("[Date "))
                            game.Date = GetValueFromTag(line);
                        else if (line.StartsWith("[Round "))
                            game.Round = GetValueFromTag(line);
                        else if (line.StartsWith("[White "))
                            game.White = GetValueFromTag(line);
                        else if (line.StartsWith("[Black "))
                            game.Black = GetValueFromTag(line);
                        else if (line.StartsWith("[Result "))
                            game.Result = GetValueFromTag(line);
                        else if (line.StartsWith("[WhiteElo "))
                            game.WhiteElo = GetValueFromTag(line);
                        else if (line.StartsWith("[BlackElo "))
                            game.BlackElo = GetValueFromTag(line);
                        else if (line.StartsWith("[EventDate "))
                            game.EventDate = GetValueFromTag(line);
                    }
                    if (GamesCheck(game))
                    {
                        games.Add(game);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return games;
        }

        private string GetValueFromTag(string line)
        {
            int startIndex = line.IndexOf('"') + 1;
            int endIndex = line.LastIndexOf('"');
            return line.Substring(startIndex, endIndex - startIndex);
        }

        private bool GamesCheck(ChessGame game)
        {
            if (string.IsNullOrEmpty(game.Event) ||
                string.IsNullOrEmpty(game.Site) ||
                string.IsNullOrEmpty(game.Date) ||
                string.IsNullOrEmpty(game.Round) ||
                string.IsNullOrEmpty(game.White) ||
                string.IsNullOrEmpty(game.Black) ||
                string.IsNullOrEmpty(game.Result) ||
                string.IsNullOrEmpty(game.WhiteElo) ||
                string.IsNullOrEmpty(game.BlackElo) ||
                string.IsNullOrEmpty(game.EventDate))
                {
                    return false;
                }
            else
            {
                return true;
            }
        }
    }
}
