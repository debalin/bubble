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
    
    public class Gun : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Texture2D texture1, texture2, wtexture1, wtexture2, pwrtexture1, pwrtexture2, btrytexture1, btrytexture2, boundtexture;
        public Vector2 screensize, position, bound, velocity, screenstart, boundscale;
        public float windmass, temp, battery = 20f, boundaryscale;
        public bool wind = false, check = true;
        private SpriteBatch spriteBatch;
        private SpriteFont gunfont;
        private int sprite = 0;

        public Gun(Game game, Texture2D texture1, Texture2D texture2, Texture2D wtexture1, Texture2D wtexture2, Texture2D pwrtexture1, Texture2D pwrtexture2, Texture2D btrytexture1, Texture2D btrytexture2, Texture2D boundtexture, Vector2 screensize, Vector2 screenstart, float boundaryscale, SpriteFont gunfont)
            : base(game)
        {
            this.texture1 = texture1;
            this.texture2 = texture2;
            this.wtexture1 = wtexture1;
            this.wtexture2 = wtexture2;
            this.pwrtexture1 = pwrtexture1;
            this.pwrtexture2 = pwrtexture2;
            this.btrytexture1 = btrytexture1;
            this.btrytexture2 = btrytexture2;
            this.boundtexture = boundtexture;
            this.screensize = screensize;
            this.screenstart = screenstart;
            this.boundaryscale = boundaryscale;
            this.gunfont = gunfont;
            position = new Vector2(screenstart.X + screensize.X, (screenstart.Y + screensize.Y) / 2);
            bound = new Vector2((position.Y + position.Y + 83) / 2 - 30, (position.Y + position.Y + 83) / 2 + 30);
            velocity = new Vector2(-8, 0);
            boundscale = new Vector2(.8f, 1);
            windmass = 5;
        }

        public new void UnloadContent()
        {
            texture1.Dispose();
            texture2.Dispose();
            btrytexture1.Dispose();
            btrytexture2.Dispose();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            if ((keyboard.IsKeyDown(Keys.Up) && (position.Y >= screenstart.Y)) || (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0.0f && (position.Y >= screenstart.Y))) //move the gunman up and down
            {
                position = new Vector2(position.X, position.Y - 8);
            }
            if ((keyboard.IsKeyDown(Keys.Down) && (position.Y <= screenstart.Y + screensize.Y - 100f)) || (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0.0f && (position.Y <= screenstart.Y + screensize.Y - 100f)))
            {
                position = new Vector2(position.X, position.Y + 8);
            }
            if ((keyboard.IsKeyDown(Keys.Left) && (velocity.X <= -4)) || (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X < 0.0f && (velocity.X <= -8))) //increase or decrease wind velocity 
            {
                temp = velocity.X + 0.1f;
                velocity = new Vector2(temp, velocity.Y);
            }
            if ((keyboard.IsKeyDown(Keys.Right) && (velocity.X >= -15)) || (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X > 0.0f && (velocity.X >= -25)))
            {
                temp = velocity.X - 0.1f;
                velocity = new Vector2(temp, velocity.Y);
            }
            if ((keyboard.IsKeyDown(Keys.Space) && battery > 0) || (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) && battery > 0)) //fire gun and exhaust it slowly
            {
                wind = true;
                battery = battery - 0.02f;
            }
            else
            {
                wind = false;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

                    if (check)
                    {
                        spriteBatch.Draw(texture1, position, Color.White);
                        spriteBatch.Draw(pwrtexture1, new Rectangle((int)position.X, (int)position.Y - 7, (int)(-(velocity.X + 4) * 80 / 11) + 20, 15), new Rectangle(0, 0, (int)(-(velocity.X + 4) * 80 / 11) + 20, 15), Color.White);
                        if (Math.Round(battery) != 0)
                            spriteBatch.Draw(btrytexture1, new Rectangle((int)position.X, (int)position.Y + 83 + 5, (int)(battery / 20 * 100), 15), new Rectangle(0, 0, (int)(battery / 20 * 100), 15), Color.Wheat);
                        else
                            spriteBatch.DrawString(gunfont, "  Battery\nExhausted!", new Vector2(position.X + 12, position.Y + 83 + 5), Color.White);
                        if (wind)
                            spriteBatch.Draw(boundtexture, new Vector2(screenstart.X + screensize.X - 5f, (position.Y + position.Y + 83) / 2 - 27), null, Color.White, 0f, new Vector2(boundtexture.Width, 0), boundscale, SpriteEffects.None, 0.2f);
                        sprite++;
                    }
                    if (sprite == 10)
                        check = false;
                    if (!check)
                    {
                        spriteBatch.Draw(texture2, position, Color.White);
                        spriteBatch.Draw(pwrtexture2, new Rectangle((int)position.X, (int)position.Y - 7, (int)(-(velocity.X + 4) * 80 / 11) + 20, 15), new Rectangle(0, 0, (int)(-(velocity.X + 4) * 80 / 11) + 20, 15), Color.White);
                        if (Math.Round(battery) != 0)
                            spriteBatch.Draw(btrytexture1, new Rectangle((int)position.X, (int)position.Y + 83 + 5, (int)(battery / 20 * 100), 15), new Rectangle(0, 0, (int)(battery / 20 * 100), 15), Color.Wheat);
                        else
                            spriteBatch.DrawString(gunfont, "  Battery\nExhausted!", new Vector2(position.X + 12, position.Y + 83 + 5), Color.White);
                        if (wind)
                            spriteBatch.Draw(boundtexture, new Vector2(screenstart.X + screensize.X - 5f, (position.Y + position.Y + 83) / 2 - 27), null, Color.White, 0f, new Vector2(boundtexture.Width, 0), boundscale, SpriteEffects.None, 0.2f);
                        sprite--;
                    }
                    if (sprite == 0)
                        check = true;

            base.Draw(gameTime);
        }
    }
}
