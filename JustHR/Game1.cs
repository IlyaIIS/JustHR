using JustHR.Classes;
using JustHR.Classes.Basic;
using JustHR.Classes.SceneObjects;
using JustHR.Classes.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace JustHR
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont mainFont;
        private Drawer drawer;
        private Controller controller;
        private IScene currentScene;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            controller = new Controller();

            graphics.PreferredBackBufferWidth = Settings.WindowWidth;
            graphics.PreferredBackBufferHeight = Settings.WindowHeight;

            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1d / 30d);

            Window.AllowUserResizing = false;
            Window.AllowAltF4 = false;
            Window.Title = "JustHR";

            //Window.IsBorderless = true;


            var sceneObjects = new List<ISceneObject>();
            sceneObjects.Add(new Whiteboard());
            sceneObjects.Add(new Garland());
            sceneObjects.Add(new Door());
            sceneObjects.Add(new ChristmasTree());
            sceneObjects.Add(new Calendar());
            sceneObjects.Add(new Clock());
            sceneObjects.Add(new Cooler());
            sceneObjects.Add(new BackChair());
            sceneObjects.Add(new SideChair());
            sceneObjects.Add(null);
            sceneObjects.Add(new Table());
            sceneObjects.Add(new CurriculumVitae(controller));

            var buttons = new List<Button>();
            buttons.Add(new Button(ButtonEnum.HeirForHR,new Vector2(50, 480), new Vector2(200, 70), controller, () => {
                controller.ClearEvents();
                currentScene = new OfficeScene(27, sceneObjects, controller); 
            }));
            Phone phone = new Phone(buttons);

            Menu menu = new Menu(phone, null);

            currentScene = new StreetScene(menu);
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            var fonts = new Dictionary<Enum, SpriteFont>();
            fonts.Add(FontsEnum.Main, Content.Load<SpriteFont>("main_font"));
            fonts.Add(FontsEnum.Pixel, Content.Load<SpriteFont>("pixel_font"));

            var sprites = new Dictionary<Enum, Texture2D>();
            sprites.Add(BackgroundsEnum.Street, Content.Load<Texture2D>("Sprites/street"));
            sprites.Add(BackgroundsEnum.Office, Content.Load<Texture2D>("Sprites/office_background"));
            sprites.Add(SpriteEnum.Phone, Content.Load<Texture2D>("Sprites/phone"));
            sprites.Add(SpriteEnum.TextPlace, Content.Load<Texture2D>("Sprites/text_place"));
            sprites.Add(SpriteEnum.NextPageArrow, Content.Load<Texture2D>("Sprites/next_page_arrow"));

            sprites.Add(SceneObjectSpriteEnum.BackChair, Content.Load<Texture2D>("Sprites/back_chair"));
            sprites.Add(SceneObjectSpriteEnum.Calendar, Content.Load<Texture2D>("Sprites/calendar_clock"));
            sprites.Add(SceneObjectSpriteEnum.ChristmasTree, Content.Load<Texture2D>("Sprites/christmas_tree"));
            sprites.Add(SceneObjectSpriteEnum.Clock, Content.Load<Texture2D>("Sprites/calendar_clock"));
            sprites.Add(SceneObjectSpriteEnum.Cooler, Content.Load<Texture2D>("Sprites/cooler"));
            sprites.Add(SceneObjectSpriteEnum.Door, Content.Load<Texture2D>("Sprites/door"));
            sprites.Add(SceneObjectSpriteEnum.SideChair, Content.Load<Texture2D>("Sprites/side_chair"));
            sprites.Add(SceneObjectSpriteEnum.Table, Content.Load<Texture2D>("Sprites/table"));
            sprites.Add(SceneObjectSpriteEnum.Whiteboard, Content.Load<Texture2D>("Sprites/whiteboard"));
            sprites.Add(SceneObjectSpriteEnum.CurriculumVitae, Content.Load<Texture2D>("Sprites/table_cv"));
            sprites.Add(SceneObjectSpriteEnum.ExpandedCurriculumVitae, Content.Load<Texture2D>("sprites/big_cv"));

            var tileSets = new Dictionary<Enum, SpriteListTileMap>();
            tileSets.Add(GarlandAnimationEnum.BlinkAnimation, new SpriteListTileMap(new List<Texture2D> {
                Content.Load<Texture2D>("Sprites/lights1"),
                Content.Load<Texture2D>("Sprites/lights2")
            }));

            drawer = new Drawer(spriteBatch, sprites, tileSets, fonts);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            controller.TriggerControlEvents();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            drawer.DrawScene(currentScene);
            drawer.DrawMenu(currentScene.Menu);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    enum FontsEnum
    {
        Main,
        Pixel,
    }

    enum TestEnum
    {
        Main,
    }
}
