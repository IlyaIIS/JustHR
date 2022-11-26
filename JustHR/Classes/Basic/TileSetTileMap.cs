using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustHR.Classes.Basic
{
    /// <summary>
    /// Класс для работы с tileset'ами. Разбивает текстуру-сет на тайлы и даёт возможность отрисовывать их.
    /// </summary>
    class TileSetTileMap
    {
        public Texture2D Texture { get; private set; }
        public int Width { get; }
        public int Height { get; }
        public Point TileNum { get; }
        public int TotalTileNum { get; }
        public Point TileSize { get; }

        public delegate void DrawTile(Texture2D texuture, Rectangle sourceRectangle);

        public TileSetTileMap(Texture2D texture, int horizontalTileNum, int verticalTileNum, int totalTileNum = -1)
        {
            Texture = texture;
            Width = texture.Width;
            Height = texture.Height;
            TileNum = new Point(horizontalTileNum, verticalTileNum);
            TileSize = new Point(Width / TileNum.X, Height / TileNum.Y);
            if (totalTileNum < -1 || totalTileNum > TileNum.X * TileNum.Y)
                throw new AggregateException("Wrong totalTileNum parameter");
            TotalTileNum = (totalTileNum == -1) ? TileNum.X * TileNum.Y : totalTileNum;
        }

        public Rectangle GetSourceRectangle(int tileNum)
        {
            return GetSourceRectangle(tileNum % TileNum.X, tileNum / TileNum.Y);
        }
        public Rectangle GetSourceRectangle(int horizontileTileNum, int verticaleTileNum)
        {
            int relativeX = horizontileTileNum * TileSize.X;
            int relativeY = verticaleTileNum * TileSize.Y;
            return new Rectangle(relativeX, relativeY, TileSize.X, TileSize.Y);
        }

        public void Draw(int tileNum, DrawTile drawFunction)
        {
            Rectangle sourceRectangle = GetSourceRectangle(tileNum);
            drawFunction.Invoke(Texture, sourceRectangle);
        }
        public void Draw(int horizontileTileNum, int verticaleTileNum, DrawTile drawFunction)
        {
            Rectangle sourceRectangle = GetSourceRectangle(horizontileTileNum, verticaleTileNum);
            drawFunction.Invoke(Texture, sourceRectangle);
        }

        public void SimpleDraw(SpriteBatch spriteBatch, int tileNum, Vector2 position, Color color)
        {
            Rectangle sourceRectangle = GetSourceRectangle(tileNum);
            spriteBatch.Draw(Texture, position, sourceRectangle, color, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
        }
        public void SimpleDraw(SpriteBatch spriteBatch, int horizontileTileNum, int verticaleTileNum, Vector2 position, Color color)
        {
            Rectangle sourceRectangle = GetSourceRectangle(horizontileTileNum, verticaleTileNum);
            spriteBatch.Draw(Texture, position, sourceRectangle, color, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
