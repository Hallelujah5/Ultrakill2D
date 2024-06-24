using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PNGtest2.Map;
using SplashKitSDK;
using Ultrakill2D.Weapon;
using static System.Net.Mime.MediaTypeNames;
using Color = SplashKitSDK.Color;
using Rectangle = System.Drawing.Rectangle;

namespace Ultrakill2D
{
    public class V1
    {
        //movement 
        private Boolean load = false;
        private Bitmap _V1stand;
        private Bitmap _V1move;
        private Bitmap _V1fall;
        private Bitmap _V1dash;
        private Bitmap _V1slide;

        private Bitmap currentPose;
        private Point2D _location;

        //weapon
        private Bitmap _Piston_G;
        private Bitmap _Piston_GF;
        private Bitmap _Shotgun_B;
        private Bitmap _Shotgun_BF;

        private Bitmap _currentWeapon;
        private List<Rectangle> _rects;
        private Bullets _bullets;
        private List<Enemy> _enemylist;
        private Boolean _status_bar = false;
        private Boolean _invicible = false;

        public void Load(double x, double y)
        {
            _V1stand = new Bitmap("V1stand", "F:\\Projects\\Ultrakill2D\\Ultrakill2D\\Resources\\images\\V1stand.png");
            _V1move = new Bitmap("V1move", "F:\\Projects\\Ultrakill2D\\Ultrakill2D\\Resources\\images\\Duck.png");
            _V1fall = new Bitmap("V1fall", "F:\\Projects\\Ultrakill2D\\Ultrakill2D\\Resources\\images\\V1fall.png");
            _V1dash = new Bitmap("V1dash", "F:\\Projects\\Ultrakill2D\\Ultrakill2D\\Resources\\images\\V1dash.png");
            _V1slide = new Bitmap("V1slide", "F:\\Projects\\Ultrakill2D\\Ultrakill2D\\Resources\\images\\V1slide.png");

            currentPose = _V1fall;

            _Piston_G = new Bitmap("Piston_G", "F:\\Projects\\Ultrakill2D\\Ultrakill2D\\Resources\\images\\Piston_G.png");
            _Piston_GF = new Bitmap("Piston_GF", "F:\\Projects\\Ultrakill2D\\Ultrakill2D\\Resources\\images\\Piston_GF.png");
            _Shotgun_B = new Bitmap("Shotgun_B", "F:\\Projects\\Ultrakill2D\\Ultrakill2D\\Resources\\images\\Shotgun_B.png");
            _Shotgun_BF = new Bitmap("Shotgun_BF", "F:\\Projects\\Ultrakill2D\\Ultrakill2D\\Resources\\images\\Shotgun_BF.png");

            SelectedWeapon = "piston";


            _location.X = x;
            _location.Y = y;

            _center.X = _location.X + 32;
            _center.Y = _location.Y + 32;

            _hp = 100;
            _stamina = 3;
            _harddamage = 0;

            //Console.WriteLine("v1 load done");
            load = true;
        }
        public void V1_main(Window gameWindow, List<Rectangle> rects, Bullets bullets, List<Enemy> enemylist, Shader shader, DrawMap _Map)
        {
            if (SplashKit.KeyTyped(KeyCode.IKey)) { if (!_invicible) { _invicible = true; } else { _invicible = false; } }

            if (_hp <= 0 && !_invicible)//death
            {
                bullets.Clear();
                enemylist.Clear();
                _Map.ResetBossfight();
                Load(1300,136);
            }
            if (!load) { Load(500,-1500); }
            _rects = rects;
            _bullets = bullets;
            _enemylist = enemylist;

            if (SplashKit.KeyTyped(KeyCode.TabKey)) { if (!_status_bar) { _status_bar = true; } else { _status_bar = false; } }
            if (_status_bar)
            {
                location(rects);
            }

            Move(rects);

            Loop();

            SplashKit.MoveCameraTo(_location.X - gameWindow.Width / 2 + 32, _location.Y - gameWindow.Height / 2 + 32);

        }

        public Point2D V1_location()
        {
            return _location;
        }

        private Rectangle _rectUp;
        private Rectangle _rectDown;
        private Rectangle _rectLeft;
        private Rectangle _rectRight;
        private Rectangle _hitbox;

        private void setRect()
        {
            _rectUp = new Rectangle(Convert.ToInt32(_location.X) + 32, Convert.ToInt32(_location.Y)-1 +15, 1, 1);
            _rectDown = new Rectangle(Convert.ToInt32(_location.X) + 32, Convert.ToInt32(_location.Y) + 64, 1, 1);

            _rectLeft = new Rectangle(Convert.ToInt32(_location.X) - 1+15, Convert.ToInt32(_location.Y) + 32, 1, 1);
            _rectRight = new Rectangle(Convert.ToInt32(_location.X) + 64-15, Convert.ToInt32(_location.Y) + 32, 1, 1);

            if (!_sliding)
            {
                _hitbox = new Rectangle(Convert.ToInt32(_location.X) + 32 - 5, Convert.ToInt32(_location.Y) + 32 - 5, 10, 10);
            }
            else
            {
                _hitbox = new Rectangle(Convert.ToInt32(_location.X) + 32 - 5, Convert.ToInt32(_location.Y) + 32 - 5 + 32 - 5, 10, 10);

            }

        }
        private Boolean Collosion(List<Rectangle> rects, Rectangle RECT)
        {

            foreach (Rectangle rect in rects)
            {

                if (RECT.IntersectsWith(rect))
                {
                    return false;
                }

            }
            return true;
        }
        private double Antistuck(List<Rectangle> rects, Rectangle RECT, double speedX, double speedY)
        {
            double distance;
            if (speedX == 0)
            {
                distance = speedY;
            }
            else { distance = speedX; }

            Boolean negative;
            if (speedX < 0 || speedY < 0)
            {
                negative = true;
            }
            else { negative = false; }


            Rectangle c_rect = RECT;

            for (double i = 0; i < Math.Abs(distance)-1; i++)
            {
                if (speedX != 0)
                {
                    if (speedX > 0) { c_rect.X++; }
                    else { c_rect.X--; }
                }
                if (speedY != 0)
                {
                    if (speedY > 0) { c_rect.Y++; }
                    else { c_rect.Y--; }
                }
                foreach (Rectangle rect in rects)
                {
                    if (c_rect.IntersectsWith(rect))
                    {
                        if (!negative) { return i; }
                        else { return i * -1; }
                    }
                }
            }
            return distance;

        }

        //----movement data----
        public Boolean _faceLeft = false;
        public Boolean move = false;
        private Boolean _Left = false;


        private int _speed = 10;
        //on air stuff
        public int _Yvelocity =0;
        private int _gravity = 1;
        //slam jump
        private int _jumpPlus = 0;
        //dash
        private int _dash = 0;
        //jumpdash
        private int _Xvelocity = 0;
        private Boolean _pullLeft = false;
        //slide
        public Boolean _sliding = false;
        private Boolean _slidingLeft = false;
        private double _slidespeed = 0;
        // v1 static

        private double _stamina = 3.00;

        private void Move(List<Rectangle> rects)
        {
            
            setRect();
            move = false;
            

            //some timer
            if (_jumpPlus >0) { _jumpPlus--; }
            if (_dash > 0) { _dash--; }

            //draw v1
            Draw(currentPose, _Left);
            Weapon();


            //set pose
            if (!move && !onAir(rects) && _dash == 0 && !_sliding)
            {
                currentPose = _V1stand;

            }
            else if (onAir(rects) && _dash == 0 && !_sliding)
            {
                currentPose = _V1fall;

            }


            //dash
            if (SplashKit.KeyTyped(KeyCode.LeftShiftKey) && _stamina >=1)
            {
                _dash = 10;
                _stamina -= 1;
            }
            if (_dash > 0)
            {
                move = true;
                if (_dash == 10)
                {
                    _Yvelocity = 0;
                    _Xvelocity = 0;
                }
                currentPose = _V1dash;
                if (_faceLeft)
                {
                    double speed = Antistuck(rects, _rectLeft, -25, 0);
                    _location.X += speed;
                }
                else 
                {
                    double speed = Antistuck(rects, _rectRight, 25, 0);
                    _location.X += speed;
                }

            }
            // x velocity
            if (onAir(rects) && _dash == 0 && _Xvelocity >0)
            {
                move = true;
                if (_pullLeft)
                {
                    double speed = Antistuck(rects, _rectLeft, -_Xvelocity, 0);
                    _location.X += speed;
                    if (speed == 0) { _Xvelocity=0;}
                }
                else
                {
                    double speed = Antistuck(rects, _rectRight, _Xvelocity, 0);
                    _location.X += speed;
                    if (speed == 0) { _Xvelocity = 0;}
                }
            } 

            if (!onAir(rects)) { _Xvelocity = 0; }

            //slide
            if (SplashKit.KeyDown(KeyCode.SKey) && !onAir(rects) && _dash == 0 && _jumpPlus ==0)
            {
                if (!_sliding)
                {
                    _sliding = true;
                    _slidespeed = 15;

                }
                _slidingLeft = _faceLeft;
            }
            else { _sliding = false; }

            if (_sliding)
            {
                currentPose = _V1slide;
                if (_slidingLeft)
                {
                    double speed = Antistuck(rects, _rectLeft, -_slidespeed, 0);
                    _location.X += speed;
                }
                else
                {
                    double speed = Antistuck(rects, _rectRight, _slidespeed, 0);
                    _location.X += speed;
                }
                if (_slidespeed > 13)
                {
                    _slidespeed -= 0.5;
                }
                if (_stamina != 3)
                {
                    _stamina -= 0.01;
                }


            }
            //slam
            if (SplashKit.KeyTyped(KeyCode.SKey) && onAir(rects) && _dash == 0)
            {
                _Yvelocity = 80;
                _jumpPlus = 10;
            }
            //air break
            if (SplashKit.KeyDown(KeyCode.SpaceKey) && onAir(rects) && _dash == 0)
            {
                if (_Yvelocity > 0)
                {
                    _Yvelocity = 3;
                }
                if (_stamina != 3)
                {
                    _stamina -= 0.01;
                }

            }


            //jump
            if (SplashKit.KeyDown(KeyCode.SpaceKey) && Collosion(rects, _rectUp) && (!onAir(rects)))
            {
                _location.Y -= 10;
                if (_jumpPlus > 0) { _Yvelocity = -50; if (_stamina >= 1) { _stamina -= 1; } }
                else if (_dash ==0 && !_sliding) { _Yvelocity = -30; }
                if (_dash>0 && _stamina >= 1) 
                {
                    _Xvelocity = 25;
                    _pullLeft = _faceLeft;
                    _Yvelocity = -15;
                    _stamina -= 1;
                }
                if (_sliding)
                {
                    _Xvelocity = (int)Math.Round(_slidespeed+5);
                    _pullLeft = _faceLeft;
                    _Yvelocity = -10;
                }
            }
            //up
            if ( _Yvelocity < 0)
            {
                if (Math.Abs(_Yvelocity) <= 50)
                {
                    _Yvelocity += _gravity;
                }
                double speed = Antistuck(rects, _rectUp, 0, _Yvelocity);
                _location.Y += speed;
                if (speed == 0)
                {
                    _Yvelocity = 0;
                }
            }
            //down
            if (_Yvelocity >= 0 && onAir(rects) && _dash == 0)
            {
                if (Math.Abs(_Yvelocity) <= 50)
                {
                    _Yvelocity += _gravity;
                }

                double speed = Antistuck(rects, _rectDown, 0, _Yvelocity);
                _location.Y += speed;
                if (speed == 0)
                {
                    _Yvelocity =0;
                }
            }

            //ground move
            if (SplashKit.KeyDown(KeyCode.AKey) && Collosion(rects, _rectLeft) && _dash == 0 &&!_sliding)
            {
                _faceLeft = true;
                move = true;

                if (!onAir(rects))
                {
                    currentPose = _V1move;   
                }
                double speed = Antistuck(rects, _rectLeft, -_speed, 0);
                _location.X += speed;

            }
            else if (SplashKit.KeyDown(KeyCode.DKey) && Collosion(rects, _rectRight) && _dash == 0&&!_sliding)
            {
                _faceLeft = false;
                move = true;

                if (!onAir(rects))
                {
                    currentPose = _V1move;
                }
                double speed = Antistuck(rects, _rectRight, _speed, 0);
                _location.X += speed;
            }


        }

        private Boolean onAir(List<Rectangle> rects)
        {

            foreach (Rectangle rect in rects)
            {

                if (_rectDown.IntersectsWith(rect))
                {
                    return false;
                }

            }
            return true;
        }


        //weapon funcs
        private Point2D _center;
        private double _angle = 0;
        private void Weapon()
        {
            Point2D mouse;
            mouse.X = SplashKit.MouseX();
            mouse.Y = SplashKit.MouseY();
            Angle(mouse);

            Armory();

            SplashKit.FillRectangle(Color.Red, _GunHead.X-2, _GunHead.Y-2, 4, 4);
            WeaponAct();

            WDraw();
        }
        private int pistonCD = 0;
        private int shotgunCD = 0;
        private void WeaponAct()
        {
            if (pistonCD > 0) { pistonCD--; }
            if (shotgunCD > 0) { shotgunCD--; }

            if (SplashKit.MouseDown(MouseButton.LeftButton))
            {
                //Console.WriteLine("Shot!");
                if (SelectedWeapon == "piston" && pistonCD ==0)
                {
                    Piston piston = new Piston();
                    piston.Shot(_rects, _GunHead, _bulletV, _bullets, _enemylist);
                    Blood(10, 100, piston.point2);
                    pistonCD = 30;

                }
                if (SelectedWeapon == "shotgun" && shotgunCD ==0)
                {
                    Shotgun shotgun = new Shotgun();
                    shotgun.Shot(_rects, _GunHead, _bulletV, _bullets, _enemylist);
                    shotgunCD = 10;
                }
 
            }
        }
        private string SelectedWeapon;
        private void Armory()
        {
            if (SplashKit.KeyTyped(KeyCode.Num1Key))
            {
                SelectedWeapon = "piston";
            }
            if (SplashKit.KeyTyped(KeyCode.Num2Key))
            {
                SelectedWeapon = "shotgun";
            }

        }

        private void Angle(Point2D point)
        {
            Vector2D V;
            V.X = point.X- 1280 / 2;  V.Y = point.Y - 700 / 2;
            V = SplashKit.VectorMultiply(V, 1/SplashKit.VectorMagnitude(V));

            _angle = (Math.Atan2(V.Y, V.X) - Math.Atan2(0, 1)) / Math.PI * 180;
            GunHead(V);
            _bulletV = V;
        }

        private Point2D _GunHead;
        private Vector2D _bulletV;
        private void GunHead(Vector2D V)
        {
            Point2D point;
            V = SplashKit.VectorMultiply(V,32);
            if (!_sliding)
            {
                point.X = V.X + 1280 / 2; point.Y = V.Y + 700 / 2;
            }
            else
            {
                point.X = V.X + 1280 / 2; point.Y = V.Y + 32 -5 + 700 / 2;
            }
            _GunHead = SplashKit.ToWorld(point);           
        }

        private void WDraw()
        {
            if (!_sliding)
            {
                if (_angle > -90 && _angle < 90)
                {
                    if (SelectedWeapon == "piston") { _currentWeapon = _Piston_G; }
                    if (SelectedWeapon == "shotgun") { _currentWeapon = _Shotgun_B; }

                    SplashKit.DrawBitmap(_currentWeapon, _location.X, _location.Y, SplashKit.OptionRotateBmp(_angle));
                    _Left = false;
                }
                else//fk splashkit since it doesnt allow to rotated and flip Bitmap at the same time
                {
                    if (SelectedWeapon == "piston") { _currentWeapon = _Piston_GF;}
                    if (SelectedWeapon == "shotgun") { _currentWeapon = _Shotgun_BF; }

                    SplashKit.DrawBitmap(_currentWeapon, _location.X, _location.Y, SplashKit.OptionRotateBmp(_angle));
                    _Left = true;
                }
            }
            else
            {
                SplashKit.DrawBitmap(_currentWeapon, _location.X, _location.Y +32-4, SplashKit.OptionRotateBmp(_angle));
            }
        }

        private void Draw(Bitmap _png,Boolean rotate)
        {
            if (rotate == true)
            {
                SplashKit.DrawBitmap(_png,_location.X, _location.Y,SplashKit.OptionFlipY());
            }
            else
            {
                _png.Draw(_location.X, _location.Y);
            }
            //SplashKit.FillRectangle(Color.Aqua, 300, 50, 30, 30);
            //hitbox
            //if (!_sliding)
            //{ SplashKit.FillRectangle(Color.Crimson, _location.X + 32 - 5, _location.Y + 32 - 5, 10, 10); }
            //else { SplashKit.FillRectangle(Color.Crimson, _location.X + 32 - 5, _location.Y + 32 - 5 +32 -5, 10, 10); }

        }

        private void location(List<Rectangle> rects)
        {

            SplashKit.DrawText("X: " + _location.X, Color.White,_location.X,_location.Y+64);
            SplashKit.DrawText("Y: " + _location.Y, Color.White, _location.X, _location.Y+74);
            SplashKit.DrawText("On Air: " + onAir(rects), Color.White, _location.X, _location.Y + 84);
            SplashKit.DrawText("Y velocity: " + _Yvelocity, Color.White, _location.X, _location.Y + 94);
            SplashKit.DrawText("X velocity: " + _Xvelocity, Color.White, _location.X, _location.Y + 104);
            SplashKit.DrawText("Angle: " + Math.Round(_angle), Color.White, _location.X, _location.Y + 114);
            SplashKit.DrawText("Stamina: " + _stamina, Color.Aqua, _location.X, _location.Y + 124);
            SplashKit.DrawText("Hp: " + _hp, Color.Red, _location.X, _location.Y + 134);
            SplashKit.DrawText("Hard Damge: " + _harddamage, Color.Gray, _location.X, _location.Y + 144);
            SplashKit.DrawText("Undead? : " + _invicible, Color.White, _location.X, _location.Y + 154);



        }

        public int _hp = 1000;
        public double _harddamage = 0.0;
        public int _harddamage_timer = 0;

        private int _iframe = 0;
        //methods to get data---------------------------------------------------------
        public double stamina()
        {
            return _stamina;
        }
        public int hp()
        {
            return _hp;
        }
        public double HardDamage()
        {
            return _harddamage;
        }

        public double X()
        {
            return _location.X + 32;
        }
        public double Y()
        {
            return _location.Y + 32;
        }
        public Rectangle Rect()
        {
            return _hitbox;
        }
        //---------------------------------------------------------------------------

        public void Damage(int damage)
        {
            if (_dash == 0 && _iframe == 0 )
            {
                _hp -= damage;
                _iframe = 30;
                if (_hp < 0) { _hp = 0; }

                _harddamage += damage / 5.0;
                _harddamage_timer = 120;

            }
        }
        public void Blood(int heal,double distance, Point2D location)
        {
            double distance2 = SplashKit.PointPointDistance(_location, location);
            if (distance2 <= distance) { Heal(heal); }
        }
        public void Heal(int heal)
        {
            _hp += heal;
            if (_hp > 100) { _hp = 100; }
        }

        private void Loop()
        {
            if (_stamina < 3) { _stamina += 0.01; }
            if (_stamina > 3) { _stamina = 3; }
            if (_stamina < 0) { _stamina = 0; }

            if (_iframe > 0) { _iframe--; }
            //hard damage
            if (_harddamage_timer > 0) { _harddamage_timer--; }
            else if (_harddamage > 0) {  _harddamage -= 0.2; }
            int max_hp = 100 - Convert.ToInt32(_harddamage);
            if (_hp > max_hp) { _hp = max_hp; }
        }


    }
}
