using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
 
namespace DynamicArray // Имя вашего пространства имён
{
    /// <summary>Класс одномерного динамического массива.</summary>
    /// <typeparam name="T">Тип элементов массива.</typeparam>
    public partial class DynamicArray<T> : IList<T>
    {
        private T[] array;
 
        public int Count { get; private set; }
 
        public bool IsReadOnly => false;
 
        public void CopyTo(T[] array, int arrayIndex)
            => Array.Copy(this.array, 0, array, arrayIndex, Count);
 
        public IEnumerator<T> GetEnumerator()
            => array.Take(Count).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
 
        public int IndexOf(T item)
            => Array.FindIndex(array, 0, Count, t => object.Equals(t, item));
        public bool Contains(T item)
            => IndexOf(item) >= 0;
 
    }
 
    // Методы изменяющие массив.
    public partial class DynamicArray<T> : IList<T>
    {
        public virtual T this[int index]
        {
            get => array[index];
            set
            {
                if (index >= Count)
                    throw new IndexOutOfRangeException("Индекс больше индекса последнего элемента.");
 
                array[index] = value;
            }
        }
 
        public virtual void Clear()
        {
            Array.Clear(array, 0, Count);
            Count = 0;
        }
 
        public virtual void Add(T item)
        {
            if (Count == Capacity)
            {
                int newSize = (int)(Capacity * IncreaseInCapacity);
                if (newSize == Capacity)
                    newSize++;
 
                Array.Resize(ref array, newSize);
            }
 
            array[Count] = item;
            Count++;
        }
        public virtual bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }
        public void RemoveAt(int index)
        {
            RemoveAt(index, out _);
        }
        protected virtual void RemoveAt(int index, out T item)
        {
            item = this[index];
 
            for (int ind = index + 1; ind < Count; ind++)
            {
                array[ind - 1] = array[ind];
            }
            Count--;
            array[Count] = default;
        }
        public virtual void Insert(int index, T item)
        {
            if (index < 0 || index > Count)
                throw new ArgumentOutOfRangeException(nameof(index));
 
            if (index == Count)
                Add(item);
            else
            {
                T temp = array[Count - 1];
                for (int ind = Count - 1; ind > index; ind--)
                {
                    array[ind] = array[ind - 1];
                }
                array[index] = item;
                Add(temp);
            }
        }
    }
 
 
    // Сведения о ёмкости и управление ею.
    partial class DynamicArray<T>
    {
        private double _increaseInCapacity = 2;
 
        /// <summary>Текущая вместимость массива.</summary>
        public int Capacity => array.Length;
 
        /// <summary>Изменяет ёмкость массива.</summary>
        /// <param name="capacity">Требуемое значение ёмкости.
        /// Если значение vtmit  <see cref="Count"/>, то уменьшается до <see cref="Count"/>.</param>
        /// <returns>Возвращает новую ёмкость.</returns>
        public int SetCapacity(int capacity)
        {
            if (capacity < Count)
                capacity = Count;
 
            if (capacity != Capacity)
                Array.Resize(ref array, capacity);
 
            return capacity;
        }
 
        /// <summary>Коэффициент роста <see cref="Capacity"/>, если для добавляемого элемента нет места в массиве.
        /// Может принимать значение от 1.1 до 2.0.</summary>
        public double IncreaseInCapacity
        {
            get => _increaseInCapacity;
            set
            {
                if (value < 1.1)
                    _increaseInCapacity = 1.1;
                else if (value > 2.0)
                    _increaseInCapacity = 2.0;
                else
                    _increaseInCapacity = value;
            }
        }
    }
 
    // Конструкторы.
    partial class DynamicArray<T>
    {
        public DynamicArray() : this(10) { }
        public DynamicArray(int capacity)
            => array = new T[capacity];
 
        public DynamicArray(IEnumerable<T> source)
        {
            array = source.ToArray();
            Count = array.Length;
        }
 
        public DynamicArray(params T[] source)
            : this((IEnumerable<T>)source)
        { }
    }
 
    // Операторы.
    partial class DynamicArray<T>
    {
        public static DynamicArray<T> operator --(DynamicArray<T> array)
        {
            array.RemoveAt(array.Count - 1);
            return array;
        }
 
        public static DynamicArray<T> operator +(DynamicArray<T> array, T item)
        {
            array.Add(item);
            return array;
        }
        public static DynamicArray<T> operator +(T item, DynamicArray<T> array)
        {
            array.Insert(0, item);
            return array;
        }
        public static DynamicArray<T> operator -(DynamicArray<T> array, T item)
        {
            array.Remove(item);
            return array;
        }
 
    }
}