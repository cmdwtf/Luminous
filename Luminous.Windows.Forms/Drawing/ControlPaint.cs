#region License
// Copyright © 2014 Łukasz Świątkowski
// http://www.lukesw.net/
//
// This library is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this library.  If not, see <http://www.gnu.org/licenses/>.
#endregion

namespace Luminous.Drawing
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    /// <summary>Provides various methods for drawing controls. This class cannot be inherited.</summary>
    internal static class ControlPaint
    {
        public static Bitmap ResizeImage(Image image, Size newSize)
        {
            return ResizeImage(image, new Rectangle(Point.Empty, image.Size), newSize);
        }

        public static Bitmap ResizeBitmap(Bitmap bitmap, Size newSize)
        {
            return ResizeBitmap(bitmap, new Rectangle(Point.Empty, bitmap.Size), newSize);
        }

        public static Bitmap ResizeImage(Image image, Rectangle srcRect, Size newSize)
        {
            if (image is Bitmap)
            {
                return ResizeBitmap(image as Bitmap, srcRect, newSize);
            }
            using (image = new Bitmap(image))
            {
                return ResizeBitmap(image as Bitmap, srcRect, newSize);
            }
        }

        public static Bitmap ResizeBitmap(Bitmap bitmap, Rectangle srcRect, Size newSize)
        {
            if (srcRect.Size.Height <= 0 || srcRect.Size.Width <= 0)
            {
                throw new ArgumentOutOfRangeException("sourceRectangle.Size", srcRect.Size, "sourceRectangle.Size <= (0,0)");
            }
            if (newSize.Height <= 0 || newSize.Width <= 0)
            {
                throw new ArgumentOutOfRangeException("newSize", newSize, "newSize <= (0,0)");
            }
            if (srcRect.Location.IsEmpty && srcRect.Size == bitmap.Size)
            {
                if (srcRect.Size == newSize)
                {
                    return new Bitmap(bitmap);
                }
                using (Image image = bitmap.GetThumbnailImage(newSize.Width, newSize.Height, null, IntPtr.Zero))
                {
                    return new Bitmap(image);
                }
            }
            using (Bitmap bmp = new Bitmap(srcRect.Width, srcRect.Height, PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Transparent);
                    g.DrawImage(bitmap, new Rectangle(Point.Empty, srcRect.Size), srcRect, GraphicsUnit.Pixel);
                }
                using (Image image = bmp.GetThumbnailImage(newSize.Width, newSize.Height, null, IntPtr.Zero))
                {
                    return new Bitmap(image);
                }
            }
        }

        public static void DrawImage(Graphics g, Image image, Rectangle destRect, Rectangle srcRect)
        {
            using (Bitmap bitmap = ResizeImage(image, srcRect, destRect.Size))
            {
                g.DrawImageUnscaled(bitmap, destRect);
            }
        }

        public static void DrawStringGlow(Graphics g, string s, Font font, Brush brush, Color glowColor, PointF point)
        {
            DrawStringGlow(g, s, font, brush, glowColor, new RectangleF(point.X, point.Y, 0f, 0f), null);
        }

        public static void DrawStringGlow(Graphics g, string s, Font font, Brush brush, Color glowColor, RectangleF layoutRectangle)
        {
            DrawStringGlow(g, s, font, brush, glowColor, layoutRectangle, null);
        }

        public static void DrawStringGlow(Graphics g, string s, Font font, Brush brush, Color glowColor, PointF point, StringFormat format)
        {
            DrawStringGlow(g, s, font, brush, glowColor, new RectangleF(point.X, point.Y, 0f, 0f), format);
        }

        public static void DrawStringGlow(Graphics g, string s, Font font, Brush brush, Color glowColor, float x, float y)
        {
            DrawStringGlow(g, s, font, brush, glowColor, new RectangleF(x, y, 0f, 0f), null);
        }

        public static void DrawStringGlow(Graphics g, string s, Font font, Brush brush, Color glowColor, float x, float y, StringFormat format)
        {
            DrawStringGlow(g, s, font, brush, glowColor, new RectangleF(x, y, 0f, 0f), format);
        }

        public static void DrawStringGlow(Graphics g, string s, Font font, Brush brush, Color glowColor, RectangleF layoutRectangle, StringFormat format)
        {
            if (brush == null)
            {
                throw new ArgumentNullException("brush");
            }
            if ((s != null) && (s.Length != 0))
            {
                if (font == null)
                {
                    throw new ArgumentNullException("font");
                }
                using (Brush glowBrush = new SolidBrush(Color.FromArgb(16, glowColor)))
                {
                    for (int y = -4; y <= 4; y++)
                    {
                        for (int x = -4; x <= 4; x++)
                        {
                            if (x * x + y * y <= 16)
                            {
                                g.DrawString(s, font, glowBrush, new RectangleF(layoutRectangle.X + x, layoutRectangle.Y + y, layoutRectangle.Width, layoutRectangle.Height), format);
                            }
                        }
                    }
                }
                g.DrawString(s, font, brush, layoutRectangle, format);
            }
        }
    }
}
