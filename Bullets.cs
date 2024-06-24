using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Rectangle = System.Drawing.Rectangle;

namespace Ultrakill2D
{
    public class Bullets
    {
        private List<Bullet> _bullets; //(tip) List = array but with flex length

        public Bullets()
        {
            _bullets = new List<Bullet>();// creating array
        }

        public void Add(Bullet item)
        {
            _bullets.Add(item); // add folder or file
        }

        public void Draw(List<Enemy> _enemylist, Effect _effect, V1 v1) //print out all what it have inside the array
        {
            for (int i = _bullets.Count - 1; i >= 0; i--)
            {
                Bullet item = _bullets[i];
                item.Func(_enemylist, _effect,v1);
                if (item.Remove())
                {
                    _bullets.RemoveAt(i);
                }
                item.Draw();

            }
        }
        public void Clear()
        {
            _bullets?.Clear();
        }

    }
    public abstract class Bullet
    {
        public abstract void Draw();
        public abstract void Func(List<Enemy> _enemylist, Effect _effect, V1 v1);

        public abstract Boolean Remove();
    }

    public class piston_bullet : Bullet
    {
        private Point2D _point1;
        private Point2D _point2;
        private int _livetime = 5;
        private Boolean _remove = false;
        public piston_bullet(Point2D start, Point2D end)
        {
            _point1 = start;
            _point2 = end;
        }
        public override void Draw()
        {
            if (_livetime > 0)
            {
                SplashKit.DrawLine(SplashKitSDK.Color.Yellow, _point1, _point2);
                _livetime -= 1;
                
            }
            else
            {
                _remove = true;
            }
        }
        public override Boolean Remove()
        {
            return _remove;
        }
        public override void Func(List<Enemy> enemylist, Effect _effect, V1 v1)
        {
            //do notthing :))
        }
    }
    public class Warning : Bullet
    {
        private Point2D _point1;
        private Point2D _point2;
        private int _livetime = 30;
        private Boolean _remove = false;
        public Warning(Point2D start, Point2D end)
        {
            _point1 = start;
            _point2 = end;
        }
        public override void Draw()
        {
            if (_livetime > 0)
            {
                SplashKit.DrawLine(SplashKitSDK.Color.RGBAColor(255,0,0,50), _point1, _point2);
                _livetime -= 1;

            }
            else
            {
                _remove = true;
            }
        }
        public override Boolean Remove()
        {
            return _remove;
        }
        public override void Func(List<Enemy> enemylist, Effect _effect, V1 v1)
        {
            //do notthing :))
        }
    }

    public class shotgun_bullet : Bullet
    {
        private Point2D _point;
        private Vector2D _vector;
        private int _livetime = 600;
        private Boolean _remove = false;
        private List<Rectangle> _rects;
        private Rectangle _rect;

        public shotgun_bullet(Point2D start, Vector2D V, List<Rectangle> rects)
        {
            _point = start;
            _vector = V;
            _rects = rects;
        }
        public override void Draw()
        {
            if (_livetime > 0)
            {
                SplashKit.FillRectangle(SplashKitSDK.Color.Yellow, _point.X-4, _point.Y-4, 8, 8);
                _livetime -= 1;
            }
            else 
            { 
                _remove = true;
            }
        }
        public override Boolean Remove()
        {
            return _remove;
        }

        public override void Func(List<Enemy> enemylist, Effect _effect, V1 v1)
        {
            _rect = new Rectangle(Convert.ToInt32(_point.X - 4), Convert.ToInt32(_point.Y - 4), 8, 8);

            foreach (Enemy enemy in enemylist)
            {
                if (_rect.IntersectsWith(enemy.Rect()))
                {
                    _remove = true;
                    enemy.Damage(0.6);
                    //
                    Circle hiteffect = new Circle(_point.X-4, _point.Y-4, 10, 1, 4, SplashKit.RGBAColor(255, 0, 0, 150));
                    _effect.Add(hiteffect);
                    //
                    v1.Blood(6,100,_point);

                    break;
                }
            }

            foreach (Rectangle block in _rects)
            {
                //Console.WriteLine("test");
                if (_rect.IntersectsWith(block))
                {
                    //Console.WriteLine("Hit!");
                    _remove = true;
                    //
                    Circle hiteffect = new Circle(_point.X - 4, _point.Y - 4, 10, 1, 2, SplashKit.RGBAColor(255, 255, 0, 150));
                    _effect.Add(hiteffect);

                    break;
                }
            }
            _point.X += _vector.X*30;
            _point.Y += _vector.Y*30;
        }

    }

    public class Homing : Bullet
    {
        private Point2D _point;
        private Vector2D _vector;
        private int _livetime = 600;
        private Boolean _remove = false;
        private List<Rectangle> _rects;
        private Rectangle _rect;
        private int _timer = 0;
        private double _speed = 0;
        public Homing(Point2D start, Vector2D V, List<Rectangle> rects)
        {
            _point = start;
            _vector = V;
            _rects = rects;
        }
        public override void Draw()
        {
            if (_livetime > 0)
            {
                Bitmap blueorb = new Bitmap("BlueOrb", "F:\\Projects\\Ultrakill2D\\Ultrakill2D\\Resources\\images\\blue_bullet.png");
                SplashKit.DrawBitmap(blueorb, _point.X, _point.Y);
                _livetime -= 1;
            }
            else
            {
                _remove = true;
            }
        }
        public override Boolean Remove()
        {
            return _remove;
        }

        public override void Func(List<Enemy> enemylist, Effect _effect, V1 v1)
        {
            _rect = new Rectangle(Convert.ToInt32(_point.X), Convert.ToInt32(_point.Y), 32, 32);

            HomingFunc(v1);

            if (_rect.IntersectsWith(v1.Rect()))
            {
                _remove = true;
                v1.Damage(30);
                //
                Circle hiteffect = new Circle(_point.X - 4, _point.Y - 4, 10, 1, 10, SplashKit.RGBAColor(0, 255, 255, 100));
                _effect.Add(hiteffect);
                //
                
            }
           

            foreach (Rectangle block in _rects)
            {
                //Console.WriteLine("test");
                if (_rect.IntersectsWith(block))
                {
                    //Console.WriteLine("Hit!");
                    _remove = true;
                    //
                    Circle hiteffect = new Circle(_point.X - 4, _point.Y - 4, 10, 1, 10, SplashKit.RGBAColor(0, 255, 255, 100));
                    _effect.Add(hiteffect);

                    break;
                }
            }
            _point.X += _vector.X * _speed;
            _point.Y += _vector.Y * _speed;
        }
        private void HomingFunc(V1 v1)
        {
            // set vector
            Vector2D targetV = new Vector2D() { X=v1.X()-(_point.X+16), Y= v1.Y() - (_point.Y+16) };
            targetV = SplashKit.VectorMultiply(targetV, -1 / SplashKit.VectorMagnitude(targetV));
            // calculate wiht Ox
            double targetX = Math.Atan2(targetV.X, targetV.Y) / Math.PI * 180;
            double currentX = Math.Atan2(_vector.X, _vector.Y) / Math.PI * 180;
            // convert the - angle to + one
            if (targetX < 0) { targetX = 360 + targetX; }
            if (currentX < 0) { currentX = 360 + currentX; }
            //compare vector
            double angle = Math.Abs(currentX -targetX);
            //rotation
            double r_speed = Math.PI / 120;
            double r_speed2 = r_speed * -1;
            double X = 0;
            double Y = 0;
            if ((angle <=180 && currentX < targetX)||(angle > 180 && currentX > targetX))
            {
                X = _vector.X * Math.Cos(r_speed) - _vector.Y * Math.Sin(r_speed);
                Y = _vector.X * Math.Sin(r_speed) + _vector.Y * Math.Cos(r_speed);
            }
            if ((angle > 180 && currentX < targetX)||(angle <= 180 && currentX > targetX))
            {
                X = _vector.X * Math.Cos(r_speed2) - _vector.Y * Math.Sin(r_speed2);
                Y = _vector.X * Math.Sin(r_speed2) + _vector.Y * Math.Cos(r_speed2);
            }
            if (angle == 0)
            {
                X = _vector.X; Y = _vector.Y;
            }
            //store new vector
            _vector.X = X; _vector.Y = Y;
            //speed controlling
            double Aspeed = 15;
            double Bspeed = Aspeed * (_timer / (60 * 1.5));
            _timer += 1;
            //new speed
            _speed = Bspeed;
            
            




        }

    }

}
        

