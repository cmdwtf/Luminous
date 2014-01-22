#region License
// Copyright © 2014 £ukasz Œwi¹tkowski
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

    public class XCssRules
    {
        public XCssRules(params XCssRule[] rules)
        {
            Rules = rules == null ? new List<XCssRule>() : rules.ToList();
        }

        public XCssRules(string name, params XCssRule[] rules)
            : this(rules)
        {
            Name = name;
        }

        public string Name { get; set; }

        public void Add(string selector, params XCssDeclaration[] declarations)
        {
            Rules.Add(new XCssRule(selector, declarations));
        }

        public void Remove(string selector)
        {
            var r = Rules.Find(_r => _r.Selector == selector);
            if (r != null)
            {
                Rules.Remove(r);
            }
        }

        public void RemoveAll(string selector)
        {
            Rules.RemoveAll(_r => _r.Selector == selector);
        }

        public List<XCssRule> Rules { get; private set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(Rules.Count * 10);
            if (!string.IsNullOrEmpty(Name))
            {
                sb.AppendLine()
                    .Append(Name)
                    .Append(" {");
                foreach (var rule in Rules)
                {
                    sb.AppendLine()
                        .AppendLine(rule.ToString().Indent());
                }
                sb.AppendLine("}");
            }
            else
            {
                foreach (var rule in Rules)
                {
                    sb.AppendLine()
                        .AppendLine(rule.ToString());
                }
            }
            return sb.ToString();
        }

        public XCssRules Optimize()
        {
            var d = new Dictionary<XCssDeclaration, List<string>>();
            foreach (var rule in Rules)
            {
                foreach (var declaration in rule.Declarations)
                {
                    if (!d.Keys.Any(_d => _d.ToString() == declaration.ToString()))
                    {
                        d[declaration] = new List<string>();
                    }
                    var l = d[d.Keys.First(_d => _d.ToString() == declaration.ToString())];
                    if (!l.Contains(rule.Selector))
                    {
                        l.Add(rule.Selector);
                    }
                }
            }

            var rules = new XCssRules();
            
            foreach (var declaration in d.Keys)
            {
                var l = d[declaration];

                rules.Add(string.Join(", ", l.ToArray()), declaration);
            }

            return rules;
        }
    }
}
