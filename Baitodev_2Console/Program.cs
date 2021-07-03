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
                if (attempts <= 0){ attempts = 1; }

                Console.WriteLine("insert a number to play (0-9)");


                var WorldUrl = "http://localhost:22058/api/word/" + Console.ReadLine();
                using (var http = new HttpClient())
                {
                    var response = await http.GetStringAsync(WorldUrl);
                    var WorlGet = JsonConvert.DeserializeObject<List<SelectedWord>>(response);
                    char[] GameWorl = new char[0];
                    int WinersMove = 0;

                    foreach (var p in WorlGet)
                    {
                        GameWorl = new char[p.wordLenght];
                        for (int i = 0; i < p.wordLenght; i++)
                        {
                            GameWorl[i] = ('_');
                        }
                        Console.WriteLine($"you word is: {new String(GameWorl)}");
                    }

                    while (attempts != 0)
                    {
                        String Move = Console.ReadLine();
                        
                        var ResultUrl = ($"{WorldUrl}/{Move}");
                        using (var httpGame = new HttpClient())
                        {
                            var Gameresponse = await http.GetStringAsync(ResultUrl);
                            var GameGet = JsonConvert.DeserializeObject<List<MoveResult>>(Gameresponse);
                            foreach (var G in GameGet)
                            {
                                if (G.isWin) { Console.WriteLine("you Win!"); attempts = 0; }
                                else
                                {
                                    if (G.isCorrect)
                                    {
                                        char change = Convert.ToChar(Move);
                                        foreach (var item in G.wordPosition)
                                        {
                                            GameWorl[item] = change;
                                            WinersMove++;
                                        }
                                        Console.WriteLine($"you word is: {new String(GameWorl)}");
                                    }
                                    else
                                    {
                                        Console.WriteLine("try again!");
                                        Console.WriteLine($"you word is: {new String(GameWorl)}");
                                    }
                                }
                            }

                        }
                        if (WinersMove >= GameWorl.Length) { Console.WriteLine("you Win!"); break; }
                        attempts--;
                        if (attempts == 0) { Console.WriteLine("you lose!"); }
                    }

                }






            }


        }
    }
}
