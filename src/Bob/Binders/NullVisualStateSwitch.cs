namespace Bob.Binders
{
    public class NullVisualStateSwitch : VisualStateSwitch
    {
        protected override bool IsValueMatch(object value)
        {
            return value == null;
        }
    }
}