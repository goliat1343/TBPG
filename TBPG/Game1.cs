using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Xml;

namespace TBPG
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int _ScreenWidth = 800;
        int _ScreenHeight = 800;


        FemaleZombie femaleZombie;

        // This is a texture we can render.
        Texture2D textureMilena;
        Texture2D textureShield;

        // Set the coordinates to draw the sprite at.
        Vector2 spritePosition = Vector2.Zero;

        // Store some information about the sprite's motion.
        Vector2 spriteSpeedR = new Vector2(70, 0);
        Vector2 spriteSpeedL = new Vector2(-70, 0);
        Vector2 spriteSpeedU = new Vector2(0, -70);
        Vector2 spriteSpeedD = new Vector2(0, 70);

        int frame = 0, maxFrame = 8, animationSpeed = 1;
        int direction = 0;  //UNDONE co to ma robic?

        bool walk = false;
        bool shielded = false;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = _ScreenHeight;
            graphics.PreferredBackBufferWidth = _ScreenWidth;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textureMilena = Content.Load<Texture2D>("milena");
            textureShield = Content.Load<Texture2D>("shield");

            femaleZombie = new FemaleZombie();
            femaleZombie.tex = Content.Load<Texture2D>("zombie_female");
            femaleZombie.load(GraphicsDevice, @"SpriteXMLs\zombie_female.xml",
                    GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            int MaxX, MinX, MaxY, MinY;


            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            if (Keyboard.GetState().IsKeyDown(Keys.D)
                || Keyboard.GetState().IsKeyDown(Keys.A)
                || Keyboard.GetState().IsKeyDown(Keys.W)
                || Keyboard.GetState().IsKeyDown(Keys.S))
            {
                walk = true;

                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    direction = 0;
                    // Move the sprite right by speed, scaled by elapsed time.
                    spritePosition += spriteSpeedR * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    direction = 1;
                    // Move the sprite left by speed, scaled by elapsed time.
                    spritePosition += spriteSpeedL * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    direction = 2;
                    spritePosition += spriteSpeedU * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    direction = 3;
                    spritePosition += spriteSpeedD * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }



                //int MaxX = graphics.GraphicsDevice.Viewport.Width - myTexture.Width;
                MaxX = graphics.GraphicsDevice.Viewport.Width - 64;
                MinX = 0;
                //int MaxY = graphics.GraphicsDevice.Viewport.Height - myTexture.Height;
                MaxY = graphics.GraphicsDevice.Viewport.Height - 64;
                MinY = 0;


                // Check for bounce.
                if (spritePosition.X > MaxX)
                {
                    //spriteSpeedR.X *= -1;
                    spritePosition.X = MaxX;
                    //direction = 1;
                    walk = false;
                }

                else if (spritePosition.X < MinX)
                {
                    //spriteSpeedR.X *= -1;
                    spritePosition.X = MinX;
                    //direction = 0;
                    walk = false;
                }

                if (spritePosition.Y > MaxY)
                {
                    //spriteSpeedR.Y *= -1;
                    spritePosition.Y = MaxY;
                    walk = false;
                }

                else if (spritePosition.Y < MinY)
                {
                    //spriteSpeedR.Y *= -1;
                    spritePosition.Y = MinY;
                    walk = false;
                }
            }
            else walk = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Space)) shielded = true;
            else shielded = false;



            femaleZombie.move(gameTime);


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw the sprite.
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            if (walk)
            {
                if (frame == maxFrame)
                    frame = 0;
                else if (animationSpeed == 2) frame++;

                if (animationSpeed == 2)
                    animationSpeed = 0;
                else animationSpeed++;


                if (direction == 0)
                    spriteBatch.Draw(textureMilena, spritePosition, new Rectangle(frame * 64, 704, 64, 64), Color.White);
                else if (direction == 1)
                    spriteBatch.Draw(textureMilena, spritePosition, new Rectangle(frame * 64, 576, 64, 64), Color.White);
                else if (direction == 2)
                    spriteBatch.Draw(textureMilena, spritePosition, new Rectangle(frame * 64, 8 * 64, 64, 64), Color.White);
                else
                    spriteBatch.Draw(textureMilena, spritePosition, new Rectangle(frame * 64, 10 * 64, 64, 64), Color.White);

            }
            else spriteBatch.Draw(textureMilena, spritePosition, new Rectangle(frame * 64, 640, 64, 64), Color.White);

            if (shielded)
                spriteBatch.Draw(textureShield, spritePosition, Color.White);






            //spriteBatch.Draw(femaleZombie.texDead[0], new Rectangle(200, 200, 57, 76), Color.White);
            spriteBatch.Draw(femaleZombie.texAttack[1], femaleZombie.position, Color.White);

            spriteBatch.DrawString(Content.Load<SpriteFont>("Times New Roman"),
                gameTime.TotalGameTime.TotalSeconds.ToString(),
                new Vector2(100, 100),
                Color.Black);


            spriteBatch.End();

            base.Draw(gameTime);
        }


    }

    /// <summary>
    /// 
    /// </summary>
    class FemaleZombie
    {
        public Texture2D tex;
        public List<Texture2D> texIdle, texWalk, texAttack, texDead;
        public int texWidth, texHeight;
        public int texCurrent = 0;
        public XmlDocument texXml;

        public Vector2 position;
        public Vector2 speedCurrent, speedT, speedD, speedR, speedL;

        int screenWidth, screenHeight;

        bool directionChanged = true;

        public FemaleZombie()
        {
            texIdle = new List<Texture2D>();
            texWalk = new List<Texture2D>();
            texAttack = new List<Texture2D>();
            texDead = new List<Texture2D>();

            position = new Vector2(200, 200);
            speedT = new Vector2(0, 50);
            speedD = new Vector2(0, -50);
            speedR = new Vector2(50, 0);
            speedL = new Vector2(-50, 0);

            speedCurrent = speedL;
        }

        public void load(GraphicsDevice g, string texture2DxmlPath, int sWidth, int sHeight)
        {

            int x, y, width, height;
            texXml = new XmlDocument();
            texXml.Load(texture2DxmlPath);




            screenWidth = sWidth;
            screenHeight = sHeight;



            Color[] data;

            XmlNodeList nodeList = texXml.DocumentElement.SelectNodes("/TextureAtlas/SubTexture");


            int.TryParse(nodeList[0].Attributes["width"].Value, out texWidth);
            int.TryParse(nodeList[0].Attributes["height"].Value, out texHeight);


            foreach (XmlNode n in nodeList)
            {
                int.TryParse(n.Attributes["x"].Value, out x);
                int.TryParse(n.Attributes["y"].Value, out y);
                int.TryParse(n.Attributes["width"].Value, out width);
                int.TryParse(n.Attributes["height"].Value, out height);

                data = new Color[width * height];
                tex.GetData(0, new Rectangle(x, y, width, height), data, 0, data.Length);

                if (n.Attributes["name"].Value.Contains("Attack"))
                {
                    texAttack.Add(new Texture2D(g, width, height));
                    //texAttack.Last().SetData(data);
                    texAttack[texAttack.Count - 1].SetData<Color>(data);
                }
                else if (n.Attributes["name"].Value.Contains("Dead"))
                {
                    texDead.Add(new Texture2D(g, width, height));
                    texDead[texDead.Count - 1].SetData<Color>(data);
                }
                else if (n.Attributes["name"].Value.Contains("Idle"))
                {
                    texIdle.Add(new Texture2D(g, width, height));
                    texIdle[texIdle.Count - 1].SetData<Color>(data);
                }
                else if (n.Attributes["name"].Value.Contains("Walk"))
                {
                    texWalk.Add(new Texture2D(g, width, height));
                    texWalk[texWalk.Count - 1].SetData<Color>(data);
                }
            }
        }

        internal void move(GameTime gameTime)
        {

            //int MaxX = graphics.GraphicsDevice.Viewport.Width - myTexture.Width;
            int MaxX = screenWidth - texWidth;
            int MinX = 0;
            //int MaxY = graphics.GraphicsDevice.Viewport.Height - myTexture.Height;
            int MaxY = screenHeight - texHeight;
            int MinY = 0;


            //TODO zmienic na jakich warunkach zombie ma zmieniac kierunek ruchu
            if (!directionChanged && gameTime.TotalGameTime.Seconds % 2 == 0)
            {
                switch (new Random().Next(4))
                {
                    case 0:
                        speedCurrent = speedT;
                        break;
                    case 1:
                        speedCurrent = speedD;
                        break;
                    case 2:
                        speedCurrent = speedR;
                        break;
                    case 3:
                        speedCurrent = speedL;
                        break;
                }
                directionChanged = true;
            }
            else if (gameTime.TotalGameTime.Seconds % 2 != 0)
            {
                directionChanged = false;
            }

            position += speedCurrent * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (position.X < MinX || position.X > MaxX || position.Y < MinY || position.Y > MaxY)
                speedCurrent *= -1;

        }

    }
}





