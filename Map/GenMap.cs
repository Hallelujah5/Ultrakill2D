using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultrakill2D;
using Color = SplashKitSDK.Color;
using Rectangle = System.Drawing.Rectangle;

namespace PNGtest2.Map
{
    public class GenMap
    {
        private List<Obj> _objs;
        public GenMap()
        {
            _objs = new List<Obj>();
        }
        public void Add(Obj item)
        {
            _objs.Add(item);
        }
        public void Draw()
        {
            //Console.WriteLine(_objs.Count); 
            foreach (Obj item in _objs)
            {
                item.Draw();
            }
        }
        private List<Rectangle> _rects;
        public List<Rectangle> GetRect()
        {   
            if (_rects == null)
            {
                _rects = new List<Rectangle>();
            }
            foreach (Obj item in _objs)
            {
                _rects.Add(item.Rect());
                //Console.WriteLine(item.Rect().ToString());
            }
            return _rects;
        }
    }
    public class DrawMap
    {
        private GenMap _map;


        public void Draw()
        {
            _map = new GenMap();
            //first wall
            DarkBlock block0 = new DarkBlock(-256,-1720,128,2500);
            _map.Add(block0);
            // ground build
            for (int i = 0; i<38; i++)
            {
                FleshBlock block1 = new FleshBlock(-128+128*i,200);
                _map.Add(block1);
            }
            for (int i = 1; i > -5; i--)
            {
                FleshBlock block69 = new FleshBlock(-128 + 128 * i, 200);
                _map.Add(block69);
            }

            //mid wall build
            for (int x = 0; x < 5; x++)
            {
                for ( int y = 0; y < 14; y++) 
                {
                    FleshBlock block2 = new FleshBlock(1024+(x*128), -1720+(y*128));
                    _map.Add(block2);

                }
            }
            //terminal
            Terminal terminal = new Terminal(1024 + (2 * 128), -1720 + (14 * 128));
            _map.Add(terminal);

            //last wall build
            for (int y = 0;y < 16; y++)
            {
                FleshBlock block3 = new FleshBlock(4864-128, -1720 + (y * 128));
                _map.Add(block3);

            }
            //celling
            DarkBlock block4 = new DarkBlock(-256, -1720, 128* 50, 128);
            _map.Add(block4);

            if ( _boss )
            {
                FleshBlock wall = new FleshBlock(1024 + (4 * 128), -1720 + (14 * 128));
                _map.Add(wall);
                DarkBlock block = new DarkBlock(1024 , -1720 ,128*4 , (16 * 128));
                _map.Add(block);

            }





            _map.Draw();

        }
        public List<Rectangle> GetRect()
        {
            return _map.GetRect();
        }
        private Boolean _boss = false;
        public Boolean Bossfight()
        {
            return _boss;
        }
        public void ResetBossfight()
        {
            _boss = false;
        }


        public void Func(Enemies _enemies, double v1_location,Shader _shader,Window gameWindow)
        {
            
            if (v1_location > 2500 && !_boss)
            {
                Prison prison = new Prison(1664 + 1600 + 32, -1720 + 1920 / 2 + 32 + 400);
                _enemies.Add(prison);

                _boss = true;
                
            }
            if (!_boss)
            {
                Point2D start; start.X = 0; start.Y = 0;
                start = SplashKit.ToWorld(start);
                Point2D center; center.X = gameWindow.Width / 2; center.Y = gameWindow.Height / 2;
                center = SplashKit.ToWorld(center);

                //set dark cell
                _shader.shader_load(gameWindow, start.X, start.Y);

                //v1 light
                _shader.set_lighBud(center.X, center.Y, 200, gameWindow);
                //terminal func
                if ( v1_location > 1024 && v1_location < 1024 + 128 * 5)
                {
                    _shader.set_lighBud(1024 + (2 * 128)+64, -1720 + (14 * 128)+64, 250, gameWindow);
                }
                if (v1_location > 1024 + 128*2 && v1_location < 1024 + 128 * 3)
                {
                    SplashKit.FillRectangle(Color.White, 1024 + (2 * 128), -1720 + (13 * 128), 128, 128);
                    SplashKit.FillRectangle(Color.Black, 1024 + (2 * 128)+2, -1720 + (13 * 128)+2, 128-4, 128-4);
                    _shader.set_lighBud(1024 + (2 * 128)+64, -1720 + (13 * 128)+64, 200, gameWindow);
                    SplashKit.DrawText("--------------", Color.White, 1024 + (2 * 128) +10, -1720 + (13 * 128) + 5);
                    SplashKit.DrawText("A game made by", Color.White, 1024 + (2 * 128) + 10, -1720 + (13 * 128) + 15);
                    SplashKit.DrawText("  P.HoangAnh  ", Color.White, 1024 + (2 * 128) + 10, -1720 + (13 * 128) + 25);
                    SplashKit.DrawText(" swh104672843 ", Color.White, 1024 + (2 * 128) + 10, -1720 + (13 * 128) + 35);
                    SplashKit.DrawText("---GameTips---", Color.White, 1024 + (2 * 128) + 10, -1720 + (13 * 128) + 55);
                    SplashKit.DrawText("+Shot the eyes", Color.White, 1024 + (2 * 128) + 10, -1720 + (13 * 128) + 65);
                    SplashKit.DrawText("+Dont shot the", Color.White, 1024 + (2 * 128) + 10, -1720 + (13 * 128) + 75);
                    SplashKit.DrawText(" Boss first.  ", Color.White, 1024 + (2 * 128) + 10, -1720 + (13 * 128) + 85);
                    SplashKit.DrawText("+Pay attendtio", Color.White, 1024 + (2 * 128) + 10, -1720 + (13 * 128) + 95);
                    SplashKit.DrawText("n to green bar", Color.White, 1024 + (2 * 128) + 10, -1720 + (13 * 128) + 105);
                    SplashKit.DrawText("+  Good luck  ", Color.White, 1024 + (2 * 128) + 10, -1720 + (13 * 128) + 115);


                }

                _shader.draw_loadcell();

            }

        }
    }
}
