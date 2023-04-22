using System;
using System.Collections.Generic;

namespace Pairs_Bajan_Ramona
{
    public class User
    {
        public string username { get; set; }
        public string image { get; set; }
        public int games { get; set; }
        public int wins { get; set; }
        public int width { set; get; }
        public int height { set; get; }
        public int level { set; get; }
        public List<List<Tile>> configuratie { set; get; }
        public int time { set; get; }

        public User(string username, string image, int games, int wins, int width, int height, int level, List<List<Tile>> configuratie, int time)
        {
            this.username = username;
            this.image = image;
            this.games = games;
            this.wins = wins;
            this.width = width;
            this.height = height;
            this.level = level;
            this.configuratie = configuratie;
            this.time = time;
        }

        public User(User t)
        {
            username = t.username;
            image = t.image;
            games = t.games;
            wins = t.wins;
            width = t.width;
            height = t.height;
            level = t.level;
            configuratie = t.configuratie;
            time = t.time;
        }

        public User()
        {
            username = "";
            image = "i1.jpg";
            games = 0;
            wins = 0;
            width = 5;
            height = 5;
            level = 1;
            time = 60;
            prepare();
        }

        public static List<int> GenerateRandomNumbers(int n)
        {
            List<int> numbers = new List<int>();

            for (int i = 1; i <= n; i++)
            {
                numbers.Add(i);
                numbers.Add(i);
            }

            Random rand = new Random();
            for (int i = 0; i < numbers.Count; i++)
            {
                int r = rand.Next(i, numbers.Count);
                int temp = numbers[i];
                numbers[i] = numbers[r];
                numbers[r] = temp;
            }

            return numbers;
        }

        public void prepare()
        {
            configuratie = new List<List<Tile>>();
            int pieceNumbers;
            if (width * height % 2 == 1)
            {
                pieceNumbers = ((width * height) - 1) / 2;
                List<int> numbers = GenerateRandomNumbers(pieceNumbers);

                for (int i = 0; i < width; i++)
                {
                    List<Tile> aux = new List<Tile>();
                    for (int j = 0; j < height; j++)
                    {
                        if (i != (width / 2) || j != (height / 2))
                        {
                            Tile piece = new Tile();
                            piece.Image = $"i{numbers[numbers.Count - 1]}.jpg";
                            piece.Flip = false;
                            piece.Done = false;
                            piece.imageInBinding = "i0.jpg";
                            numbers.RemoveAt(numbers.Count - 1);
                            aux.Add(piece);
                        }
                    }
                    configuratie.Add(aux);
                }
            }
            else
            {
                pieceNumbers = width * height / 2;
                List<int> numbers = GenerateRandomNumbers(pieceNumbers);

                for (int i = 0; i < width; i++)
                {
                    List<Tile> aux = new List<Tile>();
                    for (int j = 0; j < height; j++)
                    {
                        Tile piece = new Tile();
                        piece.Image = $"i{numbers[numbers.Count - 1]}.jpg";
                        piece.Flip = false;
                        piece.Done = false;
                        piece.imageInBinding = "i0.jpg";
                        numbers.RemoveAt(numbers.Count - 1);
                        aux.Add(piece);
                    }
                    configuratie.Add(aux);
                }
            }
        }
    }
}
