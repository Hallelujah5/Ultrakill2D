using PNGtest2.Map;
using SplashKitSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rectangle = System.Drawing.Rectangle;


namespace Ultrakill2D
{
    public class Game
    {
        private Boolean load = false;
        private DrawMap _Map;
        private Shader _shader;
        private V1 _player;
        private List<Rectangle> _rects;

        private Bullets _bullets;

        private Enemies _enemies;
        private List<Enemy> _enemylist;

        private Effect _effect;

        private Point2D v1_location;

        private UI _ui;

        //window to world
        private Point2D _point;
        public void Load() 
        {
            _Map = new DrawMap();
            

            _shader = new Shader();

            _player = new V1();

            _bullets = new Bullets();

            _effect = new Effect();

            _enemies = new Enemies();
            _ui = new UI();

            _point = new Point2D();

            load = true;
        }
        private Boolean _pause = false;
        public void Game_main(Window gameWindow)
        {
            if (!load) { Load(); }

            if (SplashKit.KeyTyped(KeyCode.EscapeKey)) { if (!_pause) { _pause = true; } else { _pause = false; } }
            _point.X = 0; _point.Y = 0;
            _point = SplashKit.ToWorld(_point);

            if (!_pause)
            {
                SplashKit.ClearWindow(gameWindow, Color.Black);

                v1_location = _player.V1_location();


                //map
                _Map.Draw();
                _Map.Func(_enemies, v1_location.X, _shader, gameWindow);

                _rects = _Map.GetRect();

                //living thing
                _enemylist = _enemies.GetList();
                _enemies.Draw(_effect, _player, _rects, _bullets);

                _player.V1_main(gameWindow, _rects, _bullets, _enemylist, _shader, _Map);


                //effect thing
                _bullets.Draw(_enemylist, _effect, _player);

                _effect.Draw();



                _ui.DrawUI(_player);

            }
            else
            {
                //Console.WriteLine("X: "+ _point.X + " Y: "+ _point.Y);
                SplashKit.DrawText("Pause. Press ESC to continue...",Color.White,_point.X+520,_point.Y+550);
            }
        }
    }
}
