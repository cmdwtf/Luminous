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

namespace Luminous.Xml.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class XInlineStyle
    {
        public XInlineStyle(params XCssDeclaration[] declarations)
        {
            Declarations = declarations == null ? new List<XCssDeclaration>() : declarations.ToList();
        }

        public void Add(string name, string value)
        {
            Declarations.Add(new XCssDeclaration(name, value));
        }

        public void Remove(string name)
        {
            var d = Declarations.Find(_d => _d.Name == name);
            if (d != null)
            {
                Declarations.Remove(d);
            }
        }

        public void RemoveAll(string name)
        {
            Declarations.RemoveAll(_d => _d.Name == name);
        }

        public List<XCssDeclaration> Declarations { get; private set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(Declarations.Count * 10);
            foreach (var d in Declarations)
            {
                if (sb.Length > 0) sb.Append(' ');
                sb.Append(d.ToString());
            }
            return sb.ToString();
        }
    }
}