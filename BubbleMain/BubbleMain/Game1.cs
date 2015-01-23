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
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml.Serialization;

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
        private bool check1 = true, check2 = false, check3 = false, check4 = false, GameSaveRequested = false, songstart1 = true;
        private IAsyncResult result;
        private Song song1, song2, song3, song4, song5, song6;

        [Serializable]
        public struct SaveGameData
        {
            public float score1;
            public float score2;
            public float score3;
            public float score4;
            public float score5;
        }

        SaveGameData data;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 750;
            graphics.PreferredBackBufferWidth = 1000;
            graphics.IsFullScreen = false;
            this.IsMouseVisible = false;
            this.Window.Title = "Bubble Demo";
            this.Components.Add(new GamerServicesComponent(this)); 
        }

        protected override void Initialize()
        {
            Services.AddService(typeof(GraphicsDeviceManager), graphics); //initialize services
            Services.AddService(typeof(ContentManager), Content);
            openingScene = new OpeningScene(this);
            openingscenetime = 8;
            song1 = Content.Load<Song>(@"Core Elements\Strange Bubble World");
            song2 = Content.Load<Song>(@"Core Elements\Start off with a night");
            song3 = Content.Load<Song>(@"Core Elements\Boulder-y issues");
            song4 = Content.Load<Song>(@"Core Elements\Passive Sharpness");
            song5 = Content.Load<Song>(@"Core Elements\Machine Language");
            song6 = Content.Load<Song>(@"Core Elements\Silent Awakening");
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
            if ((songstart1 && gameTime.TotalGameTime.TotalSeconds > 2) || (songstart1 && !check1))
            {
                MediaPlayer.Play(song1);
                MediaPlayer.IsRepeating = true;
                songstart1 = false;
            }

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
                    if (menuScene.choice == 2) //quit
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

                    if (menuScene.choice == 1) //reset high scores
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
                                string filename = "highscore.sav";
                                data.score1 = 0;
                                data.score2 = 0;
                                data.score3 = 0;
                                data.score4 = 0;
                                data.score5 = 0;

                                result = device.BeginOpenContainer("StorageDemo", null, null);
                                result.AsyncWaitHandle.WaitOne();
                                StorageContainer containerlater = device.EndOpenContainer(result); //get the container
                                result.AsyncWaitHandle.Close();
                                if (containerlater.FileExists(filename))
                                    containerlater.DeleteFile(filename);
                                Stream streamlater = containerlater.CreateFile(filename);
                                XmlSerializer serializerlater = new XmlSerializer(typeof(SaveGameData));
                                serializerlater.Serialize(streamlater, data);
                                streamlater.Close();
                                containerlater.Dispose();
                            }
                            GameSaveRequested = false;
                        }
                        menuScene.resetcomplete = true;
                        menuScene.enter = false;
                        for (i = 0; i < menuScene.actors.Count; i++)
                        {
                            if (menuScene.actors[i].GetType() == typeof(TakeInput))
                                ((TakeInput)menuScene.actors[i]).enter = false;
                        }
                    }
                }
            }

            if (check3) //level scene
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back)) //go back to menu scene
                {
                    levelScene.Hide();
                    MediaPlayer.Stop();
                    MediaPlayer.Play(song1);
                    MediaPlayer.IsRepeating = true;
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
                        MediaPlayer.Stop();
                        MediaPlayer.Play(song2);
                        MediaPlayer.IsRepeating = true;
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
                        MediaPlayer.Stop();
                        MediaPlayer.Play(song3);
                        MediaPlayer.IsRepeating = true;
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
                        MediaPlayer.Stop();
                        MediaPlayer.Play(song4);
                        MediaPlayer.IsRepeating = true;
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
                        MediaPlayer.Stop();
                        MediaPlayer.Play(song5);
                        MediaPlayer.IsRepeating = true;
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
                        MediaPlayer.Stop();
                        MediaPlayer.Play(song6);
                        MediaPlayer.IsRepeating = true;
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
                if ( (level1.paused || level2.paused || level3.paused || level4.paused || level5.paused || level1.endgame || level2.endgame || level3.endgame || level4.endgame || level5.endgame) && (Keyboard.GetState().IsKeyDown(Keys.X) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back))) //go back to level scene
                {
                    check4 = false;
                    check2 = true;

                    menuScene.enter = true;
                    menuScene.choice = 0;

                    level1.Hide();
                    level1.timecheck = true;
                    level1.endgame = false;
                    level1.paused = false;

                    level2.Hide();
                    level2.timecheck = true;
                    level2.endgame = false;
                    level2.paused = false;

                    level3.Hide();
                    level3.timecheck = true;
                    level3.endgame = false;
                    level3.paused = false;

                    level4.Hide();
                    level4.timecheck = true;
                    level4.endgame = false;
                    level4.paused = false;

                    level5.Hide();
                    level5.timecheck = true;
                    level5.endgame = false;
                    level5.paused = false;
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
