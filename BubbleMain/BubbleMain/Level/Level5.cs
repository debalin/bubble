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
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml.Serialization;


namespace BubbleMain.Level
{

    public class Level5 : GameScene
    {
        private ContentManager Content;
        private GraphicsDeviceManager graphics;
        private Vector2 displayratio, boundaryposition, gunposition, bluebubbleposition, bubblesize, bluebubblevelocity, initsize, hole, redbubbleposition, redbubblevelocity;
        private float boundaryscale, bubblemass, redbubblemass, bluestep;
        private Game game;
        private int i, j, k, index, largepos, gunindex, killed, killedcount, savedcount;
        private double starttime, bubbleomega, score, redbubbleomega, pausedtime, resumetime, totalpausedtime = 0, happytime, sadtime;
        public bool timecheck = true, endgame = false, stop = false, GameSaveRequested = false, savehighscore = true, paused = false, happy, sad;
        private double tempvelx1, tempvelx2, tempvelx3, tempvelx4, tempvely1, tempvely2, collisionangle, magnitude1, magnitude2, direction1, direction2;
        private Random random;
        private Bubble[] Bubbles;
        private Bubble large;
        private int BBLQTY, saved, totaltime, finished, redbubbles, redkilledcount;
        private string timetaken, scoretext, bubblessaved, redkilledtext, escapetext;
        private Rectangle[] spike1;
        private IAsyncResult result;
        private AudioEngine engine;
        private SoundBank soundBank;
        private WaveBank waveBank;

        SaveGameData data;

        public Level5(Game game)
            : base(game)
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
            bluestep = 8;
            bluebubbleposition = new Vector2(20, 600);
            bubblesize = new Vector2(64, 64);
            initsize = new Vector2(64, 64);
            bluebubblevelocity = new Vector2(2, -2);
            bubblemass = 15f;
            bubbleomega = 0.3d;
            random = new Random();
            BBLQTY = 10;
            Bubbles = new Bubble[BBLQTY];
            hole = new Vector2(displayratio.Y / 2 - 70, displayratio.Y / 2 + 70);
            timetaken = "Time : 0";
            bubblessaved = "Bubbles Saved : 0 out of " + BBLQTY;
            redkilledtext = "Red Bubbles Killed : 0";
            saved = 0;
            finished = 0;
            stop = false;
            scoretext = "Score : 0";
            score = 0;
            totalpausedtime = 0;
            paused = false;
            redbubbles = 0;
            redbubbleposition = new Vector2(800, 80);
            redbubblevelocity = new Vector2(0, 3);
            redbubbleomega = .05;
            redbubblemass = 50;
            spike1 = new Rectangle[2];
            spike1[0] = new Rectangle(600, 30, 150, 50);
            spike1[1] = new Rectangle(600, 670, 150, 50);
            escapetext = "Press ESC to resume \nand X to go back.";
            savehighscore = true;
            happy = false;
            savedcount = 0;
            happytime = 0;
            killed = 0;
            killedcount = 0;
            sad = true;
            sadtime = 0;
            redkilledcount = 0;
            engine = new AudioEngine(@"Content\Level\voices.xgs");
            soundBank = new SoundBank(engine, @"Content\Level\Sound Bank.xsb");
            waveBank = new WaveBank(engine, @"Content\Level\Wave Bank.xwb");

            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Level\pause"), Content.Load<Texture2D>(@"Level\pause"), new Vector2(810, 75), 10, Color.White, .5f, 4, .7f));
            actors.Add(new DrawText(game, escapetext, Content.Load<SpriteFont>(@"Level\infotext"), new Vector2(600, 80), Color.White, 10));
            actors.Add(new DrawBackground(game, Content.Load<Texture2D>(@"Level\Level5\backlevel5_4_3"), Content.Load<Texture2D>(@"Level\Level5\backlevel5_4_3"), Content.Load<Texture2D>(@"Level\Level5\backlevel5_4_3")));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\entry1"), Content.Load<Texture2D>(@"Core Elements\entry2"), new Vector2(0, displayratio.Y / 2 - 77f), 10, Color.White, .5f, 2, 1));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\uplowbound1"), Content.Load<Texture2D>(@"Core Elements\uplowbound2"), new Vector2(15, 40), 10, Color.White, .6f, 3, 1));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\uplowbound2"), Content.Load<Texture2D>(@"Core Elements\uplowbound1"), new Vector2(15, 700), 10, Color.White, .6f, 3, 1));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\sidebound1"), Content.Load<Texture2D>(@"Core Elements\sidebound2"), new Vector2(900, 60), 10, Color.White, .6f, 3, 1));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\sidebound1"), Content.Load<Texture2D>(@"Core Elements\sidebound2"), new Vector2(0, 60), 10, Color.White, .6f, 3, 1));
            actors.Add(new DrawText(game, timetaken, Content.Load<SpriteFont>(@"Core Elements\text"), new Vector2(625, 10), Color.White));
            actors.Add(new DrawText(game, bubblessaved, Content.Load<SpriteFont>(@"Core Elements\text"), new Vector2(130, 10), Color.White));
            actors.Add(new DrawText(game, scoretext, Content.Load<SpriteFont>(@"Core Elements\scorefont"), new Vector2(350, 300), Color.White));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\spike1"), Content.Load<Texture2D>(@"Core Elements\spike2"), new Vector2(spike1[0].X, spike1[0].Y), 10, Color.White, .5f, 5, 1));
            actors.Add(new RedBubble(game, redbubbleposition, bubblesize, redbubblevelocity, new Vector2(890, 650), new Vector2(10, 50), redbubblemass, redbubbleomega, true, false, hole, spike1, 2));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\redbubble1"), Content.Load<Texture2D>(@"Core Elements\redbubble2"), redbubbleposition, 10, Color.Maroon, .6f, 6, 1));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\spike2"), Content.Load<Texture2D>(@"Core Elements\spike1"), new Vector2(spike1[1].X, spike1[1].Y), 10, Color.White, .5f, 5, 1));
            actors.Add(new DrawText(game, redkilledtext, Content.Load<SpriteFont>(@"Core Elements\text"), new Vector2(360, 720), Color.White));
            actors.Add(new Gun(game, Content.Load<Texture2D>(@"Core Elements\gun1"), Content.Load<Texture2D>(@"Core Elements\gun2"), Content.Load<Texture2D>(@"Core Elements\gunwind1"), Content.Load<Texture2D>(@"Core Elements\gunwind2"), Content.Load<Texture2D>(@"Core Elements\powerbar1"), Content.Load<Texture2D>(@"Core Elements\powerbar2"), Content.Load<Texture2D>(@"Core Elements\battery1"), Content.Load<Texture2D>(@"Core Elements\battery2"), Content.Load<Texture2D>(@"Core Elements\bound"), new Vector2(890, 650), new Vector2(10, 50), boundaryscale, Content.Load<SpriteFont>(@"Core Elements\gunfont")));
            gunindex = 16;
            ((AnimateImage)actors[gunindex - 2]).rotation = Math.PI; 
            k = gunindex + 1;
            ((RedBubble)actors[12]).Bubbles = Bubbles;

            ((AnimateImage)actors[0]).MakeDisappear();
            ((AnimateImage)actors[13]).redsadtexture = Content.Load<Texture2D>(@"Core Elements\redsad");
            ((AnimateImage)actors[13]).redhappytexture = Content.Load<Texture2D>(@"Core Elements\redhappy");
            ((DrawText)actors[10]).MakeDisappear();
            ((DrawText)actors[8]).MakeAppear();
            ((DrawText)actors[9]).MakeAppear();
            ((DrawText)actors[15]).MakeAppear();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (timecheck) //execute only for the first time this update method is run
            {
                actors.Clear();
                this.Initialize();

                starttime = gameTime.TotalGameTime.TotalSeconds;
                timecheck = false;
            }

            if (!paused && (Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back)) && (resumetime + 1f < gameTime.TotalGameTime.TotalSeconds))
            {
                paused = true;
                pausedtime = gameTime.TotalGameTime.TotalSeconds;
                ((AnimateImage)actors[0]).MakeAppear();
                ((DrawText)actors[1]).MakeAppear();
            }

            if (paused && (Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back)) && (pausedtime + 1f < gameTime.TotalGameTime.TotalSeconds))
            {
                paused = false;
                resumetime = gameTime.TotalGameTime.TotalSeconds;
                totalpausedtime += (resumetime - pausedtime);
                ((DrawText)actors[1]).MakeDisappear();
                ((AnimateImage)actors[0]).MakeDisappear();
            }

            if (!paused)
            {
                if (!endgame) //update the time taken
                {
                    totaltime = (int)(gameTime.TotalGameTime.TotalSeconds - starttime - totalpausedtime);
                    timetaken = "Time : " + totaltime;
                    ((DrawText)actors[8]).ChangeText(timetaken);
                }

                if (gameTime.TotalGameTime.TotalSeconds - starttime - totalpausedtime > bluestep && bluestep <= 12.5) //add blue bubbles
                {
                    actors.Add(new Bubble(game, bluebubbleposition, bubblesize, bluebubblevelocity, new Vector2(890, 650), new Vector2(10, 50), bubblemass, bubbleomega, true, false, hole, spike1, 2));
                    actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\bluebubble1"), Content.Load<Texture2D>(@"Core Elements\bluebubble2"), bluebubbleposition, 5, Color.DeepSkyBlue, 0.1f, index++, 1));
                    Bubbles[index] = (Bubble)actors[k];
                    ((AnimateImage)actors[k + 1]).happytexture = Content.Load<Texture2D>(@"Core Elements\bluehappy");
                    ((AnimateImage)actors[k + 1]).sadtexture = Content.Load<Texture2D>(@"Core Elements\bluesad");
                    k = k + 2;
                    bluebubblevelocity = new Vector2(3 + random.Next(-2, 1), -3 + random.Next(-1, 2));
                    bluestep += 0.5f;
                    ((RedBubble)actors[12]).index = index;
                }

                if (!((RedBubble)actors[12]).draw)
                {
                    actors[12] = new RedBubble(game, redbubbleposition, bubblesize, redbubblevelocity, new Vector2(890, 650), new Vector2(10, 50), redbubblemass, redbubbleomega, true, false, hole, spike1, 2);
                    ((RedBubble)actors[12]).Bubbles = Bubbles;
                    ((RedBubble)actors[12]).index = index;
                    redbubbles++;
                    ((DrawText)actors[15]).ChangeText(("Red Bubbles Killed : " + redbubbles));
                }

                if (redkilledcount < redbubbles)
                {
                    soundBank.PlayCue("happy");
                    happy = true;
                    redkilledcount = redbubbles;
                    happytime = gameTime.TotalGameTime.TotalMilliseconds;
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
                        if (Bubbles[i].draw)
                        {
                            bool up = false, down = false, both = false; //start calculating the intersection point of wind and the bubble
                            float intersectX, intersectY = 0;
                            if (Bubbles[i].position.Y + initsize.Y <= ((Gun)actors[gunindex]).bound.Y && Bubbles[i].position.Y + initsize.Y >= ((Gun)actors[gunindex]).bound.X) //up wind bound intersects the bubble
                            {
                                up = true;
                                intersectY = (((Gun)actors[gunindex]).bound.X + Bubbles[i].position.Y + initsize.Y) / 2;
                            }
                            else if (Bubbles[i].position.Y <= ((Gun)actors[gunindex]).bound.Y && Bubbles[i].position.Y >= ((Gun)actors[gunindex]).bound.X) //down wind bound intersects the bubble
                            {
                                down = true;
                                intersectY = (Bubbles[i].position.Y + ((Gun)actors[gunindex]).bound.Y) / 2;
                            }
                            else if (Bubbles[i].position.Y <= ((Gun)actors[gunindex]).bound.X && Bubbles[i].position.Y + initsize.Y >= ((Gun)actors[gunindex]).bound.Y) //both the bounds intersect the bubble
                            {
                                both = true;
                                intersectY = (((Gun)actors[gunindex]).bound.X + ((Gun)actors[gunindex]).bound.Y) / 2;
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

                for (i = gunindex + 1; i < actors.Count; i++) //check for collisions and respond accordingly
                {
                    if (i == gunindex + 1)
                    {
                        saved = 0;
                        killed = 0;
                        finished = 0;
                    }
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
                        if (((RedBubble)actors[12]).BubblesCollide((Bubble)actors[i])) //collision with red bubble
                        {
                            collisionangle = Math.Atan2((((Bubble)actors[i]).position.Y + ((Bubble)actors[i]).size.Y / 2) - (((RedBubble)actors[12]).position.Y + ((RedBubble)actors[12]).size.Y / 2), -(((Bubble)actors[i]).position.X + ((Bubble)actors[i]).size.X / 2) + (((RedBubble)actors[12]).position.X + ((RedBubble)actors[12]).size.X / 2));
                            direction1 = Math.Atan2(-((Bubble)actors[i]).velocity.Y, ((Bubble)actors[i]).velocity.X);
                            direction2 = Math.Atan2(-((RedBubble)actors[12]).velocity.Y, ((RedBubble)actors[12]).velocity.X);
                            magnitude1 = Math.Sqrt(((Bubble)actors[i]).velocity.X * ((Bubble)actors[i]).velocity.X + ((Bubble)actors[i]).velocity.Y * ((Bubble)actors[i]).velocity.Y);
                            magnitude2 = Math.Sqrt(((RedBubble)actors[12]).velocity.X * ((RedBubble)actors[12]).velocity.X + ((RedBubble)actors[12]).velocity.Y * ((RedBubble)actors[12]).velocity.Y);
                            tempvelx1 = magnitude1 * Math.Cos(direction1 - collisionangle);
                            tempvely1 = magnitude1 * Math.Sin(direction1 - collisionangle);
                            tempvelx2 = magnitude2 * Math.Cos(direction2 - collisionangle);
                            tempvely2 = magnitude2 * Math.Sin(direction2 - collisionangle);
                            tempvelx3 = ((((Bubble)actors[i]).mass - ((RedBubble)actors[12]).mass) * tempvelx1 / (((Bubble)actors[i]).mass + ((RedBubble)actors[12]).mass)) + (2 * ((RedBubble)actors[12]).mass * tempvelx2 / (((Bubble)actors[i]).mass + ((RedBubble)actors[12]).mass));
                            tempvelx4 = -((((Bubble)actors[i]).mass - ((RedBubble)actors[12]).mass) * tempvelx2 / (((Bubble)actors[i]).mass + ((RedBubble)actors[12]).mass)) + (2 * ((Bubble)actors[i]).mass * tempvelx1 / (((Bubble)actors[i]).mass + ((RedBubble)actors[12]).mass));
                            magnitude1 = Math.Sqrt(tempvelx3 * tempvelx3 + tempvely1 * tempvely1);
                            magnitude2 = Math.Sqrt(tempvelx4 * tempvelx4 + tempvely2 * tempvely2);
                            direction1 = Math.Atan2(tempvely1, tempvelx3) + collisionangle;
                            direction2 = Math.Atan2(tempvely2, tempvelx4) + collisionangle;
                            ((Bubble)actors[i]).omega = (tempvely2 - tempvely1) * 2 * 10 / (((Bubble)actors[i]).size.X * ((Bubble)actors[i]).mass);
                            ((RedBubble)actors[12]).omega = -(tempvely1 - tempvely2) * 2 * 10 / (((RedBubble)actors[12]).size.X * ((RedBubble)actors[12]).mass);
                            ((Bubble)actors[i]).velocity = new Vector2((float)(magnitude1 * Math.Cos(direction1)), -(float)(magnitude1 * Math.Sin(direction1)));
                            ((RedBubble)actors[12]).velocity = new Vector2((float)(magnitude2 * Math.Cos(direction2)), -(float)(magnitude2 * Math.Sin(direction2)));
                        }
                    }
                    else if (actors[i].GetType() == typeof(Bubble) && !((Bubble)actors[i]).draw) //update the bubbles saved count
                    {
                        if (((Bubble)actors[i]).draw1)
                        {
                            bubblessaved = "Bubbles Saved : " + ++saved + " out of " + BBLQTY;
                            ((DrawText)actors[9]).ChangeText(bubblessaved);
                        }
                        else
                        {
                            killed++;
                        }
                        finished++;
                    }
                    if (actors[i].GetType() == typeof(AnimateImage)) //update the position of the blue bubbles in AnimateImage
                    {
                        if ((happy && (happytime + 1500 > gameTime.TotalGameTime.TotalMilliseconds)) || (sad && (sadtime + 1500 > gameTime.TotalGameTime.TotalMilliseconds)))
                        {
                            if (happy)
                                ((AnimateImage)actors[i]).happy = true;
                            else if (sad)
                                ((AnimateImage)actors[i]).sad = true;
                            ((AnimateImage)actors[i]).position = ((Bubble)actors[i - 1]).position;
                            ((AnimateImage)actors[i]).rotation = ((Bubble)actors[i - 1]).rotation;
                        }
                        else
                        {
                            ((AnimateImage)actors[i]).happy = false;
                            ((AnimateImage)actors[i]).sad = false;
                            ((AnimateImage)actors[i]).position = ((Bubble)actors[i - 1]).position;
                            ((AnimateImage)actors[i]).rotation = ((Bubble)actors[i - 1]).rotation;
                            happy = false;
                            sad = true;
                        }
                    }
                    if (finished == BBLQTY || Math.Round(((Gun)actors[gunindex]).battery) == 0)
                        endgame = true;
                }

                if (savedcount < saved)
                {
                    soundBank.PlayCue("happy");
                    happy = true;
                    savedcount = saved;
                    happytime = gameTime.TotalGameTime.TotalMilliseconds;
                }

                if (killedcount < killed)
                {
                    soundBank.PlayCue("sad");
                    sad = true;
                    killedcount = killed;
                    sadtime = gameTime.TotalGameTime.TotalMilliseconds;
                }

                if (happy && (happytime + 1500 > gameTime.TotalGameTime.TotalMilliseconds))
                    ((AnimateImage)actors[13]).redsad = true;
                else if (!endgame)
                    ((AnimateImage)actors[13]).redsad = false;

                if (sad && (sadtime + 1500 > gameTime.TotalGameTime.TotalMilliseconds))
                    ((AnimateImage)actors[13]).redhappy= true;
                else if (!endgame)
                    ((AnimateImage)actors[13]).redhappy = false;
                
                ((AnimateImage)actors[13]).position = ((RedBubble)actors[12]).position;
                ((AnimateImage)actors[13]).rotation = ((RedBubble)actors[12]).rotation;

                if (endgame && !stop) //scoring
                {
                    score = ((((Gun)actors[gunindex]).battery / 14f) * Math.Pow((saved / (float)BBLQTY), 2) * (22f / totaltime) * 5);
                    if (score > 5)
                        score = 5;
                    scoretext = "Score : " + string.Format("{0:0.0}", score) + " / 5";
                    ((DrawText)actors[10]).ChangeText(scoretext);
                    ((DrawText)actors[10]).MakeAppear();
                    stop = true;
                    if (savehighscore)
                    {
                        Save();
                        savehighscore = false;
                    }
                }
                base.Update(gameTime);
            }
            ((DrawText)actors[1]).Update(gameTime);
        }

        private void Save()
        {
            if ((!Guide.IsVisible) && (GameSaveRequested == false))
            {
                GameSaveRequested = true;
                result = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
            }
            if ((GameSaveRequested) && (result.IsCompleted))
            {
                StorageDevice device = StorageDevice.EndShowSelector(result); //get the device
                if (device != null && device.IsConnected)
                {
                    result = device.BeginOpenContainer("StorageDemo", null, null);
                    result.AsyncWaitHandle.WaitOne();
                    StorageContainer container = device.EndOpenContainer(result); //get the container
                    result.AsyncWaitHandle.Close();
                    string filename = "highscore.sav";
                    if (container.FileExists(filename))
                    {
                        Stream stream = container.OpenFile(filename, FileMode.Open);
                        XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
                        data = (SaveGameData)serializer.Deserialize(stream);
                        if (data.score5 < score)
                        {
                            data.score5 = (float)score;
                        }
                        stream.Close();
                    }
                    else
                    {
                        data.score5 = (float)score;
                        data.score1 = 0;
                        data.score2 = 0;
                        data.score3 = 0;
                        data.score4 = 0;
                    }
                    container.Dispose();

                    result = device.BeginOpenContainer("StorageDemo", null, null);
                    result.AsyncWaitHandle.WaitOne();
                    StorageContainer containerlater = device.EndOpenContainer(result); //get the container
                    result.AsyncWaitHandle.Close();
                    containerlater.DeleteFile(filename);
                    Stream streamlater = containerlater.CreateFile(filename);
                    XmlSerializer serializerlater = new XmlSerializer(typeof(SaveGameData));
                    serializerlater.Serialize(streamlater, data);
                    streamlater.Close();
                    containerlater.Dispose();
                }
                GameSaveRequested = false;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
