using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;
using Rectangle = System.Drawing.Rectangle;


namespace Ultrakill2D.Weapon
{
    public class Shotgun
    {
        public void Shot(List<Rectangle> rects, Point2D point, Vector2D V, Bullets _bullets,List<Enemy> _enemylist)
        {
            shotgun_bullet bullet = new shotgun_bullet(point,V, rects);
            _bullets.Add(bullet);

            //up spread
            for(int i  = 0; i < 2; i++)
            {
                Vector2D vector = Rotate(V,1);
                bullet = new shotgun_bullet(point, vector, rects);
                _bullets.Add(bullet);
            }
            //down spread
            for (int i = 0; i < 2; i++)
            {
                Vector2D vector = Rotate(V, -1);
                bullet = new shotgun_bullet(point, vector, rects);
                _bullets.Add(bullet);
            }

        }
        public Vector2D Rotate(Vector2D V,int i)
        {
            double index = 180 / random() * i;
            double angle = Math.PI/index;
            V.X = V.X * Math.Cos(angle) - V.Y * Math.Sin(angle);
            V.Y = V.X * Math.Sin(angle) + V.Y * Math.Cos(angle);
            //Console.WriteLine(V.X + " || "+ V.Y);

            return V;
        }

        public double random()
        {
            double index;
            Random rnd = new Random();

            double index2 = rnd.Next(0, 10);
            double index3 = rnd.NextDouble() + 0.1;

            index = index2 + index3;
            return index;

        }
    }
}
