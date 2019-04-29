namespace SweetBirdStudios.Helpers
{
    public static class Enum
    {
        public static T ParseEnum<T>(this string t) => (T)System.Enum.Parse(typeof(T), t, true);
    }
}
