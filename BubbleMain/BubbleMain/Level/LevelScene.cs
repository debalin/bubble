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

    public class LevelScene : GameScene
    {
        private Game game;
        private ContentManager Content;
        private GraphicsDeviceManager graphics;
        private Vector2 displayratio, levelpicposition, leveltextposition, levelpichangeposition;
        private DrawText[] textlist;
        private double starttime;
        private string escapetext;
        private SpriteFont font;

        public bool enter = false, timecheck = true;
        public int choice = 0, i, j;

        public LevelScene(Game game) : base(game)
        {
            this.game = game;
        }

        public override void Initialize()
        {
            Content = (ContentManager)Game.Services.GetService(typeof(ContentManager)); //initialize resources and variables
            graphics = (GraphicsDeviceManager)Game.Services.GetService(typeof(GraphicsDeviceManager));
            displayratio = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            textlist = new DrawText[5];
            levelpicposition = new Vector2(displayratio.X / 2 - 400, displayratio.Y / 2 + 120);
            leveltextposition = new Vector2(levelpicposition.X + 66, levelpicposition.Y + 160);
            levelpichangeposition = new Vector2(160, 0);
            escapetext = "Press ESC to go back.";
            font = Content.Load<SpriteFont>(@"Level\text");;

            actors.Add(new DrawBackground(game, Content.Load<Texture2D>(@"Level\backlevel4_3"), Content.Load<Texture2D>(@"Level\backlevel16_9"), Content.Load<Texture2D>(@"Level\backlevel16_10"))); //add all the actors
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Level\level1small1"), Content.Load<Texture2D>(@"Level\level1small2"), levelpicposition, 10, Color.White, 0.5f, 1, 1));
            actors.Add(new DrawText(game, "1", font, leveltextposition, Color.White));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\gunwind1"), Content.Load<Texture2D>(@"Core Elements\gunwind2"), levelpicposition + new Vector2(40, 35), 10, Color.White, 0.4f, 11, 0.55f)); //add the level defining logo elements 
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\entry1"), Content.Load<Texture2D>(@"Core Elements\entry2"), levelpicposition + new Vector2(25, -1), 10, Color.White, 0.4f, 12, 0.7f)); 
            levelpicposition += levelpichangeposition;
            leveltextposition += levelpichangeposition;
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Level\level2small1"), Content.Load<Texture2D>(@"Level\level2small2"), levelpicposition, 10, Color.White, 0.5f, 2, 1));
            actors.Add(new DrawText(game, "2", font, leveltextposition, Color.White));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\gunwind1"), Content.Load<Texture2D>(@"Core Elements\gunwind2"), levelpicposition + new Vector2(40, 5), 10, Color.White, 0.4f, 21, 0.55f));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\entry1"), Content.Load<Texture2D>(@"Core Elements\entry2"), levelpicposition + new Vector2(25, -1), 10, Color.White, 0.4f, 22, 0.7f));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\obstacle1_1"), Content.Load<Texture2D>(@"Core Elements\obstacle1_2"), levelpicposition + new Vector2(16, 30), 10, Color.White, 0.4f, 23, 0.3f)); 
            levelpicposition += levelpichangeposition;
            leveltextposition += levelpichangeposition;
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Level\level3small1"), Content.Load<Texture2D>(@"Level\level3small2"), levelpicposition, 10, Color.White, 0.5f, 3, 1));
            actors.Add(new DrawText(game, "3", font, leveltextposition, Color.White));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\gunwind1"), Content.Load<Texture2D>(@"Core Elements\gunwind2"), levelpicposition + new Vector2(40, -5), 10, Color.White, 0.4f, 31, 0.5f));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\entry1"), Content.Load<Texture2D>(@"Core Elements\entry2"), levelpicposition + new Vector2(20, -1), 10, Color.White, 0.4f, 32, 0.7f));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\obstacle1_1"), Content.Load<Texture2D>(@"Core Elements\obstacle1_2"), levelpicposition + new Vector2(16, 33), 10, Color.White, 0.4f, 33, 0.3f));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\spike1"), Content.Load<Texture2D>(@"Core Elements\spike2"), levelpicposition + new Vector2(12, 45), 10, Color.White, 0.4f, 34, 0.5f)); 
            levelpicposition += levelpichangeposition;
            leveltextposition += levelpichangeposition;
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Level\level4small1"), Content.Load<Texture2D>(@"Level\level4small2"), levelpicposition, 10, Color.White, 0.5f, 4, 1));
            actors.Add(new DrawText(game, "4", font, leveltextposition, Color.White));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\gunwind1"), Content.Load<Texture2D>(@"Core Elements\gunwind2"), levelpicposition + new Vector2(40, -1), 10, Color.White, 0.4f, 41, 0.5f));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\entry1"), Content.Load<Texture2D>(@"Core Elements\entry2"), levelpicposition + new Vector2(20, -1), 10, Color.White, 0.4f, 42, 0.7f));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\obstacle1_1"), Content.Load<Texture2D>(@"Core Elements\obstacle1_2"), levelpicposition + new Vector2(25, 20), 10, Color.White, 0.4f, 43, 0.3f));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\redbubble1"), Content.Load<Texture2D>(@"Core Elements\redbubble2"), levelpicposition + new Vector2(24, 45), 10, Color.Maroon, 0.4f, 44, 0.6f)); 
            levelpicposition += levelpichangeposition;
            leveltextposition += levelpichangeposition;
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Level\level5small1"), Content.Load<Texture2D>(@"Level\level5small2"), levelpicposition, 10, Color.White, 0.5f, 5, 1));
            actors.Add(new DrawText(game, "5", font, leveltextposition, Color.White));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\gunwind1"), Content.Load<Texture2D>(@"Core Elements\gunwind2"), levelpicposition + new Vector2(40, -6), 10, Color.White, 0.4f, 51, 0.5f));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\entry1"), Content.Load<Texture2D>(@"Core Elements\entry2"), levelpicposition + new Vector2(20, -1), 10, Color.White, 0.4f, 52, 0.7f));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\spike1"), Content.Load<Texture2D>(@"Core Elements\spike2"), levelpicposition + new Vector2(15, 45), 10, Color.White, 0.4f, 53, 0.5f));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\redbubble1"), Content.Load<Texture2D>(@"Core Elements\redbubble2"), levelpicposition + new Vector2(55, 75), 10, Color.Maroon, 0.4f, 54, 0.6f)); 
            
            for (i = 0, j = 0; i < actors.Count; i++) //create an array of level choosing options and feed them to TakeInput
            {
                if (actors[i].GetType() == typeof(DrawText))
                    textlist[j++] = (DrawText)actors[i];
            }
            actors.Add(new TakeInput(game, textlist, 4)); 

            for (i = 0; i < actors.Count; i++) //make all the level choosing options appear
            {
                if (typeof(DrawText) == actors[i].GetType())
                    ((DrawText)actors[i]).MakeAppear();
            }

            actors.Add(new DrawText(game, escapetext, font, new Vector2(displayratio.X - font.MeasureString(escapetext).X - 20, 20), Color.Black, 3));

            for (i = 0; i < actors.Count; i++) //make all the level defining logo elements disappear
            {
                if (actors[i].GetType() == typeof(AnimateImage) && ((AnimateImage)actors[i]).index > 10)
                    ((AnimateImage)actors[i]).MakeDisappear();
            }

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            for (i = 0; i < actors.Count; i++) 
            {
                if (actors[i].GetType() == typeof(TakeInput))
                {
                    enter = ((TakeInput)actors[i]).enter; //know what choice has been selected and whether enter has been pressed or not
                    choice = ((TakeInput)actors[i]).choice;

                    if (enter && (choice == 0 || choice == 1 || choice == 2 || choice == 3 || choice == 4)) 
                        ((TakeInput)actors[i]).enter = false;
                }

                if (actors[i].GetType() == typeof(AnimateImage) && ((AnimateImage)actors[i]).index < 10) //start and stop animation on the small level screens depending on whether they are selected or not
                {
                    if (((AnimateImage)actors[i]).index == choice + 1)
                        ((AnimateImage)actors[i]).StartAnimation();
                    else
                        ((AnimateImage)actors[i]).StopAnimation();
                }
            }

            for (i = 0; i < actors.Count; i++) //make level defining logo elements appear or disappear
            {
                if (actors[i].GetType() == typeof(AnimateImage) && (((AnimateImage)actors[i]).index > (choice + 1) * 10 && ((AnimateImage)actors[i]).index <= (choice + 2) * 10))
                    ((AnimateImage)actors[i]).MakeAppear();
                else if (actors[i].GetType() == typeof(AnimateImage) && ((AnimateImage)actors[i]).index > 10)
                    ((AnimateImage)actors[i]).MakeDisappear();
            }


            if (timecheck) //store the time when this part is executed for the first time
            {
                starttime = gameTime.TotalGameTime.TotalMilliseconds;
                timecheck = false;
            }

            if (starttime + 1000 < gameTime.TotalGameTime.TotalMilliseconds)
                ((DrawText)actors[actors.Count - 1]).MakeAppear();
            if (starttime + 4000 < gameTime.TotalGameTime.TotalMilliseconds)
                ((DrawText)actors[actors.Count - 1]).MakeDisappear();

            if (starttime + 200 < gameTime.TotalGameTime.TotalMilliseconds)//give some lag when this scene is showed first so that enter doesn't remain pressed
                base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
