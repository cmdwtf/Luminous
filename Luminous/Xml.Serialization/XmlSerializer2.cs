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

namespace Luminous.Xml.Serialization
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public static class XmlSerializer2
    {
        public static string Serialize<T>(T obj)
            where T : new()
        {
            return @"<?xml version=""1.0"" encoding=""utf-8""?>" + Environment.NewLine + SerializeWithoutXmlDeclaration(obj);
        }

        public static string SerializeWithoutXmlDeclaration<T>(T obj)
            where T : new()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                new XmlSerializer(typeof(T)).Serialize(XmlWriter.Create(ms, new XmlWriterSettings
                {
                    CheckCharacters = false,
                    Encoding = Encoding.UTF8,
                    //Indent = true,
                    //IndentChars = "  ", 
                    NewLineHandling = NewLineHandling.Replace,
                    NewLineChars = Environment.NewLine,
                    OmitXmlDeclaration = true,
                }), obj);

                string xml = Encoding.UTF8.GetString(ms.ToArray());

                if (xml.StartsWith("\ufeff")) xml = xml.Substring(1);

                xml = ProcessXml(xml);

                return xml;
            }
        }

        public static string ProcessXml(string xml)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xml);

            ProcessXmlDocument(xdoc);

            return xdoc.OuterXml;
        }

        private static void ProcessXmlDocument(XmlDocument xdoc)
        {
            ProcessElement(0, xdoc.DocumentElement);
        }

        private static void ProcessElement(int level, XmlElement x)
        {
            if (level > 0)
            {
                x.ParentNode.InsertBefore(x.OwnerDocument.CreateSignificantWhitespace(Environment.NewLine + new string(' ', 2 * level)), x);
            }

            if (x.ChildNodes == null || x.ChildNodes.Count == 0 || (x.ChildNodes.Count == 1 && !(x.FirstChild is XmlElement))) return;

            var a = x.Attributes["xml:space"];
            if (a != null && a.Value == "preserve") return;

            for (int i = x.ChildNodes.Count - 1; i >= 0; i--)
            {
                var c = x.ChildNodes[i] as XmlElement;
                if (c != null)
                {
                    ProcessElement(level + 1, c);
                }
                else
                {
                    x.InsertBefore(x.OwnerDocument.CreateSignificantWhitespace(Environment.NewLine + new string(' ', 2 + 2 * level)), x.ChildNodes[i]);
                }
            }

            x.InsertAfter(x.OwnerDocument.CreateSignificantWhitespace(Environment.NewLine + new string(' ', 2 * level)), x.LastChild);
        }

        public static object Deserialize(string xml, Type type)
        {
            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer xs = new XmlSerializer(type);
                return xs.Deserialize(new SignificantWhitespaceToTextXmlTextReader(sr));
            }
        }

        public static T Deserialize<T>(string xml)
            where T : new()
        {
            return (T)Deserialize(xml, typeof(T));
        }

        internal class SignificantWhitespaceToTextXmlTextReader : XmlTextReader
        {
            public SignificantWhitespaceToTextXmlTextReader(TextReader r) : base(r) { }
            public SignificantWhitespaceToTextXmlTextReader(Stream s) : base(s) { }

            public override XmlNodeType NodeType
            {
                get
                {
                    XmlNodeType nt = base.NodeType;
                    if (nt == XmlNodeType.SignificantWhitespace)
                    {
                        nt = XmlNodeType.Text;
                    }
                    return nt;
                }
            }
        }
    }
}
