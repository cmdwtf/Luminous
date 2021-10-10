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
	using System.Text;

	public class XCssRule
	{
		public XCssRule(string selector, params XCssDeclaration[] declarations)
		{
			Selector = selector;
			Declarations = declarations == null ? new List<XCssDeclaration>() : declarations.ToList();
		}

		public string Selector { get; set; }

		public void Add(string name, string value) => Declarations.Add(new XCssDeclaration(name, value));

		public void Remove(string name)
		{
			XCssDeclaration decl = Declarations.Find(d => d.Name == name);
			if (decl != null)
			{
				Declarations.Remove(decl);
			}
		}

		public void RemoveAll(string name) => Declarations.RemoveAll(d => d.Name == name);

		public List<XCssDeclaration> Declarations { get; private set; }

		public override string ToString()
		{
			var sb = new StringBuilder(Declarations.Count * 10);
			foreach (XCssDeclaration declaration in Declarations)
			{
				sb.Append("    ").AppendLine(declaration.ToString());
			}
			sb.Insert(0, Selector + " {" + Environment.NewLine);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
