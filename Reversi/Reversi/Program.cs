using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi
{
    class Program
    {
        static void Main(string[] args)
        {
            int cursorX = 0;
            int cursorY = 0;
            int MAX_WEIGHT = 8;
            int MAX_HEIGHT = 8;

            while (true)
            {
                
                switch (Console.Read().ToString())
                {
                    case "w":
                        cursorY++;
                        break;
                    case "a":
                        cursorX--;
                        break;
                    case "s":
                        cursorY--;
                        break;
                    case "d":
                        cursorX++;
                        break;
                    default:
                        break;
                }
                Console.Clear();
                for (int y = 0; y < MAX_HEIGHT; y++)
                {
                    for (int x = 0; x < MAX_WEIGHT; x++)
                    {
                        if (cursorX == x && cursorY == y)
                        {
                            Console.Write("＠");
                        }
                        else
                        {
                            Console.Write("・");
                        }
                    }
                    Console.Write("\r\n");
                }
            }
            
            Console.ReadKey();
        }
    }
}
