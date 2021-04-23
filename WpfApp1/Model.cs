using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    
    class Model
    {
        const int X = 30;
        const int Y = 30;
        int money;
        int[,] pole;

        public Model()
        {
            pole = new int[X, Y];
            money = 0;
            ReadFile();
        }

        private void ReadFile()
        {
            StreamReader sr = new StreamReader(@"Map.txt");
            String line;
            

            for (int i= 0; i < X; ++i)
            {
                line = sr.ReadLine();
                WriteToMap(line, i);
                
            }
            sr.Close();
        }

        internal int getX()
        {
            return X;
        }

        internal int getY()
        {
            return Y;
        }

        private void WriteToMap(string line, int x)
        {
            for (int j = 0; j < Y; ++j)
            {
                pole[x, j] = Int32.Parse(line[j].ToString());
                if (pole[x, j] == 5)
                    money++;
            }
        }

        internal object getNum(int i, int j)
        {
            return pole[i, j];
        }

        internal bool CheckWall(int x_tank, int y_tank)
        {
            if (pole[x_tank, y_tank] == 1)
                return true;

            return false;
            
        }

        internal void RemoveWall(int x_bul, int i)
        {
                pole[x_bul, i] = 0;
        }

        internal bool IsMoney(int x, int y)
        {
            if (pole[x, y] == 5)
            {
                money--;
                pole[x, y] = 0;
                return true;
            }
            return false;
               
        }

        internal bool Win()
        {
            if (money == 0)
                return true;

            return false;
        }
    }
}
