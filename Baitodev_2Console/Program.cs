using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Baitodev_2Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcom to Hangman Game!");
            Console.WriteLine("if want stop game write (exit)");
            Console.WriteLine("");
            Console.WriteLine("");
            String UserWorld = string.Empty;
            while (!UserWorld.ToLower().Equals("exit"))
            {
                Console.WriteLine("Insert your number of attempts?");
                int attempts;
                while (!int.TryParse(Console.ReadLine(), out attempts))
                {
                    Console.WriteLine("Insert a valid number of attempts?");
                }

                Console.WriteLine("insert a number to play (0-9)");


                var WorldUrl = "http://localhost:22058/api/word/" + Console.ReadLine();
                using (var http = new HttpClient())
                {
                    var response = await http.GetStringAsync(WorldUrl);
                    var WorlGet = JsonConvert.DeserializeObject<List<SelectedWord>>(response);
                    foreach (var p in WorlGet)
                    {
                        char[] GameWorl = new char[p.wordLenght];
                        for (int i = 0; i < p.wordLenght; i++)
                        {
                            GameWorl[i] = ('_');
                        }
                        Console.WriteLine($"you word is: {new String(GameWorl)}");
                    }
                    String Move = Console.ReadLine();
                    var ResultUrl = ($"{WorldUrl}/{Move}");
                    using (var httpGame = new HttpClient())
                    {
                        var Gameresponse = await http.GetStringAsync(ResultUrl);
                        var GameGet = JsonConvert.DeserializeObject<List<MoveResult>>(response);
                        foreach (var G in GameGet)
                        {
                            if (G.isWin) { Console.WriteLine("you Win!"); }
                        }
                    }


                }






            }


        }
    }
}
