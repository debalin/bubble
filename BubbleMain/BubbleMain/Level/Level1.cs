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
using BubbleMain.Core_Elements;


namespace BubbleMain.Level
{

    public class Level1 : GameScene
    {
        private ContentManager Content;
        private GraphicsDeviceManager graphics;
        private Vector2 displayratio, boundaryposition, gunposition, bluebubbleposition, bubblesize, bluebubblevelocity, initsize, hole;
        private float boundaryscale, bubblemass, bluestep, score;
        private Game game;
        private int i, j, k, index, largepos, gunindex;
        private double starttime, bubbleomega;
        public bool timecheck = true, endgame = false;
        private double tempvelx1, tempvelx2, tempvelx3, tempvelx4, tempvely1, tempvely2, collisionangle, magnitude1, magnitude2, direction1, direction2;
        private Random random;
        private Bubble[] Bubbles;
        private Bubble large;
        private int BBLQTY, saved, totaltime;
        private string timetaken, scoretext, bubblessaved;

        public Level1(Game game) : base(game)
        {
            this.game = game; 
        }

        public override void Initialize()
        {
            Content = (ContentManager)Game.Services.GetService(typeof(ContentManager)); //initialize resources and variables
            graphics = (GraphicsDeviceManager)Game.Services.GetService(typeof(GraphicsDeviceManager));
            displayratio = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            boundaryposition = new Vector2(0, 0);
            boundaryscale = 1; //in case there is a resizable boundary in the game
            gunposition = new Vector2(900, displayratio.Y / 2);
            index = -1;
            bluestep = 4;
            bluebubbleposition = new Vector2(20, 600);
            bubblesize = new Vector2(64, 64);
            initsize = new Vector2(64, 64);
            bluebubblevelocity = new Vector2(5, -5);
            bubblemass = 15f;
            bubbleomega = 0.3d;
            random = new Random();
            BBLQTY = 10;
            Bubbles = new Bubble[BBLQTY];
            hole = new Vector2(displayratio.Y / 2 - 70, displayratio.Y / 2 + 70);
            timetaken = "Time : 0";
            bubblessaved = "Bubbles Saved : 0 out of " + BBLQTY;
            saved = 0;
            scoretext = "Score : 0";
            score = 0;

            actors.Add(new DrawBackground(game, Content.Load<Texture2D>(@"Level\Level1\backlevel1_4_3"), Content.Load<Texture2D>(@"Level\Level1\backlevel1_16_9"), Content.Load<Texture2D>(@"Level\Level1\backlevel1_16_10")));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Level\Level1\star1"), Content.Load<Texture2D>(@"Level\Level1\star2"), new Vector2(150, 50), 10, Color.White, .7f, 1, 1));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Level\Level1\star2"), Content.Load<Texture2D>(@"Level\Level1\star1"), new Vector2(300, 100), 10, Color.White, .7f, 1, 1));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Level\Level1\star1"), Content.Load<Texture2D>(@"Level\Level1\star2"), new Vector2(500, 40), 10, Color.White, .7f, 1, 1));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Level\Level1\star2"), Content.Load<Texture2D>(@"Level\Level1\star1"), new Vector2(800, 100), 10, Color.White, .7f, 1, 1));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Level\Level1\moon"), Content.Load<Texture2D>(@"Level\Level1\moon"), new Vector2(600, 50), 10, Color.White, .7f, 1, .7f));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\entry1"), Content.Load<Texture2D>(@"Core Elements\entry2"), new Vector2(0, displayratio.Y / 2 - 77f), 10, Color.White, .6f, 2, 1));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\uplowbound1"), Content.Load<Texture2D>(@"Core Elements\uplowbound2"), new Vector2(15, 40), 10, Color.White, .6f, 3, 1));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\uplowbound2"), Content.Load<Texture2D>(@"Core Elements\uplowbound1"), new Vector2(15, 700), 10, Color.White, .6f, 3, 1));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\sidebound1"), Content.Load<Texture2D>(@"Core Elements\sidebound2"), new Vector2(900, 60), 10, Color.White, .6f, 3, 1));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\sidebound1"), Content.Load<Texture2D>(@"Core Elements\sidebound2"), new Vector2(0, 60), 10, Color.White, .6f, 3, 1));
            actors.Add(new DrawText(game, timetaken, Content.Load<SpriteFont>(@"Core Elements\text"), new Vector2(625, 10), Color.White));
            actors.Add(new DrawText(game, bubblessaved, Content.Load<SpriteFont>(@"Core Elements\text"), new Vector2(130, 10), Color.White));
            actors.Add(new DrawText(game, scoretext, Content.Load<SpriteFont>(@"Core Elements\scorefont"), new Vector2(300, 340), Color.White));
            actors.Add(new Gun(game, Content.Load<Texture2D>(@"Core Elements\gun1"), Content.Load<Texture2D>(@"Core Elements\gun2"), Content.Load<Texture2D>(@"Core Elements\gunwind1"), Content.Load<Texture2D>(@"Core Elements\gunwind2"), Content.Load<Texture2D>(@"Core Elements\powerbar1"), Content.Load<Texture2D>(@"Core Elements\powerbar2"), Content.Load<Texture2D>(@"Core Elements\battery1"), Content.Load<Texture2D>(@"Core Elements\battery2"), Content.Load<Texture2D>(@"Core Elements\bound"), new Vector2(890, 650), new Vector2(10, 50), boundaryscale, Content.Load<SpriteFont>(@"Core Elements\gunfont")));
            gunindex = 14;
            k = gunindex + 1;

            ((DrawText)actors[13]).MakeDisappear();
            ((DrawText)actors[11]).MakeAppear();
            ((DrawText)actors[12]).MakeAppear();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (timecheck)
            {
                actors.Clear();
                this.Initialize();

                starttime = gameTime.TotalGameTime.TotalSeconds;
                timecheck = false;
            }

            if (!endgame)
            {
                totaltime = (int)(gameTime.TotalGameTime.TotalSeconds - starttime);
                timetaken = "Time : " + totaltime;
                ((DrawText)actors[11]).ChangeText(timetaken);
            }

            if (gameTime.TotalGameTime.TotalSeconds - starttime > bluestep && bluestep <= 8.5) //add blue bubbles
            {
                actors.Add(new Bubble(game, bluebubbleposition, bubblesize, bluebubblevelocity, new Vector2(890, 650), new Vector2(10, 50), bubblemass, bubbleomega, true, false, hole));
                actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\bluebubble1"), Content.Load<Texture2D>(@"Core Elements\bluebubble2"), bluebubbleposition, 5, Color.DeepSkyBlue, 0.1f, index++, 1));
                Bubbles[index] = (Bubble)actors[k];
                k = k + 2;
                bluebubblevelocity = new Vector2(5 + random.Next(-3, 1), -5 + random.Next(-1, 3));
                bluestep += 0.5f;
            }

            for (i = index; i > 0; i--) //(straight selection) sort the bubbles according to their x positions
            {
                large = Bubbles[0];
                largepos = 0;
                for (j = 1; j <= i; j++)
                    if (Bubbles[j].position.X > large.position.X)
                    {
                        large = Bubbles[j];
                        largepos = j;
                    }
                Bubbles[largepos] = Bubbles[i];
                Bubbles[i] = large;
            }

            if (((Gun)actors[gunindex]).wind && !endgame) //if wind gun is on calculate gun effect on bubbles
            {
                ((Gun)actors[gunindex]).bound = new Vector2((((Gun)actors[gunindex]).position.Y + ((Gun)actors[gunindex]).position.Y + 83) / 2 - 30, (((Gun)actors[gunindex]).position.Y + ((Gun)actors[gunindex]).position.Y + 83) / 2 + 30);
                for (i = index; i >= 0; i--)
                {
                    if (Bubbles[i].draw) //calculate gun effect on bubble only if the bubble hasn't entered the hole yet 
                    {
                        bool up = false, down = false, both = false; //start calculating the intersection point of wind and the bubble
                        float intersectX, intersectY = 0;
                        if (Bubbles[i].position.Y + initsize.Y <= ((Gun)actors[gunindex]).bound.Y && Bubbles[i].position.Y + initsize.Y >= ((Gun)actors[gunindex]).bound.X) //up wind bound intersects the bubble
                        {
                            up = true;
                            intersectY = (((Gun)actors[gunindex]).bound.X + Bubbles[i].position.Y + initsize.Y) / 2;
                            //((Gun)actors[gunindex]).bound = new Vector2(Bubbles[i].position.Y + initsize.Y / 2, ((Gun)actors[gunindex]).bound.Y);
                        }
                        else if (Bubbles[i].position.Y <= ((Gun)actors[gunindex]).bound.Y && Bubbles[i].position.Y >= ((Gun)actors[gunindex]).bound.X) //down wind bound intersects the bubble
                        {
                            down = true;
                            intersectY = (Bubbles[i].position.Y + ((Gun)actors[gunindex]).bound.Y) / 2;
                            //((Gun)actors[gunindex]).bound = new Vector2(((Gun)actors[gunindex]).bound.X, Bubbles[i].position.Y);
                        }
                        else if (Bubbles[i].position.Y <= ((Gun)actors[gunindex]).bound.X && Bubbles[i].position.Y + initsize.Y >= ((Gun)actors[gunindex]).bound.Y) //both the bounds intersect the bubble
                        {
                            both = true;
                            intersectY = (((Gun)actors[gunindex]).bound.X + ((Gun)actors[gunindex]).bound.Y) / 2;
                            //((Gun)actors[gunindex]).bound = new Vector2(-1, -1);
                        }
                        if (up || down || both) //calculate effect of wind
                        {
                            intersectX = (float)((Bubbles[i].position.X + initsize.X / 2) + Math.Sqrt((initsize.X) * (initsize.X) / 4 - (intersectY - (Bubbles[i].position.Y + initsize.Y / 2)) * (intersectY - (Bubbles[i].position.Y + initsize.Y / 2))));
                            collisionangle = Math.Atan2((Bubbles[i].position.Y + initsize.Y / 2) - intersectY, -(Bubbles[i].position.X + initsize.X / 2) + intersectX);
                            direction1 = Math.Atan2(-Bubbles[i].velocity.Y, Bubbles[i].velocity.X);
                            direction2 = Math.Atan2(-((Gun)actors[gunindex]).velocity.Y, ((Gun)actors[gunindex]).velocity.X);
                            magnitude1 = Math.Sqrt(Bubbles[i].velocity.X * Bubbles[i].velocity.X + Bubbles[i].velocity.Y * Bubbles[i].velocity.Y);
                            magnitude2 = Math.Sqrt(((Gun)actors[gunindex]).velocity.X * ((Gun)actors[gunindex]).velocity.X + ((Gun)actors[gunindex]).velocity.Y * ((Gun)actors[gunindex]).velocity.Y);
                            tempvelx1 = magnitude1 * Math.Cos(direction1 - collisionangle);
                            tempvely1 = magnitude1 * Math.Sin(direction1 - collisionangle);
                            tempvelx2 = magnitude2 * Math.Cos(direction2 - collisionangle);
                            tempvely2 = magnitude2 * Math.Sin(direction2 - collisionangle);
                            tempvelx3 = ((Bubbles[i].mass - ((Gun)actors[gunindex]).windmass) * tempvelx1 / (Bubbles[i].mass + ((Gun)actors[gunindex]).windmass)) + (2 * ((Gun)actors[gunindex]).windmass * tempvelx2 / (Bubbles[i].mass + ((Gun)actors[gunindex]).windmass));
                            tempvelx4 = -((Bubbles[i].mass - ((Gun)actors[gunindex]).windmass) * tempvelx2 / (Bubbles[i].mass + ((Gun)actors[gunindex]).windmass)) + (2 * Bubbles[i].mass * tempvelx1 / (Bubbles[i].mass + ((Gun)actors[gunindex]).windmass));
                            magnitude1 = Math.Sqrt(tempvelx3 * tempvelx3 + tempvely1 * tempvely1);
                            magnitude2 = Math.Sqrt(tempvelx4 * tempvelx4 + tempvely2 * tempvely2);
                            direction1 = Math.Atan2(tempvely1, tempvelx3) + collisionangle;
                            direction2 = Math.Atan2(tempvely2, tempvelx4) + collisionangle;
                            Bubbles[i].omega = (tempvely2 - tempvely1) * 2 * 10 / (Bubbles[i].size.X * Bubbles[i].mass);
                            Bubbles[i].velocity = new Vector2((float)(magnitude1 * Math.Cos(direction1)), -(float)(magnitude1 * Math.Sin(direction1)));
                        }
                    }
                }
            }

            for (i = 15; i < actors.Count; i++) //check for collisions and respond accordingly
            {
                if (i == 15)
                    saved = 0;
                if (actors[i].GetType() == typeof(Bubble) && ((Bubble)actors[i]).draw)
                {
                    for (int j = i + 1; j < actors.Count; j++)
                    {
                        if (actors[j].GetType() == typeof(Bubble) && ((Bubble)actors[j]).draw && ((Bubble)actors[i]).BubblesCollide((Bubble)actors[j]))
                        {
                            collisionangle = Math.Atan2((((Bubble)actors[i]).position.Y + ((Bubble)actors[i]).size.Y / 2) - (((Bubble)actors[j]).position.Y + ((Bubble)actors[j]).size.Y / 2), -(((Bubble)actors[i]).position.X + ((Bubble)actors[i]).size.X / 2) + (((Bubble)actors[j]).position.X + ((Bubble)actors[j]).size.X / 2));
                            direction1 = Math.Atan2(-((Bubble)actors[i]).velocity.Y, ((Bubble)actors[i]).velocity.X);
                            direction2 = Math.Atan2(-((Bubble)actors[j]).velocity.Y, ((Bubble)actors[j]).velocity.X);
                            magnitude1 = Math.Sqrt(((Bubble)actors[i]).velocity.X * ((Bubble)actors[i]).velocity.X + ((Bubble)actors[i]).velocity.Y * ((Bubble)actors[i]).velocity.Y);
                            magnitude2 = Math.Sqrt(((Bubble)actors[j]).velocity.X * ((Bubble)actors[j]).velocity.X + ((Bubble)actors[j]).velocity.Y * ((Bubble)actors[j]).velocity.Y);
                            tempvelx1 = magnitude1 * Math.Cos(direction1 - collisionangle);
                            tempvely1 = magnitude1 * Math.Sin(direction1 - collisionangle);
                            tempvelx2 = magnitude2 * Math.Cos(direction2 - collisionangle);
                            tempvely2 = magnitude2 * Math.Sin(direction2 - collisionangle);
                            tempvelx3 = ((((Bubble)actors[i]).mass - ((Bubble)actors[j]).mass) * tempvelx1 / (((Bubble)actors[i]).mass + ((Bubble)actors[j]).mass)) + (2 * ((Bubble)actors[j]).mass * tempvelx2 / (((Bubble)actors[i]).mass + ((Bubble)actors[j]).mass));
                            tempvelx4 = -((((Bubble)actors[i]).mass - ((Bubble)actors[j]).mass) * tempvelx2 / (((Bubble)actors[i]).mass + ((Bubble)actors[j]).mass)) + (2 * ((Bubble)actors[i]).mass * tempvelx1 / (((Bubble)actors[i]).mass + ((Bubble)actors[j]).mass));
                            magnitude1 = Math.Sqrt(tempvelx3 * tempvelx3 + tempvely1 * tempvely1);
                            magnitude2 = Math.Sqrt(tempvelx4 * tempvelx4 + tempvely2 * tempvely2);
                            direction1 = Math.Atan2(tempvely1, tempvelx3) + collisionangle;
                            direction2 = Math.Atan2(tempvely2, tempvelx4) + collisionangle;
                            ((Bubble)actors[i]).omega = (tempvely2 - tempvely1) * 2 * 10 / (((Bubble)actors[i]).size.X * ((Bubble)actors[i]).mass);
                            ((Bubble)actors[j]).omega = -(tempvely1 - tempvely2) * 2 * 10 / (((Bubble)actors[j]).size.X * ((Bubble)actors[j]).mass);
                            ((Bubble)actors[i]).velocity = new Vector2((float)(magnitude1 * Math.Cos(direction1)), -(float)(magnitude1 * Math.Sin(direction1)));
                            ((Bubble)actors[j]).velocity = new Vector2((float)(magnitude2 * Math.Cos(direction2)), -(float)(magnitude2 * Math.Sin(direction2)));
                        }
                    }
                }
                else if (actors[i].GetType() == typeof(Bubble) && !((Bubble)actors[i]).draw) //check if the bubble has entered the gate and increase the bubbles saved quantity
                {
                    bubblessaved = "Bubbles Saved : " + ++saved + " out of " + BBLQTY;
                    ((DrawText)actors[12]).ChangeText(bubblessaved);
                }
                if (actors[i].GetType() == typeof(AnimateImage))
                {
                    ((AnimateImage)actors[i]).position = ((Bubble)actors[i - 1]).position;
                    ((AnimateImage)actors[i]).rotation = ((Bubble)actors[i - 1]).rotation;
                }
                if (saved == BBLQTY || Math.Round(((Gun)actors[gunindex]).battery) == 0)
                    endgame = true;
            }

            if (endgame)
            {
                score = ((((Gun)actors[gunindex]).battery / 14f) * (saved / (float)BBLQTY) * (16f / totaltime) * 5);
                if (score > 5)
                    score = 5;
                scoretext = "Score : " + string.Format("{0:0.0}", score) + " / 5";
                ((DrawText)actors[13]).ChangeText(scoretext);
                ((DrawText)actors[13]).MakeAppear();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
