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


namespace BubbleMain.Opening_Credits
{   
    public class OpeningScene : GameScene
    {
        private ContentManager Content;
        private GraphicsDeviceManager graphics;
        private Vector2 logoposition, displayratio, nameposition;
        private Game game;
        private double starttime;
        private bool timecheck = true;

        public OpeningScene(Game game) : base(game)
        {
            this.game = game;
        }

        public override void Initialize()
        {
            Content = (ContentManager)Game.Services.GetService(typeof(ContentManager)); //initialize resources and variables
            graphics = (GraphicsDeviceManager)Game.Services.GetService(typeof(GraphicsDeviceManager));
            displayratio = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            logoposition = new Vector2(displayratio.X / 2 - 220, displayratio.Y / 2 - 125);
            nameposition = new Vector2(displayratio.X / 2 - 45, displayratio.Y / 2 - 80);

            actors.Add(new DrawBackground(game, Content.Load<Texture2D>(@"Opening Credits\background4_3"), Content.Load<Texture2D>(@"Opening Credits\background16_9"), Content.Load<Texture2D>(@"Opening Credits\background16_10"))); //add all the actors to the scene
            actors.Add(new AnimateImage(game, Content.Load<Texture2D>(@"Opening Credits\logo1"), Content.Load<Texture2D>(@"Opening Credits\logo2"), logoposition, 8, Color.White, 0f, 1, 1));
            actors.Add(new DrawText(game, "   Team\nE-Crackers", Content.Load<SpriteFont>(@"Opening Credits\text"), nameposition, Color.Black, 3));
            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (timecheck) //store the time when this part is executed for the first time
            {
                starttime = gameTime.TotalGameTime.TotalSeconds;
                timecheck = false;
            }

            if (((gameTime.TotalGameTime.TotalSeconds - starttime) > 1f) && (logoposition.X > displayratio.X / 2 - 300)) //move logo to the left when the time is right
            {
                logoposition = new Vector2(logoposition.X - 0.5f, logoposition.Y);
                ((AnimateImage)actors[1]).position = logoposition;
            }

            if ((gameTime.TotalGameTime.TotalSeconds - starttime) > 2.5f) //fade in the name
            {
                ((DrawText)actors[2]).MakeAppear();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
