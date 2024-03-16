using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChessBrowser
{
    internal class PgnReader
    {
        public List<ChessGame> ReadGames(string filePath)
        {
            string pattern = "'([^']*)'";
            Regex regex = new Regex(pattern);
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    if (line.StartsWith("[E"))
                    {
                        MatchCollection match = regex.Matches(line);
                    }
                }
            }
            catch (Exception ex)
            {

            }
    }

    }
}
