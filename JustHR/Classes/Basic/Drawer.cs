using JustHR.Classes.Interface;
using JustHR.Classes.SceneObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace JustHR.Classes.Basic
{
    /// <summary>
    /// Содержит функции для отрисовки игровых объектов.
    /// </summary>
    class Drawer
    {
        Dictionary<Enum, SpriteFont> fonts;
        Dictionary<Enum, Texture2D> sprites;
        Dictionary<Enum, SpriteListTileMap> spriteTileMaps;
        Dictionary<Enum, TileSetTileMap> tileMap;
        SpriteBatch spriteBatch;
        Dictionary<Enum, SoundEffectInstance> soundEffects;
        SoundsEnum currentSong;


        public Drawer(SpriteBatch spriteBatch , Dictionary<Enum, Texture2D> sprites,
            Dictionary<Enum, SpriteListTileMap> spriteTileMaps, Dictionary<Enum, TileSetTileMap> tileMap, 
            Dictionary<Enum, SpriteFont> fonts, Dictionary<Enum, SoundEffectInstance> soundEffects)
        {
            this.soundEffects = soundEffects;
            this.tileMap = tileMap;
            this.spriteBatch = spriteBatch;
            this.sprites = sprites;
            this.spriteTileMaps = spriteTileMaps;
            this.fonts = fonts;
            this.currentSong = SoundsEnum.song_last_christmas_cover;
        }

        public void DrawScene(IScene scene)
        {
            spriteBatch.Draw(sprites[scene.Background], new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None,0.000f);

            if(scene.Background == BackgroundsEnum.Office)
            {

                if(soundEffects[currentSong].State == SoundState.Stopped && Settings.Mentality >= 25)
                {
                    soundEffects[SoundsEnum.sound_ambient].Stop();
                    if (currentSong == SoundsEnum.song_last_christmas_cover)
                        currentSong = SoundsEnum.song_the_blizzard_is_ringing;
                    else if (currentSong == SoundsEnum.song_the_blizzard_is_ringing)
                        currentSong = SoundsEnum.song_last_christmas_cover;
                    else 
                    {
                        //
                    }
                    soundEffects[currentSong].Play();
                }
            }
            else
            {
                if(soundEffects[currentSong].State == SoundState.Playing)
                {
                    soundEffects[currentSong].Stop();
                }
                if (soundEffects[SoundsEnum.sound_ambient].State == SoundState.Stopped)
                {
                    soundEffects[SoundsEnum.sound_ambient].Play();
                }
            }

            for (int i = 0; i < scene.Objects.Count; i++)
            {
                ISceneObject obj = scene.Objects[i];
                if (obj != null)
                {
                    Type type = obj.GetType();
                    if (type == typeof(BackChair))
                    {
                        spriteBatch.Draw(sprites[SceneObjectSpriteEnum.BackChair], new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.017f);
                    }
                    else if (type == typeof(Calendar))
                    {
                        spriteBatch.Draw(sprites[SceneObjectSpriteEnum.Calendar], new Vector2(460, -90), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.002f);
                        spriteBatch.DrawString(fonts[FontsEnum.Pixel], ((OfficeScene)scene).Day.ToString(), new Vector2(450+ 460, 220-90), Color.Black, 0, Vector2.Zero, 2f, SpriteEffects.None, 0.009f);
                    }
                    else if (type == typeof(ChristmasTree))
                    {
                        DrawChristmasTree();
                    }
                    else if (type == typeof(Clock))
                    {
                        DrawClock();
                    }
                    else if (type == typeof(Cooler))
                    {
                        DrawCooler();
                    }
                    else if (type == typeof(CurriculumVitae))
                    {
                        DrawCurriculumVitae();
                    }
                    else if (type == typeof(Door))
                    {
                        DrawDoor();
                    }
                    else if (type == typeof(SideChair))
                    {
                        spriteBatch.Draw(sprites[SceneObjectSpriteEnum.SideChair], new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.026f);
                    }
                    else if (type == typeof(Character))
                    {
                        DrawCharacter();
                    }
                    else if (type == typeof(Table))
                    {
                        spriteBatch.Draw(sprites[SceneObjectSpriteEnum.Table], new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.0215f);
                    }
                    else if (type == typeof(Cat))
                    {
                        DrawCat();
                    }
                    else if (type == typeof(Whiteboard))
                    {
                        DrawWhiteboard();
                    } else if (type == typeof(Garland))
                    {
                        DrawGarland();
                    } else
                    {
                        throw new NotImplementedException("Забыл дабавить отрисовку объекта сцены");
                    }

                    void DrawChristmasTree()
                    {
                        ChristmasTree tree = (ChristmasTree)obj;
                        spriteTileMaps[TreeAnimationEnum.Blink].Draw(tree.Animator.GetFrame(), (texture) => spriteBatch.Draw(texture, new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.025f));
                        tree.Animator.DoTick();
                    }
                    void DrawClock()
                    {
                        spriteBatch.Draw(sprites[SceneObjectSpriteEnum.Clock], new Vector2(263, 26), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.004f);
                        float rotation = MathF.PI + MathF.PI * 1.5f * ((((OfficeScene)scene).Hour - 9) / 9f);
                        spriteBatch.Draw(sprites[SceneObjectSpriteEnum.ClockArrow], new Vector2(310, 80), null, Color.White, rotation, new Vector2(
                                sprites[SceneObjectSpriteEnum.ClockArrow].Width / 2,
                                sprites[SceneObjectSpriteEnum.ClockArrow].Height / 2),
                                1, SpriteEffects.None, 0.005f);
                    }
                    void DrawCooler()
                    {
                        Cooler cooler = (Cooler)obj;

                        float height = cooler.CofeeLvl * 1.4f;
                        spriteBatch.Draw(sprites[SpriteEnum.Pixel], new Vector2(260, 380-height), null, new Color(112, 78, 55, 200), 0, Vector2.Zero, new Vector2(111, height), SpriteEffects.None, 0.015f);
                        spriteBatch.Draw(sprites[SceneObjectSpriteEnum.Cooler], new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.0151f);
                    }
                    void DrawCurriculumVitae() 
                    {
                        CurriculumVitae cv = (CurriculumVitae)obj;
                        if (cv.IsExpanded)
                        {
                            spriteBatch.Draw(sprites[SceneObjectSpriteEnum.ExpandedCurriculumVitae], new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);

                            Character character = ((OfficeScene)scene).GetObject<Character>();
                            string text = character.FirstName + " " + character.SecondName + " " + character.Patronumic + '\n';
                            text += character.Birthday + '\n';
                            text += "Специальность:    " + (character.IsBoss ? "Он тут босс" : profTranslates[character.Profession]) + '\n';
                            text += "Грэйд:            " + character.Grade.ToString() + '\n';
                            text += "Желаемая зарплата:"; 
                            spriteBatch.DrawString(fonts[FontsEnum.Pixel], text, new Vector2(470, 75), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.22f);
                            string salary = character.RequiredSalary + " тыс.";
                            float dif = (Settings.AvarajeSalary[character.Grade] - character.RequiredSalary) / (Settings.AvarajeSalary[character.Grade] * 0.3f);
                            Color color = (dif < -0.5f) ? Color.Red : dif < 0.5f ? Color.Orange : Color.Green;
                            spriteBatch.DrawString(fonts[FontsEnum.Pixel], salary, new Vector2(725, 205), color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.22f);

                            text = "Профессионализм: " + (character.Professionality > 0 ? "+" : "") + character.Professionality;
                            color = character.Professionality < 0 ? Color.Red : character.Professionality == 0 ? Color.Orange : Color.Green;
                            spriteBatch.DrawString(fonts[FontsEnum.Pixel], text, new Vector2(525, 455), color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.22f);
                            text = "Социанльные навыки: " + (character.SocialIntelligence > 0 ? "+" : "") + character.SocialIntelligence;
                            color = character.SocialIntelligence < 0 ? Color.Red : character.SocialIntelligence == 0 ? Color.Orange : Color.Green;
                            spriteBatch.DrawString(fonts[FontsEnum.Pixel], text, new Vector2(525, 490), color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.22f);
                        }
                        else
                        {
                            Character character = ((OfficeScene)scene).GetCharacter();
                            if (character.IsSitting())
                                spriteBatch.Draw(sprites[SceneObjectSpriteEnum.CurriculumVitae], new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.0216f);
                        }
                    }
                    void DrawDoor()
                    {
                        Door door = ((OfficeScene)scene).GetObject<Door>();
                        if (door.State == DoorState.Closed)
                            spriteBatch.Draw(sprites[SceneObjectSpriteEnum.Door], new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.006f);
                        else
                            spriteBatch.Draw(sprites[SceneObjectSpriteEnum.OpenedDoor], new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.006f);
                    }
                    void DrawCharacter()
                    {
                        Character character = (Character)obj;
                        if (character.IsSitting())
                        {
                            spriteBatch.Draw(sprites[SpriteEnum.SittingCharacter], character.Pos, null, Color.White, 0, Vector2.Zero, character.scale, SpriteEffects.None, 0.022f);
                            spriteTileMaps[ClothesEnum.SittingClothes].Draw(character.ClothNum, (t) => spriteBatch.Draw(t, character.Pos, null, Color.White, 0, Vector2.Zero, character.scale, SpriteEffects.None, 0.023f));
                        }
                        else
                        {
                            spriteBatch.Draw(sprites[SpriteEnum.Character], character.Pos, null, Color.White, 0, Vector2.Zero, character.scale, SpriteEffects.None, 0.020f);
                            spriteTileMaps[ClothesEnum.Clothes].Draw(character.ClothNum, (t) => spriteBatch.Draw(t, character.Pos, null, Color.White, 0, Vector2.Zero, character.scale, SpriteEffects.None, 0.021f));
                        }
                        spriteTileMaps[ClothesEnum.Eyes].Draw(character.Eyes, (t) => spriteBatch.Draw(t, character.Pos, null, Color.White, 0, Vector2.Zero, character.scale, SpriteEffects.None, 0.0235f));
                        spriteTileMaps[ClothesEnum.Hairs].Draw(character.Hairs, (t) => spriteBatch.Draw(t, character.Pos, null, Color.White, 0, Vector2.Zero, character.scale, SpriteEffects.None, 0.0236f));
                        if (character.Accessory != -1)
                            spriteTileMaps[ClothesEnum.Accessories].Draw(character.Accessory, (t) => spriteBatch.Draw(t, character.Pos, null, Color.White, 0, Vector2.Zero, character.scale, SpriteEffects.None, 0.0237f));

                        character.DoTick();
                    }
                    void DrawCat()
                    {
                        Cat cat = (Cat)obj;
                        if (cat.Position == CatPosition.OnChair)
                        {
                            CatState state = cat.IsAngry ? CatState.AngryChair : CatState.Chair;
                            spriteBatch.Draw(sprites[state], new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.027f);
                        }
                        else if (cat.Position == CatPosition.OnCooler)
                        {
                            CatState state = cat.IsAngry ? CatState.AngryCooler : CatState.Cooler;
                            spriteBatch.Draw(sprites[state], new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.016f);
                        }
                        else if (cat.Position == CatPosition.OnTable)
                        {
                            CatState state = cat.IsAngry ? CatState.AngryPhone : CatState.Phone;
                            spriteBatch.Draw(sprites[state], new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.13f);
                        }
                    }
                    void DrawWhiteboard()
                    {
                        spriteBatch.Draw(sprites[SceneObjectSpriteEnum.Whiteboard], new Vector2(-180, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.003f);
                        OfficeScene ofScene = ((OfficeScene)scene);
                        string text = "Нужно нанять\n";
                        foreach (KeyValuePair<ProfessionEnum, int> pair in ofScene.Requirements)
                        {
                            text += profTranslates[pair.Key] + ": " + pair.Value + " (" + ofScene.HairedRatio[pair.Key] + ")\n";
                        }
                        spriteBatch.DrawString(fonts[FontsEnum.Pixel], text, new Vector2(410, 185), Color.Black, 0, Vector2.Zero, 0.95f, SpriteEffects.None, 0.009f);

                        spriteBatch.Draw(sprites[SpriteEnum.Pixel], new Vector2(800 - 20, 200), null, new Color(230, 220, 220), 0, Vector2.Zero, new Vector2(18, 100), SpriteEffects.None, 0.0032f);
                        spriteBatch.Draw(sprites[SpriteEnum.Pixel], new Vector2(800 - 20 * 3, 200), null, new Color(230, 220, 220), 0, Vector2.Zero, new Vector2(18, 100), SpriteEffects.None, 0.0032f);
                        spriteBatch.Draw(sprites[SpriteEnum.Pixel], new Vector2(800 - 20 * 5, 200), null, new Color(230, 220, 220), 0, Vector2.Zero, new Vector2(18, 100), SpriteEffects.None, 0.0032f);
                        spriteBatch.Draw(sprites[SpriteEnum.Pixel], new Vector2(800 - 20 * 7, 200), null, new Color(230, 220, 220), 0, Vector2.Zero, new Vector2(18, 100), SpriteEffects.None, 0.0032f);

                        spriteBatch.Draw(sprites[SpriteEnum.Pixel], new Vector2(800 - 20, 200 + (100 - Settings.BossSatisfaction)), null, new Color(250 - Settings.BossSatisfaction / 2, 170, 65), 0, Vector2.Zero, new Vector2(18, Settings.BossSatisfaction), SpriteEffects.None, 0.0035f);
                        spriteBatch.Draw(sprites[SpriteEnum.Pixel], new Vector2(800 - 20 * 3, 200 + (100 - Settings.Mentality)), null, new Color(160 - Settings.Mentality / 2, 166, 122), 0, Vector2.Zero, new Vector2(18, Settings.Mentality), SpriteEffects.None, 0.0035f);
                        spriteBatch.Draw(sprites[SpriteEnum.Pixel], new Vector2(800 - 20 * 5, 200 + (100 - Settings.Unity)), null, new Color(245, 193, 126 - Settings.Unity / 2), 0, Vector2.Zero, new Vector2(18, Settings.Unity), SpriteEffects.None, 0.0035f);
                        spriteBatch.Draw(sprites[SpriteEnum.Pixel], new Vector2(800 - 20 * 7, 200 + (100 - Settings.Professionality)), null, new Color(220, 190, 200 - Settings.Professionality / 2), 0, Vector2.Zero, new Vector2(18, Settings.Professionality), SpriteEffects.None, 0.0035f);

                    }
                    void DrawGarland()
                    {
                        Garland garland = (Garland)obj;
                        spriteTileMaps[GarlandAnimationEnum.BlinkAnimation].Draw(garland.Animator.GetFrame(), (texute) =>
                            spriteBatch.Draw(texute, new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.004f));
                        garland.Animator.DoTick();
                    }
                }
            }

            if (scene is StreetScene)
            {
                StreetScene stScene = (StreetScene)scene;
                foreach(Particle particle in stScene.Snowflakes)
                {
                    spriteBatch.Draw(sprites[SpriteEnum.Pixel], particle.Pos, null, Color.White, 0, Vector2.Zero, particle.Scale, SpriteEffects.None, 0.004f);
                }
            } 
        }

        public void DrawMenu(Menu menu, IScene scene)
        {
            if (menu.TextPlace != null)
            {
                spriteBatch.Draw(sprites[SpriteEnum.TextPlace], new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.3f);
                if (!menu.TextPlace.IsLastPage)
                {
                    spriteBatch.Draw(sprites[SpriteEnum.NextPageArrow], new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.31f);
                }
                
                string text = menu.TextPlace.GetText();
                int letterNum = 0;
                string[] words = text.Split(' ');
                StringBuilder builder = new StringBuilder();
                foreach(string word in words)
                {
                    if (letterNum + word.Length + 1 <= 50)
                    {
                        builder.Append(word + ' ');
                        letterNum += word.Length + 1;
                    }
                    else
                    {
                        builder.Append("\n" + word + ' ');
                        letterNum = word.Length + 1;
                    }
                }

                string text1;
                if (Settings.Mentality < 25)
                    text1 = builder.ToString().ToUpper();
                else
                    text1 = builder.ToString();

                spriteBatch.DrawString(fonts[FontsEnum.Pixel], text1, new Vector2(320, 560), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.333f);


                int count = (scene as OfficeScene).RecallCharacters.Count;
                if (count> 0)
                    spriteBatch.DrawString(fonts[FontsEnum.Pixel], count.ToString(), new Vector2(135, 435), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.533f);


                menu.TextPlace.DoTick(soundEffects);
            }

            spriteBatch.Draw(sprites[SpriteEnum.Phone], new Vector2(10, 350), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.4f);
            foreach(Button btn in menu.Phone.Buttons)
            {
                spriteBatch.Draw(sprites[btn.Name], btn.Pos, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.41f);
            }
        }

        public void DrawCharacter()
        {
            
        }

        private static Dictionary<ProfessionEnum, string> profTranslates = new Dictionary<ProfessionEnum, string>{
            { ProfessionEnum.Developer, "Разработчик"},
            { ProfessionEnum.Designer, "Дезайнер"},
            { ProfessionEnum.Analyst, "Аналитик"},
            { ProfessionEnum.Cleaner, "Уборщик"},
            { ProfessionEnum.Manager, "Мэнеджер"},
            { ProfessionEnum.Marketer, "Маркетолог"},
            { ProfessionEnum.Sysadmin, "Сисадмин"},
            { ProfessionEnum.Accountant, "Бухгалтер"},
        };
    }

    enum SpriteEnum
    {
        Phone,
        TextPlace,
        OfficeBackground,
        NextPageArrow,
        Pixel,
        Character,
        SittingCharacter,
    }

    enum SceneObjectSpriteEnum
    {
        BackChair,
        Calendar,
        ChristmasTree,
        Clock,
        ClockArrow,
        Cooler,
        CurriculumVitae,
        ExpandedCurriculumVitae,
        Door,
        OpenedDoor,
        SideChair,
        Table,
        Whiteboard,
        CatSits,
        CatOnCooler,
        CatOnChair,
    }

    enum SoundsEnum
    {
        door_closing,
        door_opening,
        drink_coffee,
        mur_long,
        mur_long_with_a_pet_cat,
        mur_short,
        mur_super_short,
        phone_btn_sound_1,
        phone_btn_sound_2,
        phone_btn_sound_3,
        song_carol_of_the_bells,
        song_last_christmas_cover,
        song_the_blizzard_is_ringing,
        sound_ambient,
        vc_closing,
        vc_opening,
        voice_long,
        voice_long_raised_tone,
        voice_short,
        cat_hisses,
        six_steps,
        step_1,
        step_2,
        tap_a_clock,
        tick_1,
        tick_2,
        transition_sound,
        rage,
    }
}
