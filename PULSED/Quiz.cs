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
    public class Quiz
    {
        //initialize
        Texture2D answerInterface;
        SpriteFont fontvragen;
        SpriteFont fontantwoorden;
        KeyboardState currKeyboardState;
        KeyboardState prevKeyboardState;
        Database quizDB;
        Timer timer; 
        int questionNum = 0;
        int questionCount = 7;
        int correctAnswerCount = 0;
        bool drawFeedBack = false;
        bool lastAnswerIsTrue = false;

        public Quiz(ContentManager content)
        {
            answerInterface = content.Load<Texture2D>("interface1");
            fontvragen = content.Load<SpriteFont>("QuizSpritefont");
            fontantwoorden = content.Load<SpriteFont>("fontAntwoorden");
            quizDB = new Database();
            timer = new Timer(400);
            timer.Elapsed += new ElapsedEventHandler(stopFeedbackDraw);
        }
        private void stopFeedbackDraw(object sender, ElapsedEventArgs e)
        {
            drawFeedBack = false;
            timer.Stop();

        }

        public bool isQuizOver()
        {
            if (questionNum > questionCount)
            {
                quizDB.close();
                return true;
            }
            return false;
        }
        public int getCorrectAnswers()
        {
            return correctAnswerCount;
        }
        public void Update(GameTime gameTime)
        {
            currKeyboardState = Keyboard.GetState();
            if (currKeyboardState != prevKeyboardState)
            {
                if (currKeyboardState.IsKeyDown(Keys.D1))
                {
                    checkAnswer(1);
                    drawFeedBack = true;
                    timer.Start();
                }
                else if (currKeyboardState.IsKeyDown(Keys.D2))
                {
                    checkAnswer(2);
                    drawFeedBack = true; 
                    timer.Start();
                }
                else if (currKeyboardState.IsKeyDown(Keys.D3))
                {
                    checkAnswer(3);
                    drawFeedBack = true; 
                    timer.Start();
                }
            }

            prevKeyboardState = Keyboard.GetState();

        }
        private void checkAnswer(int answerNum)
        {
            if (quizDB.isAnswerCorrect(questionNum, answerNum) == true)
            {
                correctAnswerCount += 1;
                lastAnswerIsTrue = true;
            }
            else
            {
                lastAnswerIsTrue = false;
            }
            questionNum += 1;
        }
        private string introduceLineBreaks(string original)
        {
            string[] split = original.Split(' ');

            string withNewlines = "";
            for (int i = 0; i < split.Length; i++)
            {
                if (i % 6 == 0 && i > 0)
                {
                    withNewlines = withNewlines + split[i] + "\r\n";
                }
                else
                {
                    withNewlines = withNewlines + split[i] + " ";

                }
            }
            return withNewlines;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(answerInterface, new Vector2(550, 460), Color.White);
            if (drawFeedBack)
            {
                if (lastAnswerIsTrue)
                {
                    spriteBatch.DrawString(fontvragen, "Het antwoord was juist", new Vector2(660, 320), Color.Green); // Vraag 1
                }
                else
                {
                    spriteBatch.DrawString(fontvragen, "Het antwoord was onjuist", new Vector2(660, 320), Color.Red); // Vraag 1
                }
            }
            else
            {
                spriteBatch.DrawString(fontvragen, introduceLineBreaks(quizDB.getQuestion(questionNum)), new Vector2(460, 320), Color.Black); // Vraag 1
                Dictionary<int, String> answers = quizDB.getAnswersForQuestion(questionNum);
                int answerY = 550;
                foreach (KeyValuePair<int, String> answer in answers)
                {
                    spriteBatch.DrawString(fontantwoorden, introduceLineBreaks(answer.Value), new Vector2(680, answerY), Color.Black); // Antwoord A
                    answerY += 125;
                }
            }

        }

    }
}
