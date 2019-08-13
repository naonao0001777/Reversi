using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi
{
    class Program
    {
        const int BOARD_WIDTH = 8;
        const int BOARD_HEIGHT = 8;

        enum Color {
            COLOR_NONE = -1,
            COLOR_BLACK = 0,
            COLOR_WHITE = 1
        }
        enum Direction
        {
            UP,
            UP_RIGHT,
            RIGHT,
            DOWN_RIGHT,
            DOWN,
            DOWN_LEFT,
            LEFT,
            UP_LEFT,
            DIRECTION_MAX
        }
        static void Main(string[] args)
        {
            int cursorX = 0;
            int cursorY = 0;

            int[,] cells = new int[BOARD_HEIGHT, BOARD_WIDTH];

            int turn = (int)Color.COLOR_BLACK;

            char[][] ColorNames = new char[2][];
            ColorNames[0] = new char[] { '黒' };
            ColorNames[1] = new char[] { '白' };

            int[,] direction = new int [BOARD_HEIGHT,BOARD_WIDTH];//[-1, 0]
            direction[-1,0] = (int)Direction.UP ; // UP


            //direction[-1, 1] = (int)Direction.UP_RIGHT; // UP_RIGHT
            //direction[0, 1] = (int)Direction.RIGHT;  // RIGHT
            //direction[1, 1] = (int)Direction.DOWN_RIGHT; // DOWN_RIGHT
            //direction[1, 0] = (int)Direction.DOWN; // DOWN
            //direction[1, -1] = (int)Direction.DOWN_LEFT; // DOWN_LEFT
            //direction[0, -1] = (int)Direction.LEFT; // LEFT
            //direction[-1, -1] = (int)Direction.UP_LEFT; // UP_LEFT


            // 石を置けないフラグ
            bool CantPut = false;

            // セル画面を初期化
            DrawingCells();

            while (true)
            {
                // 画面をクリア
                Console.Clear();

                // 盤面を描画
                DrawingBoard();
            }

            // 配置可否チェック処理
            bool CheckCanPut(int _turn, int _cursorX, int _cursorY)
            {
                if (cells[_cursorY,_cursorX] != (int)Color.COLOR_NONE)
                {
                    return false;
                }

                for (int i = 0;i < (int)Direction.DIRECTION_MAX;i++)
                {
                    int x = cursorX, y = cursorY;

                    switch (i)
                    {
                        case (int)Direction.UP:
                            if (cells[y-1, x] == (turn ^ 1))
                            {
                                for (int n = 2; n < BOARD_HEIGHT;n++)
                                {
                                    if(cells[y-n,x] == turn&&y>-1)
                                    {
                                        cells[y-n, x] = turn;
                                    }
                                }
                            }break;
                        default:
                            break;
                    }
                   
                }
                return true;
            }

            // 盤面を初期化する処理
            void DrawingCells()
            {
                for (int y = 0; y < BOARD_HEIGHT; y++)
                {
                    for (int x = 0; x < BOARD_WIDTH; x++)
                    {
                        if (y == 3 && x == 3)
                        {
                            cells[y, x] = (int)Color.COLOR_WHITE;
                        }
                        else if (y == 3 && x == 4)
                        {
                            cells[y, x] = (int)Color.COLOR_BLACK;
                        }
                        else if (y == 4 && x == 4)
                        {
                            cells[y, x] = (int)Color.COLOR_WHITE;
                        }
                        else if (y == 4 && x == 3)
                        {
                            cells[y, x] = (int)Color.COLOR_BLACK;
                        }
                        else
                        {
                            cells[y, x] = (int)Color.COLOR_NONE;
                        }
                    }
                }
            }

            // 盤面を描画する処理
            void DrawingBoard()
            {
                for (int y = 0; y < BOARD_HEIGHT; y++)
                {
                    for (int x = 0; x < BOARD_WIDTH; x++)
                    {
                        if (cursorX == x && cursorY == y)
                        {
                            Console.Write("＠");
                        }
                        else
                        {
                            switch (cells[y, x])
                            {
                                case (int)Color.COLOR_NONE:
                                    Console.Write("・");
                                    break;
                                case (int)Color.COLOR_BLACK:
                                    Console.Write("●");
                                    break;
                                case (int)Color.COLOR_WHITE:
                                    Console.Write("〇");
                                    break;
                            }
                        }
                    }
                    Console.Write("\r\n");
                }
                if (CantPut)
                {
                    Console.Write("そこは置けません");
                }
                else
                {
                    Console.Write(ColorNames[turn]);
                    Console.Write("のターン");
                }

                // カーソル移動
                OperatingCursor(cursorX,cursorY,turn);
            }

            //　カーソル移動に関する処理
            void OperatingCursor(int _cursorX,int _cursorY,int _turn)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                string cursorKey = key.Key.ToString();

                CantPut = false;

                switch (cursorKey)
                {
                    case "W":
                        cursorY--;
                        break;
                    case "A":
                        cursorX--;
                        break;
                    case "S":
                        cursorY++;
                        break;
                    case "D":
                        cursorX++;
                        break;
                    default:
                        if (!(CheckCanPut(_turn, _cursorX, _cursorY)))
                        {
                            CantPut = true;
                            break;
                        }
                        cells[cursorY,cursorX] = _turn;
                        turn ^= 1;
                        break;
                }

                // カーソルが盤外に出た場合の処理
                if (cursorX == -1)
                {
                    cursorX = BOARD_WIDTH-1;
                }else if (cursorY == -1)
                {
                    cursorY = BOARD_HEIGHT-1;
                }else if (cursorX == BOARD_WIDTH)
                {
                    cursorX %= BOARD_WIDTH;
                }else if (cursorY == BOARD_HEIGHT)
                {
                    cursorY %= BOARD_HEIGHT;
                }

            }
        }
    }
}
