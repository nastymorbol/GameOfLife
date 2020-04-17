using System.Linq;
using System.Reflection;
using System.Resources;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpriteFontPlus;
namespace GameOfLife
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Rectangle _rectangle;
        private DynamicSpriteFont _spriteFont;
        private Color[] _data;
        private Texture2D _rectTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);                        

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.ApplyChanges();

            _rectangle = new Rectangle(10,10,20,20);
            _data = new Color[_rectangle.Width * _rectangle.Height];
            _rectTexture = new Texture2D(GraphicsDevice, _rectangle.Width, _rectangle.Height);
            for (int i = 0; i < _data.Length; ++i) 
                _data[i] = Color.White;

            _rectTexture.SetData(_data);
            
            var resNames = Assembly.GetEntryAssembly().GetManifestResourceNames();
            var resStream = Assembly.GetEntryAssembly().GetManifestResourceStream(resNames.First(n => n.EndsWith("space_age.ttf")));
            var resBuffer = new byte[resStream.Length];
            resStream.Read(resBuffer, 0, resBuffer.Length);
            resStream.Close();

            //_spriteFont = SpriteFontPlus.DynamicSpriteFont.FromTtf(File.ReadAllBytes(@"Fonts/space_age.ttf"), 10);
            _spriteFont = SpriteFontPlus.DynamicSpriteFont.FromTtf(resBuffer, 10);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            // TODO: Add your update logic here

            var stickX = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;
            var stickY = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y;

            if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed )
                _rectangle = new Rectangle(_rectangle.X - (10), _rectangle.Y, _rectangle.Width, _rectangle.Height);
            else if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed )
                _rectangle = new Rectangle(_rectangle.X + (10), _rectangle.Y, _rectangle.Width, _rectangle.Height);

            else if (stickX != 0 || stickY != 0)
                _rectangle = new Rectangle(_rectangle.X + (int)(stickX * 50), _rectangle.Y - (int)(stickY * 50), _rectangle.Width, _rectangle.Height);
            
            if(_rectangle.X < 0)
                _rectangle = new Rectangle(0, _rectangle.Y, _rectangle.Width, _rectangle.Height);
            if(_rectangle.Y < 0)
                _rectangle = new Rectangle(_rectangle.X, 0, _rectangle.Width, _rectangle.Height);

            if(_rectangle.X > GraphicsDevice.Viewport.Width - _rectangle.Width)
                _rectangle = new Rectangle(GraphicsDevice.Viewport.Width - _rectangle.Width, _rectangle.Y, _rectangle.Width, _rectangle.Height);
            if(_rectangle.Y > GraphicsDevice.Viewport.Height - _rectangle.Height)
                _rectangle = new Rectangle(_rectangle.X, GraphicsDevice.Viewport.Height - _rectangle.Height, _rectangle.Width, _rectangle.Height);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here


            var position = new Vector2(_rectangle.Left, _rectangle.Top);

            _spriteBatch.Begin();

            _spriteBatch.Draw(_rectTexture, position, Color.White);
            _spriteFont.Size = 18;
            _spriteBatch.DrawString(_spriteFont, "WOW", new Vector2(1, 1), Color.Black);
            _spriteFont.Size = 12;
            _spriteBatch.DrawString(_spriteFont, "Coole Sache", position, Color.Black, new Vector2(2,2));
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
