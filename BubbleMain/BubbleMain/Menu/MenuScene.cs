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


namespace BubbleMain.Menu
{
    public class MenuScene : GameScene
    {
        private Game game;
        private ContentManager Content;
        private GraphicsDeviceManager graphics;
        private Vector2 sunposition, displayratio, cloudposition1, cloudposition2, textstartposition, textchangeposition, bubbletextposition, bubblesize, bubbletextvelocity, bluebubbleposition, redbubbleposition, bluebubblevelociy, redbubblevelocity, bubbletextsize, arrowposition, arrowchangeposition;
        private float bubblemass, bubbletextmass;
        private int bluestep, redstep, index;
        private Random random;
        private bool timecheck = true, resettextshow = false;
        private double starttime;
        private DrawText[] textList;
        private double tempvelx1, tempvelx2, tempvelx3, tempvelx4, tempvely1, tempvely2, collisionangle, magnitude1, magnitude2, direction1, direction2, bubbletextomega, bubbleomega, resettexttime;

        public bool enter = false, resetcomplete = false;
        public int choice = 0;

        public MenuScene(Game game) : base(game)
        {
            this.game = game;
        }

        public override void Initialize()
        {
            Content = (ContentManager)Game.Services.GetService(typeof(ContentManager)); //initialize resources and variables
            graphics = (GraphicsDeviceManager)Game.Services.GetService(typeof(GraphicsDeviceManager));
            random = new Random();
            displayratio = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            sunposition = new Vector2(displayratio.X / 2 + 200, 50);
            cloudposition1 = new Vector2(displayratio.X / 2 - 450, 30);
            cloudposition2 = new Vector2(displayratio.X / 2 - 130, 70);
            textstartposition = new Vector2(displayratio.X / 2 + 280, displayratio.Y / 2 + 170);
            textchangeposition = new Vector2(0, 45);
            bubbletextposition = new Vector2(displayratio.X / 2 - 325, displayratio.Y / 2 - 32);
            bluebubbleposition = new Vector2(50, displayratio.Y - 50);
            redbubbleposition = new Vector2(displayratio.X - 114, displayratio.Y - 50);
            bubblesize = new Vector2(64, 64);
            bubbletextsize = new Vector2(100, 100);
            bubbletextvelocity = new Vector2(0, 0);
            bubbleomega = 0.05d;
            bubbletextomega = 0d;
            bluebubblevelociy = new Vector2(10, -10);
            redbubblevelocity = new Vector2(-10, -10);
            bubblemass = 15f;
            bubbletextmass = 25f;
            arrowposition = new Vector2(displayratio.X / 2 + 215, displayratio.Y / 2 + 180);
            arrowchangeposition = new Vector2(0, 44);
            bluestep = 4;
            redstep = 5;
            index = 4;
            resettexttime = 0;
            
            actors.Add(new DrawBackground(game, Content.Load<Texture2D>(@"Menu\backmenu4_3"), Content.Load<Texture2D>(@"Menu\backmenu16_9"), Content.Load<Texture2D>(@"Menu\backmenu16_10"))); //add actors
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Menu\sun1"), Content.Load<Texture2D>(@"Menu\sun2"), sunposition, 10, Color.White, 0.8f, 1, 1));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Menu\cloud1"), Content.Load<Texture2D>(@"Menu\cloud2"), cloudposition1, 10, Color.White, 0f, 2, 1));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Menu\cloud2"), Content.Load<Texture2D>(@"Menu\cloud1"), cloudposition2, 10, Color.White, 0f, 3, 1));
            actors.Add(new DrawText(game, "Play", Content.Load<SpriteFont>(@"Menu\text"), textstartposition, Color.White));
            textstartposition += textchangeposition;
            actors.Add(new DrawText(game, "Reset", Content.Load<SpriteFont>(@"Menu\text"), textstartposition, Color.White));
            textstartposition += textchangeposition;
            actors.Add(new DrawText(game, "Quit", Content.Load<SpriteFont>(@"Menu\text"), textstartposition, Color.White));
            textList = new DrawText[4];
            
            for (int i = 0; i <= 2; i++) //create an array of menu options and feed them to TakeInput
                textList[i] = (DrawText)actors[i + 4];         
            actors.Add(new TakeInput(game, textList, 2));

            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Menu\arrow1"), Content.Load<Texture2D>(@"Menu\arrow2"), arrowposition, 10, Color.DeepSkyBlue, Color.Maroon, 0.05f, 4, 1)); //add the big bubbles in the center of the screen         
            actors.Add(new Bubble(game, bubbletextposition, bubbletextsize, bubbletextvelocity, displayratio, Vector2.Zero, bubbletextmass, bubbletextomega, false, false, Vector2.Zero));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Menu\bubbleB1"), Content.Load<Texture2D>(@"Menu\bubbleB2"), bubbletextposition, 10, Color.Maroon, 0.1f, index++, 1));
            bubbletextposition += new Vector2(110, 0);
            actors.Add(new Bubble(game, bubbletextposition, bubbletextsize, bubbletextvelocity, displayratio, Vector2.Zero, bubbletextmass, bubbletextomega, false, false, Vector2.Zero));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Menu\bubbleU1"), Content.Load<Texture2D>(@"Menu\bubbleU2"), bubbletextposition, 10, Color.DeepSkyBlue, 0.1f, index++, 1));
            bubbletextposition += new Vector2(110, 0);
            actors.Add(new Bubble(game, bubbletextposition, bubbletextsize, bubbletextvelocity, displayratio, Vector2.Zero, bubbletextmass, bubbletextomega, false, false, Vector2.Zero));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Menu\bubbleB2"), Content.Load<Texture2D>(@"Menu\bubbleB1"), bubbletextposition, 10, Color.Maroon, 0.1f, index++, 1));
            bubbletextposition += new Vector2(110, 0);
            actors.Add(new Bubble(game, bubbletextposition, bubbletextsize, bubbletextvelocity, displayratio, Vector2.Zero, bubbletextmass, bubbletextomega, false, false, Vector2.Zero));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Menu\bubbleB1"), Content.Load<Texture2D>(@"Menu\bubbleB2"), bubbletextposition, 10, Color.DeepSkyBlue, 0.1f, index++, 1));
            bubbletextposition += new Vector2(110, 0);
            actors.Add(new Bubble(game, bubbletextposition, bubbletextsize, bubbletextvelocity, displayratio, Vector2.Zero, bubbletextmass, bubbletextomega, false, false, Vector2.Zero));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Menu\bubbleL1"), Content.Load<Texture2D>(@"Menu\bubbleL2"), bubbletextposition, 10, Color.Maroon, 0.1f, index++, 1));
            bubbletextposition += new Vector2(110, 0);
            actors.Add(new Bubble(game, bubbletextposition, bubbletextsize, bubbletextvelocity, displayratio, Vector2.Zero, bubblemass, bubbletextomega, false, false, Vector2.Zero));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Menu\bubbleE1"), Content.Load<Texture2D>(@"Menu\bubbleE2"), bubbletextposition, 10, Color.DeepSkyBlue, 0.1f, index++, 1));
            
            for (int i = 0; i < actors.Count; i++) //make all the menu options appear
            {
                if (typeof(DrawText) == actors[i].GetType())
                    ((DrawText)actors[i]).MakeAppear();
            }

            actors.Add(new DrawText(game, "Highscores reset.", Content.Load<SpriteFont>(@"Level\infotext"), new Vector2(830, 20), Color.Black, 3));
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            enter = ((TakeInput)actors[7]).enter; //know what choice is selected and whether enter is pressed or not
            choice = ((TakeInput)actors[7]).choice;

            ((AnimateImage)actors[8]).ChangePosition(arrowposition + choice * arrowchangeposition); //change the position of the arrow as the user goes through the options
            
            if (timecheck) //store the time when this part is executed for the first time
            {
                starttime = gameTime.TotalGameTime.TotalSeconds;
                timecheck = false;
            }

            if (resetcomplete)
            {
                ((DrawText)actors[21]).MakeAppear();
                resettexttime = gameTime.TotalGameTime.TotalMilliseconds;
                resetcomplete = false;
                resettextshow = true;
            }

            if (resettextshow && (resettexttime + 3000 < gameTime.TotalGameTime.TotalMilliseconds))
            {
                ((DrawText)actors[21]).MakeDisappear();
                resettextshow = false;
            }

            if (gameTime.TotalGameTime.TotalSeconds - starttime > bluestep && bluestep <= 16) //add blue bubbles
            {
                actors.Add(new Bubble(game, bluebubbleposition, bubblesize, bluebubblevelociy, displayratio, Vector2.Zero, bubblemass, bubbleomega, false, true, Vector2.Zero));
                actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\bluebubble1"), Content.Load<Texture2D>(@"Core Elements\bluebubble2"), bluebubbleposition, 5, Color.DeepSkyBlue, 0.1f, index++, 1));
                bluebubblevelociy = new Vector2(10 + random.Next(-2, 2), -10 + random.Next(-2, 2));
                bluestep += 4;
            }

            if (gameTime.TotalGameTime.TotalSeconds - starttime > redstep && redstep <= 17) //add red bubbles
            {
                actors.Add(new Bubble(game, redbubbleposition, bubblesize, redbubblevelocity, displayratio, Vector2.Zero, bubblemass, bubbleomega, false, true, Vector2.Zero));
                actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\redbubble1"), Content.Load<Texture2D>(@"Core Elements\redbubble2"), redbubbleposition, 5, Color.Maroon, 0.1f, index++, 1));
                redbubblevelocity = new Vector2(-10 + random.Next(-2, 2), -10 + random.Next(-2, 2));
                redstep += 4;
            }

            for (int i = 9; i < actors.Count; i++) //check for collisions and respond accordingly
            {
                if (actors[i].GetType() == typeof(Bubble))
                {
                    for (int j = i + 1; j < actors.Count; j++)
                    {
                        if (actors[j].GetType() == typeof(Bubble) && ((Bubble)actors[i]).BubblesCollide((Bubble)actors[j]))  
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
                if (actors[i].GetType() == typeof(AnimateImage)) //change the AnimateImage position to synchronise with its corresponding Bubble
                {
                    ((AnimateImage)actors[i]).position = ((Bubble)actors[i-1]).position;
                    ((AnimateImage)actors[i]).rotation = ((Bubble)actors[i-1]).rotation;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
