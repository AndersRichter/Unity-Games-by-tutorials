using System;
using UnityEngine;
using Utils.Disposable;

namespace Model.Data.Properties
{
    [Serializable]
    public abstract class ObservableProperty<TPropertyType>
    {
        [SerializeField] protected TPropertyType _value;
        
        public delegate void OnPropertyChanged(TPropertyType newValue, TPropertyType oldValue);
        public event OnPropertyChanged OnChanged;
        
        public IDisposable Subscribe(OnPropertyChanged action)
        {
            OnChanged += action;
            return new ActionDisposable(() => OnChanged -= action);
        }

        protected ObservableProperty(TPropertyType defaultValue)
        {
            _value = defaultValue;
        }
        
        public virtual TPropertyType Value
        {
            get => _value;
            set
            {
                var isEqual = _value.Equals(value);
                if (isEqual) return;

                var oldValue = _value;
                _value = value;
                
                InvokeOnChangedEvent(_value, oldValue);
            }
        }

        protected void InvokeOnChangedEvent(TPropertyType newValue, TPropertyType oldValue)
        {
            OnChanged?.Invoke(newValue, oldValue);
        }
    }
}