using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace JustHR.Classes.Basic
{
    class SpriteListTileMap
    {
        public List<Texture2D> Textures { get; private set; }
        public int TotalTileNum { get; }
        public Point TileSize { get; }

        public delegate void DrawSprite(Texture2D texture);

        public SpriteListTileMap(List<Texture2D> textures)
        {
            Textures = textures;

            TileSize = new Point(textures[0].Width, textures[0].Height);
            foreach (Texture2D texture in Textures)
            {
                if (TileSize.X != texture.Width || TileSize.Y != texture.Height)
                    throw new Exception("Текстуры тайлсета не одинакового размера");
            }
            
            TotalTileNum = textures.Count;
        }

        public void Draw(int tileNum, DrawSprite drawFunction)
        {
            drawFunction.Invoke(Textures[tileNum]);
        }

        public void SimpleDraw(SpriteBatch spriteBatch, int tileNum, Vector2 position, Color color)
        {
            spriteBatch.Draw(Textures[tileNum], position, color);
        }

        public Texture2D GetTileTexture(int tileNum)
        {
            return Textures[tileNum];
        }
    }
}
