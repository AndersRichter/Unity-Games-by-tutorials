namespace Test
{
    public class TestModel
    {
        private int Value { get; set; }

        private readonly int _increaseValue;
        private readonly int _decreaseValue;
        
        public TestModel(int increaseValue, int decreaseValue)
        {
            _increaseValue = increaseValue;
            _decreaseValue = decreaseValue;
        }

        public void Increase()
        {
            Value += _increaseValue;
        }

        public void Decrease()
        {
            Value -= _decreaseValue;
        }

#if UNITY_EDITOR
        public int GetValue()
        {
            return Value;
        }
#endif
    }
}