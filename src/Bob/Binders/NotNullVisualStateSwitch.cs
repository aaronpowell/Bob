namespace Bob.Binders
{
    public class NotNullVisualStateSwitch : VisualStateSwitch
    {
        protected override bool IsValueMatch(object value)
        {
            return value != null;
        }
    }
}