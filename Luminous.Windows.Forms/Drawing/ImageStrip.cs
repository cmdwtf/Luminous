#region License
// Copyright © 2021 Chris Marc Dailey (nitz) <https://cmd.wtf>
// Copyright © 2014 Łukasz Świątkowski <http://www.lukesw.net/>
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
#endregion License

namespace Luminous.Drawing
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;

	/// <summary>Supports drawing states of control from image strip.</summary>
	public class ImageStrip : IDisposable
	{
		#region Fields & Properties

		/// <summary>Gets the bitmap which contains sub-images of this image strip.</summary>
		public Bitmap ImageStripBitmap { get; private set; }

		/// <summary>Gets the orientation of the image strip.</summary>
		public ImageStripOrientation Orientation { get; }

		/// <summary>Gets the number of sub-images in the image strip.</summary>
		public int SubImagesCount { get; private set; }

		private Size _subImageSize;
		/// <summary>Gets the size of each sub-image in the image strip.</summary>
		public Size SubImageSize => _subImageSize;

		/// <summary>Gets the default border thickness of each sub-image in the image strip.</summary>
		public int DefaultBorderThickness { get; private set; }

		private struct DrawingParameters
		{
			public int SubImage;
			public int BorderThickness;
			public Size Size;
		}

		private struct Pair
		{
			public DrawingParameters DrawingParameters;
			public Bitmap Bitmap;
		}

		private const int BitmapListMaximumSize = 8;
		private readonly List<Pair> _bitmapList;
		private readonly Dictionary<DrawingParameters, Bitmap> _bitmapDictionary;

		#endregion

		#region Constructor

		/// <summary>Initializes a new instance of the <see cref="T:Luminous.Drawing.ImageStrip"/> class.</summary>
		/// <param name="imageStrip">The image strip.</param>
		/// <param name="orientation">The orientation of the image strip.</param>
		/// <param name="subImagesCount">Number of sub-images in the image strip.</param>
		/// <param name="subImageBorderThickness">Default border thickness of each sub-image in the image strip.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="imageStrip" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="subImagesCount"/> is less than one.
		/// -- or --
		/// <paramref name="subImageBorderThickness" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">Height (or width, depends on orientation) of imageStrip isn't divisible by subImagesCount.</exception>
		public ImageStrip(Image imageStrip, ImageStripOrientation orientation, int subImagesCount, int subImageBorderThickness)
		{
			if (imageStrip == null)
			{
				throw new ArgumentNullException(nameof(imageStrip));
			}
			if (subImagesCount < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(subImagesCount));
			}
			switch (orientation)
			{
				case ImageStripOrientation.Horizontal:
					if (imageStrip.Width % subImagesCount != 0)
					{
						throw new ArgumentException(null, nameof(subImagesCount));
					}
					break;
				case ImageStripOrientation.Vertical:
					if (imageStrip.Height % subImagesCount != 0)
					{
						throw new ArgumentException(null, nameof(subImagesCount));
					}
					break;
				default:
					throw new ArgumentException(null, nameof(orientation));
			}
			if (subImageBorderThickness < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(subImageBorderThickness));
			}
			ImageStripBitmap = new Bitmap(imageStrip);
			Orientation = orientation;
			SubImagesCount = subImagesCount;
			_subImageSize = orientation == ImageStripOrientation.Horizontal ?
				new Size(imageStrip.Width / subImagesCount, imageStrip.Height) :
				new Size(imageStrip.Width, imageStrip.Height / subImagesCount);
			DefaultBorderThickness = subImageBorderThickness;

			_bitmapList = new List<Pair>();
			_bitmapDictionary = new Dictionary<DrawingParameters, Bitmap>();
		}

		#endregion

		#region Methods

		/// <summary>Draws the specified sub-image of image strip.</summary>
		/// <param name="g">The graphic representing drawing surface.</param>
		/// <param name="subImageNumber">Part number.</param>
		/// <param name="destinationRectangle">Destination rectangle.</param>
		/// <exception cref="T:System.ArgumentNullException">g is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="subImageNumber" /> is less than zero or isn't less than number of sub-images.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Thrown when image strip is disposed.</exception>
		public void DrawSubImage(Graphics g, int subImageNumber, Rectangle destinationRectangle) => DrawSubImage(g, subImageNumber, destinationRectangle, DefaultBorderThickness);

		/// <summary>Draws the specified sub-image of image strip.</summary>
		/// <param name="g">The graphic representing drawing surface.</param>
		/// <param name="subImageNumber">Part number.</param>
		/// <param name="destinationRectangle">Destination rectangle.</param>
		/// <param name="borderThickness">The part border thickness.</param>
		/// <exception cref="T:System.ArgumentNullException">g is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="subImageNumber" /> is less than zero or isn't less than number of sub-images.
		/// -- or --
		/// <paramref name="borderThickness" /> is less than zero.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Thrown when image strip is disposed.</exception>
		public void DrawSubImage(Graphics g, int subImageNumber, Rectangle destinationRectangle, int borderThickness)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (g == null)
			{
				throw new ArgumentNullException(nameof(g));
			}
			if (subImageNumber < 0 || subImageNumber >= SubImagesCount)
			{
				throw new ArgumentOutOfRangeException(nameof(subImageNumber));
			}
			if (borderThickness < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(borderThickness));
			}

			var dp = new DrawingParameters
			{
				BorderThickness = borderThickness,
				Size = destinationRectangle.Size,
				SubImage = subImageNumber
			};

			if (!_bitmapDictionary.ContainsKey(dp))
			{
				var b = new Bitmap(destinationRectangle.Width, destinationRectangle.Height);
				using (var gb = Graphics.FromImage(b))
				{
					gb.Clear(Color.Transparent);
					switch (Orientation)
					{
						case ImageStripOrientation.Horizontal:
							DrawSubImage(gb, ImageStripBitmap, new Rectangle(new Point(SubImageSize.Width * subImageNumber, 0), SubImageSize), destinationRectangle, borderThickness);
							break;
						case ImageStripOrientation.Vertical:
							DrawSubImage(gb, ImageStripBitmap, new Rectangle(new Point(0, SubImageSize.Height * subImageNumber), SubImageSize), destinationRectangle, borderThickness);
							break;
					}
				}

				var pair = new Pair
				{
					DrawingParameters = dp,
					Bitmap = b
				};

				_bitmapList.Add(pair);
				_bitmapDictionary[dp] = b;

				if (_bitmapList.Count > BitmapListMaximumSize)
				{
					Pair pairToDel = _bitmapList[0];
					_bitmapList.RemoveAt(0);
					_bitmapDictionary.Remove(pairToDel.DrawingParameters);
				}
			}

			g.DrawImage(_bitmapDictionary[dp], destinationRectangle);
		}

		/// <summary>Draws the specified sub-image of image.</summary>
		/// <param name="g">The graphic representing drawing surface.</param>
		/// <param name="image">The image.</param>
		/// <param name="sourceRectangle">Source rectangle.</param>
		/// <param name="destinationRectangle">Destination rectangle.</param>
		/// <param name="borderThickness">The border thickness.</param>
		/// <exception cref="T:System.ArgumentNullException">g is null.
		/// -- or --
		/// image is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="borderThickness" /> is less than zero.</exception>
		public static void DrawSubImage(Graphics g, Image image, Rectangle sourceRectangle, Rectangle destinationRectangle, int borderThickness)
		{
			if (g == null)
			{
				throw new ArgumentNullException(nameof(g));
			}
			if (image == null)
			{
				throw new ArgumentNullException(nameof(image));
			}
			if (borderThickness < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(borderThickness));
			}

			if (borderThickness == 0)
			{
				ControlPaint.DrawImage(g, image, destinationRectangle, sourceRectangle);
				return;
			}

			int sx1 = sourceRectangle.X;
			int sx2 = sourceRectangle.X + borderThickness;
			int sx3 = sourceRectangle.X + sourceRectangle.Width - borderThickness;
			int[] sx = new int[3] { sx1, sx2, sx3 };
			int sw1 = borderThickness;
			int sw2 = sourceRectangle.Width - (borderThickness << 1)/* - 1*/;
			int sw3 = borderThickness;
			int[] sw = new int[3] { sw1, sw2, sw3 };

			int sy1 = sourceRectangle.Y;
			int sy2 = sourceRectangle.Y + borderThickness;
			int sy3 = sourceRectangle.Y + sourceRectangle.Height - borderThickness;
			int[] sy = new int[3] { sy1, sy2, sy3 };
			int sh1 = borderThickness;
			int sh2 = sourceRectangle.Height - (borderThickness << 1)/* - 1*/;
			int sh3 = borderThickness;
			int[] sh = new int[3] { sh1, sh2, sh3 };

			int dx1 = destinationRectangle.X;
			int dx2 = destinationRectangle.X + borderThickness;
			int dx3 = destinationRectangle.X + destinationRectangle.Width - borderThickness;
			int[] dx = new int[3] { dx1, dx2, dx3 };
			int dw1 = borderThickness;
			int dw2 = destinationRectangle.Width - (borderThickness << 1);
			int dw3 = borderThickness;
			int[] dw = new int[3] { dw1, dw2, dw3 };

			int dy1 = destinationRectangle.Y;
			int dy2 = destinationRectangle.Y + borderThickness;
			int dy3 = destinationRectangle.Y + destinationRectangle.Height - borderThickness;
			int[] dy = new int[3] { dy1, dy2, dy3 };
			int dh1 = borderThickness;
			int dh2 = destinationRectangle.Height - (borderThickness << 1);
			int dh3 = borderThickness;
			int[] dh = new int[3] { dh1, dh2, dh3 };

			for (int y = 0; y < 3; y++)
			{
				for (int x = 0; x < 3; x++)
				{
					ControlPaint.DrawImage(g, image, new Rectangle(dx[x], dy[y], dw[x], dh[y]), new Rectangle(sx[x], sy[y], sw[x], sh[y]));
				}
			}
		}

		#endregion

		#region Dispose Pattern

		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private bool _disposed;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing"><c>true</c> if managed resources should be disposed; otherwise, <c>false</c>.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				_disposed = true;
				if (disposing)
				{
					if (ImageStripBitmap != null)
					{
						ImageStripBitmap.Dispose();
						ImageStripBitmap = null;
						SubImagesCount = 0;
						_subImageSize = Size.Empty;
						DefaultBorderThickness = 0;

						_bitmapDictionary.Clear();
						foreach (Pair item in _bitmapList)
						{
							item.Bitmap.Dispose();
						}
						_bitmapList.Clear();
					}
				}
			}
		}

		#endregion
	}
}
