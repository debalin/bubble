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
    public class DrawBackground : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Texture2D background4_3, background16_9, background16_10;
        private Vector2 displayratio;
        private SpriteBatch spriteBatch;
        private GraphicsDeviceManager graphics;

        public DrawBackground(Game game, Texture2D background4_3, Texture2D background16_9, Texture2D background16_10) : base(game)
        {
            this.background4_3 = background4_3;
            this.background16_9 = background16_9;
            this.background16_10 = background16_10;

        }

        public override void Initialize()
        {
            graphics = (GraphicsDeviceManager)Game.Services.GetService(typeof(GraphicsDeviceManager)); //initialize resources and variables
            displayratio = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight); //the display ratio of the window

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void UnloadContent()
        {
            background16_10.Dispose();
            background16_9.Dispose();
            background4_3.Dispose();
            base.UnloadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch)); //initialize spritebatch

            //if ((displayratio.Y / displayratio.X) == (4f / 3f)) //for 4:3 resolutions
                spriteBatch.Draw(background4_3, new Rectangle(0, 0, (int)displayratio.X, (int)displayratio.Y), null, Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, 1f);
            
            /*else if ((displayratio.Y / displayratio.X) == (16f / 9f)) //for 16:9 resolutions
                spriteBatch.Draw(background16_9, new Rectangle(0, 0, (int)displayratio.X, (int)displayratio.Y), null, Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, 1f);
            
            else //for 16:10 resolutions
                spriteBatch.Draw(background16_10, new Rectangle(0, 0, (int)displayratio.X, (int)displayratio.Y), null, Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, 1f);*/
            
            base.Draw(gameTime);
        }
    }
}
