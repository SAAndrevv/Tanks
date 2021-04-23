using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

using System.Windows.Media.Animation;

namespace WpfApp1
{
    enum Rotate { UP, RIGH, DOWN, LEFT};
    

    public partial class MainWindow : Window
    {
        Model model;
        Image Tank;
        Image Bul;
       
        DispatcherTimer MyTimer;
        RotateTransform rotate;

        int X, Y;
        int l_bul;
        int x_bul;
        int y_bul;
        int X_tank, Y_tank;
        const int dir = 4;

        Rotate rot = Rotate.UP;
        Rotate rot_b = Rotate.UP;

        struct Pos
        {
            public int x, y;
        }
        Pos[] pos;

        public MainWindow()
        {
            InitializeComponent();
            X_tank = 12;
            Y_tank = 12;
            
            pos = new Pos[4];

            model = new Model();
            X = model.getX();
            Y = model.getY();
            InitPos();
            GenerateLocation();

            MyTimer = new DispatcherTimer();
            MyTimer.Interval = TimeSpan.FromMilliseconds(500);
            MyTimer.Tick += new EventHandler(Bul_Tick);

        }

        private void InitPos()
        {
            pos[0].x = X_tank; pos[0].y = Y_tank;
            pos[1].x = X_tank; pos[1].y = Y_tank + (dir);
            pos[2].x = X_tank + (dir); pos[2].y = Y_tank;
            pos[3].x = X_tank + (dir); pos[3].y = Y_tank + (dir);

        }

        private void GenerateLocation()
        {
            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    Image img = new Image();
                    string num = model.getNum(i, j).ToString();
                    
                    if (num == "1") 
                    {
                        img.Source = new BitmapImage(new Uri("pack://application:,,,/Images/wall.png"));
                    }
                    else if (num == "5")
                    {
                        img.Source = new BitmapImage(new Uri("pack://application:,,,/Images/money.png"));
                    }
                    Grid.SetRow(img, i);
                    Grid.SetColumn(img, j);
           
                    grd.Children.Add(img);
                }
            }
            RotateTransform rotate = new RotateTransform(0);
            SetTank(rotate);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case (Key)26:
                    TurnDown();
                    break;
                case (Key)24:
                    TurnUp();
                    break;
                case (Key)23:
                    TurnLeft();
                    break;
                case (Key)25:
                    TurnRight();
                    break;
                case (Key)18:
                    if (!MyTimer.IsEnabled)
                        Shot();
                    break;
            }
            
        }

        private void Shot()
        {
            rotate = new RotateTransform(0);
            switch (rot)
            {
                case Rotate.UP:
                    l_bul = pos[0].y;
                    x_bul = X_tank - 1;
                    y_bul = Y_tank + 1;                   
                    break;
                case Rotate.DOWN:
                    l_bul = pos[0].y;
                    x_bul = X_tank + dir;
                    y_bul = Y_tank + 1;
                    rotate = new RotateTransform(180);
                    break;
                case Rotate.LEFT:
                    l_bul = pos[0].x;
                    x_bul = X_tank + 2;
                    y_bul = Y_tank - 1;
                    rotate = new RotateTransform(270);
                    break;
                case Rotate.RIGH:
                    l_bul = pos[0].x;
                    x_bul = X_tank + 1;
                    y_bul = Y_tank + dir;
                    rotate = new RotateTransform(90);
                    
                    break;

            }
            if (x_bul >= 0 && x_bul < X &&
                y_bul >= 0 && y_bul < Y)
            {
                rot_b = rot;
                NewBul(x_bul, y_bul, rotate);
                MyTimer.Start();
            }
        }

        private void Bul_Tick(object sender, EventArgs e)
        {
            
            grd.Children.Remove(Bul);
            NewBul(x_bul, y_bul, rotate);

            switch (rot_b)
            {
                case Rotate.UP:
                    BulUp();
                    break;
                case Rotate.DOWN:
                    BulDown();
                    break;
                case Rotate.LEFT:
                    BulLeft();
                    break;
                case Rotate.RIGH:
                    BulRight();
                    break;

            }
        }

        private void BulRight()
        {
           
            if (y_bul < Y - 1 && (!model.CheckWall(x_bul, y_bul) && !model.CheckWall(++x_bul, y_bul)))
            {
                y_bul++;
                --x_bul;
            }
                   
            if (y_bul <= Y - 1 && model.CheckWall(x_bul, y_bul))
            {
                if (RemoveWall(l_bul + 1, y_bul))
                    RemoveWall(l_bul, y_bul);
                if (RemoveWall(l_bul + 2, y_bul))
                    RemoveWall(l_bul + 3, y_bul);
     
                grd.Children.Remove(Bul);
                MyTimer.Stop();
            }
            else if (y_bul == Y - 1)
            {
                grd.Children.Remove(Bul);
                MyTimer.Stop();
            }           
        }

        private void BulLeft()
        {
             
            if (y_bul > 0 && (!model.CheckWall(x_bul, y_bul) && !model.CheckWall(--x_bul, y_bul)))
            {
                y_bul--;
                ++x_bul;
            }
          
            if (y_bul >= 0 && model.CheckWall(x_bul, y_bul))
            {
                if (RemoveWall(l_bul + 1, y_bul))
                    RemoveWall(l_bul, y_bul);
                if (RemoveWall(l_bul + 2, y_bul))
                    RemoveWall(l_bul + 3, y_bul);

                grd.Children.Remove(Bul);
                MyTimer.Stop();      
            }
            else if (y_bul == 0)
            {
                grd.Children.Remove(Bul);
                MyTimer.Stop();
            }
        }

        private void BulDown()
        {
            

            if (x_bul < X - 1 && (!model.CheckWall(x_bul, y_bul) && !model.CheckWall(x_bul, ++y_bul)))
            {
                x_bul++;
                --y_bul;
            }

            if (x_bul <= X - 1 && model.CheckWall(x_bul, y_bul))
            {
                if (RemoveWall(x_bul, l_bul + 1))
                    RemoveWall(x_bul, l_bul);
                if (RemoveWall(x_bul, l_bul + 2))
                    RemoveWall(x_bul, l_bul + 3);

                grd.Children.Remove(Bul);
                MyTimer.Stop();
            }
            else if (x_bul == X - 1)
            {
                grd.Children.Remove(Bul);
                MyTimer.Stop();
            }

        }

        private void BulUp()
        {

            if (x_bul > 0 && (!model.CheckWall(x_bul, y_bul) && !model.CheckWall(x_bul, ++y_bul)))
            { 
                x_bul--;
                --y_bul;
            }

            if (x_bul >= 0 && model.CheckWall(x_bul, y_bul))
            {
                if (RemoveWall(x_bul, l_bul + 1))
                    RemoveWall(x_bul, l_bul);
                if (RemoveWall(x_bul, l_bul + 2))
                    RemoveWall(x_bul, l_bul + 3);

                grd.Children.Remove(Bul);
                MyTimer.Stop();
            }
            else if (x_bul == 0)
            {
                grd.Children.Remove(Bul);
                MyTimer.Stop();
            }   
        }

        private void TurnRight()
        {
            
            if (rot != Rotate.RIGH)
            {
                rot = Rotate.RIGH;
                grd.Children.Remove(Tank);
                RotateTransform rotate = new RotateTransform(90);
                SetTank(rotate);
            }

            if (Y_tank < Y - dir)
            {
                for (int i = 0; i < dir; ++i)
                {
                    if (model.CheckWall(X_tank + i, Y_tank + dir))
                        return;
                }

                Y_tank += 1;
                for (int i = 0; i < 4; ++i)
                    pos[i].y += 1;
                Grid.SetRow(Tank, X_tank);
                Grid.SetColumn(Tank, Y_tank);
            }   
            CheckMoney();  
        }

        private void TurnLeft()
        {
            if (rot != Rotate.LEFT)
            {
                rot = Rotate.LEFT;
                grd.Children.Remove(Tank);

                RotateTransform rotate = new RotateTransform(270);
                SetTank(rotate);       
            }
            
            if (Y_tank > 0)
            {
                for (int i = 0; i < dir; ++i)
                {  
                    if (model.CheckWall(X_tank + i, Y_tank - 1))
                        return;
                }

                Y_tank -= 1;
                for (int i = 0; i < 4; ++i)
                    pos[i].y -= 1;
                Grid.SetRow(Tank, X_tank);
                Grid.SetColumn(Tank, Y_tank);
            }
            CheckMoney();
        }

        private void TurnUp()
        {
            if (rot != Rotate.UP)
            {                       
                rot = Rotate.UP;
                grd.Children.Remove(Tank);
                RotateTransform rotate = new RotateTransform(0);
                SetTank(rotate);
            }
         
            if (X_tank > 0)
            {
                for (int i = 0; i < dir; ++i)
                {
                    if (model.CheckWall(X_tank - 1, Y_tank + i))
                        return;
                }

                X_tank -= 1;
                for (int i = 0; i < 4; ++i)
                    pos[i].x -= 1;
                Grid.SetRow(Tank, X_tank);
                Grid.SetColumn(Tank, Y_tank);
            }
            CheckMoney();
        }


        private void TurnDown()
        { 
            if (rot != Rotate.DOWN)
            {
                rot = Rotate.DOWN;
                grd.Children.Remove(Tank);
                RotateTransform rotate = new RotateTransform(180);
                SetTank(rotate);
            }
         
            if (X_tank < X - dir)
            {
                for (int i = 0; i < dir; ++i)
                {
                    if (model.CheckWall(X_tank + dir, Y_tank + i))                     
                        return;        
                }

                X_tank += 1;
                for (int i = 0; i < 4; ++i)
                    pos[i].x += 1;
                Grid.SetRow(Tank, X_tank);
                Grid.SetColumn(Tank, Y_tank);
            }
            CheckMoney();

        }

        private void SetTank (RotateTransform rotate)
        {
            Tank = new Image();
            Tank.Source = new BitmapImage(new Uri("pack://application:,,,/Images/tank.png"));
            Tank.RenderTransformOrigin = new Point(0.5, 0.5);
            Tank.RenderTransform = rotate;

            Grid.SetRow(Tank, X_tank);
            Grid.SetColumn(Tank, Y_tank);
            Grid.SetColumnSpan(Tank, dir);
            Grid.SetRowSpan(Tank, dir);

            grd.Children.Add(Tank);
        }

        private bool RemoveWall(int x, int y)
        {
            if (model.CheckWall(x, y))
            {
                model.RemoveWall(x, y);
                CreateAnimation(ReurnChildren(x, y), 2);
                return true;
            }
            return false;

        }

        private void NewBul(int x_bul, int y_bul, RotateTransform rotate)
        {
            Bul = new Image();
            Bul.Source = new BitmapImage(new Uri("pack://application:,,,/Images/bul.png"));
            if (rot_b == Rotate.LEFT || rot_b == Rotate.RIGH)
                Bul.RenderTransformOrigin = new Point(0, 0.5);
            else
                Bul.RenderTransformOrigin = new Point(0.5, 0.5);
            Bul.RenderTransform = rotate;

            Grid.SetRow(Bul, x_bul);
            Grid.SetColumn(Bul, y_bul);
            Grid.SetColumnSpan(Bul, 2);

            grd.Children.Add(Bul);

        }
        private Image ReurnChildren(int x_bul, int i)
        {
            for (int j = 0; j < grd.Children.Count; ++j)
            {
                if (Grid.GetRow(grd.Children[j]) == x_bul &&
                    Grid.GetColumn(grd.Children[j]) == i)
                    return (Image)grd.Children[j];
            }
            return null;
        }
        private void CheckMoney()
        {
            for (int x = pos[0].x; x < pos[2].x; ++x)
            {
                for (int y = pos[0].y; y < pos[1].y; ++y)
                {
                    if (model.IsMoney(x, y))
                    {
                        CreateAnimation(ReurnChildren(x, y), 1);
                    }
                }
            }
            if (model.Win())
                MessageBox.Show("Вы выиграли!");
        }

        private void CreateAnimation(Image img, int time)
        {
            DoubleAnimation widthAnimation = new DoubleAnimation();
            widthAnimation.From = img.ActualWidth;
            widthAnimation.To = 0;
            widthAnimation.Duration = TimeSpan.FromSeconds(time);

            DoubleAnimation heightAnimation = new DoubleAnimation();
            heightAnimation.From = img.ActualHeight;
            heightAnimation.To = 0;
            heightAnimation.Duration = TimeSpan.FromSeconds(time);

            img.BeginAnimation(Image.WidthProperty, widthAnimation);
            img.BeginAnimation(Image.HeightProperty, heightAnimation);
        }

    }



    
}
