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

		public void Add(string selector, params XCssDeclaration[] declarations) => Rules.Add(new XCssRule(selector, declarations));

		public void Remove(string selector)
		{
			XCssRule rule = Rules.Find(r => r.Selector == selector);
			if (rule != null)
			{
				Rules.Remove(rule);
			}
		}

		public void RemoveAll(string selector) => Rules.RemoveAll(r => r.Selector == selector);

		public List<XCssRule> Rules { get; private set; }

		public override string ToString()
		{
			var sb = new StringBuilder(Rules.Count * 10);
			if (!string.IsNullOrEmpty(Name))
			{
				sb.AppendLine()
					.Append(Name)
					.Append(" {");
				foreach (XCssRule rule in Rules)
				{
					sb.AppendLine()
						.AppendLine(rule.ToString().Indent());
				}
				sb.AppendLine("}");
			}
			else
			{
				foreach (XCssRule rule in Rules)
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
			foreach (XCssRule rule in Rules)
			{
				foreach (XCssDeclaration declaration in rule.Declarations)
				{
					if (!d.Keys.Any(d => d.ToString() == declaration.ToString()))
					{
						d[declaration] = new List<string>();
					}
					List<string> l = d[d.Keys.First(d => d.ToString() == declaration.ToString())];
					if (!l.Contains(rule.Selector))
					{
						l.Add(rule.Selector);
					}
				}
			}

			var rules = new XCssRules();

			foreach (XCssDeclaration declaration in d.Keys)
			{
				List<string> l = d[declaration];

				rules.Add(string.Join(", ", l.ToArray()), declaration);
			}

			return rules;
		}
	}
}
