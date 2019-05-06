using SweetBirdStudios.Helpers;

public enum Character { Wizard, Knight, Rogue, Warrior }

/// <summary>
/// Helper class for Character enum.
/// </summary>
public static class Characters
{
    public static int Length => System.Enum.GetNames(typeof(Character)).Length;
    public static string Name(int i) => Get(i).ToString();
    public static Character Get(int i) => (Character)i;
    public static Character Get(string s) => s.ParseEnum<Character>();
}
