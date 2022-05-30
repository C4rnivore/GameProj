using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameProj.Model
{
    public class Enemy
    {
        //coords
        public float X { get; set; }
        public float Y { get; set; }
        public HitBox Hitbox { get; set; }
        public bool FacingRight { get; set; }


        //movement
        public float XSpeed { get; set; }
        public float YSpeed { get; set; }
        public float Acceleration = 1;


        //texture
        public Image Texture { get; set; }
        public int XScale { get; set; }
        public int YScale { get; set; }

        //size
        public int Height { get; set; }
        public int Width { get; set; }

        //behaviour
        public float Health;
        private bool IsGrounded;
        private bool IsAlive;
        private bool UnderEffect;
        private bool InBlock;
        private int EffectDurability = 200;
        private int CurrentEffectTime;
        private int ViewDistance = 400;
        private int AttackDistance = 75;
        private double BlockProbability = 0.4;
        private string CurEffect;

        public AnimationType CurrentAnimationType { get; private set; }
        public static List<string> RunningSprites { get; set; }
        public static List<string> AttackingSprites { get; set; }
        public static List<string> InvertedRunningSprites { get; set; }
        public static List<string> InvertedAttackingSprites { get; set; }
        public enum AnimationType
        {
            RunR,
            RunL,
            Attack,
            Stay,
            Fall
        }
        public static int CurRunFrame = 1;
        public static int CurJumpFrame = 1;
        public static int CurAttackFrame = 1;


        public Enemy(int x, int y)
        {
            X = x;
            Y = y;
            Health = 100;
            Texture = new Bitmap(@"C:\Users\Kirill\Desktop\Game\Images\Sprites\Enemy\Stay\l.png");
            XScale = Texture.Width * 2;
            YScale = Texture.Height * 2;
            Width = Texture.Width * 2;
            Height = Texture.Height * 2;
            Hitbox = new HitBox(X + Width / 2, Y, Y + Height);
            InitSprites();
        }

        private void InitSprites()
        {
            RunningSprites = Directory.GetFiles(@"C:\Users\Kirill\Desktop\Game\Images\Sprites\Enemy\RunR").ToList();
            InvertedRunningSprites = Directory.GetFiles(@"C:\Users\Kirill\Desktop\Game\Images\Sprites\Enemy\RunL").ToList();
            AttackingSprites = Directory.GetFiles(@"C:\Users\Kirill\Desktop\Game\Images\Sprites\Enemy\AttackR").ToList();
            InvertedAttackingSprites = Directory.GetFiles(@"C:\Users\Kirill\Desktop\Game\Images\Sprites\Enemy\AttackL").ToList();
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
                        if (FacingRight)
                        {
                            Texture = new Bitmap(AttackingSprites[CurAttackFrame - 1]);
                            if (CurAttackFrame != 6)
                            {
                                CurAttackFrame++;
                                if (CurAttackFrame == 4) HitPlayer(15);
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
                                if (CurAttackFrame == 4) HitPlayer(15);
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
                            new Bitmap(@"C:\Users\Kirill\Desktop\Game\Images\Sprites\Enemy\Stay\r.png") :
                            new Bitmap(@"C:\Users\Kirill\Desktop\Game\Images\Sprites\Enemy\Stay\l.png");
                        return;
                    }
            }
        }



        public void EnemyInteractions()
        {
            IsAlive = Health > 0;
            if (IsAlive)
            {
                SimulateGravity();
                 IsGrounded = Y + Height == GameForm.Level.Ground.Y;
                SimuateAgressiveBehaviour();
                 X += XSpeed * Acceleration;
                CheckIfPLayerIsClose();
                ApplyEffect(CurEffect);
                if (X < 0) X = 0;
                if (X + Width > Window.Width) X = Window.Width - Width;
                Hitbox = new HitBox(X + Width, Y, Y + Height);
            }
        }
        public void SimulateGravity()
        {
            Y += YSpeed;
            YSpeed += 1;
            if (YSpeed > 25)
                YSpeed = 25;

            if (Y + Height >= GameForm.Level.Ground.Y)
            {
                Y = GameForm.Level.Ground.Y - Height;
                YSpeed = 0;
            }
        }
        public bool CanSeePlayer()
        {
            return (Math.Abs(Player.Hitbox.X - Hitbox.X) < ViewDistance);
        }
        public void SimuateAgressiveBehaviour()
        {
            if (IsGrounded)
            {
                if (CanSeePlayer())
                {
                    if (Hitbox.X < Player.Hitbox.X)
                    {
                        Move(1);
                    }
                    else if (Hitbox.X > Player.Hitbox.X)
                    {
                        Move(-1);
                    }
                    if (Math.Abs(Hitbox.X - Player.Hitbox.X) < Player.Width / 2)
                        Stay();
                }
                else Stay();
            }   
        }


        public void Move(int direction)
        {
            XSpeed = direction * 4.5f;
            CurrentAnimationType = XSpeed > 0 ? AnimationType.RunR : AnimationType.RunL;
        }
        public void Stay()
        {
            XSpeed = 0;
            CurrentAnimationType = AnimationType.Stay;
        }

        public void HitPlayer(int damage)
        {
            Player.Health -= damage;
        }

        public void CheckIfPLayerIsClose()
        {
            if (Math.Abs(Hitbox.X - Player.Hitbox.X) <= AttackDistance)
                GetNextMove();
        }

        public void GetNextMove()
        {
            var rnd = new Random();
            if (rnd.NextDouble() <= BlockProbability)
                InBlock = true;
            else
                CurrentAnimationType = AnimationType.Attack;
        }



        public void GetHitFromPlayer(int direction,int damage, string effect)
        {
            if (IsGrounded && !InBlock)
            {
                Health -= damage;
                XSpeed = direction * 10f;
                YSpeed = -15;

                if(UnderEffect == false && effect!="None")
                    CurEffect = effect;
            }
            if (InBlock) InBlock = false;
        }
        public void ApplyEffect(string curEffect)
        {
            switch (curEffect)
            {
                case ("Burning"):
                    {
                        if (CurrentEffectTime < EffectDurability)
                        {
                            Health -= 0.01f;
                            CurrentEffectTime++;
                            UnderEffect = true;
                        }
                        else
                        {
                            CurEffect = "None";
                            CurrentEffectTime = 0;
                            UnderEffect = false;
                        }

                        return;
                    }
                case ("Slowing"):
                    {
                        if (CurrentEffectTime < EffectDurability)
                        {
                            Acceleration = 0.5f;
                            CurrentEffectTime++;
                            UnderEffect = true;
                        }
                        else
                        {
                            Acceleration = 1;
                            CurEffect = "None";
                            CurrentEffectTime = 0;
                            UnderEffect = false;
                        }
                        return;
                    }
            }
        }

    }
}
