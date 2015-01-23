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
    [Serializable]
    public struct SaveGameData
    {
        public float score1;
        public float score2;
        public float score3;
        public float score4;
        public float score5;
    }
    
    public class LevelScene : GameScene
    {
        private Game game;
        private ContentManager Content;
        private GraphicsDeviceManager graphics;
        private Vector2 displayratio, levelpicposition, leveltextposition, levelpichangeposition;
        private DrawText[] textlist;
        private double starttime;
        private string escapetext, text1, text2, text3, text4, text5;
        private SpriteFont font, infofont;
        private float score1, score2, score3, score4, score5;
        public bool enter = false, timecheck = true, GameSaveRequested = false, showinfofirst = true;
        public int choice = 0, i, j, choicelast;
        private IAsyncResult result;

        SaveGameData data;

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
            leveltextposition = new Vector2(levelpicposition.X + 30, levelpicposition.Y + 160);
            levelpichangeposition = new Vector2(160, 0);
            escapetext = "Press ESC to go back anytime,\n      UP/DOWN to select levels\n        and ENTER to proceed.";
            font = Content.Load<SpriteFont>(@"Level\text");
            infofont = Content.Load<SpriteFont>(@"Level\infotext");
            text1 = "UP/DOWN     - Move wind\n                   gun up/down.\nSPACE        - Blow wind.\nRIGHT/LEFT - Increase/Decrease\n                   wind impact.\n\nYour objective is to blow all the \nblue bubbles through the gate\nin the shortest time and with the\nleast use of battery (shown below\n the gun) possible.";
            text2 = "UP/DOWN     - Move wind\n                   gun up/down.\nSPACE        - Blow wind.\nRIGHT/LEFT - Increase/Decrease\n                   wind impact.\n\nThe obstacles in this level \nare passive in nature. Guide\nthe bubbles past them and\nthrough the gate.";
            text3 = "UP/DOWN     - Move wind\n                   gun up/down.\nSPACE        - Blow wind.\nRIGHT/LEFT - Increase/Decrease\n                   wind impact.\n\nAvoid the spikes at any cost.\nThe bubbles will die otherwise.";
            text4 = "UP/DOWN     - Move wind\n                   gun up/down.\nSPACE        - Blow wind.\nRIGHT/LEFT - Increase/Decrease\n                   wind impact.\n\nThe red bubble will keep on\nguarding the gate, but they can\nbe pushed out of their track.\nTry and blow the bubbles past\nit and through the gate.";
            text5 = "UP/DOWN     - Move wind\n                   gun up/down.\nSPACE        - Blow wind.\nRIGHT/LEFT - Increase/Decrease\n                   wind impact.\n\nThe red bubble can shoot the\nblue bubbles towards the spikes.\nSo, be careful!";

            actors.Add(new DrawText(game, text1, infofont, new Vector2(70, 80), Color.White, 3));
            actors.Add(new DrawText(game, text2, infofont, new Vector2(70, 80), Color.White, 3));
            actors.Add(new DrawText(game, text3, infofont, new Vector2(70, 80), Color.White, 3));
            actors.Add(new DrawText(game, text4, infofont, new Vector2(70, 80), Color.White, 3));
            actors.Add(new DrawText(game, text5, infofont, new Vector2(70, 80), Color.White, 3));
            actors.Add(new DrawBackground(game, Content.Load<Texture2D>(@"Level\backlevel4_3"), Content.Load<Texture2D>(@"Level\backlevel16_9"), Content.Load<Texture2D>(@"Level\backlevel16_10"))); //add all the actors
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Level\level1small1"), Content.Load<Texture2D>(@"Level\level1small2"), levelpicposition, 10, Color.White, 0.5f, 1, 1));
            actors.Add(new DrawText(game, (score1.ToString()+"/5"), font, leveltextposition, Color.White));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\gunwind1"), Content.Load<Texture2D>(@"Core Elements\gunwind2"), levelpicposition + new Vector2(40, 35), 10, Color.White, 0.4f, 11, 0.55f)); //add the level defining logo elements 
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\entry1"), Content.Load<Texture2D>(@"Core Elements\entry2"), levelpicposition + new Vector2(25, -1), 10, Color.White, 0.4f, 12, 0.7f)); 
            levelpicposition += levelpichangeposition;
            leveltextposition += levelpichangeposition;
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Level\level2small1"), Content.Load<Texture2D>(@"Level\level2small2"), levelpicposition, 10, Color.White, 0.5f, 2, 1));
            actors.Add(new DrawText(game, (score2.ToString() + "/5"), font, leveltextposition, Color.White));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\gunwind1"), Content.Load<Texture2D>(@"Core Elements\gunwind2"), levelpicposition + new Vector2(40, 5), 10, Color.White, 0.4f, 21, 0.55f));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\entry1"), Content.Load<Texture2D>(@"Core Elements\entry2"), levelpicposition + new Vector2(25, -1), 10, Color.White, 0.4f, 22, 0.7f));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\obstacle1_1"), Content.Load<Texture2D>(@"Core Elements\obstacle1_2"), levelpicposition + new Vector2(16, 30), 10, Color.White, 0.4f, 23, 0.3f)); 
            levelpicposition += levelpichangeposition;
            leveltextposition += levelpichangeposition;
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Level\level3small1"), Content.Load<Texture2D>(@"Level\level3small2"), levelpicposition, 10, Color.White, 0.5f, 3, 1));
            actors.Add(new DrawText(game, (score3.ToString() + "/5"), font, leveltextposition, Color.White));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\gunwind1"), Content.Load<Texture2D>(@"Core Elements\gunwind2"), levelpicposition + new Vector2(40, -5), 10, Color.White, 0.4f, 31, 0.5f));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\entry1"), Content.Load<Texture2D>(@"Core Elements\entry2"), levelpicposition + new Vector2(20, -1), 10, Color.White, 0.4f, 32, 0.7f));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\obstacle1_1"), Content.Load<Texture2D>(@"Core Elements\obstacle1_2"), levelpicposition + new Vector2(16, 33), 10, Color.White, 0.4f, 33, 0.3f));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\spike1"), Content.Load<Texture2D>(@"Core Elements\spike2"), levelpicposition + new Vector2(12, 45), 10, Color.White, 0.4f, 34, 0.5f)); 
            levelpicposition += levelpichangeposition;
            leveltextposition += levelpichangeposition;
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Level\level4small1"), Content.Load<Texture2D>(@"Level\level4small2"), levelpicposition, 10, Color.White, 0.5f, 4, 1));
            actors.Add(new DrawText(game, (score4.ToString() + "/5"), font, leveltextposition, Color.White));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\gunwind1"), Content.Load<Texture2D>(@"Core Elements\gunwind2"), levelpicposition + new Vector2(40, -1), 10, Color.White, 0.4f, 41, 0.5f));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\entry1"), Content.Load<Texture2D>(@"Core Elements\entry2"), levelpicposition + new Vector2(20, -1), 10, Color.White, 0.4f, 42, 0.7f));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\obstacle1_1"), Content.Load<Texture2D>(@"Core Elements\obstacle1_2"), levelpicposition + new Vector2(25, 20), 10, Color.White, 0.4f, 43, 0.3f));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\redbubble1"), Content.Load<Texture2D>(@"Core Elements\redbubble2"), levelpicposition + new Vector2(24, 45), 10, Color.Maroon, 0.4f, 44, 0.6f)); 
            levelpicposition += levelpichangeposition;
            leveltextposition += levelpichangeposition;
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Level\level5small1"), Content.Load<Texture2D>(@"Level\level5small2"), levelpicposition, 10, Color.White, 0.5f, 5, 1));
            actors.Add(new DrawText(game, (score5.ToString() + "/5"), font, leveltextposition, Color.White));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\gunwind1"), Content.Load<Texture2D>(@"Core Elements\gunwind2"), levelpicposition + new Vector2(40, -6), 10, Color.White, 0.4f, 51, 0.5f));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\entry1"), Content.Load<Texture2D>(@"Core Elements\entry2"), levelpicposition + new Vector2(20, -1), 10, Color.White, 0.4f, 52, 0.7f));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\spike1"), Content.Load<Texture2D>(@"Core Elements\spike2"), levelpicposition + new Vector2(15, 45), 10, Color.White, 0.4f, 53, 0.5f));
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Core Elements\redbubble1"), Content.Load<Texture2D>(@"Core Elements\redbubble2"), levelpicposition + new Vector2(55, 75), 10, Color.Maroon, 0.4f, 54, 0.6f)); 
            
            for (i = 5, j = 0; i < actors.Count; i++) //create an array of level choosing options and feed them to TakeInput
            {
                if (actors[i].GetType() == typeof(DrawText))
                    textlist[j++] = (DrawText)actors[i];
            }
            actors.Add(new TakeInput(game, textlist, 4));

            for (i = 5; i < actors.Count; i++) //make all the level choosing options appear
            {
                if (typeof(DrawText) == actors[i].GetType())
                    ((DrawText)actors[i]).MakeAppear();
            }

            actors.Add(new DrawText(game, escapetext, infofont, new Vector2(displayratio.X - font.MeasureString(escapetext).X + 170, 16), Color.Black, 3));

            for (i = 0; i < actors.Count; i++) //make all the level defining logo elements disappear
            {
                if (actors[i].GetType() == typeof(AnimateImage) && ((AnimateImage)actors[i]).index > 10)
                    ((AnimateImage)actors[i]).MakeDisappear();
            }

            base.Initialize();
        }

        private void Load()
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
                        score1 = data.score1;
                        score2 = data.score2;
                        score3 = data.score3;
                        score4 = data.score4;
                        score5 = data.score5;
                        stream.Close();
                        container.Dispose();
                    }
                    else
                    {
                        container.Dispose();
                    }
                }
                GameSaveRequested = false;
            }
            ((DrawText)actors[7]).ChangeText(string.Format("{0:0.0}", score1) + "/5");
            ((DrawText)actors[11]).ChangeText(string.Format("{0:0.0}", score2) + "/5");
            ((DrawText)actors[16]).ChangeText(string.Format("{0:0.0}", score3) + "/5");
            ((DrawText)actors[22]).ChangeText(string.Format("{0:0.0}", score4) + "/5");
            ((DrawText)actors[28]).ChangeText(string.Format("{0:0.0}", score5) + "/5");
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
                Load();

            }

            if (starttime < gameTime.TotalGameTime.TotalMilliseconds)
                ((DrawText)actors[actors.Count - 1]).MakeAppear();
            if (starttime + 6000 < gameTime.TotalGameTime.TotalMilliseconds)
                ((DrawText)actors[actors.Count - 1]).MakeDisappear();
            if ((starttime + 2000 < gameTime.TotalGameTime.TotalMilliseconds) && showinfofirst)
            {
                ((DrawText)actors[choice]).MakeAppear();
                choicelast = choice;
                showinfofirst = false;
            }
            if (!showinfofirst && (choice != choicelast))
            {
                ((DrawText)actors[choicelast]).MakeDisappear();
                ((DrawText)actors[choice]).MakeAppear();
                choicelast = choice;
            }

            if (starttime + 200 < gameTime.TotalGameTime.TotalMilliseconds)//give some lag when this scene is showed first so that enter doesn't remain pressed
                base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
