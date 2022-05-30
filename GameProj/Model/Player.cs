using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameProj.Model
{
    public class Player 
    {
        //coords
        public static float X { get; private set; }
        public static float Y { get; private set; }
        public static float RightSide { get; private set; }
        public static float LeftSide { get; private set; }
        public static float Top { get; private set; }
        public static float Bottom { get; private set; }
        public static HitBox Hitbox { get; private set; }


        //movement
        public static float XSpeed { get; private set; }
        public static float YSpeed { get; private set; }
        public static bool IsGrounded { get; private set; }
        public static bool IsRunning { get; private set; }
        public static bool OnPlatform { get; private set; }
        public static bool FacingRight { get; private set; }


        //texture
        public Image Texture { get; set; }
        public int XScale { get; set; }
        public int YScale { get; set; }

        //size
        public static int Height { get; set; }
        public static int Width { get; set; }

        //EntityAttributes
        public static float Health { get; set; }
        public static float DefaultAttackDistance = 100;
        public static int Damage = 10;

        //Animation
        public static List<string> RunningSprites { get; set; }
        public static List<string> InvertedRunningSprites { get; set; }
        public static List<string> JumpingRSprites { get; set; }
        public static List<string> AttackingSprites { get; set; }
        public static List<string> InvertedAttackingSprites { get; set; }



        public static AnimationType CurrentAnimationType { get; private set; }

        public static int CurRunFrame = 1;
        public static int CurJumpFrame = 1;
        public static int CurAttackFrame = 1;
        public enum AnimationType
        {
            RunR,
            RunL,
            Attack,
            Stay,
            Fall
        }

        public Player(int x, int y)
        {
            X = x;
            Y = y;
            Texture = new Bitmap("C:\\Users\\Kirill\\Desktop\\Game\\Images\\Sprites\\Player\\Standing\\facing_right.png");
            XScale = Texture.Width * 2;
            YScale = Texture.Height * 2;
            Width = Texture.Width * 2;
            Height = Texture.Height * 2;
            Hitbox = new HitBox(X + Width/2, Y, Y + Height);
            FacingRight = true;
            CurrentAnimationType = AnimationType.Stay;
            InitSpries();
            Health = 150;
        }



        //AnimationPart
        public void InitSpries()
        {
            RunningSprites = Directory.GetFiles("C:\\Users\\Kirill\\Desktop\\Game\\Images\\Sprites\\Player\\Running").ToList();
            InvertedRunningSprites = Directory.GetFiles("C:\\Users\\Kirill\\Desktop\\Game\\Images\\Sprites\\Player\\RunningInverted").ToList();
            AttackingSprites = Directory.GetFiles("C:\\Users\\Kirill\\Desktop\\Game\\Images\\Sprites\\Player\\Attack").ToList();
            InvertedAttackingSprites = Directory.GetFiles("C:\\Users\\Kirill\\Desktop\\Game\\Images\\Sprites\\Player\\AttackInverted").ToList();
        }

        public void PlayCurrentAnimation()
        {
           switch (CurrentAnimationType)
           {
             case AnimationType.RunR:
                  {
                         Texture = new Bitmap(RunningSprites[CurRunFrame - 1]); CurRunFrame++; FacingRight = true;
                         if (CurRunFrame == 6) CurRunFrame = 1;
                     return;
                  }
             case (AnimationType.RunL):
                 {
                         Texture = new Bitmap(InvertedRunningSprites[CurRunFrame - 1]); CurRunFrame++; FacingRight = false;
                         if (CurRunFrame == 6) CurRunFrame = 1; 
                     return;
                 }
             case (AnimationType.Attack):
                 {
                     if(FacingRight)
                     {
                         Texture = new Bitmap(AttackingSprites[CurAttackFrame - 1]);
                            if (CurAttackFrame != 6)
                            {
                                CurAttackFrame++;
                                if (CurAttackFrame == 4) HitEnemy(Damage, GameForm.Level.Enemies);
                            }
                            else
                            {
                                CurAttackFrame = 1; CurrentAnimationType = AnimationType.Stay;
                                return;
                            }
                     }
                     else
                      {
                         Texture = new Bitmap(InvertedAttackingSprites[CurAttackFrame - 1]);
                            if (CurAttackFrame != 6)
                            {
                                CurAttackFrame++;
                                if (CurAttackFrame == 4) HitEnemy(Damage, GameForm.Level.Enemies);
                            }
                            else
                            {
                                CurAttackFrame = 1; CurrentAnimationType = AnimationType.Stay;
                                return;
                            }
                     }
                     return;
                 }
             case (AnimationType.Stay):
                 {
                        Texture = FacingRight ?
                            new Bitmap("C:\\Users\\Kirill\\Desktop\\Game\\Images\\Sprites\\Player\\Standing\\facing_right.png"):
                            new Bitmap("C:\\Users\\Kirill\\Desktop\\Game\\Images\\Sprites\\Player\\Standing\\facing_left.png");
                     return;
                 }
             case (AnimationType.Fall):
                 {
                        Texture = FacingRight ?
                            new Bitmap("C:\\Users\\Kirill\\Desktop\\Game\\Images\\Sprites\\Player\\Falling\\right.png"):
                            new Bitmap("C:\\Users\\Kirill\\Desktop\\Game\\Images\\Sprites\\Player\\Falling\\left.png");
                     return;
                 }
           }
        }

        //MovementPart
        public void PlayerInteractions()
        {
            IsGrounded = Hitbox.Bottom == GameForm.Level.Ground.Y || OnPlatform ;
            IsRunning = XSpeed != 0;

            Hitbox = new HitBox(X + Width/2, Top, Y + Height);

            X += XSpeed;
            if (X < 0)
                X = 0;
            if (X + Width > Window.Width)
                X = Window.Width - Width;

            SimulateGravity(1.5f);
            SimulatePlatformInteractions();
        }

        public void SimulateGravity(float gravityPower)
        {
            //gravity
            Y += YSpeed;
            YSpeed += gravityPower;
            if (YSpeed > 25)
                YSpeed = 25;

            if (Y + Height >= GameForm.Level.Ground.Y)
            {
                Y = GameForm.Level.Ground.Y - Height;
                YSpeed = 0;
            }
        }

        public void SimulatePlatformInteractions()
        {
            foreach (var el in GameForm.Level.Platforms)
            {
                var x = Hitbox.X;
                var bottom = Hitbox.Bottom;
                bool InBounds = x >= el.LeftSide && x <= el.RightSide && bottom >= el.Top && bottom < el.Bottom;

                if (InBounds && YSpeed > 0)
                {
                    Y = el.Top - Height;
                    YSpeed = 0;
                }
            }
            OnPlatform = GameForm.Level.Platforms.Any(x => Hitbox.Bottom == x.Top );
        }

        public void Jump(int jumpforce)
        {
            YSpeed = IsGrounded ? -jumpforce : YSpeed;
        }
        public void Run(int direction)
        {
            XSpeed = direction * 7;
            CurrentAnimationType = XSpeed > 0 ? AnimationType.RunR  : AnimationType.RunL;
        }
        public void Stay()
        {
            XSpeed = 0;
            CurrentAnimationType = AnimationType.Stay;
        }




        public void Attack(int damage, Enemy[] enemies)
        {
            CurrentAnimationType = AnimationType.Attack;
        }
        private void HitEnemy(int damage, Enemy[] enemies)
        {
            var targets = FindEnemiesInAttackRange(enemies);
            targets.ForEach(x => x.GetHitFromPlayer((x.X > X ? 1 : -1), 0, "Slowing"));
        }

        public List<Enemy> FindEnemiesInAttackRange(Enemy[] enemies)
        {
            var enemyList = new List<Enemy>();
            foreach(var el in enemies)
            {
                if (InRange(el))
                    enemyList.Add(el);
            }
            return enemyList;
        } 

        public bool InRange(Enemy enemy)
        {
            if (Math.Abs(enemy.Hitbox.X - Hitbox.X) > DefaultAttackDistance)
                return false;

            if(enemy.Hitbox.X < Hitbox.X)
            {
                if (FacingRight) return false;
                else if (enemy.Hitbox.X <= Hitbox.X - Width 
                    || enemy.Hitbox.X >= Hitbox.X - Width - DefaultAttackDistance) return true;
            }
            if (enemy.Hitbox.X > Hitbox.X)
            {
                if (!FacingRight) return false;
                else if (enemy.Hitbox.X >= Hitbox.X 
                   || enemy.Hitbox.X <= Hitbox.X + DefaultAttackDistance) return true;
            }
            return false;
        }
    }
}
