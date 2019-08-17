using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi
{
    class CPU
    {
        const int BOARD_HEIGHT = 8;
        const int BOARD_WIDTH = 8;

        int[,] cells = new int[BOARD_HEIGHT, BOARD_WIDTH];

        void Draw()
        {
            for (int y = 0; y < BOARD_HEIGHT; y++)
            {
                for (int x = 0; x < BOARD_WIDTH; x++)
                {
                    
                }
            }
        }

        bool CPUCheckCanPut(int _turn, int _cursorX, int _cursorY, bool checkCanPutAllFlug)
        {
            bool canPut = false;
            // y軸、x軸のベクトル値を定義
            int[][] vector = new int[2][];

            vector[0] = new int[] { 0, 1, 1, 1, 0, -1, -1, -1 }; // x軸
            vector[1] = new int[] { -1, -1, 0, 1, 1, 1, 0, -1 }; // y軸

            int x = _cursorX;
            int y = _cursorY;
           
            if (cells[y, x] != (int)Program.Color.COLOR_NONE)
            {
                return false;
            }

            // 全方向のベクトルをチェック
            for (int i = 0; i < (int)Program.Direction.DIRECTION_MAX; i++)
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
                    while (true)
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
                            while (!checkCanPutAllFlug && !(x == _cursorX && y == _cursorY))
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

        // 置けるかどうかをすべてをチェック
        bool CheckCanPutAll()
        {
            int turn = 0;
            for (int y = 0; y < BOARD_HEIGHT; y++)
            {
                for (int x = 0; x < BOARD_WIDTH; x++)
                {
                    if (cells[y, x] == (int)Program.Color.COLOR_NONE)
                    {
                        if (CPUCheckCanPut(turn, x, y, true))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
