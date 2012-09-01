namespace Bob.Binders
{
    public class VisualStateSwitchCase : VisualStateSwitch
    {
        public string Value { get; set; }

        protected override bool IsValueMatch(object value)
        {
            return value != null && value.ToString() == Value;
        }
    }
}