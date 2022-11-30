using JustHR.Classes.Interface;
using JustHR.Classes.SceneObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

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

                if(soundEffects[currentSong].State == SoundState.Stopped && Player.Mentality >= 25)
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

            if (scene is OfficeScene)
            {
                OfficeScene ofScene = (OfficeScene)scene;

                DrawBackChair();
                DrawCalendar();
                DrawChristmasTree();
                DrawClock();
                DrawCooler();
                DrawCurriculumVitae();
                DrawDoor();
                DrawSideChair();
                DrawCharacter();
                DrawTable();
                DrawCat();
                DrawWhiteboard();
                DrawGarland();

                #region Методы рисования предметов сцены офиса
                void DrawBackChair()
                {
                    spriteBatch.Draw(sprites[SceneObjectSpriteEnum.BackChair], new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, ofScene.Objects.BackChair.Z);
                }
                void DrawCalendar()
                {
                    spriteBatch.Draw(sprites[SceneObjectSpriteEnum.Calendar], new Vector2(460, -90), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, ofScene.Objects.Calendar.Z);
                    spriteBatch.DrawString(fonts[FontsEnum.Pixel], ((OfficeScene)scene).Day.ToString(), new Vector2(450 + 460, 220 - 90), Color.Black, 0, Vector2.Zero, 2f, SpriteEffects.None, ofScene.Objects.Calendar.Z + 0.001f);
                }
                void DrawChristmasTree()
                {
                    ChristmasTree tree = ofScene.Objects.ChristmasTree;
                    spriteTileMaps[TreeAnimationEnum.Blink].Draw(tree.Animator.GetFrame(), (texture) => spriteBatch.Draw(texture, new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, ofScene.Objects.ChristmasTree.Z));
                    tree.Animator.DoTick();
                }
                void DrawClock()
                {
                    spriteBatch.Draw(sprites[SceneObjectSpriteEnum.Clock], new Vector2(263, 26), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, ofScene.Objects.Clock.Z);
                    float rotation = MathF.PI + MathF.PI * 1.5f * ((((OfficeScene)scene).Hour - 9) / 9f);
                    spriteBatch.Draw(sprites[SceneObjectSpriteEnum.ClockArrow], new Vector2(310, 80), null, Color.White, rotation, new Vector2(
                            sprites[SceneObjectSpriteEnum.ClockArrow].Width / 2,
                            sprites[SceneObjectSpriteEnum.ClockArrow].Height / 2),
                            1, SpriteEffects.None, ofScene.Objects.Clock.Z + 0.001f);
                }
                void DrawCooler()
                {
                    Cooler cooler = ofScene.Objects.Cooler;

                    float height = cooler.CofeeLvl * 1.4f;
                    spriteBatch.Draw(sprites[SpriteEnum.Pixel], new Vector2(260, 380 - height), null, new Color(112, 78, 55, 200), 0, Vector2.Zero, new Vector2(111, height), SpriteEffects.None, ofScene.Objects.Cooler.Z);
                    spriteBatch.Draw(sprites[SceneObjectSpriteEnum.Cooler], new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, ofScene.Objects.Cooler.Z + 0.001f);
                }
                void DrawCurriculumVitae()
                {
                    if (ofScene.Objects.CurriculumVitae.IsExpanded)
                    {
                        spriteBatch.Draw(sprites[SceneObjectSpriteEnum.ExpandedCurriculumVitae], new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, ofScene.Objects.CurriculumVitae.Z);

                        Character character = ofScene.Objects.Character;
                        string text = character.Traits.FirstName + " " + character.Traits.SecondName + " " + character.Traits.Patronumic + '\n';
                        text += character.Traits.Birthday + '\n';
                        text += "Специальность:    " + (character.Traits.IsBoss ? "Он тут босс" : profTranslates[character.Traits.Profession]) + '\n';
                        text += "Грэйд:            " + character.Traits.Grade.ToString() + '\n';
                        text += "Желаемая зарплата:";
                        spriteBatch.DrawString(fonts[FontsEnum.Pixel], text, new Vector2(470, 75), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, ofScene.Objects.CurriculumVitae.Z + 0.001f);
                        string salary = character.Traits.RequiredSalary + " тыс.";
                        float dif = (Settings.AvarajeSalary[character.Traits.Grade] - character.Traits.RequiredSalary) / (Settings.AvarajeSalary[character.Traits.Grade] * 0.3f);
                        Color color = (dif < -0.5f) ? Color.Red : dif < 0.5f ? Color.Orange : Color.Green;
                        spriteBatch.DrawString(fonts[FontsEnum.Pixel], salary, new Vector2(725, 205), color, 0, Vector2.Zero, 1f, SpriteEffects.None, ofScene.Objects.CurriculumVitae.Z + 0.001f);

                        text = "Профессионализм: " + (character.Traits.Professionality > 0 ? "+" : "") + character.Traits.Professionality;
                        color = character.Traits.Professionality < 0 ? Color.Red : character.Traits.Professionality == 0 ? Color.Orange : Color.Green;
                        spriteBatch.DrawString(fonts[FontsEnum.Pixel], text, new Vector2(525, 455), color, 0, Vector2.Zero, 1f, SpriteEffects.None, ofScene.Objects.CurriculumVitae.Z + 0.001f);
                        text = "Социанльные навыки: " + (character.Traits.SocialIntelligence > 0 ? "+" : "") + character.Traits.SocialIntelligence;
                        color = character.Traits.SocialIntelligence < 0 ? Color.Red : character.Traits.SocialIntelligence == 0 ? Color.Orange : Color.Green;
                        spriteBatch.DrawString(fonts[FontsEnum.Pixel], text, new Vector2(525, 490), color, 0, Vector2.Zero, 1f, SpriteEffects.None, ofScene.Objects.CurriculumVitae.Z + 0.001f);
                    }
                    else
                    {
                        if (ofScene.Objects.Character.IsSitting())
                            spriteBatch.Draw(sprites[SceneObjectSpriteEnum.CurriculumVitae], new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, ofScene.Objects.CurriculumVitae.Z + 0.016f);
                    }
                }
                void DrawDoor()
                {
                    Door door = ofScene.Objects.Door;
                    if (door.State == DoorState.Closed)
                        spriteBatch.Draw(sprites[SceneObjectSpriteEnum.Door], new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, ofScene.Objects.Door.Z);
                    else
                        spriteBatch.Draw(sprites[SceneObjectSpriteEnum.OpenedDoor], new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, ofScene.Objects.Door.Z);
                }
                void DrawSideChair()
                {
                    spriteBatch.Draw(sprites[SceneObjectSpriteEnum.SideChair], new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, ofScene.Objects.SideChair.Z);
                }
                void DrawCharacter()
                {
                    Character character = ofScene.Objects.Character;
                    if (character.IsSitting())
                    {
                        spriteBatch.Draw(sprites[SpriteEnum.SittingCharacter], character.Pos, null, Color.White, 0, Vector2.Zero, character.Scale, SpriteEffects.None, 0.022f);
                        spriteTileMaps[ClothesEnum.SittingClothes].Draw(character.ClothNum, (t) => spriteBatch.Draw(t, character.Pos, null, Color.White, 0, Vector2.Zero, character.Scale, SpriteEffects.None, 0.023f));
                    }
                    else
                    {
                        spriteBatch.Draw(sprites[SpriteEnum.Character], character.Pos, null, Color.White, 0, Vector2.Zero, character.Scale, SpriteEffects.None, 0.020f);
                        spriteTileMaps[ClothesEnum.Clothes].Draw(character.ClothNum, (t) => spriteBatch.Draw(t, character.Pos, null, Color.White, 0, Vector2.Zero, character.Scale, SpriteEffects.None, 0.021f));
                    }
                    spriteTileMaps[ClothesEnum.Eyes].Draw(character.Traits.Eyes, (t) => spriteBatch.Draw(t, character.Pos, null, Color.White, 0, Vector2.Zero, character.Scale, SpriteEffects.None, 0.0235f));
                    spriteTileMaps[ClothesEnum.Hairs].Draw(character.Traits.Hairs, (t) => spriteBatch.Draw(t, character.Pos, null, Color.White, 0, Vector2.Zero, character.Scale, SpriteEffects.None, 0.0236f));
                    if (character.Traits.Accessory != -1)
                        spriteTileMaps[ClothesEnum.Accessories].Draw(character.Traits.Accessory, (t) => spriteBatch.Draw(t, character.Pos, null, Color.White, 0, Vector2.Zero, character.Scale, SpriteEffects.None, 0.0237f));

                    character.DoTick();
                }
                void DrawTable()
                {
                    spriteBatch.Draw(sprites[SceneObjectSpriteEnum.Table], new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, ofScene.Objects.Table.Z);
                }
                void DrawCat()
                {
                    Cat cat = ofScene.Objects.Cat;
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

                    spriteBatch.Draw(sprites[SpriteEnum.Pixel], new Vector2(800 - 20, 200 + (100 - Player.BossSatisfaction)), null, new Color(250 - Player.BossSatisfaction / 2, 170, 65), 0, Vector2.Zero, new Vector2(18, Player.BossSatisfaction), SpriteEffects.None, 0.0035f);
                    spriteBatch.Draw(sprites[SpriteEnum.Pixel], new Vector2(800 - 20 * 3, 200 + (100 - Player.Mentality)), null, new Color(160 - Player.Mentality / 2, 166, 122), 0, Vector2.Zero, new Vector2(18, Player.Mentality), SpriteEffects.None, 0.0035f);
                    spriteBatch.Draw(sprites[SpriteEnum.Pixel], new Vector2(800 - 20 * 5, 200 + (100 - Player.Unity)), null, new Color(245, 193, 126 - Player.Unity / 2), 0, Vector2.Zero, new Vector2(18, Player.Unity), SpriteEffects.None, 0.0035f);
                    spriteBatch.Draw(sprites[SpriteEnum.Pixel], new Vector2(800 - 20 * 7, 200 + (100 - Player.Professionality)), null, new Color(220, 190, 200 - Player.Professionality / 2), 0, Vector2.Zero, new Vector2(18, Player.Professionality), SpriteEffects.None, 0.0035f);

                }
                void DrawGarland()
                {
                    Garland garland = ofScene.Objects.Garland;
                    spriteTileMaps[GarlandAnimationEnum.BlinkAnimation].Draw(garland.Animator.GetFrame(), (texute) =>
                        spriteBatch.Draw(texute, new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, ofScene.Objects.Garland.Z));
                    garland.Animator.DoTick();
                }
                #endregion 
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
                if (Player.Mentality < 25)
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
