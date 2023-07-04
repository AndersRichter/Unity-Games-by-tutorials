using System;
using UnityEngine;

namespace Model.Data.Properties
{
    [Serializable]
    public abstract class ObservableProperty<TPropertyType>
    {
        [SerializeField] private TPropertyType _value;
        
        public delegate void OnPropertyChanged(TPropertyType newValue, TPropertyType oldValue);
        public event OnPropertyChanged OnChanged;

        protected ObservableProperty(TPropertyType defaultValue)
        {
            _value = defaultValue;
        }
        
        public TPropertyType Value
        {
            get => _value;
            set
            {
                var isEqual = _value.Equals(value);
                if (isEqual) return;

                var oldValue = _value;
                _value = value;
                
                OnChanged?.Invoke(_value, oldValue);
            }
        }
    }
}