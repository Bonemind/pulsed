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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        SpriteFont font;
        SpriteFont usernameFont;
        SpriteFont instructionfont;
        SpriteFont highscoreFont;
        Playing beat;
        Quiz quiz;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D PulsedLogo;
        Vector2 LogoPosition = new Vector2(620, 75);
        Texture2D Button;
        Texture2D SoundOn;
        Texture2D SoundOff;
        Texture2D TextBox;
        String username = "";
        Vector2 StartButton = new Vector2(680, 450);
        Vector2 InstructionStartButton = new Vector2(700, 950);
        Vector2 HiScores = new Vector2(680, 575);
        Vector2 Credits = new Vector2(680, 700);
        Vector2 SoundOnPos = new Vector2(600, 300);
        Vector2 SoundOffPos = new Vector2(1100, 300);
        SoundEffect Heartbeat;

        Texture2D Groepsfoto;
        Vector2 groepsfotoposition = new Vector2(700, 50);
        KeyboardState currKeyboardState;
        KeyboardState prevKeyboardState;

        int correctAnswers = 0;
        int currentUserId;

        State currState = State.main;
        /// <summary>
        /// Enum to point to the correct gamestate
        /// </summary>
        enum State
        {
            main,
            playing,
            highscores,
            credits,
            quiz,
            instructions
        }
        /// <summary>
        /// Basic defines and settings for the game are done here
        /// </summary>
        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Resolution.Init(ref graphics);
            Resolution.SetVirtualResolution(1920, 1080);
            int xRes = GraphicsDevice.DisplayMode.Width;
            int yRes = GraphicsDevice.DisplayMode.Height;
            Resolution.SetResolution(xRes, yRes, true);
            // quiz = new Quiz(Content);
            //beat = new Playing(this.Content); 
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            this.IsMouseVisible = true;



        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            PulsedLogo = this.Content.Load<Texture2D>("Pulsed");
            Button = this.Content.Load<Texture2D>("Button");
            font = Content.Load<SpriteFont>("Myfont");
            usernameFont = Content.Load<SpriteFont>("usernameFont");
            instructionfont = Content.Load<SpriteFont>("instructionfont");
            Groepsfoto = Content.Load<Texture2D>("GroepsFoto");
            SoundOn = Content.Load<Texture2D>("On");
            SoundOff = Content.Load<Texture2D>("Off");
            TextBox = Content.Load<Texture2D>("TextBox");
            highscoreFont = Content.Load<SpriteFont>("QuizSpriteFont");


            Resolution.SetVirtualResolution(1920, 1080);
            int xRes = graphics.GraphicsDevice.DisplayMode.Width;
            int yRes = graphics.GraphicsDevice.DisplayMode.Height;
            Resolution.SetResolution(xRes, yRes, false);

            Heartbeat = Content.Load<SoundEffect>("heartbeat");
            SoundEffectInstance instance = Heartbeat.CreateInstance();
            Heartbeat.Play();
            instance.IsLooped = true;


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

        }
        /// <summary>
        /// Handles the keypresses and game state changes for the main menu
        /// </summary>
        public void MainMenuKeys()
        {
            if (currKeyboardState.IsKeyDown(Keys.Enter) && currKeyboardState != prevKeyboardState)
            {
                currState = State.instructions;
                if (currKeyboardState.IsKeyDown(Keys.Space))
                {
                    currState = State.quiz;
                    quiz = new Quiz(Content);
                }

            }
            else if (currKeyboardState.IsKeyDown(Keys.F1) && currKeyboardState != prevKeyboardState)
            {
                currState = State.highscores;
            }
            else if (currKeyboardState.IsKeyDown(Keys.F12) && currKeyboardState != prevKeyboardState)
            {
                currState = State.credits;
            }




        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            currKeyboardState = Keyboard.GetState();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            switch (currState)
            {
                case State.main:
                    MainMenuKeys();
                    break;
                case State.playing:
                    beat.Update(gameTime);
                    if (beat.isGameOver())
                    {
                        Database conn = new Database();
                        conn.insertHighScore(currentUserId, beat.getFinalScore());
                        conn.close();
                        Console.WriteLine("Closed db connection after writing highscore");
                        beat = null;
                        currState = State.highscores;
                    }
                    break;
                case State.instructions:
                    username += GetChars();
                    if (currKeyboardState.IsKeyDown(Keys.Back) && currKeyboardState != prevKeyboardState && username.Length > 0)
                    {
                        username = username.Remove(username.Length - 1);
                    }
                    break;
                case State.highscores:
                    break;
                case State.credits:
                    break;
                case State.quiz:
                    quiz.Update(gameTime);
                    if (quiz.isQuizOver())
                    {
                        correctAnswers = quiz.getCorrectAnswers();
                        beat = new Playing(this.Content, correctAnswers, currentUserId);
                        currState = State.playing;
                    }
                    break;
                default:
                    break;
            }
            base.Update(gameTime);
            prevKeyboardState = currKeyboardState;
        }

        /// <summary>
        /// Handles the drawing of the main menu, expects it is called from within a spritebatch
        /// </summary>
        public void DrawMainMenu()
        {

            // spriteBatch.Draw(MovingRectangle, new Vector2(movingRectanglePosition, 790), Color.White);
            spriteBatch.Draw(PulsedLogo, LogoPosition, Color.White);
            spriteBatch.Draw(Button, StartButton, Color.White);
            spriteBatch.Draw(Button, HiScores, Color.White);
            spriteBatch.Draw(Button, Credits, Color.White);
            spriteBatch.DrawString(font, "Start (Enter)", new Vector2(930, 475), Color.Black);
            spriteBatch.DrawString(font, "Hiscores (F1)", new Vector2(930, 600), Color.Black);
            spriteBatch.DrawString(font, "Credits (F12)", new Vector2(930, 725), Color.Black);


        }
        /// <summary>
        /// Handles the drawing of the credits screen, expects to be called from within a spritebatch
        /// </summary>
        public void DrawCreditsScreen()
        {
            spriteBatch.Draw(Groepsfoto, groepsfotoposition, Color.White);
            spriteBatch.DrawString(font, "Donny Blomen", new Vector2(900, 500), Color.Black);
            spriteBatch.DrawString(font, "Max van Oostveen", new Vector2(900, 550), Color.Black);
            spriteBatch.DrawString(font, "Obrian McKenzie", new Vector2(900, 600), Color.Black);
            spriteBatch.DrawString(font, "Sjoerd Smits", new Vector2(900, 650), Color.Black);
            spriteBatch.DrawString(font, "Subhi Dweik", new Vector2(900, 700), Color.Black);

            if (currKeyboardState.IsKeyDown(Keys.Back))
            {
                currState = State.main;
            }
        }

        public void DrawInstructions()
        {
            spriteBatch.DrawString(instructionfont, "INSTRUCTIES", new Vector2(500, 150), Color.Black);
            spriteBatch.DrawString(instructionfont, "We beginnen met een quiz om je vaardigheden\nte testen\nOm een antwoord te geven druk je op\n1 = A\n2 = B\n3 = C \n \n\nNa de quiz begint de praktijk oefening\n\nVul hier uw naam in: ", new Vector2(500, 210), Color.Black);
            spriteBatch.Draw(TextBox, new Vector2(1100, 775), Color.White);
            spriteBatch.Draw(Button, InstructionStartButton, Color.White);

            spriteBatch.DrawString(usernameFont, username, new Vector2(1200, 825), Color.Black);
            spriteBatch.DrawString(font, "Start (Spatie)", new Vector2(950, 975), Color.Black);

            if (currKeyboardState.IsKeyDown(Keys.Space))
            {
                Database conn = new Database();
                conn.insertUser(username);
                currentUserId = (int) conn.getUserIdFromUserName(username);
                conn.close();
                currState = State.quiz;
                quiz = new Quiz(Content);
            }
        }


        public string GetChars()
        {
            string output = "";
            bool shift = (CheckControl(Keys.LeftShift) > 0 || CheckControl(Keys.RightShift) > 0);

            if (CheckControl(Keys.Space) == 1)
                output += " ";

            for (int i = (int)Keys.NumPad0; i <= (int)Keys.NumPad9; i++)
                if (CheckControl((Keys)i) == 1)
                    output += (char)(i - 48);

            if (CheckControl(Keys.Divide) == 1)
                output += '/';
            if (CheckControl(Keys.Multiply) == 1)
                output += '*';
            if (CheckControl(Keys.Subtract) == 1)
                output += '-';
            if (CheckControl(Keys.Add) == 1)
                output += '+';
            if (CheckControl(Keys.Decimal) == 1)
                output += '.';

            if (!shift)
            {
                for (int i = (int)Keys.D0; i <= (int)Keys.D9; i++)
                    if (CheckControl((Keys)i) == 1)
                        output += (char)i;

                for (int i = (int)Keys.A; i <= (int)Keys.Z; i++)
                    if (CheckControl((Keys)i) == 1)
                        output += (char)(i + 32);

                if (CheckControl(Keys.OemMinus) == 1)
                    output += '-';
                if (CheckControl(Keys.OemPlus) == 1)
                    output += '=';
                if (CheckControl(Keys.OemOpenBrackets) == 1)
                    output += '[';
                if (CheckControl(Keys.OemCloseBrackets) == 1)
                    output += ']';
                if (CheckControl(Keys.OemPipe) == 1)
                    output += '\\';
                if (CheckControl(Keys.OemSemicolon) == 1)
                    output += ';';
                if (CheckControl(Keys.OemQuotes) == 1)
                    output += '\'';
                if (CheckControl(Keys.OemComma) == 1)
                    output += ',';
                if (CheckControl(Keys.OemPeriod) == 1)
                    output += '.';
                if (CheckControl(Keys.OemQuestion) == 1)
                    output += '/';
            }
            else
            {
                for (int i = (int)Keys.A; i <= (int)Keys.Z; i++)
                    if (CheckControl((Keys)i) == 1)
                        output += (char)i;

                if (CheckControl(Keys.D0) == 1)
                    output += ')';
                if (CheckControl(Keys.D1) == 1)
                    output += '!';
                if (CheckControl(Keys.D2) == 1)
                    output += '@';
                if (CheckControl(Keys.D3) == 1)
                    output += '#';
                if (CheckControl(Keys.D4) == 1)
                    output += '$';
                if (CheckControl(Keys.D5) == 1)
                    output += '%';
                if (CheckControl(Keys.D6) == 1)
                    output += '^';
                if (CheckControl(Keys.D7) == 1)
                    output += '&';
                if (CheckControl(Keys.D8) == 1)
                    output += '*';
                if (CheckControl(Keys.D9) == 1)
                    output += '(';

                if (CheckControl(Keys.OemMinus) == 1)
                    output += '_';
                if (CheckControl(Keys.OemPlus) == 1)
                    output += '+';
                if (CheckControl(Keys.OemOpenBrackets) == 1)
                    output += '{';
                if (CheckControl(Keys.OemCloseBrackets) == 1)
                    output += '}';
                if (CheckControl(Keys.OemPipe) == 1)
                    output += '|';
                if (CheckControl(Keys.OemSemicolon) == 1)
                    output += ':';
                if (CheckControl(Keys.OemQuotes) == 1)
                    output += '"';
                if (CheckControl(Keys.OemComma) == 1)
                    output += '<';
                if (CheckControl(Keys.OemPeriod) == 1)
                    output += '>';
                if (CheckControl(Keys.OemQuestion) == 1)
                    output += '?';
            }

            return output;
        }

        private float CheckControl(Keys control)
        {
            float output = 0; // nothing 

            bool currentState = currKeyboardState.IsKeyDown(control);
            bool previousState = prevKeyboardState.IsKeyDown(control);

            if (currentState && !previousState)
                output = 1; // trigger 
            else if (currentState && previousState)
                output = 0.5f; // hold 
            else if (!currentState && previousState)
                output = -1; // release 

            return output;
        }

        public void DrawQuizScreen(SpriteBatch spriteBatch)
        {
            quiz.Draw(spriteBatch);

        }

        public void DrawPlayscreen(SpriteBatch spriteBatch)
        {
            if (beat == null)
            {
                beat = new Playing(this.Content, correctAnswers, currentUserId);
            }
            beat.Draw(spriteBatch);


            if (currKeyboardState.IsKeyDown(Keys.Back))
            {
                currState = State.main;
            }

        }


        public void DrawHighScore()
        {
            spriteBatch.DrawString(highscoreFont, "HIGHSCORES!", new Vector2(900, 200), Color.Black);
            Database conn = new Database();
            Dictionary<String, int> highscores = conn.getHighScores(20);
            conn.close();
            int currY = 250;
            foreach (KeyValuePair<string, int> currHighscore in highscores)
            {
                spriteBatch.DrawString(highscoreFont, currHighscore.Key.Remove(currHighscore.Key.Length - 1) + "- " + currHighscore.Value.ToString(), new Vector2(900, currY), Color.Black);
                currY += 31;
            }
            if (currKeyboardState.IsKeyDown(Keys.Back))
            {
                currState = State.main;
            }
        }

        /// <summary>
        /// Calls the correct draw function based on the state the game is in
        /// </summary>
        public void DrawCurrentState()
        {
            if (currState == State.main)
            {
                DrawMainMenu();
            }
            else if (currState == State.playing)
            {
                DrawPlayscreen(spriteBatch);
            }
            else if (currState == State.highscores)
            {
                DrawHighScore();
            }
            else if (currState == State.credits)
            {
                DrawCreditsScreen();
            }
            else if (currState == State.instructions)
            {
                DrawInstructions();
            }
            else if (currState == State.quiz)
            {

                DrawQuizScreen(spriteBatch);
            }


        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Orange);
            Resolution.BeginDraw();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            DrawCurrentState();
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
