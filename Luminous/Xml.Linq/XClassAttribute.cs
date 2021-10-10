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

namespace Luminous.Xml.Linq
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Xml.Linq;

	public class XClassAttribute : XAttribute
	{
		public XClassAttribute(params string[] classes)
			: base("class", string.Join(" ", classes))
		{
			_classes = (classes ?? Array.Empty<string>()).Distinct().ToList();
		}

		public void Add(string @class)
		{
			if (!_classes.Contains(@class))
			{
				_classes.Add(@class);
			}
			Value = ToString();
		}

		public void Remove(string @class)
		{
			_classes.Remove(@class);
			Value = ToString();
		}

		public IList<string> Classes() => _classes.AsReadOnly();

		private readonly List<string> _classes;

		public override string ToString() => string.Join(" ", _classes.ToArray());
	}

	public static class XClassExtensions
	{
		public static void AddClass(this XElement @this, string @class)
		{
			if (@this == null)
			{
				throw new ArgumentNullException(nameof(@this));
			}

			XClassAttribute c = @this.ClassAttribute();
			if (c == null)
			{
				@this.Add(c = new XClassAttribute());
			}
			c.Add(@class);
		}

		public static void RemoveClass(this XElement @this, string @class)
		{
			if (@this == null)
			{
				throw new ArgumentNullException(nameof(@this));
			}

			XClassAttribute c = @this.ClassAttribute();
			if (c != null)
			{
				c.Remove(@class);
			}
		}

		public static bool HasClassAttribute(this XElement @this)
			=> @this == null
				? throw new ArgumentNullException(nameof(@this))
				: @this.ClassAttribute() != null;

		public static bool HasClass(this XElement @this, string @class)
		{
			if (@this == null)
			{
				throw new ArgumentNullException(nameof(@this));
			}

			XClassAttribute c = @this.ClassAttribute();
			return c != null && c.Classes().Contains(@class);
		}

		public static void ToggleClass(this XElement @this, string @class)
		{
			if (@this == null)
			{
				throw new ArgumentNullException(nameof(@this));
			}

			if (@this.HasClass(@class))
			{
				@this.RemoveClass(@class);
			}
			else
			{
				@this.AddClass(@class);
			}
		}

		public static XClassAttribute ClassAttribute(this XElement @this)
			=> @this == null
				? throw new ArgumentNullException(nameof(@this))
				: @this.Attribute("class") as XClassAttribute;
	}
}
