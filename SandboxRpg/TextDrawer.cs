using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SandboxRpg
{
    public class TextDrawer
    {
        private const float LineSpacing = 5f;

        private readonly SpriteFont font;
        private readonly float lineHeight;

        private readonly ICollection<TextEntry> lines = new List<TextEntry>();

        public TextDrawer(SpriteFont font)
        {
            this.font = font;

            lineHeight = font.MeasureString("TextDrawer").Y;
        }

        public TextEntry CreateEntry(Color? color = null)
        {
            var entry = new TextEntry(System.String.Empty, color);
            lines.Add(entry);

            return entry;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var position = Vector2.Zero;
            foreach (var line in lines)
            {
                spriteBatch.DrawString(font, line.Text, position, line.Color);

                position.Y += lineHeight + LineSpacing;
            }
        }

        public class TextEntry
        {
            public TextEntry(string text, Color? color = null)
            {
                Text = text;
                Color = color ?? Color.White;
            }

            public string Text { get; set; }
            public Color Color { get; }
        }
    }
}
