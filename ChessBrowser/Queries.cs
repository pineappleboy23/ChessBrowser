using Microsoft.Maui.Controls;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

/*
  Author: Daniel Kopta and ...
  Chess browser backend 
*/

namespace ChessBrowser
{
    internal class Queries
    {
        //test

        /// <summary>
        /// This function runs when the upload button is pressed.
        /// Given a filename, parses the PGN file, and uploads
        /// each chess game to the user's database.
        /// </summary>
        /// <param name="PGNfilename">The path to the PGN file</param>
        internal static async Task InsertGameData(string PGNfilename, MainPage mainPage)
        {
            // This will build a connection string to your user's database on atr,
            // assuimg you've typed a user and password in the GUI
            string connection = mainPage.GetConnectionString();



            var reader = new PgnReader();
            var games = reader.ParsePgn(PGNfilename);

            // TODO:
            //       Load and parse the PGN file
            //       We recommend creating separate libraries to represent chess data and load the file

            // TODO:
            //       Use this to tell the GUI's progress bar how many total work steps there are
            //       For example, one iteration of your main upload loop could be one work step
            //       mainPage.SetNumWorkItems( ... );
            mainPage.SetNumWorkItems(games.Count);


            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                try
                {
                    // Open a connection
                    conn.Open();

                    // Iterate through games
                    foreach (ChessGame game in games)
                    {
                        using (MySqlTransaction transaction = conn.BeginTransaction())
                        {
                            try
                            {
                                string[] queries = {
                        "INSERT IGNORE INTO Players (Name, Elo) VALUES (@BlackPlayer, @BlackElo) ON DUPLICATE KEY UPDATE Elo = GREATEST(Elo, @BlackElo)",
                        "INSERT IGNORE INTO Players (Name, Elo) VALUES (@WhitePlayer, @WhiteElo) ON DUPLICATE KEY UPDATE Elo = GREATEST(Elo, @WhiteElo)",
                        "INSERT IGNORE INTO Events(Name, Site, Date) VALUES (@Event, @Site, @Date)",
                        "INSERT INTO Games (Round, Result, Moves, BlackPlayer, WhitePlayer, eID) VALUES (@Round, @Result, @Moves, " +
                        "(SELECT pID FROM Players WHERE Name = @BlackPlayer), " +
                        "(SELECT pID FROM Players WHERE Name = @WhitePlayer), " +
                        "(SELECT eID FROM Events WHERE Name = @Event AND Site = @Site AND Date = @Date))"
                    };

                                foreach (string query in queries)
                                {
                                    using (MySqlCommand command = new MySqlCommand(query, conn, transaction))
                                    {
                                        command.Parameters.AddWithValue("@BlackPlayer", game.Black);
                                        command.Parameters.AddWithValue("@WhitePlayer", game.White);
                                        command.Parameters.AddWithValue("@BlackElo", game.BlackElo);
                                        command.Parameters.AddWithValue("@WhiteElo", game.WhiteElo);
                                        command.Parameters.AddWithValue("@Event", game.Event);
                                        command.Parameters.AddWithValue("@Site", game.Site);
                                        command.Parameters.AddWithValue("@Date", game.Date);
                                        command.Parameters.AddWithValue("@Round", game.Round);
                                        command.Parameters.AddWithValue("@Result", game.Result);
                                        command.Parameters.AddWithValue("@Moves", game.Moves);

                                        command.ExecuteNonQuery();
                                    }
                                }

                                transaction.Commit();

                                // Notify GUI that one work step has completed
                                await mainPage.NotifyWorkItemCompleted();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                System.Diagnostics.Debug.WriteLine("Transaction failed: " + ex.Message);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("An error occurred: " + e.Message);
                }
            }

        }

        /// <summary>
        /// Queries the database for games that match all the given filters.
        /// The filters are taken from the various controls in the GUI.
        /// </summary>
        /// <param name="white">The white player, or null if none</param>
        /// <param name="black">The black player, or null if none</param>
        /// <param name="opening">The first move, e.g. "1.e4", or null if none</param>
        /// <param name="winner">The winner as "W", "B", "D", or null if none</param>
        /// <param name="useDate">True if the filter includes a date range, False otherwise</param>
        /// <param name="start">The start of the date range</param>
        /// <param name="end">The end of the date range</param>
        /// <param name="showMoves">True if the returned data should include the PGN moves</param>
        /// <returns>A string separated by newlines containing the filtered games</returns>
        internal static string PerformQuery(string white, string black, string opening, string winner, bool useDate, DateTime start, DateTime end, bool showMoves, MainPage mainPage)
            {
                // This will build a connection string to your user's database on atr,
                // assuimg you've typed a user and password in the GUI
                //return "server=atr.eng.utah.edu;database=" + database.Text + ";uid=" + username.Text + ";password=" + password.Text;
                string connection = mainPage.GetConnectionString();

                // Build up this string containing the results from your query
                string parsedResult = "";

                // Use this to count the number of rows returned by your query
                // (see below return statement)
                int numRows = 0;



                using (MySqlConnection conn = new MySqlConnection(connection))
                {
                    try
                    {
                        // Open a connection
                        conn.Open();



                        MySqlCommand command = conn.CreateCommand();

                        command.CommandText = "SELECT * FROM Games " +
                            "JOIN Players p1 ON Games.WhitePlayer = p1.pID " +
                            "JOIN Players p2 ON Games.BlackPlayer = p2.pID " +
                            "JOIN Events ON Games.eID = Events.eID";

                        if (white != null)
                        {
                            command.CommandText += " AND p1.Name = @varwh";
                            command.Parameters.AddWithValue("@varwh", white);
                        }
                        if (black != null)
                        {
                            command.CommandText += " AND p2.Name = @varwb";
                            command.Parameters.AddWithValue("@varwb", black); // Corrected parameter name
                        }
                        if (opening != null)
                        {
                            command.CommandText += " AND Games.Moves LIKE @varo";
                            command.Parameters.AddWithValue("@varo", opening + "%"); // Concatenate wildcard with parameter value
                        }
                        if (winner != null)
                        {
                            command.CommandText += " AND Games.Result = @varw";
                            command.Parameters.AddWithValue("@varw", winner);
                        }
                        if (useDate)
                        {
                            command.CommandText += " AND Events.Date BETWEEN @varsd AND @vared";
                            command.Parameters.AddWithValue("@varsd", start);
                            command.Parameters.AddWithValue("@vared", end);
                        }

                        command.CommandText += ";";


                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                numRows += 1;

                                parsedResult += "Event: ";
                                parsedResult += reader[12];

                                parsedResult += "\nSite: ";
                                parsedResult += reader[13];

                                parsedResult += "\nDate: ";
                                parsedResult += reader[14];

                                parsedResult += "\nWhite: ";
                                parsedResult += reader[6];
                                parsedResult += " (";
                                parsedResult += reader[7];
                                parsedResult += ")";


                                parsedResult += "\nBlack: ";
                                parsedResult += reader[9];
                                parsedResult += " (";
                                parsedResult += reader[10];
                                parsedResult += ")";

                                parsedResult += "\nResult: ";
                                parsedResult += reader[1];

                                if (showMoves)
                                {
                                    parsedResult += "\n ";
                                    parsedResult += reader[2];
                                }

                                parsedResult += "\n ";
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }
                }

                return numRows + " results\n" + parsedResult;
            }
        }
    }

