using System;

namespace Model.Data.Properties
{
    [Serializable]
    public class IntObservableProperty : ObservableProperty<int>
    {
        public IntObservableProperty(int defaultValue) : base(defaultValue) {}
    }
}