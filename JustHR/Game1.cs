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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace JustHR
{
    public class Game1 : Game
    {
        Effect waveShader;
        Effect wrapShader;
        RenderTarget2D canvas;
        RenderTarget2D wrapCanvas;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont mainFont;
        private Drawer drawer;
        private Controller controller;
        private IScene currentScene;
        private long ticksSinceSceneStart;
        private int day;
        private Dictionary<Enum, SoundEffectInstance> soundEffects = new Dictionary<Enum, SoundEffectInstance>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            controller = new Controller();

            graphics.PreferredBackBufferWidth = Settings.WindowWidth;
            graphics.PreferredBackBufferHeight = Settings.WindowHeight;

            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1d / Settings.FPS);

            Window.AllowUserResizing = false;
            Window.AllowAltF4 = false;
            Window.Title = "JustHR";

            StartNewGame();
        }

        private void StartNewGame()
        {
            day = 27;

            Player.Professionality = 60;
            Player.Unity = 60;
            Player.Mentality = 60;
            Player.BossSatisfaction = 60;

            var buttons = new List<Button>();
            buttons.Add(new Button(ButtonEnum.StartButton, new Vector2(50, 450), new Vector2(220, 210), controller, () => {
                soundEffects[SoundsEnum.phone_btn_sound_2].Play();

                controller.ClearEvents();
                currentScene = new OfficeScene(day, controller, soundEffects);
                ticksSinceSceneStart = 0;
            }));
            Phone phone = new Phone(buttons);

            Menu menu = new Menu(phone, null);

            currentScene = new StreetScene(menu, soundEffects);
        }

        protected override void Initialize()
        {
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            canvas = new RenderTarget2D(GraphicsDevice, Settings.WindowWidth, Settings.WindowHeight);
            wrapCanvas = new RenderTarget2D(GraphicsDevice, Settings.WindowWidth, Settings.WindowHeight);

            #region sound load
            SoundEffect.MasterVolume = Settings.GlobalVolume; //volume
            soundEffects.Add(SoundsEnum.door_closing, Content.Load<SoundEffect>("Sounds/door_closing").CreateInstance());
            soundEffects.Add(SoundsEnum.door_opening, Content.Load<SoundEffect>("Sounds/door_opening").CreateInstance());
            soundEffects.Add(SoundsEnum.drink_coffee, Content.Load<SoundEffect>("Sounds/drink_coffee").CreateInstance());
            soundEffects.Add(SoundsEnum.mur_long, Content.Load<SoundEffect>("Sounds/mur_long").CreateInstance());
            soundEffects.Add(SoundsEnum.mur_long_with_a_pet_cat, Content.Load<SoundEffect>("Sounds/mur_long_with_a_pet_cat").CreateInstance());
            soundEffects.Add(SoundsEnum.mur_short, Content.Load<SoundEffect>("Sounds/mur_short").CreateInstance());
            soundEffects.Add(SoundsEnum.mur_super_short, Content.Load<SoundEffect>("Sounds/mur_super_short").CreateInstance());
            soundEffects.Add(SoundsEnum.phone_btn_sound_1, Content.Load<SoundEffect>("Sounds/phone_btn_sound_1").CreateInstance());
            soundEffects.Add(SoundsEnum.phone_btn_sound_2, Content.Load<SoundEffect>("Sounds/phone_btn_sound_2").CreateInstance());
            soundEffects.Add(SoundsEnum.phone_btn_sound_3, Content.Load<SoundEffect>("Sounds/phone_btn_sound_3").CreateInstance());
            soundEffects.Add(SoundsEnum.song_carol_of_the_bells, Content.Load<SoundEffect>("Sounds/song_carol_of_the_bells").CreateInstance());
            soundEffects.Add(SoundsEnum.song_last_christmas_cover, Content.Load<SoundEffect>("Sounds/song_last_christmas_cover").CreateInstance());
            soundEffects.Add(SoundsEnum.song_the_blizzard_is_ringing, Content.Load<SoundEffect>("Sounds/song_the_blizzard_is_ringing").CreateInstance());
            soundEffects.Add(SoundsEnum.sound_ambient, Content.Load<SoundEffect>("Sounds/sound_ambient").CreateInstance());
            soundEffects.Add(SoundsEnum.vc_closing, Content.Load<SoundEffect>("Sounds/vc_closing").CreateInstance());
            soundEffects.Add(SoundsEnum.vc_opening, Content.Load<SoundEffect>("Sounds/vc_opening").CreateInstance());
            soundEffects.Add(SoundsEnum.voice_long, Content.Load<SoundEffect>("Sounds/voice_long").CreateInstance());
            soundEffects.Add(SoundsEnum.voice_long_raised_tone, Content.Load<SoundEffect>("Sounds/voice_long_raised_tone").CreateInstance());
            soundEffects.Add(SoundsEnum.voice_short, Content.Load<SoundEffect>("Sounds/voice_short").CreateInstance());
            soundEffects.Add(SoundsEnum.cat_hisses, Content.Load<SoundEffect>("Sounds/cat_hisses").CreateInstance());
            soundEffects.Add(SoundsEnum.six_steps, Content.Load<SoundEffect>("Sounds/six_steps").CreateInstance());
            soundEffects.Add(SoundsEnum.step_1, Content.Load<SoundEffect>("Sounds/step_1").CreateInstance());
            soundEffects.Add(SoundsEnum.step_2, Content.Load<SoundEffect>("Sounds/step_2").CreateInstance());
            soundEffects.Add(SoundsEnum.tap_a_clock, Content.Load<SoundEffect>("Sounds/tap_a_clock").CreateInstance());
            soundEffects.Add(SoundsEnum.tick_1, Content.Load<SoundEffect>("Sounds/tick_1").CreateInstance());
            soundEffects.Add(SoundsEnum.tick_2, Content.Load<SoundEffect>("Sounds/tick_2").CreateInstance());
            soundEffects.Add(SoundsEnum.transition_sound, Content.Load<SoundEffect>("Sounds/transition_sound").CreateInstance());
            soundEffects.Add(SoundsEnum.rage, Content.Load<SoundEffect>("Sounds/Rage").CreateInstance());

            foreach (KeyValuePair<Enum, SoundEffectInstance> entry in soundEffects)
            {
                entry.Value.Volume = Settings.EffectsVolume;
                if ((SoundsEnum)entry.Key == SoundsEnum.song_the_blizzard_is_ringing)
                    entry.Value.Volume = 0.7f;
            }
            #endregion

            //load fonts
            var fonts = new Dictionary<Enum, SpriteFont>();
            fonts.Add(FontsEnum.Main, Content.Load<SpriteFont>("main_font"));
            fonts.Add(FontsEnum.Pixel, Content.Load<SpriteFont>("pixel_font"));

            waveShader = Content.Load<Effect>("WaveShader");
            wrapShader = Content.Load<Effect>("WrapShader");

            #region sprite load
            var sprites = new Dictionary<Enum, Texture2D>();
            sprites.Add(BackgroundsEnum.Street, Content.Load<Texture2D>("Sprites/street"));
            sprites.Add(BackgroundsEnum.Office, Content.Load<Texture2D>("Sprites/office_background"));
            sprites.Add(SpriteEnum.Phone, Content.Load<Texture2D>("Sprites/phone"));

            sprites.Add(ButtonEnum.AcceptButton, Content.Load<Texture2D>("Sprites/accept_button"));
            sprites.Add(ButtonEnum.JobResponsibilitiesButton, Content.Load<Texture2D>("Sprites/job_jesponsibilities_button"));
            sprites.Add(ButtonEnum.RecallButton, Content.Load<Texture2D>("Sprites/recall_button"));
            sprites.Add(ButtonEnum.RejectButton, Content.Load<Texture2D>("Sprites/rejected_button"));
            sprites.Add(ButtonEnum.StartButton, Content.Load<Texture2D>("Sprites/start_button"));
            sprites.Add(ButtonEnum.StartDayButton, Content.Load<Texture2D>("Sprites/start_day"));


            sprites.Add(SpriteEnum.TextPlace, Content.Load<Texture2D>("Sprites/text_place"));
            sprites.Add(SpriteEnum.NextPageArrow, Content.Load<Texture2D>("Sprites/next_page_arrow"));
            sprites.Add(SpriteEnum.Pixel, Content.Load<Texture2D>("Sprites/pixel"));
            sprites.Add(SpriteEnum.Character, Content.Load<Texture2D>("Sprites/Characters/dude"));
            sprites.Add(SpriteEnum.SittingCharacter, Content.Load<Texture2D>("Sprites/Characters/sitting_dude"));

            sprites.Add(SceneObjectSpriteEnum.BackChair, Content.Load<Texture2D>("Sprites/back_chair"));
            sprites.Add(SceneObjectSpriteEnum.Calendar, Content.Load<Texture2D>("Sprites/calendar"));
            sprites.Add(SceneObjectSpriteEnum.ChristmasTree, Content.Load<Texture2D>("Sprites/christmas_tree"));
            sprites.Add(SceneObjectSpriteEnum.Clock, Content.Load<Texture2D>("Sprites/clock"));
            sprites.Add(SceneObjectSpriteEnum.ClockArrow, Content.Load<Texture2D>("sprites/clock_arrow"));
            sprites.Add(SceneObjectSpriteEnum.Cooler, Content.Load<Texture2D>("Sprites/cooler"));
            sprites.Add(SceneObjectSpriteEnum.Door, Content.Load<Texture2D>("Sprites/door"));
            sprites.Add(SceneObjectSpriteEnum.Exit, Content.Load<Texture2D>("Sprites/exit"));
            sprites.Add(SceneObjectSpriteEnum.OpenedDoor, Content.Load<Texture2D>("Sprites/opened_door"));
            sprites.Add(SceneObjectSpriteEnum.SideChair, Content.Load<Texture2D>("Sprites/side_chair"));
            sprites.Add(SceneObjectSpriteEnum.Table, Content.Load<Texture2D>("Sprites/desk"));
            sprites.Add(SceneObjectSpriteEnum.Whiteboard, Content.Load<Texture2D>("Sprites/whiteboard"));
            sprites.Add(SceneObjectSpriteEnum.CurriculumVitae, Content.Load<Texture2D>("Sprites/table_cv"));
            sprites.Add(SceneObjectSpriteEnum.ExpandedCurriculumVitae, Content.Load<Texture2D>("sprites/big_cv"));
            sprites.Add(CatState.Phone, Content.Load<Texture2D>("sprites/cat_sits"));
            sprites.Add(CatState.Cooler, Content.Load<Texture2D>("sprites/cooler_cat"));
            sprites.Add(CatState.Chair, Content.Load<Texture2D>("sprites/nasty_cat"));
            sprites.Add(CatState.AngryPhone, Content.Load<Texture2D>("sprites/angry_cat"));
            sprites.Add(CatState.AngryCooler, Content.Load<Texture2D>("sprites/angry_cooler_cat"));
            sprites.Add(CatState.AngryChair, Content.Load<Texture2D>("sprites/angry_chair_cat"));
            #endregion

            #region sprite tile set load
            var tileSets = new Dictionary<Enum, SpriteListTileMap>();
            tileSets.Add(GarlandAnimationEnum.BlinkAnimation, new SpriteListTileMap(new List<Texture2D> {
                Content.Load<Texture2D>("Sprites/lights1"),
                Content.Load<Texture2D>("Sprites/lights2")
            }));
            tileSets.Add(TreeAnimationEnum.Blink, new SpriteListTileMap(new List<Texture2D> {
                Content.Load<Texture2D>("Sprites/tree1"),
                Content.Load<Texture2D>("Sprites/tree2")
            }));
            tileSets.Add(ClothesEnum.Clothes, new SpriteListTileMap(new List<Texture2D> {
                Content.Load<Texture2D>("Sprites/Characters/scl1"),
                Content.Load<Texture2D>("Sprites/Characters/scl2"),
                Content.Load<Texture2D>("Sprites/Characters/scl3"),
                Content.Load<Texture2D>("Sprites/Characters/scl4"),
                Content.Load<Texture2D>("Sprites/Characters/scl5"),
                Content.Load<Texture2D>("Sprites/Characters/scl6"),
            }));
            tileSets.Add(ClothesEnum.SittingClothes, new SpriteListTileMap(new List<Texture2D> {
                Content.Load<Texture2D>("Sprites/Characters/cl1"),
                Content.Load<Texture2D>("Sprites/Characters/cl2"),
                Content.Load<Texture2D>("Sprites/Characters/cl3"),
                Content.Load<Texture2D>("Sprites/Characters/cl4"),
                Content.Load<Texture2D>("Sprites/Characters/cl5"),
                Content.Load<Texture2D>("Sprites/Characters/cl6"),
            }));
            tileSets.Add(ClothesEnum.Hairs, new SpriteListTileMap(new List<Texture2D> {
                Content.Load<Texture2D>("Sprites/Characters/h1"),
                Content.Load<Texture2D>("Sprites/Characters/h2"),
                Content.Load<Texture2D>("Sprites/Characters/h3"),
                Content.Load<Texture2D>("Sprites/Characters/h4"),
            }));
            tileSets.Add(ClothesEnum.Eyes, new SpriteListTileMap(new List<Texture2D> {
                Content.Load<Texture2D>("Sprites/Characters/a1"),
                Content.Load<Texture2D>("Sprites/Characters/a2"),
                Content.Load<Texture2D>("Sprites/Characters/a3"),
                Content.Load<Texture2D>("Sprites/Characters/a4"),
                Content.Load<Texture2D>("Sprites/Characters/a5"),
            }));
            tileSets.Add(ClothesEnum.Accessories, new SpriteListTileMap(new List<Texture2D> {
                Content.Load<Texture2D>("Sprites/Characters/fa1"),
                Content.Load<Texture2D>("Sprites/Characters/fa2"),
            }));
            #endregion

            drawer = new Drawer(spriteBatch, sprites, tileSets, null, fonts, soundEffects);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            controller.TriggerControlEvents();

            ticksSinceSceneStart++;
            if (currentScene is OfficeScene)
            {
                OfficeScene ofScene = (OfficeScene)currentScene;

                if (ofScene.IsGameOver)
                {
                    StartNewGame();
                }

                if (ticksSinceSceneStart % (Settings.FPS * Settings.DayLengthInSec) == 0) //тик часа
                    if (!ofScene.Objects.Character.Traits.IsBoss)
                        ofScene.NextHour();
                if (ofScene.Hour == 18) //конец дня
                    ofScene.EndDay();

                if (ofScene.IsDayEnded)
                {
                    controller.ClearEvents();
                    var buttons = new List<Button>();
                    buttons.Add(new Button(ButtonEnum.StartDayButton, new Vector2(50, 450), new Vector2(220, 200), controller, () => {
                        controller.ClearEvents();
                        day++;

                        currentScene = new OfficeScene(day, controller, soundEffects);
                        ticksSinceSceneStart = 0;
                    }));
                    Phone phone = new Phone(buttons);

                    Player.Mentality = Math.Max(60, Player.Mentality);

                    Menu menu = new Menu(phone, null);

                    currentScene = new StreetScene(menu, soundEffects);

                    if (soundEffects[SoundsEnum.rage].State == SoundState.Playing)
                    {
                        soundEffects[SoundsEnum.rage].Stop();
                        soundEffects[SoundsEnum.sound_ambient].Play();
                    }
                }

                if (Player.Mentality < 25 && soundEffects[SoundsEnum.rage].State == SoundState.Stopped)
                {
                    soundEffects[SoundsEnum.song_carol_of_the_bells].Stop();
                    soundEffects[SoundsEnum.song_last_christmas_cover].Stop();
                    soundEffects[SoundsEnum.song_the_blizzard_is_ringing].Stop();
                    soundEffects[SoundsEnum.sound_ambient].Stop();
                    soundEffects[SoundsEnum.rage].Play();
                }
                if (Player.Mentality > 25 && soundEffects[SoundsEnum.rage].State == SoundState.Playing)
                {
                    soundEffects[SoundsEnum.rage].Stop();
                }
            }
            else
            {
                (currentScene as StreetScene).DoTick();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(wrapCanvas);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);

            if (currentScene is OfficeScene ofScene)
                drawer.DrawSelectedObject(ofScene);

            spriteBatch.End();


            float elapsedWeight = 0;
            if (currentScene is OfficeScene)
                elapsedWeight = 1 - MathHelper.Clamp(Player.Mentality / 100f, 0, 0.25f) / 0.25f;
            waveShader.Parameters["ElapsedTime"].SetValue((float)gameTime.TotalGameTime.TotalSeconds * 1.5f);
            waveShader.Parameters["ElapsedWeight"].SetValue(elapsedWeight);
            GraphicsDevice.SetRenderTarget(canvas);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);

            drawer.DrawScene(currentScene);

            spriteBatch.End();


            wrapShader.Parameters["Distance"].SetValue(new Vector2(1f / 300, 1f / 300));
            wrapShader.Parameters["WrapColor"].SetValue(new Vector4(150 / 255f, 220 / 255f, 120 / 255f, 1f));
            spriteBatch.Begin(effect: wrapShader, samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack, blendState: BlendState.NonPremultiplied);

            spriteBatch.Draw(wrapCanvas, new Vector2(0, 0), Color.White);

            spriteBatch.End();


            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(effect: waveShader, samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);

            spriteBatch.Draw(canvas, new Vector2(0, 0), Color.White);

            spriteBatch.End();


            spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);

            drawer.DrawMenu(currentScene.Menu, currentScene);

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
