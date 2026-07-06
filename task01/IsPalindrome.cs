namespace task01;

public static class StringExtensions
{
    public static bool IsPalindrome(this string input)
    {
        if (input == null)
        {
            return false;
        }
        string low_input = input.ToLower();
        char[] low = new char[input.Length];
        int index = 0;
        foreach (char c in low_input)
        {
            if (!char.IsPunctuation(c) && !char.IsWhiteSpace(c))
            {
                low[index] = c;
                index++;
            }
        }
        if (index == 0)
        {
            return false;
        }
        string new_input = new string(low, 0, index);
        char[] reversed_array = new_input.ToCharArray();
        Array.Reverse(reversed_array);
        string reversed = new string(reversed_array);
        return new_input == reversed;
    }
}

