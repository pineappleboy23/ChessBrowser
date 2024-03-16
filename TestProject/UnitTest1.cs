using ChessBrowser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SimpleGameParseTest()
        {

            string[] lines = {
            "[Event \"4. IIFL Wealth Mumbai Op\"]",
            "[Site \"Mumbai IND\"]",
            "[Date \"2018.12.31\"]",
            "[Round \"2.9\"]",
            "[White \"Sundararajan, Kidambi\"]",
            "[Black \"Ziatdinov, Raset\"]",
            "[Result \"1/2-1/2\"]",
            "[WhiteElo \"2458\"]",
            "[BlackElo \"2252\"]",
            "[ECO \"A25\"]",
            "[EventDate \"2018.12.30\"]",
            "",
            "1.c4 e5 2.Nc3 Nc6 3.e3 Nf6 4.a3 Be7 5.Nf3 O-O 6.Be2 d6 7.d4 exd4 8.Nxd4 Nxd4",
            "9.Qxd4 Be6 10.Nd5 c5 11.Nxe7+ Qxe7 12.Qh4 d5 13.cxd5 Bxd5 14.f3 Qe6",
            "15.O-O Nd7 16.Bd2 f5 17.Rac1 Rac8 18.Rfe1 Ne5 19.Bc3 Ng6 20.Qf2 Bb3 21.Bf1",
            "a6 22.Qg3 Qe7 23.Bd3 Rc6 24.Qf2 Re6 25.g3 h5 26.h4 b5 27.f4 Bd5 28.Be2 Kf7",
            "29.Bxh5 Rh8 30.Rcd1 Bb3 31.Bf3 Bxd1 32.Rxd1 Rd8 33.Rd5 Kg8 34.Rxf5 Rxe3",
            "35.Bd5+ Rxd5 36.Rxd5 Qe4 37.Rd1 Re2 38.Re1 Nxf4 39.Rxe2 Nxe2+ 40.Kh2 Nd4",
            "41.Qf4 Qxf4 42.gxf4 Nc6 43.Kg3 b4 44.Bd2 a5 45.Kf3 c4 46.Ke4 1/2-1/2"
            };

            string filePath = "oneGamePGN.pgn";

            // Write lines to the file.
            File.WriteAllLines(filePath, lines);

            var games = PgnReader.ReadGames(filePath);
            //List<ChessGame>

            ChessGame game1 = games[0];

            Assert.Equals(game1.Event, "4. IIFL Wealth Mumbai Op");
        }
    }
}