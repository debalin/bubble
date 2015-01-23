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
    public class Level1 : GameScene
    {
        private ContentManager Content;
        private GraphicsDeviceManager graphics;
        private Vector2 displayratio, boundaryposition, gunposition, bluebubbleposition, bubblesize, bluebubblevelocity, initsize, hole;
        private float boundaryscale, bubblemass, bluestep, score;
        private Game game;
        private int i, j, k, index, largepos, gunindex;
        private double starttime, bubbleomega, pausedtime, resumetime, totalpausedtime = 0, happytime;
        public bool timecheck = true, endgame = false, GameSaveRequested = false, savehighscore = true, paused = false, happy = false;
        private double tempvelx1, tempvelx2, tempvelx3, tempvelx4, tempvely1, tempvely2, collisionangle, magnitude1, magnitude2, direction1, direction2;
        private Random random;
        private Bubble[] Bubbles;
        private Bubble large;
        private int BBLQTY, saved, totaltime, savedcount;
        private string timetaken, scoretext, bubblessaved, escapetext;
        private IAsyncResult result;
        private AudioEngine engine;
        private SoundBank soundBank;
        private WaveBank waveBank;

        SaveGameData data;

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
            savedcount = 0;
            happytime = 0;
            totalpausedtime = 0;
            paused = false;
            scoretext = "Score : 0";
            score = 0;
            escapetext = "Press ESC to resume \nand X to go back.";
            savehighscore = true;
            engine = new AudioEngine(@"Content\Level\voices.xgs");
            soundBank = new SoundBank(engine, @"Content\Level\Sound Bank.xsb");
            waveBank = new WaveBank(engine, @"Content\Level\Wave Bank.xwb");

            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Level\pause"), Content.Load<Texture2D>(@"Level\pause"), new Vector2(810, 65), 10, Color.White, .5f, 4, .7f));
            actors.Add(new DrawText(game, escapetext, Content.Load<SpriteFont>(@"Level\infotext"), new Vector2(600, 70), Color.White, 10));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\bluehappy"), Content.Load<Texture2D>(@"Core Elements\bluehappy"), bluebubbleposition, 5, Color.DeepSkyBlue, 0.1f, -1, 1));
            actors.Add(new DrawBackground(game, Content.Load<Texture2D>(@"Level\Level1\backlevel1_4_3"), Content.Load<Texture2D>(@"Level\Level1\backlevel1_16_9"), Content.Load<Texture2D>(@"Level\Level1\backlevel1_16_10")));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Level\Level1\moon"), Content.Load<Texture2D>(@"Level\Level1\moon"), new Vector2(180, 100), 10, Color.White, .7f, 1, .7f));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\entry1"), Content.Load<Texture2D>(@"Core Elements\entry2"), new Vector2(0, displayratio.Y / 2 - 77f), 10, Color.White, .5f, 2, 1));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\uplowbound1"), Content.Load<Texture2D>(@"Core Elements\uplowbound2"), new Vector2(15, 40), 10, Color.White, .6f, 3, 1));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\uplowbound2"), Content.Load<Texture2D>(@"Core Elements\uplowbound1"), new Vector2(15, 700), 10, Color.White, .6f, 3, 1));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\sidebound1"), Content.Load<Texture2D>(@"Core Elements\sidebound2"), new Vector2(900, 60), 10, Color.White, .6f, 3, 1));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\sidebound1"), Content.Load<Texture2D>(@"Core Elements\sidebound2"), new Vector2(0, 60), 10, Color.White, .6f, 3, 1));
            actors.Add(new DrawText(game, timetaken, Content.Load<SpriteFont>(@"Core Elements\text"), new Vector2(625, 10), Color.White));
            actors.Add(new DrawText(game, bubblessaved, Content.Load<SpriteFont>(@"Core Elements\text"), new Vector2(130, 10), Color.White));
            actors.Add(new DrawText(game, scoretext, Content.Load<SpriteFont>(@"Core Elements\scorefont"), new Vector2(300, 340), Color.White));
            actors.Add(new Gun(game, Content.Load<Texture2D>(@"Core Elements\gun1"), Content.Load<Texture2D>(@"Core Elements\gun2"), Content.Load<Texture2D>(@"Core Elements\gunwind1"), Content.Load<Texture2D>(@"Core Elements\gunwind2"), Content.Load<Texture2D>(@"Core Elements\powerbar1"), Content.Load<Texture2D>(@"Core Elements\powerbar2"), Content.Load<Texture2D>(@"Core Elements\battery1"), Content.Load<Texture2D>(@"Core Elements\battery2"), Content.Load<Texture2D>(@"Core Elements\bound"), new Vector2(890, 650), new Vector2(10, 50), boundaryscale, Content.Load<SpriteFont>(@"Core Elements\gunfont")));
            gunindex = 13;
            k = gunindex + 1;

            ((AnimateImage)actors[0]).MakeDisappear();
            ((AnimateImage)actors[2]).MakeDisappear();
            ((DrawText)actors[12]).MakeDisappear();
            ((DrawText)actors[10]).MakeAppear();
            ((DrawText)actors[11]).MakeAppear();
            ((AnimateImage)actors[4]).rotation = Math.PI / 1.2;

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
                if (!endgame)
                {
                    totaltime = (int)(gameTime.TotalGameTime.TotalSeconds - starttime - totalpausedtime);
                    timetaken = "Time : " + totaltime;
                    ((DrawText)actors[10]).ChangeText(timetaken);
                }

                if (gameTime.TotalGameTime.TotalSeconds - starttime - totalpausedtime> bluestep && bluestep <= 8.5) //add blue bubbles
                {
                    actors.Add(new Bubble(game, bluebubbleposition, bubblesize, bluebubblevelocity, new Vector2(890, 650), new Vector2(10, 50), bubblemass, bubbleomega, true, false, hole));
                    actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\bluebubble1"), Content.Load<Texture2D>(@"Core Elements\bluebubble2"), bluebubbleposition, 5, Color.DeepSkyBlue, 0.1f, index++, 1));
                    ((AnimateImage)actors[k + 1]).happytexture = Content.Load<Texture2D>(@"Core Elements\bluehappy");
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
                                //((Gun)actors[gunindex]).bound = new Vector2(Bubbles[i].position.Y + initsize.Y, ((Gun)actors[gunindex]).bound.Y);
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

                for (i = 12; i < actors.Count; i++) //check for collisions and respond accordingly
                {
                    if (i == 12)
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
                        ((DrawText)actors[11]).ChangeText(bubblessaved);
                    }
                    if (actors[i].GetType() == typeof(AnimateImage))
                    {
                        if (happy && (happytime + 1500 > gameTime.TotalGameTime.TotalMilliseconds))
                        {
                            ((AnimateImage)actors[i]).happy = true;
                            ((AnimateImage)actors[i]).position = ((Bubble)actors[i - 1]).position;
                            ((AnimateImage)actors[i]).rotation = ((Bubble)actors[i - 1]).rotation;
                        }
                        else
                        {
                            ((AnimateImage)actors[i]).happy = false;
                            ((AnimateImage)actors[i]).position = ((Bubble)actors[i - 1]).position;
                            ((AnimateImage)actors[i]).rotation = ((Bubble)actors[i - 1]).rotation;
                            happy = false;
                        }
                    }
                    if (saved == BBLQTY || Math.Round(((Gun)actors[gunindex]).battery) == 0)
                        endgame = true;
                }

                if (savedcount < saved)
                {
                    soundBank.PlayCue("happy");
                    happy = true;
                    savedcount = saved;
                    happytime = gameTime.TotalGameTime.TotalMilliseconds;
                }

                if (endgame)
                {
                    score = ((((Gun)actors[gunindex]).battery / 14f) * (saved / (float)BBLQTY) * (24f / totaltime) * 5);
                    if (score > 5)
                        score = 5;
                    scoretext = "Score : " + string.Format("{0:0.0}", score) + " / 5";
                    ((DrawText)actors[12]).ChangeText(scoretext);
                    ((DrawText)actors[12]).MakeAppear();
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

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
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
                        if (data.score1 < score)
                        {
                            data.score1 = score;
                        }
                        stream.Close();
                    }
                    else
                    {
                        data.score1 = score;
                        data.score2 = 0;
                        data.score3 = 0;
                        data.score4 = 0;
                        data.score5 = 0;
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
    }
}
