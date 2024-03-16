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
        public ChessGame PgnReader(string filePath)
        {
            string pattern = "'([^']*)'";
            Regex regex = new Regex(pattern);
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    if (line.StartsWith("["))
                    {
                        MatchCollection match = regex.Matches(line);
                    }
                }
            }

    }

    }
}
