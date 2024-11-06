namespace Domain.Extensions
{
    public static class EnumExtensions
    {
        public static string[] ToStringArray(this Enum enumValue)
        {
            return enumValue.ToString()
                            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x.Trim())
                            .ToArray();
        }
    }
}