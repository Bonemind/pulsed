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
    public class Score
    {
        //Initializer
        private int score = 0;
        SpriteFont font;


        public int CurrentScore
        {
            get { return score; }
            set { score = value; }
        }
        public void increaseScore(int delta)
        {
            score += delta;
        }

        public Score(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("Myfont");
            if (font == null)
            {
                Console.WriteLine("font empty");
            }

            int YourScore = 0;
            CurrentScore = YourScore;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "Score: " + score.ToString(), new Vector2(1700, 50), Color.Black);
        }

    }   
}
