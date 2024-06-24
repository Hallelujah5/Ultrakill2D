using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Rectangle = System.Drawing.Rectangle;
using Color = SplashKitSDK.Color;
using Ultrakill2D.Weapon;
using System.Reflection;

namespace Ultrakill2D
{
    public class Enemies
    {
        private List<Enemy> _enemies; //(tip) List = array but with flex length

        public Enemies()
        {
            _enemies = new List<Enemy>();// creating array
        }

        public void Add(Enemy item)
        {
            _enemies.Add(item); // add folder or file
        }

        public List<Enemy> GetList()
        {
            return _enemies;
        }
            public void Draw(Effect _effect, V1 _v1, List<Rectangle> _rects, Bullets _bullets) //print out all what it have inside the array
            {
                for (int i = 0; i <=_enemies.Count - 1; i++)
                {
                    Enemy item = _enemies[i];

                    item.Func(_enemies,_effect, _v1, _rects, _bullets);
                    item.Draw(_effect);

                    if (item.Death())
                    {
                        _enemies.RemoveAt(i);
                    }

                    if (!_enemies.Any(enemy => enemy.GetType() == typeof(Prison)))
                    {
                        _enemies.Clear(); // Remove all items from the list
                    }

                }
            }
    }

    public abstract class Enemy
    {
        public abstract void Draw(Effect _effect);
        public abstract void Func(List<Enemy> _enemies, Effect _effect, V1 _v1, List<Rectangle> _rects, Bullets _bullets);
        public abstract Boolean Death();

        public abstract void Damage(double damage);
        public abstract Rectangle Rect();

        public abstract void SetData(Boolean a);
    }

    public class Prison : Enemy
    {
        private Boolean _death = false;
        private Point2D _point;

        //image
        private Bitmap _prison;
        //hitbox
        private Rectangle _rect;
        //status
        private double _hp = 100;
        private double _hp_timer = 60.0*3;
        //blackhole
        private BlackHole _blackhole;


        public Prison(double x, double y)
        {
            _point.X = x;
            _point.Y = y;
            _prison = new Bitmap("prison", "F:\\Projects\\Ultrakill2D\\Ultrakill2D\\Resources\\images\\prison.png");
            _rect = new Rectangle(Convert.ToInt32(_point.X-320/2), Convert.ToInt32(_point.Y-320/2), 320, 320);
            _current_bar = 60.0 * 3;
            _blackhole = new BlackHole();
            _spellCD = 60 * 3;
        }

        public override Rectangle Rect()
        {
            return _rect;
        }
        public override void Draw(Effect _effect) 
        {
            SplashKit.DrawBitmap(_prison, _point.X, _point.Y,SplashKit.OptionScaleBmp(10,10));
            if (_blackhole._livetime > 0) { _blackhole.Draw(); }
            StatusBar();
            if (_hp <= 0 )
            {
                Circle die = new Circle(_point.X + 32, _point.Y + 32, 60, 1, 50, SplashKit.RGBAColor(255, 0, 0, 100));
                _effect.Add(die);

            }
        }
        
        public override Boolean Death() 
        {
            if (_hp <= 0) { _death = true; };
            return _death;
        }
        public override void Damage(double damage)
        {
            if (_heal) { _hp -= damage / 20; }
            else 
            {
                if (damage == 1) { _hp -= 0.5; _hp_timer -= damage * 25; }//piston resist 50%
                else { _hp -= damage; _hp_timer -= damage * 50; }
            }
            _damage_timer = 20;
        }
        public override void SetData(bool a)
        {
            //nothing   
        }
        private double _current_bar;
        private double _damage_timer=0;
        private int _healBar_timer = 0;

        private double _last_hp = 100;
        private void StatusBar()
        {
            //hp bar func
            Color hp = SplashKit.ColorRed();
            //damaged
            if (_damage_timer > 0) { _damage_timer--; }
            else { if (_last_hp > _hp) { _last_hp -= 0.2; }
            if ( _hp > _last_hp) { _last_hp = _hp; }
            //healed
            if (_healBar_timer > 0) { _healBar_timer--; }
            if (_healBar_timer > 0) { hp = SplashKit.RGBColor(0, 255, 0); }
            }
            // 
            Point2D point;
            point.X = 0; point.Y = 0;
            point = SplashKit.ToWorld(point);
            SplashKit.FillRectangle(Color.DarkRed,point.X,point.Y,1280,20);
            SplashKit.FillRectangle(Color.RGBColor(255,175,0), point.X, point.Y, 12.8 * _last_hp, 20);
            SplashKit.FillRectangle(hp, point.X, point.Y,12.8*_hp, 20);
            SplashKit.DrawText("FLESH PRISON", Color.White, point.X+1280/2-30, point.Y+5);
            SplashKit.FillRectangle(Color.DarkGreen, point.X, point.Y+20, 1280, 5);
            SplashKit.FillRectangle(Color.RGBColor(0,255,0), point.X, point.Y+20, 12.8 *(_hp_timer/(_current_bar/100)), 5);
            //Console.WriteLine(_hp_timer / 60.0 * 25 / 100);
        }

        
        private void Loop(List<Enemy> enemies, Effect _effect, Bullets _bullets, List<Rectangle> _rects, V1 _v1)
        {
            if (_hp_timer > 0) { _hp_timer -= 0.5; };

            //healing spell
            if (_hp_timer <=0) 
            {
                if (check_eyes(enemies) && !_heal && !_eyes_spawn) 
                {
                    _heal = true;
                    _heal_delay = 60 * 5;
                    Circle healingWarn = new Circle(_point.X + 32, _point.Y + 32, 60*5, 350, 0, SplashKit.RGBAColor(0, 255, 0, 80));
                    _effect.Add(healingWarn);
                    Set_EyeData(enemies);
                    //stop blue spell
                    _blueSpell = false;
                    _blueDelay = 0;
                    _blueIndex = 0;
                    //


                }
                else if (!_heal && !_eyes_spawn) 
                { 
                    _eyes_spawn = true;
                    _blueSpell = false;
                    _blueDelay = 0;
                    _blueIndex = 0;
                }

            }

            //healing//spawn eye
            if (_eyes_spawn) { Spawn_eyes(enemies,_effect); }
            if (_spawn_delay > 0) { _spawn_delay -= 1; };

            //healing//heal
            if (_heal) { Healing(enemies, _effect); }
            if (_heal_delay > 0) { _heal_delay -= 1; }

            //AI cast spell
            if (_hp_timer > 0)
            {
                Spell(_v1, _effect);
            }

            //blue spell
            if (SplashKit.KeyTyped(KeyCode.BKey)) { _blueSpell = true; }
            if (_blueSpell) { Blue_spell(_bullets, _rects); };
            //black spell
            if (SplashKit.KeyTyped(KeyCode.NKey)) { BlackSpell(_v1,_effect); }
            //white spell
            if (SplashKit.KeyTyped(KeyCode.MKey)) { _whiteSpell = true; }
            if (_whiteSpell) { WhiteSpell(_v1, _effect); }


        }
        //boss function
        public override void Func(List<Enemy> _enemies, Effect _effect, V1 _v1, List<Rectangle> _rects, Bullets _bullets)
        {
            Loop(_enemies,_effect,_bullets,_rects,_v1);

            if (SplashKit.KeyTyped(KeyCode.PKey)) { _hp = 0; }
        }

        private void Set_EyeData(List<Enemy> enemies)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.GetType() == typeof(Eye))
                {
                    enemy.SetData(_heal);
                }
            }

        }

        //healing spell
        private Boolean _eyes_spawn = false;
        private int _eye_index = 0;
        private int _spawn_delay = 0;

        private void Spawn_eyes(List<Enemy> enemies, Effect _effect) 
        {
            _spellCD = 60 * 10;
            Point2D center; center.X = _point.X + 32; center.Y = _point.Y + 32;
            double radius = 400;
            double degree = Math.PI / 5;
            if (_eye_index < 10 && _spawn_delay == 0)
            {
                double d = _eye_index * degree;
                double x = center.X + radius * Math.Cos(d);
                double y = center.Y + radius * Math.Sin(d);
                Eye eye = new Eye(x-16, y-16);
                enemies.Add(eye);
                _spawn_delay = 10;
                _eye_index += 1;
                _hp_timer += 60 * 3;
                //spawn effect
                Circle spawneffect = new Circle(x, y,10,20,1,SplashKit.RGBAColor(127,0,255,150));
                _effect.Add(spawneffect);

            }
            if (_eye_index == 10)
            {
                _eyes_spawn = false;
                _current_bar = _hp_timer;
                _eye_index = 0;//reset
                
            }
        }

        private Boolean _heal = false;
        private int _heal_delay = 0;

        private void Healing(List<Enemy> enemies, Effect _effect) 
        {
            if (_heal_delay == 0)
            {
                foreach (Enemy enemy in enemies)
                {
                    if (enemy.GetType() == typeof(Eye))
                    {
                        enemies.Remove(enemy);
                        _heal_delay = 30;
                        _hp += 10;
                        Circle spawneffect = new Circle(_point.X+32, _point.Y+32, 10, 700, -100, SplashKit.RGBAColor(0, 255, 0, 150));
                        _effect.Add(spawneffect);
                        _healBar_timer = 10;
                        break;
                    }
                }
            }
            if (_hp > 100) { _hp = 100; }
            if (enemies.Count == 1 && _heal_delay ==0)
            {
                _heal = false;
                _eyes_spawn = true;
            }

        }

        private Boolean check_eyes(List<Enemy> enemies)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.GetType() == typeof(Eye))
                {
                    return true;
                }
            }
            return false;
        }

        //attack spell
        //---blue
        private int _spellCD = 0;
        private void Spell(V1 _v1,Effect _effect)
        {
            if (_spellCD > 0) { _spellCD--; }
            else if ( _hp_timer > 0)
            {
                Random rnd = new Random();
                int i =rnd.Next(0,150);
                if (i < 50)
                {
                    _blueSpell = true;
                    _spellCD = 60 * 12;
                    Circle Spellwarn = new Circle(_point.X + 32, _point.Y + 32, 60 , 0, 20, SplashKit.RGBAColor(0, 255, 255, 80));
                    _effect.Add(Spellwarn);


                }
                else if (i < 100)
                {
                    if (_blackhole._livetime == 0)
                    {
                        BlackSpell(_v1, _effect);
                        _spellCD = 60 * 4;
                        Circle Spellwarn = new Circle(_point.X + 32, _point.Y + 32, 60 , 0, 20, SplashKit.RGBAColor(255, 0, 255, 80));
                        _effect.Add(Spellwarn);

                    }
                    else { Spell(_v1, _effect); }

                }
                else if (i <= 150)
                {
                    if (!_whiteSpell)
                    {
                        _whiteSpell = true;
                        _spellCD = 60 * 6;
                        Circle Spellwarn = new Circle(_point.X + 32, _point.Y + 32, 60 , 0, 20, SplashKit.RGBAColor(255, 255, 255, 80));
                        _effect.Add(Spellwarn);

                    }
                    else { Spell(_v1, _effect); }


                }
            }
        }
        private Boolean _blueSpell = false;
        private int _blueDelay = 0;
        private int _blueIndex = 0;
        private void Blue_spell(Bullets _bullets, List<Rectangle> _rects)
        {
            Point2D center = new Point2D() { X = _point.X +32, Y = _point.Y +32 };
            double radius = 100;
            double degree = (2 * Math.PI) / 36;

            if (_blueDelay > 0) { _blueDelay--; }
            if (_blueSpell && _blueDelay == 0)
            {
                double d = _blueIndex * degree;
                double x = center.X + radius * Math.Cos(d);
                double y = center.Y + radius * Math.Sin(d);
                //set data
                Point2D point = new Point2D() { X= x-16, Y= y-16 };
                Vector2D V = new Vector2D() { X = (x - center.X) / radius, Y = (y - center.Y) / radius };
                //add bullets
                Homing bullet = new Homing(point, V, _rects);
                _bullets.Add(bullet);
                //
                _blueIndex += 1;
                _blueDelay = (int)Math.Round(10 / (1 + _blueIndex / 60.0), 0);
            }
            if (_blueIndex >= 75)
            {
                _blueIndex = 0;
                _blueSpell = false;
            }
        }
        //---N Spell :))
        private void BlackSpell(V1 v1, Effect effect)
        {
            _blackhole.Load(v1, effect, _point.X, _point.Y);
        }
        //---White Spell
        private Boolean _whiteSpell = false;
        private int _whiteIndex = 0;
        private int _whiteDelay = 0;
        private int _pillarwarn = 0;
        private double _W_location;
        private void WhiteSpell(V1 v1, Effect effect)
        {
            //main
            if (_whiteDelay > 0) { _whiteDelay--; }
            else if (_whiteIndex < 3)
            {
                SetPillar(v1);
                _pillarwarn = 60;
                _whiteDelay = 60 * 4;
                //reset pillar data
                _pillareffect = true;
                _pillareHit = false;
                //
                _whiteIndex += 1;
            }
            //warn
            if (_pillarwarn > 0)
            {
                _pillarwarn--;
                Pillar_warn();
            }
            //pillar incoming
            if (_whiteDelay <= 60 * 3)
            {
                Pillar(v1, effect);
            }
            //end
            if (_whiteIndex == 3 && _whiteDelay == 0)
            {
                //Console.WriteLine("Done");
                _pillareffect = true;
                _pillareHit = false;
                _whiteDelay = 0;
                _whiteSpell = false;
                _whiteIndex = 0;
            }


        }
        private void SetPillar(V1 v1)
        {
            if (!v1.move && !v1._sliding)//v1 stand still
            {
                _W_location = v1.X();
            }
            else//v1 move
            {
                if (v1._faceLeft)
                {
                    _W_location = v1.X() - 128 *2 ;
                }
                else
                {
                    _W_location = v1.X() + 128 *2 ;
                }
            }
        }
        private void Pillar_warn()
        {
            Bitmap warn = new Bitmap("pillarwarn", "F:\\Projects\\Ultrakill2D\\Ultrakill2D\\Resources\\images\\pillar_warn.png");
            double X = _W_location - 128 - 128 / 2; double Y = 136 + 32;
            SplashKit.DrawBitmap(warn,X,Y);
            SplashKit.FillRectangle(Color.RGBAColor(255, 255, 255, 100), X, -1500, 128 * 3, 1500 + 136 + 64);
        }
        private Boolean _pillareffect = true;
        private Boolean _pillareHit = false;
        private void Pillar(V1 v1, Effect effect)
        {
            if (_pillareffect)
            {
                //Console.WriteLine("Pillar "+ _whiteIndex);
                Pillar pillar = new Pillar(_W_location, -1500, 60, 60 * 3, 1500 + 136 + 64, 128 + 128 / 2, -1.6*2, Color.RGBAColor(255,255,255,100));
                effect.Add(pillar);
                pillar = new Pillar(_W_location, -1500, 60, 60 * 3, 1500 + 136 + 64, 128, -128/60, Color.RGBAColor(255, 255, 255, 200));
                effect.Add(pillar);
                pillar = new Pillar(_W_location, -1500, 60, 60 * 3, 1500 + 136 + 64, 128/2, (-128 /2) / 60, Color.RGBAColor(255, 255, 255, 255));
                effect.Add(pillar);


                _pillareffect = false;
            }
            if (!_pillareHit)
            {
                Rectangle hitbox = new Rectangle(Convert.ToInt32(_W_location-(128+128/2)),-1500,128*3, 1500 + 136 + 64);
                if (hitbox.IntersectsWith(v1.Rect()))
                {
                    v1.Damage(20);
                    v1._Yvelocity = -30;
                    _pillareHit = true;
                }
            }

        }

    }
    //------------------------------------------------------------------
    public class Eye : Enemy
    {
        private Boolean _death = false;
        private Point2D _point;

        //image
        private Bitmap _eye;
        //hitbox
        private Rectangle _rect;
        //status
        private double _hp = 1;
        private Boolean _bosshealing = false;

        public override void SetData(bool a)
        {
            _bosshealing = a;
        }

        public Eye(double x, double y)
        {
            _point.X = x;
            _point.Y = y;
            _eye = new Bitmap("eye", "F:\\Projects\\Ultrakill2D\\Ultrakill2D\\Resources\\images\\eye.png");

        }


        public override void Draw(Effect _effect)
        {
            _rect = new Rectangle(Convert.ToInt32(_point.X - 8), Convert.ToInt32(_point.Y - 8), 40, 40);
            SplashKit.DrawBitmap(_eye, _point.X, _point.Y);
            if (_bosshealing)
            {
                SplashKit.FillCircle(Color.RGBAColor(0, 255, 0, 150), _point.X + 16, _point.Y + 16, 30);

                Point2D boss; boss.X = 3296+32; boss.Y = -328+32;
                Point2D eye; eye.X = _point.X + 16; eye.Y = _point.Y + 16;

                SplashKit.DrawLine(Color.RGBColor(0, 255, 0), eye, boss);

            }
            if (_hp <= 0)
            {
                Circle blood = new Circle(_point.X + 16, _point.Y + 16, 12, 10, 5, SplashKit.RGBAColor(255, 0, 0, 150));
                _effect.Add(blood);

            }

        }
        public override Boolean Death()
        {
            if (_hp <= 0)
            {
                _death = true;
            };

            return _death;
        }
        public override void Damage(double damage)
        {
            if (damage == 0.6) { _hp -= 0.3; }//shotgun resist
            else
            {
                _hp -= damage;
            }
        }
        public override Rectangle Rect()
        {
            return _rect;
        }
        public override void Func(List<Enemy> _enemies, Effect _effect, V1 _v1, List<Rectangle> _rects, Bullets _bullets)
        {
            Shot(_rects, _v1, _bullets);

            if (!_bosshealing && _warning == 0)
            {
                Move(_enemies);
            }
        }
        //moving
        private int MoveCD = 60 * 3;
        private int _move = 0;
        private Boolean _moveright;
        private void Move(List<Enemy> _enemies)
        {
            if (_move > 0) { _move--; Eye_movement(); }
            if (MoveCD > 0) { MoveCD--; }
            else
            {
                Random rnd = new Random();
                int i = rnd.Next(0, 150);
                int i2 = rnd.Next(60, 60 * 5);
                MoveCD = i2;
                if (i < 50)//right
                {
                    if (MoveCheck(_enemies, 0))
                    {
                        _move = 20;
                        _moveright = true;
                    }
                    else if (MoveCheck(_enemies, 1))
                    {
                        _move = 20;
                        _moveright = false;

                    }
                }
                else if (i < 100)
                {
                    if (MoveCheck(_enemies, 1))
                    {
                        _move = 20;
                        _moveright = false;

                    }
                    else if (MoveCheck(_enemies, 0))
                    {
                        _move = 20;
                        _moveright = true;

                    }
                }

            }
        }
        private Boolean MoveCheck(List<Enemy> _enemies, int i)
        {
            Enemy boss = _enemies[0];
            Rectangle bossRect = boss.Rect();

            int distance;
            if (i == 0) { distance = 128 * 2; } else { distance = -128 * 2; }
            // map check
            if (_point.X + distance <= 1900) { return false; }
            if (_point.X + distance >= 4864 - 128 * 2) { return false; }
            //hitbox check
            Rectangle checkbox = new Rectangle(Convert.ToInt32(_point.X - 8 + distance), Convert.ToInt32(_point.Y - 8), 40, 40);
            if (bossRect.IntersectsWith(checkbox)) { return false; }
            return true;
        }
        private void Eye_movement()
        {
            if (_move > 0)
            {
                if (_moveright)
                {
                    _point.X += 12.8;
                }
                else { _point.X -= 12.8; }
            }
        }
        //attacking
        private int _shotCD = 60 * 2;
        private int _warning = 0;
        private Point2D _point1;
        private Point2D _point2;
        private Vector2D _vector;
        private void Shot(List<Rectangle> _rects, V1 _v1, Bullets _bullets)
        {
            //Console.WriteLine("Test");
            if (_shotCD > 0) { _shotCD--; }
            else
            {
                //Console.WriteLine("eye shot!");
                _point1.X = _point.X + 16; _point1.Y = _point.Y + 16;

                Vector2D V;
                V.X = _v1.X() - _point1.X; V.Y = _v1.Y() - _point1.Y;
                V = SplashKit.VectorMultiply(V, 1 / SplashKit.VectorMagnitude(V));
                _vector = V;

                bool _hit = false;
                {
                    for (int i = 0; i < 2000; i += 2)
                    {
                        int x = Convert.ToInt32(_point1.X - 2 + V.X * i);
                        int y = Convert.ToInt32(_point1.Y - 2 + V.Y * i);
                        _point2.X = x; _point2.Y = y;

                        Rectangle checkbox = new Rectangle(x, y, 4, 4);
                        foreach (Rectangle r in _rects)
                        {
                            if (checkbox.IntersectsWith(r))
                            {
                                _hit = true;
                                break;
                            }
                        }
                        if (_hit) break;
                        Warning bullet = new Warning(_point1, _point2);
                        _bullets.Add(bullet);

                        _warning = 35;
                        Random rnd = new Random();
                        int u = rnd.Next(60 * 3, 60 * 10);
                        _shotCD = u;

                    }
                }
            }
            if (_warning > 0) { _warning--; }
            if (_warning == 5)
            {
                bool _hit = false;
                {
                    for (int i = 0; i < 2000; i += 2)
                    {
                        int x = Convert.ToInt32(_point1.X - 2 + _vector.X * i);
                        int y = Convert.ToInt32(_point1.Y - 2 + _vector.Y * i);
                        _point2.X = x; _point2.Y = y;

                        Rectangle checkbox = new Rectangle(x, y, 4, 4);
                        if (checkbox.IntersectsWith(_v1.Rect()))
                        {
                            _hit = true;
                            _v1.Damage(10);
                            break;
                        }

                        foreach (Rectangle r in _rects)
                        {
                            if (checkbox.IntersectsWith(r))
                            {
                                _hit = true;
                                break;
                            }
                        }
                        if (_hit) break;
                        piston_bullet bullet = new piston_bullet(_point1, _point2);
                        _bullets.Add(bullet);
                    }
                }

            }

        }
    }
}
