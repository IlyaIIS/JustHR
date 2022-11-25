using JustHR.Classes.Interface;
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
        SpriteBatch spriteBatch;

        public Drawer(SpriteBatch spriteBatch , Dictionary<Enum, Texture2D> sprites, Dictionary<Enum, SpriteFont> fonts)
        {
            this.spriteBatch = spriteBatch;
            this.sprites = sprites;
            this.fonts = fonts;
        }

        public void DrawScene(IScene scene)
        {

            spriteBatch.Draw(sprites[scene.Background], new Vector2(0, 0), Color.White);
            foreach(ISceneObject obj in scene.Objects)
            {
                if (obj != null)
                {
                    spriteBatch.Draw(sprites[(SceneObjectsEnum)Enum.Parse(typeof(SceneObjectsEnum), obj.GetType().Name)], new Vector2(0, 0), Color.White); 
                }
            }
        }

        public void DrawPhone(Phone phone)
        {
            spriteBatch.Draw(sprites[SpriteEnum.Phone], new Vector2(0, 0), Color.White);
        }

        public void DrawCharacter() //TODO
        {
            
        }
    }

    enum SpriteEnum
    {
        Phone,
        OfficeBackground
    }


}
