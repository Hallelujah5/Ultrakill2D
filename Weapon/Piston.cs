using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rectangle = System.Drawing.Rectangle;

namespace Ultrakill2D.Weapon
{
    public class Piston
    {
        public Point2D point2;
        public void Shot(List<Rectangle> rects, Point2D point, Vector2D V, Bullets _bullets, List<Enemy> _enemylist)
        {
            bool _hit = false;
            {
                for (int i = 0; i < 3000; i += 2)
                {
                    int x = Convert.ToInt32(point.X - 2 + V.X * i);
                    int y = Convert.ToInt32(point.Y - 2 + V.Y * i);
                    point2.X = x; point2.Y = y;

                    Rectangle checkbox = new Rectangle(x, y, 4, 4);
                    foreach (Enemy enemy in _enemylist)
                    {
                        if (checkbox.IntersectsWith(enemy.Rect())) 
                        {
                            _hit = true;
                            enemy.Damage(1);
                            //Console.WriteLine("Hit!");
                            break;
                        }
                    }

                    foreach (Rectangle r in rects)
                    {
                        if (checkbox.IntersectsWith(r))
                        {
                            //Console.WriteLine("Hit at X: " + x + " Y: "+y);
                            _hit = true;
                            break; // Exit the loop when hit is true
                        }
                    }
                    if (_hit) break; // Exit the outer loop if hit is true
                    //add bullet
                    piston_bullet bullet = new piston_bullet(point, point2);
                    _bullets.Add(bullet);

                }
            }

        }
    }
}
