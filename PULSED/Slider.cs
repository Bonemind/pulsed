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
using IndependentResolutionRendering;

namespace PULSED
{
    public class Slider
    {
        //Initsializer 
        Texture2D MovingRectangle;
        Texture2D StandingRedBlock;
        Texture2D StandingGreenBlock;
        Texture2D Yeah;
        public event EventHandler ReanimationEvent;
        bool raiseEvent = false;
        int movingRectanglePosition;
        int speedCounter = 0;
        int speed = 10;


        //loadContent
        public Slider(ContentManager Content)
        {
            StandingRedBlock = Content.Load<Texture2D>("RedBlock");
            StandingGreenBlock = Content.Load<Texture2D>("GreenBlock");
            MovingRectangle = Content.Load<Texture2D>("OpenRectangle");
            Yeah = Content.Load<Texture2D>("end");

        }

        //Update
        public void Update(GameTime gameTime)
        {
            if (movingRectanglePosition > 1660 && movingRectanglePosition < 1720)
            {
                movingRectanglePosition += 1;
                if (raiseEvent)
                {
                    this.ReanimationEvent(true, new EventArgs());
                    raiseEvent = false;
                }
            }
            else
            {
                movingRectanglePosition += speed;
                if (!raiseEvent)
                {
                    raiseEvent = true;
                    this.ReanimationEvent(false, new EventArgs());
                }
            }


            if (movingRectanglePosition > 1920)
            {
                movingRectanglePosition = 0;
            }


        }

        public bool correctReanimationMoment()
        {
            if (movingRectanglePosition > 1680 && movingRectanglePosition < 1720)
            {
                return true;
            }
            return false;
        }
        public void increasDifficulty()
        {
            if (speedCounter < 6)
            {
                speedCounter += 1;
            }
            else
            {
                if (speed < 57)
                {
                    speed += 10;
                }
                else
                {
                    speed = 57;
                }
                speedCounter = 0;
            }
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {

            if (movingRectanglePosition > 1680 && movingRectanglePosition < 1720)
            {
                spriteBatch.Draw(StandingGreenBlock, new Vector2(1700, 800), Color.White);

            }
            else
            {
                spriteBatch.Draw(StandingRedBlock, new Vector2(1700, 800), Color.White);
            }
            spriteBatch.Draw(MovingRectangle, new Vector2(movingRectanglePosition, 790), Color.White);
        }
    }
}