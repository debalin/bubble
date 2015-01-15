using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace BubbleMain.Core_Elements
{
    public class RedBubble : Microsoft.Xna.Framework.GameComponent
    {
        public Vector2 position;
        public Vector2 size;
        public Vector2 velocity;
        public Vector2 screensize, screenstart, hole, range;
        private float tempvelx, tempvely;
        public float kinetic, angkinetic, inertia, friction, mass, accelerationX = 0f;
        private float[] blockmass;
        public double accelerationY = 0d, obstacleacc = 1.1d;
        public double rotation = 0d, omega, time, obstacleangle;
        public bool draw = true, check = true, isgame, obstacle = false, spike = false, draw1 = true, ai = false, upper, hit = false, guarding = false, timecheck = true;
        public Rectangle bubble, bubblecheck;
        private Rectangle[] obstacle1, spike1;
        public Bubble[] Bubbles;
        private int count1, count2;
        private double m, c, A, B, C, x1, x2, x, y, bubbleangle, timer = -1, timer1 = -1, k;
        public int index = -1, flagindex = -1;

        public RedBubble(Game game, Vector2 position, Vector2 size, Vector2 velocity, Vector2 screensize, Vector2 screenstart, float mass, double omega, bool isgame, bool gravity, Vector2 hole) : base(game) //neither spike nor obstacle
        {
            this.position = position;
            this.size = size;
            this.velocity = velocity;
            this.screensize = screensize;
            this.omega = omega;
            this.mass = mass;
            this.isgame = isgame;
            this.screenstart = screenstart;
            this.hole = hole;
            bubble = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            friction = 0.15f * mass * 2f;
            if (gravity)
                accelerationY = -0.08f;
            else
                accelerationY = Math.Sign(velocity.Y) * 0.08f;
            range = new Vector2((screenstart.Y * 2 + screensize.Y) / 2 - 100, (screenstart.Y * 2 + screensize.Y) / 2 + 100);
        }

        public RedBubble(Game game, Vector2 position, Vector2 size, Vector2 velocity, Vector2 screensize, Vector2 screenstart, float mass, double omega, bool isgame, bool gravity, Vector2 hole, Rectangle[] obstacle1, int count1, float[] blockmass) : base(game) //only obstacle
        {
            this.position = position;
            this.size = size;
            this.velocity = velocity;
            this.screensize = screensize;
            this.omega = omega;
            this.mass = mass;
            this.isgame = isgame;
            this.screenstart = screenstart;
            this.hole = hole;
            obstacle = true;
            this.obstacle1 = obstacle1;
            this.count1 = count1;
            this.blockmass = blockmass;
            bubble = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            bubblecheck = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            friction = 0.15f * mass * 2f;
            if (gravity)
                accelerationY = -0.08f;
            else
                accelerationY = Math.Sign(velocity.Y) * 0.08f;
            range = new Vector2((screenstart.Y * 2 + screensize.Y) / 2 - 100, (screenstart.Y * 2 + screensize.Y) / 2 + 100);
        }

        public RedBubble(Game game, Vector2 position, Vector2 size, Vector2 velocity, Vector2 screensize, Vector2 screenstart, float mass, double omega, bool isgame, bool gravity, Vector2 hole, Rectangle[] obstacle1, int count1, float[] blockmass, Rectangle[] spike1, int count2)
            : base(game) //both spike and obstacle
        {
            this.position = position;
            this.size = size;
            this.velocity = velocity;
            this.screensize = screensize;
            this.omega = omega;
            this.mass = mass;
            this.isgame = isgame;
            this.screenstart = screenstart;
            this.hole = hole;
            obstacle = true;
            spike = true;
            this.obstacle1 = obstacle1;
            this.spike1 = spike1;
            this.count1 = count1;
            this.count2 = count2;
            this.blockmass = blockmass;
            bubble = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            bubblecheck = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            friction = 0.15f * mass * 2f;
            if (gravity)
                accelerationY = -0.08f;
            else
                accelerationY = Math.Sign(velocity.Y) * 0.08f;
            range = new Vector2((screenstart.Y * 2 + screensize.Y) / 2 - 100, (screenstart.Y * 2 + screensize.Y) / 2 + 100);
        }

         public RedBubble(Game game, Vector2 position, Vector2 size, Vector2 velocity, Vector2 screensize, Vector2 screenstart, float mass, double omega, bool isgame, bool gravity, Vector2 hole, Rectangle[] spike1, int count2)
            : base(game) //only spike
        {
            this.position = position;
            this.size = size;
            this.velocity = velocity;
            this.screensize = screensize;
            this.omega = omega;
            this.mass = mass;
            this.isgame = isgame;
            this.screenstart = screenstart;
            this.hole = hole;
            spike = true;
            ai = true;
            this.spike1 = spike1;
            this.count2 = count2;
            bubble = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            bubblecheck = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            friction = 0.15f * mass * 2f;
            if (gravity)
                accelerationY = -0.08f;
            else
                accelerationY = Math.Sign(velocity.Y) * 0.08f;
            range = new Vector2((screenstart.Y * 2 + screensize.Y) / 2 - 100, (screenstart.Y * 2 + screensize.Y) / 2 + 100);
            Bubbles = new Bubble[6];
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private void InteractObstacle()
        {
            double tempvelx1, tempvelx2, tempvelx3, tempvelx4, tempvely1, tempvely2, collisionangle, magnitude1, magnitude2, direction1, direction2;
            for (int i = 0; i < count1; i++)
            {
                if (BubblesCollide(obstacle1[i])) //bubble will intersect the obstacle
                {
                    collisionangle = Math.Atan2((position.Y + size.Y / 2) - (obstacle1[i].Top + obstacle1[i].Height / 2), -(position.X + size.X / 2) + (obstacle1[i].Left + obstacle1[i].Width / 2));
                    direction1 = Math.Atan2(-velocity.Y, velocity.X);
                    direction2 = Math.Atan2(-0, 0);
                    magnitude1 = Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
                    magnitude2 = Math.Sqrt(0 * 0 + 0 * 0);
                    tempvelx1 = magnitude1 * Math.Cos(direction1 - collisionangle);
                    tempvely1 = magnitude1 * Math.Sin(direction1 - collisionangle);
                    tempvelx2 = magnitude2 * Math.Cos(direction2 - collisionangle);
                    tempvely2 = magnitude2 * Math.Sin(direction2 - collisionangle);
                    tempvelx3 = ((mass - blockmass[i]) * tempvelx1 / (mass + blockmass[i])) + (2 * blockmass[i] * tempvelx2 / (mass + blockmass[i]));
                    tempvelx4 = -((mass - blockmass[i]) * tempvelx2 / (mass + blockmass[i])) + (2 * mass * tempvelx1 / (mass + blockmass[i]));
                    magnitude1 = Math.Sqrt(tempvelx3 * tempvelx3 + tempvely1 * tempvely1);
                    magnitude2 = Math.Sqrt(tempvelx4 * tempvelx4 + tempvely2 * tempvely2);
                    direction1 = Math.Atan2(tempvely1, tempvelx3) + collisionangle;
                    direction2 = Math.Atan2(tempvely2, tempvelx4) + collisionangle;
                    omega = (tempvely2 - tempvely1) * 2 * 10 / (size.X * mass);
                    velocity = new Vector2((float)(magnitude1 * Math.Cos(direction1)), -(float)(magnitude1 * Math.Sin(direction1)));
                }
            }
        }

        private bool InteractSpike()
        {
            bool flag = false;
            bubble.Location = new Point((int)position.X, (int)position.Y);
            bubblecheck.Location = new Point(bubble.Left + (int)Math.Round(velocity.X), bubble.Top + (int)Math.Round(velocity.Y));
            for (int i = 0; i < count2; i++)
            {
                if (spike1[i].Intersects(bubblecheck))
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        public override void Update(GameTime gameTime)
        {
            time = gameTime.ElapsedGameTime.TotalSeconds;
            if (draw)
            {
                inertia = (mass * (size.X * size.X) / 8);

                if (position.X >= 50 && position.X <= 80 && position.Y + size.Y / 2 >= range.X - 20 && position.Y + size.Y / 2 <= range.Y + 20) //red bubble hasnt been hit yet and should go on guarding the entry
                {
                    if (timecheck)
                    {
                        timer1 = gameTime.TotalGameTime.TotalMilliseconds;
                        timecheck = false;
                    }
                    if (timer1 + 1000 < gameTime.TotalGameTime.TotalMilliseconds)
                        guarding = true;
                    if (position.Y + size.Y / 2 <= range.X)
                        velocity = new Vector2(0, 2);
                    else if (position.Y + size.Y / 2 >= range.Y)
                        velocity = new Vector2(0, -2);
                    else if (Math.Round(velocity.Y) != 2 && Math.Round(velocity.Y) != -2)
                    {
                        if (position.Y + size.Y / 2 < (screenstart.Y + screensize.Y) / 2)
                            velocity = new Vector2(0, 2);
                        else
                            velocity = new Vector2(0, -2);
                    }
                }
                else if (!hit)//redbubble has been hit, so find its path back to the front of the gate
                {
                    guarding = false;
                    if (position.Y + size.Y / 2 < (screenstart.Y + screensize.Y) / 2 + 10) //if red bubble is in the upper half of the window, route it back to the upper starting point
                        obstacleangle = Math.Atan2(position.Y + size.Y / 2 - range.X, 70 - position.X);
                    else //else route it back to the lower starting point
                        obstacleangle = Math.Atan2(position.Y + size.Y / 2- range.Y, 70 - position.X);
                    
                    velocity = new Vector2((float)(velocity.X + Math.Sign(70 - position.X) * obstacleacc * Math.Abs(Math.Cos(obstacleangle)) * time), (float)(velocity.Y + Math.Sign(range.X - position.Y) * obstacleacc * Math.Abs(Math.Sin(obstacleangle)) * time));
                }

                if (ai && guarding) //A.I. bubble
                {
                    for (int i = 0; i <= index; i++)
                    {
                        if (Bubbles[i].draw)
                        {
                            if ((Bubbles[i].position.Y + Bubbles[i].size.Y / 2 < position.Y + size.Y / 2) && (Bubbles[i].position.X + Bubbles[i].size.X / 2 < spike1[0].Center.X) && (Bubbles[i].position.X + Bubbles[i].size.X / 2 >= spike1[0].Center.X / 3)) //check if this bubble is in an optimal position for the red bubble to hit
                            {
                                flagindex = index;
                                upper = true;
                                break;
                            }
                            else if ((Bubbles[i].position.Y + Bubbles[i].size.Y / 2 >= position.Y + size.Y / 2) && (Bubbles[i].position.X + Bubbles[i].size.X / 2 < spike1[1].Center.X) && (Bubbles[i].position.X + Bubbles[i].size.X / 2 >= spike1[1].Center.X / 3)) 
                            {
                                flagindex = index;
                                upper = false;
                                break;
                            }
                        }
                    }
                    if (flagindex != -1)
                    {
                        k = (Bubbles[flagindex].position.Y + Bubbles[flagindex].size.Y / 2);
                        if (upper)
                        {
                            m = (spike1[0].Center.Y - k) / ((Bubbles[flagindex].position.X + Bubbles[flagindex].size.X / 2) - spike1[0].Right); //find the slope and intercept between blue bubble and red bubble
                            c = (750 - k) - (Bubbles[flagindex].position.X + Bubbles[flagindex].size.X / 2) * m;
                        }
                        else
                        {
                            m = (spike1[1].Center.Y - k) / ((Bubbles[flagindex].position.X + Bubbles[flagindex].size.X / 2) - spike1[1].Right);
                            c = (750 - k) - (Bubbles[flagindex].position.X + Bubbles[flagindex].size.X / 2) * m;
                        }
                        A = 1 + m * m;
                        B = 2 * m * c - 2 * m * (750 - k) - 2 * (Bubbles[flagindex].position.X + Bubbles[flagindex].size.X / 2);
                        C = (Bubbles[flagindex].position.X + Bubbles[flagindex].size.X / 2) * (Bubbles[flagindex].position.X + Bubbles[flagindex].size.X / 2) + (750 - k) * (750 - k) + c * c - (Bubbles[flagindex].size.X / 2) * (Bubbles[flagindex].size.X / 2) - 2 * c * (750 - k);
                        x1 = (-B + Math.Sqrt(B * B - 4 * A * C)) / (2 * A); //find the intersection of the slope line and the selected blue bubble
                        x2 = (-B - Math.Sqrt(B * B - 4 * A * C)) / (2 * A);
                        x = x1 < x2 ? x1 : x2;
                        y = m * x + c;
                        x = x - Math.Cos(Math.Atan(m)) * (size.X / 2.68);
                        if (upper)
                            y = y - Math.Sin(Math.Atan(m)) * (size.Y / 2.68);
                        else
                            y = y + Math.Sin(Math.Atan(m)) * (size.Y / 2.68);
                        bubbleangle = Math.Atan2(y - (750 - (position.Y + size.Y / 2)), x - (position.X + size.X / 2));
                        velocity = new Vector2(30 * (float)Math.Cos(bubbleangle), -30 * (float)Math.Sin(bubbleangle)); //direct the red bubble towards that bubble in that position
                        flagindex = -1;
                        hit = true;
                        guarding = false;
                        timecheck = true;
                        timer = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }

                if (hit && (timer + 1000 < gameTime.TotalGameTime.TotalMilliseconds))
                    hit = false;

                kinetic = (float)0.5 * mass * (velocity.X * velocity.X + velocity.Y * velocity.Y);
                angkinetic = 0.5f * inertia * (float)(omega * omega);

                if (obstacle) //if obstacle is present in a level, behave accordingly
                    InteractObstacle();

                if (spike)//if spike is present in a level, behave accordingly
                {
                    if (InteractSpike())
                    {
                        draw = false;
                        draw1 = false;
                        position = new Vector2(-100, -100);
                        velocity = new Vector2(0, 0);
                        goto End;
                    }
                }
                
                if (position.X - screenstart.X < 0) //if the ball has crossed any screen boundary, bring it back
                    position = new Vector2(screenstart.X, position.Y);
                else if ((position.X + size.X) > screenstart.X + screensize.X)
                    position = new Vector2(screenstart.X + screensize.X - size.X, position.Y);
                if (position.Y - screenstart.Y < 0)
                    position = new Vector2(position.X, screenstart.Y);
                else if ((position.Y + size.Y) > screenstart.Y + screensize.Y)
                    position = new Vector2(position.X, screenstart.Y + screensize.Y - size.Y);
                
                if (((position.X + size.X + velocity.X) > screenstart.X + screensize.X)) //Loss of Kinetic Energy due to impact on side walls
                {
                    angkinetic = angkinetic - (angkinetic / 3);
                    kinetic = kinetic - (kinetic / 5);
                    try
                    {
                        tempvelx = -Math.Sign(velocity.X) * (float)Math.Sqrt((2 * kinetic / (mass * (1 + (velocity.Y / velocity.X) * (velocity.Y / velocity.X)))));
                    }
                    catch
                    {
                        tempvelx = 0f;
                    }
                    try
                    {
                        tempvely = Math.Sign(velocity.Y) * (float)Math.Sqrt((2 * kinetic / (mass * (1 + (velocity.X / velocity.Y) * (velocity.X / velocity.Y)))));
                    }
                    catch
                    {
                        tempvely = 0f;
                    }
                    velocity = new Vector2(tempvelx, tempvely);
                    try
                    {
                        omega = Math.Sign(omega) * (float)Math.Sqrt(2 * angkinetic / inertia);
                    }
                    catch
                    {
                        omega = 0d;
                    }
                    omega = omega + 0.01d * velocity.Y; //spin effect on wall collision
                }
                else if ((position.X - screenstart.X + velocity.X) < 0f)
                {
                    angkinetic = angkinetic - (angkinetic / 3);
                    kinetic = kinetic - (kinetic / 5);
                    try
                    {
                        tempvelx = -Math.Sign(velocity.X) * (float)Math.Sqrt((2 * kinetic / (mass * (1 + (velocity.Y / velocity.X) * (velocity.Y / velocity.X)))));
                    }
                    catch
                    {
                        tempvelx = 0f;
                    }
                    try
                    {
                        tempvely = Math.Sign(velocity.Y) * (float)Math.Sqrt((2 * kinetic / (mass * (1 + (velocity.X / velocity.Y) * (velocity.X / velocity.Y)))));
                    }
                    catch
                    {
                        tempvely = 0f;
                    }
                    velocity = new Vector2(tempvelx, tempvely);
                    try
                    {
                        omega = Math.Sign(omega) * (float)Math.Sqrt(2 * angkinetic / inertia);
                    }
                    catch
                    {
                        omega = 0d;
                    }
                    omega = omega - 0.01d * velocity.Y; 
                }
                
                if (((position.Y + size.Y + velocity.Y) > screenstart.Y + screensize.Y)) //Loss of Kinetic Energy due to impact on ground and top wall
                {
                    angkinetic = angkinetic - (angkinetic / 3);
                    kinetic = kinetic - (kinetic / 5);
                    try
                    {
                        tempvelx = Math.Sign(velocity.X) * (float)Math.Sqrt((2 * kinetic / (mass * (1 + (velocity.Y / velocity.X) * (velocity.Y / velocity.X)))));
                    }
                    catch
                    {
                        tempvelx = 0f;
                    }
                    try
                    {
                        tempvely = -Math.Sign(velocity.Y) * (float)Math.Sqrt((2 * kinetic / (mass * (1 + (velocity.X / velocity.Y) * (velocity.X / velocity.Y)))));
                    }
                    catch
                    {
                        tempvely = 0f;
                    }
                    velocity = new Vector2(tempvelx, tempvely);
                    try
                    {
                        omega = Math.Sign(omega) * (float)Math.Sqrt(2 * angkinetic / inertia);
                    }
                    catch
                    {
                        omega = 0d;
                    }
                    omega = omega - 0.01d * velocity.X; 
                }
                else if ((position.Y - screenstart.Y + velocity.Y) < 0f)
                {
                    angkinetic = angkinetic - (angkinetic / 3);
                    kinetic = kinetic - (kinetic / 5);
                    try
                    {
                        tempvelx = Math.Sign(velocity.X) * (float)Math.Sqrt((2 * kinetic / (mass * (1 + (velocity.Y / velocity.X) * (velocity.Y / velocity.X)))));
                    }
                    catch
                    {
                        tempvelx = 0f;
                    }
                    try
                    {
                        tempvely = -Math.Sign(velocity.Y) * (float)Math.Sqrt((2 * kinetic / (mass * (1 + (velocity.X / velocity.Y) * (velocity.X / velocity.Y)))));
                    }
                    catch
                    {
                        tempvely = 0f;
                    }
                    velocity = new Vector2(tempvelx, tempvely);
                    try
                    {
                        omega = Math.Sign(omega) * (float)Math.Sqrt(2 * angkinetic / inertia);
                    }
                    catch
                    {
                        omega = 0d;
                    }
                    omega = omega + 0.01d * velocity.X; 
                }
                
                if ((Math.Round(position.Y + size.Y) == screenstart.Y + screensize.Y) && (velocity.X != 0f)) //Effect of Friction
                {
                    if (velocity.X > 0)
                    {
                        velocity = new Vector2((float)(velocity.X - (friction / mass) * (float)time), velocity.Y);
                        accelerationX = -(friction / mass);
                    }
                    else
                    {
                        velocity = new Vector2((float)(velocity.X + (friction / mass) * (float)time), velocity.Y);
                        accelerationX = (friction / mass);
                    }
                    omega = -velocity.X * 2 / size.X; //pure rolling
                }
                else
                    accelerationX = 0f;
                
                if ((Math.Round(position.Y + size.Y) == screenstart.Y + screensize.Y) && (velocity.Y == 0)) //no acceleration when on ground
                    accelerationY = 0;
                
                if ((velocity.X < 0.015f) && (velocity.X > -0.015f)) //stop Friction
                {
                    accelerationX = 0;
                    velocity = new Vector2(0f, velocity.Y);
                }
                
                if ((velocity.X == 0f) && (velocity.Y == 0f)) //stop rotational motion
                    omega = 0d;
                
                if (float.IsNaN(velocity.X))
                    velocity = new Vector2(0f, velocity.Y);
                if (float.IsNaN(velocity.Y))
                    velocity = new Vector2(velocity.X, 0f);
                if (Math.Round(position.Y - screenstart.Y) < 5f && velocity.X == 0f && velocity.Y == 0f)
                    velocity = new Vector2(0, 0.1f);
                if (Math.Round(position.Y + size.Y) > screenstart.Y + screensize.Y - 5f && velocity.X == 0f && velocity.Y == 0f)
                    velocity = new Vector2(0, -0.1f);
            End:
                position += velocity; //update position
                bubble.Location = new Point((int)position.X, (int)position.Y);
                
                rotation -= omega; //update rotation
                
                if (omega > 0d) //air resistance
                    omega -= 0.0001d;
                else if (omega < 0d)
                    omega += 0.0001d;

            }

            base.Update(gameTime);
        }

        public bool BubblesCollide(Bubble s) //collision detection and pre-collision response between two bubbles
        {
            float pendepth, temppos1, temppos2;
            double collisionangle;
            if ((((position.X + size.X / 2) - (s.position.X + s.size.X / 2)) * ((position.X + size.X / 2) - (s.position.X + s.size.X / 2)) + ((position.Y + size.Y / 2) - (s.position.Y + s.size.Y / 2)) * ((position.Y + size.Y / 2) - (s.position.Y + s.size.Y / 2))) <= ((size.X / 2 + s.size.X / 2) * (size.X / 2 + s.size.X / 2))) //collision detection
            {   //pre-collision response
                pendepth = (float)(-Math.Sqrt(((position.X + size.X / 2) - (s.position.X + s.size.X / 2)) * ((position.X + size.X / 2) - (s.position.X + s.size.X / 2)) + ((position.Y + size.Y / 2) - (s.position.Y + s.size.Y / 2)) * ((position.Y + size.Y / 2) - (s.position.Y + s.size.Y / 2))) + (size.X / 2 + s.size.X / 2));
                collisionangle = Math.Abs(Math.Atan2(position.Y - s.position.Y, -position.X + s.position.X));
                int dir1 = 1, dir2 = 1;
                if (position.X < s.position.X)
                    dir1 = -1;
                if (position.Y < s.position.Y)
                    dir2 = -1;
                
                temppos1 = (position.X + dir1 * pendepth / 2 * (float)Math.Abs(Math.Cos(collisionangle))); //reverse penetration from both the balls equally but in opposite directions 
                temppos2 = (position.Y + dir2 * pendepth / 2 * (float)Math.Abs(Math.Sin(collisionangle)));
                position = new Vector2(temppos1, temppos2);
                temppos1 = (s.position.X - dir1 * pendepth / 2 * (float)Math.Abs(Math.Cos(collisionangle)));
                temppos2 = (s.position.Y - dir2 * pendepth / 2 * (float)Math.Abs(Math.Sin(collisionangle)));
                s.position = new Vector2(temppos1, temppos2);
                return true;
            }
            else
                return false;
        }

        public bool BubblesCollide(Rectangle r) //collision detection and pre-collision response between two bubbles
        {           
            float pendepth, temppos1, temppos2;
            double collisionangle;
            if ((((position.X + size.X / 2) - (r.Left + r.Width / 2)) * ((position.X + size.X / 2) - (r.Left + r.Width / 2)) + ((position.Y + size.Y / 2) - (r.Top + r.Height / 2)) * ((position.Y + size.Y / 2) - (r.Top + r.Height / 2))) <= ((size.X / 2 + r.Width / 2) * (size.X / 2 + r.Width / 2))) //collision detection
            {   //pre-collision response
                pendepth = (float)(-Math.Sqrt(((position.X + size.X / 2) - (r.Left + r.Width / 2)) * ((position.X + size.X / 2) - (r.Left + r.Width / 2)) + ((position.Y + size.Y / 2) - (r.Top + r.Height / 2)) * ((position.Y + size.Y / 2) - (r.Top + r.Height / 2))) + (size.X / 2 + r.Width / 2));
                collisionangle = Math.Abs(Math.Atan2(position.Y - r.Top, -position.X + r.Left));
                int dir1 = 1, dir2 = 1;
                if (position.X < r.Left)
                    dir1 = -1;
                if (position.Y < r.Top)
                    dir2 = -1;

                temppos1 = (position.X + dir1 * pendepth * (float)Math.Abs(Math.Cos(collisionangle))); //reverse penetration from one ball 
                temppos2 = (position.Y + dir2 * pendepth * (float)Math.Abs(Math.Sin(collisionangle)));
                position = new Vector2(temppos1, temppos2);
                return true;
            }
            else
                return false;
        }
    }
}
