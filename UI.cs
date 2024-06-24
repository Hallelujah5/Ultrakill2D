using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;
using Color = SplashKitSDK.Color;

namespace Ultrakill2D
{
    public class UI
    {
        private int _hp;
        private double _hard_damage;
        private double _stamina;
        private double _last_hp = 100;
        public void DrawUI(V1 v1)
        {
            _hp = v1.hp();
            _hard_damage = v1.HardDamage();
            _stamina = v1.stamina();
            //some nice animation
   
            if (_last_hp > _hp) { _last_hp -= 1; }
            if (_hp > _last_hp) { _last_hp = _hp; }
            
            //location
            Point2D point; point.X =50; point.Y =550;
            point = SplashKit.ToWorld(point);
            //frame
            SplashKit.FillRectangle(Color.RGBAColor(255, 255, 255, 50), point.X, point.Y, 128 * 3, 80);
            //hp
            Hp(point);
            //stamina
            Stamina(point);
            //troll
            if (SplashKit.KeyTyped(KeyCode.KKey)) { v1.Damage(10); }
            if (SplashKit.KeyTyped(KeyCode.LKey)) { v1.Heal(200); }


        }
        private void Hp(Point2D point)
        {
            SplashKit.FillRectangle(Color.DarkRed, point.X+5, point.Y +5 , 384-10, 35);

            if (_hp > 0)
            {
                SplashKit.FillRectangle(Color.Yellow, point.X + 5, point.Y + 5, 3.84 * _last_hp - 10, 35);

                SplashKit.FillRectangle(Color.Gray, point.X + 379+1, point.Y + 5, -3.84 * _hard_damage, 35);


                SplashKit.FillRectangle(Color.Red, point.X + 5, point.Y + 5, 3.84 * _hp - 10, 35);
            }
            SplashKit.DrawText("+", Color.DarkRed, point.X + 20, point.Y + 18);
            SplashKit.DrawText(_hp.ToString(), Color.White, point.X + 30, point.Y + 18);

        }
        
        private void Stamina(Point2D point)
        {
            Color full = Color.Aqua;
            Color notfull = Color.RGBColor(173, 255, 250);
            Color back = Color.RGBColor(36, 157, 159);
            //first bar
            SplashKit.FillRectangle(back, point.X + 5, point.Y + 45, 128 - 5, 30);
            if (_stamina < 1) { SplashKit.FillRectangle(notfull, point.X +5, point.Y + 45, 128 * _stamina-5, 30); }
            else if (_stamina >=1) { SplashKit.FillRectangle(full, point.X+5, point.Y + 45, 128-5, 30); }

            //2nd bar
            SplashKit.FillRectangle(back, point.X + 128 + 5, point.Y + 45, 128 - 5, 30);
            if ( _stamina < 2 && _stamina >1) { SplashKit.FillRectangle(notfull, point.X + 128+5, point.Y + 45, 128* (_stamina-1)-5, 30); }
            else if (_stamina >= 2) { SplashKit.FillRectangle(full, point.X + 128 + 5, point.Y + 45, 128-5, 30); }

            //3rd bar
            SplashKit.FillRectangle(back, point.X + 128 * 2 + 5, point.Y + 45, 128 - 10, 30);
            if (_stamina > 2 && _stamina < 3) { SplashKit.FillRectangle(notfull, point.X + 128 * 2 + 5, point.Y + 45, 128* (_stamina-2)-10, 30); }
            else if (_stamina == 3) { SplashKit.FillRectangle(full, point.X + 128 * 2 +5, point.Y + 45, 128-10, 30); }
        }
    }
}
