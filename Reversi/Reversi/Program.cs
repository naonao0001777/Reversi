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

            // x軸、y軸のベクトルをリスト型のDictionary<y,x>関数で定義
            List<Dictionary<int, int>> Vector = new List<Dictionary<int, int>>()
            {
                new Dictionary<int,int>(){ { -1, 0 } }, // UP
                new Dictionary<int,int>(){ { -1, 1 } }, // UP_RIGHT
                new Dictionary<int,int>(){ { 0, 1 } },  // RIGHT
                new Dictionary<int,int>(){ { 1, 1 } },  // DOWN_RIGHT
                new Dictionary<int,int>(){ { 1, 0 } },  // DOWN
                new Dictionary<int,int>(){ { 1, -1 } }, // DOWN_LEFT
                new Dictionary<int,int>(){ { 0, -1 } }, // LEFT
                new Dictionary<int,int>(){ { -1, -1 } } // UP_LEFT
            };

            // y軸、x軸のベクトル値を定義
            int[][] vector = new int[2][];
            vector[0] = new int[] {0,1,1,1,0,-1,-1,-1}; // x軸
            vector[1] = new int[] {-1,-1,0,1,1,1,0,-1}; // y軸
            

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
                bool canPut = false;

                int x = _cursorX;
                int y = _cursorY;

                if (cells[y,x] != (int)Color.COLOR_NONE)
                {
                    return false;
                }

                // 全方向のベクトルをチェック
                for (int i = 0; i < (int)Direction.DIRECTION_MAX; i++)
                {
                    
                    // x,yを初期化
                    x = _cursorX;
                    y = _cursorY;

                    // x,y軸方向を加算
                    x += vector[0][i];
                    y += vector[1][i];

                    // i方向ベクトルに相手の石が置いていた場合
                    if (x < 0 || y < 0 || x >= BOARD_WIDTH || y >= BOARD_HEIGHT)
                    {
                        continue;
                    }
                    else if (cells[y, x] == (_turn ^ 1))
                    {
                        // 相手の石が置いている方向をチェック
                        while(true)
                        {
                            x += vector[0][i];
                            y += vector[1][i];
                            
                            // 味方の石が置いている場合はフラグを立ててループを抜ける
                            if (x < 0 || y < 0 || x >= BOARD_WIDTH || y >= BOARD_HEIGHT)
                            {
                                break;
                            }
                            else if (cells[y, x] == _turn)
                            {
                                // 自分のカーソル位置まで石を返す
                                while(!(x == _cursorX && y == _cursorY))
                                {
                                    x -= vector[0][i];
                                    y -= vector[1][i];
                                    
                                    cells[y, x] = _turn;
                                }

                                canPut = true;
                                break;
                            }
                        }
                    }
                }

                // 全方向をチェックした後の可否フラグがfalseなら、falseを返す
                if (!canPut)
                {
                    return false;
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
