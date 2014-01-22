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

namespace Luminous.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [Serializable]
    public class Matrix<T> : IEnumerable<T>
    {
        #region Fields & Properties

        public int Width { get; private set; }

        private int _height;
        public int Height
        {
            get { return _height; }
            set
            {
                _height = value;

                int requiredElements = _height * Width;
                if (_items.Count < requiredElements)
                {
                    _items.AddRange(Enumerable.Repeat(default(T), requiredElements - _items.Count));
                }
                else if (_items.Count > requiredElements)
                {
                    _items.RemoveRange(requiredElements, _items.Count - requiredElements);
                }
            }
        }

        private List<T> _items;
        public T this[int row, int column]
        {
            get
            {
                if (row < 0 || row >= Height) throw new ArgumentOutOfRangeException("row", row, "0 ≤ row < Height");
                if (column < 0 || column >= Width) throw new ArgumentOutOfRangeException("column", column, "0 ≤ column < Width");

                return _items[Width * row + column];
            }
            set
            {
                if (row < 0 || row >= Height) throw new ArgumentOutOfRangeException("row", row, "0 ≤ row < Height");
                if (column < 0 || column >= Width) throw new ArgumentOutOfRangeException("column", column, "0 ≤ column < Width");

                _items[Width * row + column] = value;
            }
        }

        #endregion

        #region Constructors

        public Matrix(int width) : this(width, 0) { }

        public Matrix(int width, int height)
        {
            if (width <= 0) throw new ArgumentOutOfRangeException("width", width, "width > 0");
            if (height < 0) throw new ArgumentOutOfRangeException("height", height, "height ≥ 0");

            _items = new List<T>();
            Width = width;
            Height = height;
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (_items as IEnumerable).GetEnumerator();
        }

        #endregion

        #region Methods

        public IEnumerable<Element> GetItems()
        {
            int i = 0;
            foreach (var it in _items)
            {
                yield return new Element(i / Width, i % Width, it);
                i++;
            }
        }

        public T[][] ToArray()
        {
            var array = new T[Height][];

            for (int i = 0; i < Height; i++)
            {
                array[i] = GetRow(i).ToArray();
            }

            return array;
        }

        public void AddRow(IEnumerable<T> row)
        {
            if (row == null) throw new ArgumentNullException();

            InsertRow(Height, row);
        }

        public void InsertRow(int index, IEnumerable<T> row)
        {
            if (row == null) throw new ArgumentNullException();
            if (index < 0 || index > Height) throw new ArgumentOutOfRangeException("index", index, "0 ≤ index ≤ Height");

            var rowArray = row.ToList();
            if (rowArray.Count != Width) throw new ArgumentOutOfRangeException("row.Count", rowArray.Count, "row.Count = Width");

            _items.InsertRange(index * Width, rowArray);
            Height++;
        }

        public void RemoveRow(int index)
        {
            if (index < 0 || index >= Height) throw new ArgumentOutOfRangeException("index", index, "0 ≤ index < Height");

            _items.RemoveRange(index * Width, Width);
        }

        public List<T> GetRow(int index)
        {
            if (index < 0 || index >= Height) throw new ArgumentOutOfRangeException("index", index, "0 ≤ index < Height");

            return _items.GetRange(index * Width, Width);
        }

        public List<T> GetColumn(int index)
        {
            if (index < 0 || index >= Width) throw new ArgumentOutOfRangeException("index", index, "0 ≤ index < Width");

            var list = new List<T>(Height);

            for (int i = 0; i < Height; i++)
            {
                list.Add(_items[i * Width + index]);
            }

            return list;
        }

        #endregion

        public struct Element
        {
            public int Row { get; private set; }
            public int Column { get; private set; }
            public T Value { get; private set; }

            public Element(int row, int column, T value)
                : this()
            {
                Row = row;
                Column = column;
                Value = value;
            }
        }
    }
}
