using JustHR.Classes.Interface;
using JustHR.Classes.SceneObjects;
using Microsoft.Xna.Framework;
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
        SpriteBatch spriteBatch;

        public Drawer(SpriteBatch spriteBatch , Dictionary<Enum, Texture2D> sprites, Dictionary<Enum, SpriteListTileMap> spriteTileMaps, Dictionary<Enum, SpriteFont> fonts)
        {
            this.spriteBatch = spriteBatch;
            this.sprites = sprites;
            this.spriteTileMaps = spriteTileMaps;
            this.fonts = fonts;
        }

        public void DrawScene(IScene scene)
        {

            spriteBatch.Draw(sprites[scene.Background], new Vector2(0, 0), Color.White);
            for (int i = 0; i < scene.Objects.Count; i++)
            {
                ISceneObject obj = scene.Objects[i];
                if (obj != null)
                {
                    Type type = obj.GetType();
                    if (type == typeof(BackChair))
                    {
                        spriteBatch.Draw(sprites[SceneObjectSpriteEnum.BackChair], new Vector2(0, 0), Color.White);
                    }
                    else if (type == typeof(Calendar))
                    {
                        spriteBatch.Draw(sprites[SceneObjectSpriteEnum.Calendar], new Vector2(0, 0), Color.White);
                        spriteBatch.DrawString(fonts[FontsEnum.Pixel], ((OfficeScene)scene).Day.ToString(), new Vector2(450, 220), Color.Black, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
                    }
                    else if (type == typeof(ChristmasTree))
                    {
                        spriteBatch.Draw(sprites[SceneObjectSpriteEnum.ChristmasTree], new Vector2(0, 0), Color.White);
                    }
                    else if (type == typeof(Clock))
                    {
                        spriteBatch.Draw(sprites[SceneObjectSpriteEnum.Clock], new Vector2(263, 26), Color.White);
                        float rotation = MathF.PI + MathF.PI * 1.5f * ((((OfficeScene)scene).Hour - 9) / 9f);
                        spriteBatch.Draw(sprites[SceneObjectSpriteEnum.ClockArrow], new Vector2(310, 80), null, Color.White, rotation, new Vector2(
                                sprites[SceneObjectSpriteEnum.ClockArrow].Width/2, 
                                sprites[SceneObjectSpriteEnum.ClockArrow].Height/2), 
                                1, SpriteEffects.None, 0);
                    }
                    else if (type == typeof(Cooler))
                    {
                        spriteBatch.Draw(sprites[SceneObjectSpriteEnum.Cooler], new Vector2(0, 0), Color.White);
                    }
                    else if (type == typeof(CurriculumVitae))
                    {
                        CurriculumVitae cv = (CurriculumVitae)obj;
                        if (cv.IsExpanded)
                        {
                            spriteBatch.Draw(sprites[SceneObjectSpriteEnum.ExpandedCurriculumVitae], new Vector2(0, 0), Color.White);
                        }else
                        {
                            Character character = ((OfficeScene)scene).GetCharacter();
                            if (character.IsSitting())
                                spriteBatch.Draw(sprites[SceneObjectSpriteEnum.CurriculumVitae], new Vector2(0, 0), Color.White);
                        }
                        
                    }
                    else if (type == typeof(Door))
                    {
                        spriteBatch.Draw(sprites[SceneObjectSpriteEnum.Door], new Vector2(0, 0), Color.White);
                    }
                    else if (type == typeof(SideChair))
                    {
                        spriteBatch.Draw(sprites[SceneObjectSpriteEnum.SideChair], new Vector2(0, 0), Color.White);
                    }
                    else if (type == typeof(Character))
                    {
                        Character character = (Character)obj;
                        spriteBatch.Draw(sprites[SceneObjectSpriteEnum.Cooler], character.Pos, null, Color.White, 0, Vector2.Zero, character.scale, SpriteEffects.None, 0);
                        character.DoTick();
                    }
                    else if (type == typeof(Table))
                    {
                        spriteBatch.Draw(sprites[SceneObjectSpriteEnum.Table], new Vector2(0, 0), Color.White);
                    }
                    else if (type == typeof(Whiteboard))
                    {
                        spriteBatch.Draw(sprites[SceneObjectSpriteEnum.Whiteboard], new Vector2(0, 0), Color.White);
                    } else if (type == typeof(Garland))
                    {
                        Garland garland = (Garland)obj;
                        spriteTileMaps[GarlandAnimationEnum.BlinkAnimation].Draw(garland.Animator.GetFrame(), (texute) =>
                            spriteBatch.Draw(texute, new Vector2(0, 0), Color.White)
                        );
                        garland.Animator.DoTick();
                    } else
                    {
                        throw new NotImplementedException("Забыл дабавить отрисовку объекта сцены");
                    }
                    
                }
            }
        }

        public void DrawMenu(Menu menu)
        {
            if (menu.TextPlace != null)
            {
                spriteBatch.Draw(sprites[SpriteEnum.TextPlace], new Vector2(0, 0), Color.White);
                if (!menu.TextPlace.IsLastPage)
                {
                    spriteBatch.Draw(sprites[SpriteEnum.NextPageArrow], new Vector2(0, 0), Color.White);
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

                spriteBatch.DrawString(fonts[FontsEnum.Pixel], builder.ToString(), new Vector2(320, 560), Color.White);


                menu.TextPlace.DoTick();
            }

            spriteBatch.Draw(sprites[SpriteEnum.Phone], new Vector2(0, 0), Color.White);


        }

        public void DrawCharacter() //TODO
        {
            
        }
    }

    enum SpriteEnum
    {
        Phone,
        TextPlace,
        OfficeBackground,
        NextPageArrow
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
        SideChair,
        Table,
        Whiteboard,
    }
}
