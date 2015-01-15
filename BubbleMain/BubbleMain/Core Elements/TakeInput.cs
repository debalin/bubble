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

    public class TakeInput : Microsoft.Xna.Framework.GameComponent
    {
        private int limit;
        private DrawText[] textList;
        private bool flag1 = true, flag2 = true;
        private double time1, time2;

        public bool enter = false;
        public int choice = 0;
        
        public TakeInput(Game game, DrawText[] textList, int limit) : base(game)
        {
            this.textList = textList;
            this.limit = limit;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            textList[choice].Select(); //select a menu choice

            if ((Keyboard.GetState().IsKeyDown(Keys.Down) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0.0f) && choice <= limit && flag1) //if down arrow is pressed 
            {
                textList[choice].Unselect(); //unselect the current choice
                if (choice == limit)
                    choice = 0;
                else
                    choice++;
                textList[choice].Select(); //select the next choice
                flag1 = false;
                time1 = gameTime.TotalGameTime.TotalMilliseconds;
            }

            else if ((Keyboard.GetState().IsKeyDown(Keys.Up) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0.0f) && choice >= 0 && flag2) //if up arrow is pressed
            {
                textList[choice].Unselect(); //unselect the current choice
                if (choice == 0)
                    choice = limit;
                else
                    choice--;
                textList[choice].Select(); //select the previous choice
                flag2 = false;
                time2 = gameTime.TotalGameTime.TotalMilliseconds;
            }

            if (time1 + 200 < gameTime.TotalGameTime.TotalMilliseconds) //give some lag after the up or down arrow is pressed once
                flag1 = true;
            if (time2 + 200 < gameTime.TotalGameTime.TotalMilliseconds)
                flag2 = true;

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A)) //if enter is pressed
                enter = true;

            base.Update(gameTime);
        }
    }
}
