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
    public class AnimateImage : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Texture2D texture1, texture2, backuptexture1, backuptexture2;
        public Texture2D happytexture, sadtexture, redsadtexture, redhappytexture;
        private bool check = true, startanimation = true, appear = true, happyover = true, sadover = true, redsadover = true, redhappyover = true;
        public bool happy = false, sad = false, redsad = false, redhappy = false;
        private short changerate, count = 0;
        private Color color1, color2;
        private float layerdepth, scale = 1;
        public Vector2 position;
        public double rotation = 0d;
        public int index;

        public AnimateImage(Game game, Texture2D texture1, Texture2D texture2, Vector2 position, short changerate, Color color, float layerdepth, int index, float scale) : base(game) //normal animation
        {
            this.texture1 = texture1;
            this.texture2 = texture2;
            this.backuptexture1 = texture1;
            this.backuptexture2 = texture2;
            this.changerate = changerate;
            this.position = position;
            this.color1 = color;
            this.color2 = color;
            this.layerdepth = layerdepth;
            this.index = index;
            this.scale = scale;
        }

        public AnimateImage(Game game, Texture2D texture1, Texture2D texture2, Vector2 position, short changerate, Color color1, Color color2, float layerdepth, int index, float scale) : base(game) //animation with the color changed on each shuffling
        {
            this.texture1 = texture1;
            this.texture2 = texture2;
            this.backuptexture1 = texture1;
            this.backuptexture2 = texture2;
            this.changerate = changerate;
            this.position = position;
            this.color1 = color1;
            this.color2 = color2;
            this.layerdepth = layerdepth;
            this.index = index;
            this.scale = scale;
        }

        public override void Initialize()
        {      
            base.Initialize();
        }

        public void ChangePosition(Vector2 position) //change the position where the sprite is drawn
        {
            this.position = position;
        }

        public void StartAnimation() //start the animation
        {
            startanimation = true;
        }

        public void StopAnimation() //stop the animation
        {
            startanimation = false;
        }

        public void MakeAppear() //make the texture appear on screen
        {
            appear = true;
        }

        public void MakeDisappear() //make the texture disappear from the screen
        {
            appear = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (happy)
            {
                texture1 = texture2 = happytexture;
                happyover = true;
            }
            else if (happyover)
            {
                texture1 = backuptexture1;
                texture2 = backuptexture2;
                happyover = false;
            }
            
            if (sad)
            {
                texture1 = texture2 = sadtexture;
                sadover = true;
            }
            else if (sadover)
            {
                texture1 = backuptexture1;
                texture2 = backuptexture2;
                sadover = false;
            }

            if (redsad)
            {
                texture1 = texture2 = redsadtexture;
                redsadover = true;
            }
            else if (redsadover)
            {
                texture1 = backuptexture1;
                texture2 = backuptexture2;
                redsadover = false;
            }

            if (redhappy)
            {
                texture1 = texture2 = redhappytexture;
                redhappyover = true;
            }
            else if (redhappyover)
            {
                texture1 = backuptexture1;
                texture2 = backuptexture2;
                redhappyover = false;
            }

            base.Update(gameTime);
        }

        protected override void UnloadContent()
        {
            texture1.Dispose();
            texture2.Dispose();
            base.UnloadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch)); //initialize spritebatch

            if (appear) //draw on screen
            {
                if (check) //draw one texture
                {
                    spriteBatch.Draw(texture1, new Vector2(position.X + texture1.Width / 2, position.Y + texture2.Height / 2), null, color1, (float)rotation, new Vector2(texture1.Width / 2, texture2.Height / 2), scale, SpriteEffects.None, layerdepth);
                    if (startanimation)
                        count++;
                }

                else //draw another texture
                {
                    spriteBatch.Draw(texture2, new Vector2(position.X + texture1.Width / 2, position.Y + texture2.Height / 2), null, color2, (float)rotation, new Vector2(texture1.Width / 2, texture2.Height / 2), scale, SpriteEffects.None, layerdepth);
                    if (startanimation)
                        count--;
                }

                if (count > changerate) //shuffle the check value to form the animation
                    check = false;
                else if (count < 0)
                    check = true;
            }

            base.Draw(gameTime);
        }
    }
}
