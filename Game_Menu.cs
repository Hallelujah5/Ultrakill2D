using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;
using Ultrakill2D;


namespace Game_Menu
{
    public class Menu
    {
        private Boolean DataLoad = false;

        private Elevator[] _elevators = new Elevator[4];
        private Shader _shader = new Shader();
        public void Menu_main(Window gameWindow, DrawingOptions opts)
        {
            SplashKit.ClearWindow(gameWindow, Color.Black);

            //Draw(gameWindow);
            Elevator(gameWindow);
            V1(gameWindow, opts);
            //drawshader
            _shader.shader_load(gameWindow,0,0);
            Movinglight(gameWindow);
            _shader.set_lighBud(gameWindow.Width / 2, _lightY, 300, gameWindow);
            _shader.draw_loadcell();

            gameWindow.DrawText("Press R to start", Color.White, gameWindow.Width / 2 - 60, 500);
        }

        private Boolean _lightload = false;
        private double _lightY;
        public void Movinglight(Window gameWindow)
        {
            if (_lightload == false)
            {
                _lightY = gameWindow.Height / 2;
                _lightload = true;
            }
            _lightY -= 10;
            if (_lightY < -500)
            {
                _lightY = gameWindow.Height + 500;
            }
        }
        public void Elevator(Window gameWindow)
        {
            if (DataLoad == false)
            {
                for (int i = 0; i < 4; i++)
                {
                    _elevators[i] = new Elevator();

                    _elevators[i].elevator(gameWindow.Width / 2 - 256/2, 0 + i * 256, i);

                    //Console.WriteLine("Loaded elevator number" + i.ToString());

                }
                DataLoad = true;
            }


            foreach (Elevator elevator in _elevators)
            {
                //Console.WriteLine(elevator.ID.ToString());
                elevator.Draw(gameWindow);
                elevator.Y -= 16;
                if (elevator.Y <= -256)
                {
                    elevator.Y = 700;
                }
            }
        }
        public void V1(Window gameWindow,DrawingOptions opts)
        { 
            Bitmap V1 = new Bitmap("V1", "F:\\Projects\\Ultrakill2D\\Ultrakill2D\\Resources\\images\\Falling_V1.png");
            gameWindow.DrawBitmap(V1, gameWindow.Width / 2-16, gameWindow.Height / 2-16,opts);
        }

    }
    public class Elevator
    {
        private Bitmap _bitmap;
        private double _X;
        private double _Y;
        private int _ID;

        public void Draw(Window window)
        {
            window.DrawBitmap(_bitmap, _X, _Y);

        }
        public void elevator(int x,int y,int ID)
        {
            _bitmap = new Bitmap("elevator", "F:\\Projects\\Ultrakill2D\\Ultrakill2D\\Resources\\images\\HellElevator.png");
            _X = x;
            _Y = y;
            _ID = ID;
        }
        public int ID
        {
            get { return _ID; } 
            set { _ID = value; }
        }
        public double Y
        {
            get { return _Y; }
            set { _Y = value; }
        }



    }
}
