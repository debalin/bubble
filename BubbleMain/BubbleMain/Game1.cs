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
using BubbleMain.Opening_Credits;
using BubbleMain.Menu;
using BubbleMain.Level;

namespace BubbleMain
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private OpeningScene openingScene;
        private MenuScene menuScene;
        private LevelScene levelScene;
        private Level1 level1;
        private Level2 level2;
        private Level3 level3;
        private Level4 level4;
        private Level5 level5;
        private int openingscenetime, i;
        private bool check1 = true, check2 = false, check3 = false, check4 = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 750;
            graphics.PreferredBackBufferWidth = 1000;
            graphics.IsFullScreen = false;
            this.IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            Services.AddService(typeof(GraphicsDeviceManager), graphics); //initialize services
            Services.AddService(typeof(ContentManager), Content);
            openingScene = new OpeningScene(this);
            openingscenetime = 6;
            menuScene = new MenuScene(this);
            levelScene = new LevelScene(this);
            level1 = new Level1(this);
            level2 = new Level2(this);
            level3 = new Level3(this);
            level4 = new Level4(this);
            level5 = new Level5(this);
            
            Components.Add(openingScene); //add all the components
            Components.Add(menuScene);
            Components.Add(levelScene);
            Components.Add(level1);
            Components.Add(level2);
            Components.Add(level3);
            Components.Add(level4);
            Components.Add(level5);

            openingScene.Show(); //start with the opening scene
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            /*if (Keyboard.GetState().IsKeyDown(Keys.A)) //exit if a is pressed any time
                this.Exit();*/

            if (check1 && (gameTime.TotalGameTime.TotalSeconds > openingscenetime || Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start))) //go to menu if escape is pressed or the opening scene is over
            {
                openingScene.Hide();
                menuScene.Show();
                check1 = false;
                check2 = true;
            }

            if (check2) //menu
            {
                if (menuScene.enter)
                {
                    if (menuScene.choice == 1) //quit
                        this.Exit();

                    if (menuScene.choice == 0) //go to level scene
                    {
                        menuScene.Hide();
                        levelScene.Show();
                        menuScene.enter = false;
                        for (i = 0; i < menuScene.actors.Count; i++)
                        {
                            if (menuScene.actors[i].GetType() == typeof(TakeInput))
                                ((TakeInput)menuScene.actors[i]).enter = false;
                        }
                        check2 = false;
                        check3 = true;
                    }
                }
            }

            if (check3) //level scene
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back)) //go back to menu scene
                {
                    levelScene.Hide();
                    menuScene.Show();
                    levelScene.enter = false;
                    levelScene.timecheck = true;
                    for (i = 0; i < levelScene.actors.Count; i++)
                    {
                        if (levelScene.actors[i].GetType() == typeof(TakeInput))
                            ((TakeInput)levelScene.actors[i]).enter = false;
                    }
                    check3 = false;
                    check2 = true;
                }
                if (levelScene.enter)
                {
                    if (levelScene.choice == 0) //level 1
                    {
                        levelScene.Hide();
                        level1.Show();
                        levelScene.enter = false;
                        levelScene.timecheck = true;
                        for (i = 0; i < levelScene.actors.Count; i++)
                        {
                            if (levelScene.actors[i].GetType() == typeof(TakeInput))
                                ((TakeInput)levelScene.actors[i]).enter = false;
                        }
                        check3 = false;
                        check4 = true;
                    }
                    else if (levelScene.choice == 1) //level 2
                    {
                        levelScene.Hide();
                        level2.Show();
                        levelScene.enter = false;
                        levelScene.timecheck = true;
                        for (i = 0; i < levelScene.actors.Count; i++)
                        {
                            if (levelScene.actors[i].GetType() == typeof(TakeInput))
                                ((TakeInput)levelScene.actors[i]).enter = false;
                        }
                        check3 = false;
                        check4 = true;
                    }
                    else if (levelScene.choice == 2) //level 3
                    {
                        levelScene.Hide();
                        level3.Show();
                        levelScene.enter = false;
                        levelScene.timecheck = true;
                        for (i = 0; i < levelScene.actors.Count; i++)
                        {
                            if (levelScene.actors[i].GetType() == typeof(TakeInput))
                                ((TakeInput)levelScene.actors[i]).enter = false;
                        }
                        check3 = false;
                        check4 = true;
                    }
                    else if (levelScene.choice == 3) //level 4
                    {
                        levelScene.Hide();
                        level4.Show();
                        levelScene.enter = false;
                        levelScene.timecheck = true;
                        for (i = 0; i < levelScene.actors.Count; i++)
                        {
                            if (levelScene.actors[i].GetType() == typeof(TakeInput))
                                ((TakeInput)levelScene.actors[i]).enter = false;
                        }
                        check3 = false;
                        check4 = true;
                    }
                    else if (levelScene.choice == 4) //level 5
                    {
                        levelScene.Hide();
                        level5.Show();
                        levelScene.enter = false;
                        levelScene.timecheck = true;
                        for (i = 0; i < levelScene.actors.Count; i++)
                        {
                            if (levelScene.actors[i].GetType() == typeof(TakeInput))
                                ((TakeInput)levelScene.actors[i]).enter = false;
                        }
                        check3 = false;
                        check4 = true;
                    }
                }
            }

            if (check4)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back)) //go back to level scene
                {
                    check4 = false;
                    check2 = true;

                    menuScene.enter = true;
                    menuScene.choice = 0;

                    level1.Hide();
                    level1.timecheck = true;
                    level1.endgame = false;

                    level2.Hide();
                    level2.timecheck = true;
                    level2.endgame = false;

                    level3.Hide();
                    level3.timecheck = true;
                    level3.endgame = false;

                    level4.Hide();
                    level4.timecheck = true;
                    level4.endgame = false;

                    level5.Hide();
                    level5.timecheck = true;
                    level5.endgame = false;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, null);
            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
