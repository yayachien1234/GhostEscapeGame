using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

struct Map
{
    public bool IsGhost;  // 檢查該位置是否為鬼
    public bool IsExplored;  // 檢查這位置是否已經被探索過
    public int SurroundingGhosts;  // 周圍8格鬼的數量
}

namespace E94106012_practice_2_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int mapWidth, mapHeight;  // 空間大小
            int ghostCount;  // 鬼的數量

            Console.WriteLine("設定遊戲參數");
            Console.Write("輸入地圖的寬度和高度: ");
            string mapSize = Console.ReadLine();  // M,N
            Console.Write("輸入鬼的數量: ");
            ghostCount = int.Parse(Console.ReadLine());

            string[] mapSizes = mapSize.Split(',');
            mapWidth = int.Parse(mapSizes[0]);
            mapHeight = int.Parse(mapSizes[1]);

            if (ghostCount < mapWidth * mapHeight)
            {
                if (ghostCount <= mapWidth * mapHeight)
                {
                    string inputPosition;  // 欲查看的位置(0,A)
                    int positionX, positionY;  // 座標
                    Map[,] map = new Map[mapWidth, mapHeight];

                    Console.Clear();

                    PrintMap(mapWidth, mapHeight);

                    // 第一次輸入，生成地圖
                    do
                    {
                        Console.Write("輸入要查看的位置: ");
                        inputPosition = Console.ReadLine();
                        string[] inputs = inputPosition.Split(',');
                        positionX = int.Parse(inputs[0]);
                        positionY = GetPositionY(inputs[1]);

                        if (positionX < mapWidth && positionY < mapHeight)
                        {
                            Console.Clear();

                            GenerateMap(ref map, ghostCount, positionX, positionY);
                            PrintMap(ref map, positionX, positionY);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("無效的輸入，請再試一次");
                        }
                    } while (true);

                    int gameEnd = (mapWidth * mapHeight) - ghostCount;  // 非鬼的格數
                    int count = 1;

                    do
                    {
                        Console.Write("輸入要查看的位置: ");
                        inputPosition = Console.ReadLine();
                        string[] inputs = inputPosition.Split(',');
                        positionX = int.Parse(inputs[0]);
                        positionY = GetPositionY(inputs[1]);

                        if (positionX < mapWidth && positionY < mapHeight && !map[positionX, positionY].IsExplored)
                        {

                            Console.Clear();

                            count++;

                            if (map[positionX, positionY].IsGhost)
                            {
                                PrintMap(map);
                                Console.WriteLine("遊戲結束! 你被鬼抓到了");
                                break;
                            }
                            else if (count == gameEnd)
                            {
                                PrintMap(map);
                                Console.WriteLine("遊戲結束! 你成功躲避所有的鬼了");
                                break;
                            }
                            else
                            {
                                PrintMap(ref map, positionX, positionY);
                            }

                        }
                        else
                        {
                            Console.WriteLine("無效的輸入，請再試一次");
                        }
                    } while (true);

                }
                else
                {
                    Console.WriteLine("遊戲參數錯誤!");
                }
            }

            Console.ReadKey();

        }

        private static int GetPositionY(string position)
        {
            char posY = position[0];
            return Convert.ToInt32(posY) - Convert.ToInt32('A');
        }

        private static void GenerateMap(ref Map[,] map, int ghostCount, int positionX, int positionY)
        {
            for (int i = 0; i < map.GetLength(0); i++)  // 初始化地圖
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j].IsGhost = false;
                    map[i, j].IsExplored = false;
                }
            }

            Random rand = new Random();
            int count = 0;

            while (count < ghostCount)
            {
                int randomX, randomY;  // 鬼的位置
                randomX = rand.Next(map.GetLength(0));
                randomY = rand.Next(map.GetLength(1));

                if ((!map[randomX, randomY].IsGhost) && !(randomX == positionX && randomY == positionY)) //檢查該位置是否已經是鬼或初始位置 
                {
                    map[randomX, randomY].IsGhost = true;
                    count++;
                }

            };

            for (int i = 0; i < map.GetLength(0); i++)  //計算單格周圍鬼的數量
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (!map[i, j].IsGhost)
                    {
                        map[i, j].SurroundingGhosts = 0;

                        if (i > 0)
                        {
                            if (j > 0)
                            {
                                if (map[i - 1, j - 1].IsGhost)    // @ - -
                                {                               // - * -
                                    map[i, j].SurroundingGhosts++;            // - - -
                                }
                            }

                            if (j < map.GetLength(1) - 1)
                            {
                                if (map[i - 1, j + 1].IsGhost)    // - - @
                                {                               // - * -
                                    map[i, j].SurroundingGhosts++;            // - - -
                                }
                            }

                            if (map[i - 1, j].IsGhost)            // - @ -
                            {                                   // - * -
                                map[i, j].SurroundingGhosts++;                // - - -
                            }

                        }

                        if (i < map.GetLength(0) - 1)
                        {
                            if (j > 0)
                            {
                                if (map[i + 1, j - 1].IsGhost)    // - - -
                                {                               // - * -
                                    map[i, j].SurroundingGhosts++;            // @ - -
                                }
                            }

                            if (j < map.GetLength(1) - 1)
                            {
                                if (map[i + 1, j + 1].IsGhost)    // - - -
                                {                               // - * -
                                    map[i, j].SurroundingGhosts++;            // - - @
                                }
                            }

                            if (map[i + 1, j].IsGhost)            // - - -
                            {                                   // - * -
                                map[i, j].SurroundingGhosts++;                // - @ -
                            }

                        }

                        if (j > 0)
                        {
                            if (map[i, j - 1].IsGhost)            // - - -
                            {                                   // @ * -
                                map[i, j].SurroundingGhosts++;                // - - -
                            }
                        }

                        if (j < map.GetLength(1) - 1)
                        {
                            if (map[i, j + 1].IsGhost)            // - - -
                            {                                   // - * @
                                map[i, j].SurroundingGhosts++;                // - - -
                            }
                        }

                    }

                }
            }
        }

        private static void PrintMap(int width, int height)
        {
            Console.Write("   ");

            for (int i = 0; i < height; i++)
            {
                char posY = (char)(Convert.ToInt32('A') + i);
                Console.Write($"{posY} ");
            }

            Console.WriteLine();

            for (int i = 0; i < width; i++)
            {
                Console.Write($"{i}  ");

                for (int j = 0; j < height; j++)
                {
                    Console.Write("- ");
                }

                Console.WriteLine();
            }

        }

        private static void PrintMap(Map[,] map)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);

            Console.Write("   ");

            for (int i = 0; i < height; i++)
            {
                char posY = (char)(Convert.ToInt32('A') + i);
                Console.Write($"{posY} ");
            }

            Console.WriteLine();

            for (int i = 0; i < width; i++)
            {
                Console.Write($"{i}  ");

                for (int j = 0; j < height; j++)
                {
                    if (map[i, j].IsGhost)
                    {
                        Console.Write("X ");
                    }
                    else
                    {
                        Console.Write($"{map[i, j].SurroundingGhosts} ");
                    }
                }

                Console.WriteLine();
            }
        }

        private static void PrintMap(ref Map[,] map, int positionX, int positionY)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);

            map[positionX, positionY].IsExplored = true;

            Console.Write("   ");

            for (int i = 0; i < height; i++)
            {
                char posY = (char)(Convert.ToInt32('A') + i);
                Console.Write($"{posY} ");
            }

            Console.WriteLine();

            for (int i = 0; i < width; i++)
            {
                Console.Write($"{i}  ");

                for (int j = 0; j < height; j++)
                {
                    if (map[i, j].IsExplored)
                    {
                        Console.Write($"{map[i, j].SurroundingGhosts} ");
                    }
                    else
                    {
                        Console.Write("- ");
                    }
                }

                Console.WriteLine();
            }
        }
    }
}
