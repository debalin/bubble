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
    
    public class GameScene : Microsoft.Xna.Framework.DrawableGameComponent
    {

        public List<GameComponent> actors; //all the actors of the scene are in a List format

        public GameScene(Game game) : base(game)
        {
            actors = new List<GameComponent>();
            this.Hide();
        }

        public void Show() //show this game scene
        {
            Enabled = true;
            Visible = true;
        }

        public void Hide() //hide this game scene
        {
            Enabled = false;
            Visible = false;
        }

        public bool KnowState() //inquire whether this scene shown or hidden
        {
            if (this.Enabled && this.Visible)
                return true;
            else
                return false;
        }

        public override void Initialize()
        {
            for (int i = 0; i < actors.Count; i++)
                actors[i].Initialize();
            base.Initialize();
        }

        public override void Update(GameTime gameTime) 
        {
            for (int i = 0; i < actors.Count; i++) //update all the actors of the scene one by one
            {
                if (actors[i].Enabled)
                    actors[i].Update(gameTime);
            }
            base.Update(gameTime);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            for (int i = 0; i < actors.Count; i++) //draw all the actors of the scene if they are drawable
            {
                if ((actors[i] is DrawableGameComponent) && ((DrawableGameComponent)actors[i]).Visible)
                    ((DrawableGameComponent)actors[i]).Draw(gameTime);
            }
            base.Draw(gameTime);
        }
    }
}
