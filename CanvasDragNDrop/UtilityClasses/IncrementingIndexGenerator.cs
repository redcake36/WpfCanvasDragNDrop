namespace CanvasDragNDrop.UtilityClasses
{
    public class IncrementingIndexGenerator
    {
        public int IncrementedIndex
        {
            get { return _incrementedIndex++; }
        }
        private int _incrementedIndex = 0;
    }
}
