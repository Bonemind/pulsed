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
using System.Timers;

namespace PULSED
{
    public class Playing
    {
        //Initializer 
        Dummy dummy;
        Texture2D HeartFull;
        Texture2D HeartEmpty;
        Texture2D End;
        Slider slider;
        Score score;
        SpriteFont font;
        SerialControl gameControl;
        bool disconnected = false;
        bool beenPressed = false;
        bool gameStarted = false;
        int startScore;
        int playerId;
        ContentManager content;
        //loadContent
        public Playing(ContentManager Content, int score, int player)
        {
            HeartFull = Content.Load<Texture2D>("full heart");
            HeartEmpty = Content.Load<Texture2D>("empty heart");
            End = Content.Load<Texture2D>("end");
            font = Content.Load<SpriteFont>("Myfont");
            gameControl = new SerialControl();
            gameControl.ControlEvent += new EventHandler(InputHandler);
            content = Content;
            playerId = player;
            startScore = score;
        }
        private void startGame()
        {

            slider = new Slider(content);
            score = new Score(content);
            dummy = new Dummy();
            slider.ReanimationEvent += new EventHandler(LedSwitcher);
            score.CurrentScore = 50 * startScore;
            gameStarted = true;
        }
        public void LedSwitcher(object sender, EventArgs e)
        {
            if ((bool) sender)
            {
                gameControl.turnGreenLedOn();
            }
            else if (!(bool) sender)
            {
                gameControl.turnGreenLedOff();
                if (!beenPressed)
                {
                    dummy.Health -= 3;
                    gameControl.turnRedLedOn();
                }
                else
                {
                    beenPressed = false;
                }
            }
        }
        public void InputHandler(object sender, EventArgs e)
        {
            if (gameStarted && sender.ToString().Equals("#r%"))
            {
                if (slider.correctReanimationMoment())
                {
                    Console.WriteLine("corectmoment");
                    beenPressed = true;
                    score.increaseScore(20);
                    dummy.Health += 2;
                    slider.increasDifficulty();
                    gameControl.turnRedLedOn();
                }
                else
                {
                    dummy.Health -= 5;
                }
            }
            else if (!gameStarted && sender.ToString().Equals("#r%"))
            {
                startGame();
            }
        }
        public bool isGameOver()
        {
            if ( gameStarted && dummy.Health <= 0)
            {
                return true;
            }
            return false;
        }
        public int getFinalScore()
        {
            return score.CurrentScore;
        }


        //Update
        public void Update(GameTime gameTime)
        {
            if (gameStarted)
            {
                slider.Update(gameTime);
            }
            gameControl.Update();
            if (!gameControl.isConnected())
            {
                disconnected = true;
            }
            else
            {
                disconnected = false;
            }
        }

        //Actual game, health checking, and update healthbar.
        public void Draw(SpriteBatch spriteBatch)
        {
            if (gameStarted)
            {

                score.Draw(spriteBatch);
                slider.Draw(spriteBatch);
                if (disconnected)
                {
                    spriteBatch.DrawString(font, "DISCONNECTED", new Vector2(1000, 500), Color.Red);
                }

                //takes care of drawing the health in the form of hearts, every hearth represents 25% health
                int hearts = (int)Math.Ceiling((double)dummy.Health / 25);
                //Loops throught the number of full hearts that should be rendered, moving every heart 100px forward
                for (int i = 0; i < hearts; i++)
                {
                    spriteBatch.Draw(HeartFull, new Vector2(20 + (i * 100), 20), Color.White);
                }
                //draws the empty hearts, moves from the right to the left, decreasing the posistion of every heart by 100px
                for (int i = hearts; i <= 3; i++)
                {
                    spriteBatch.Draw(HeartEmpty, new Vector2(320 - ((3 - i) * 100), 20), Color.White);

                }
            }
            else
            {
                    spriteBatch.DrawString(font, "Druk op de knop op de pop", new Vector2(1000, 500), Color.Black);
            }

        }

    }
}
