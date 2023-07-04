using System;
using UnityEngine;

namespace Model.Data.Properties
{
    [Serializable]
    public abstract class PersistentProperty<TPropertyType>
    {
        [SerializeField] private TPropertyType _value; // value in unity editor

        private TPropertyType _storedValue; // value saved to disk
        private readonly TPropertyType _defaultValue;

        public delegate void OnPropertyChanged(TPropertyType newValue, TPropertyType oldValue);
        public event OnPropertyChanged OnChanged;

        protected PersistentProperty(TPropertyType defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public TPropertyType Value
        {
            get => _storedValue;
            set
            {
                var isEqual = _storedValue.Equals(value);
                if (isEqual) return;

                var oldValue = _storedValue;
                Write(value);
                _storedValue = _value = value;
                
                OnChanged?.Invoke(_storedValue, oldValue);
            }
        }

        protected void Init()
        {
            _storedValue = _value = Read(_defaultValue);
        }
        
        protected abstract void Write(TPropertyType value);
        protected abstract TPropertyType Read(TPropertyType defaultValue);

        // sync value from editor fields with stored to disk value
        public void Validate()
        {
            if (!_storedValue.Equals(_value))
            {
                Value = _value;
            }
        }
    }
}