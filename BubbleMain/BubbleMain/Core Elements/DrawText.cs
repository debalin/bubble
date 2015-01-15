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

    public class DrawText : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private string text;
        private SpriteFont font;
        private Vector2 position;
        private SpriteBatch spriteBatch;
        private bool appear = false;
        private Color color;
        private int faderate, alpha = 255;
        private float scale = 1f;
        
        public DrawText(Game game, String text, SpriteFont font, Vector2 position, Color color) : base(game) //without fade in 
        {
            this.text = text;
            this.font = font;
            this.position = position;
            this.color = color;
        }

        public DrawText(Game game, String text, SpriteFont font, Vector2 position, Color color, int faderate) : base(game) //with fade in
        {
            this.text = text;
            this.font = font;
            this.position = position;
            this.color = color;
            this.faderate = faderate;
            alpha = 0;
        }
        
        public override void Initialize()
        {
            base.Initialize();
        }

        public void MakeAppear()
        {
            appear = true;
        }

        public void MakeDisappear()
        {
            appear = false;
        }

        public void ChangeText(String text)
        {
            this.text = text;
        }

        public override void Update(GameTime gameTime)
        {
            if (appear && alpha <= 252) //increase the alpha value continuously to simulate fading in
                alpha += faderate;

            if (!appear && alpha >= 3) //deecrease the alpha value continuously to simulate fading out
                alpha -= faderate;

            base.Update(gameTime);
        }

        public void Select() //text selection formatting
        {
            color = Color.Black;
            scale = 1.2f;
        }

        public void Unselect() //normal text formatting
        {
            color = Color.White;
            scale = 1f;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch)); //initialize spritebatch

            if (appear)
                spriteBatch.DrawString(font, text, position, new Color(color.R, color.G, color.B, alpha), 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
 
            base.Draw(gameTime);
        }
    }
}
