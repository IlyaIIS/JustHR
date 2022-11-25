using JustHR.Classes;
using JustHR.Classes.Basic;
using JustHR.Classes.SceneObjects;
using JustHR.Classes.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ShaderPack.Classes;
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

            var sceneObjects = new List<ISceneObject>();
            sceneObjects.Add(new Whiteboard());
            sceneObjects.Add(new Door());
            sceneObjects.Add(new ChristmasTree());
            sceneObjects.Add(new Calendar());
            sceneObjects.Add(new Clock());
            sceneObjects.Add(new Cooler());
            sceneObjects.Add(new BackChair());
            sceneObjects.Add(new SideChair());
            sceneObjects.Add(null);
            sceneObjects.Add(new Table());

            var buttons = new List<Button>();
            buttons.Add(new Button(ButtonEnum.HeirForHR,new Vector2(50, 480), new Vector2(200, 70), controller, () => { currentScene = new OfficeScene(27, sceneObjects, controller); }));
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

            var sprites = new Dictionary<Enum, Texture2D>();
            sprites.Add(BackgroundsEnum.Street, Content.Load<Texture2D>("Sprites/street"));
            sprites.Add(BackgroundsEnum.Office, Content.Load<Texture2D>("Sprites/office_background"));
            sprites.Add(SpriteEnum.Phone, Content.Load<Texture2D>("Sprites/phone"));

            sprites.Add(SceneObjectsEnum.BackChair, Content.Load<Texture2D>("Sprites/back_chair"));
            sprites.Add(SceneObjectsEnum.Calendar, Content.Load<Texture2D>("Sprites/calender_clock"));
            sprites.Add(SceneObjectsEnum.ChristmasTree, Content.Load<Texture2D>("Sprites/christmas_tree"));
            sprites.Add(SceneObjectsEnum.Clock, Content.Load<Texture2D>("Sprites/calender_clock"));
            sprites.Add(SceneObjectsEnum.Cooler, Content.Load<Texture2D>("Sprites/cooler"));
            sprites.Add(SceneObjectsEnum.CurriculumVitae, Content.Load<Texture2D>("Sprites/small_cv"));
            sprites.Add(SceneObjectsEnum.Door, Content.Load<Texture2D>("Sprites/door"));
            sprites.Add(SceneObjectsEnum.SideChair, Content.Load<Texture2D>("Sprites/side_chair"));
            sprites.Add(SceneObjectsEnum.Table, Content.Load<Texture2D>("Sprites/table"));
            sprites.Add(SceneObjectsEnum.Whiteboard, Content.Load<Texture2D>("Sprites/whiteboard"));
            drawer = new Drawer(spriteBatch, sprites, fonts);
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
            drawer.DrawPhone(currentScene.Menu.Phone);
            // TODO
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }

    enum FontsEnum
    {
        Main,
    }

    enum TestEnum
    {
        Main,
    }
}
