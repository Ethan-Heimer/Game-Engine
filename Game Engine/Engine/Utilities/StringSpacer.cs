using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

public static class StringSpacer
{
    public static string Annunciated(this string input)
    {
       return input = input.Spaced().Cappitalized();
    }

    public static string Spaced(this string input)
    {
        StringBuilder newString = new StringBuilder(input);
        int offset = 0;

        for (int i = 0; i < input.Length; i++)
        {
            if (i == 0 || i == input.Length - 1)
                continue;

            if (char.IsUpper(input[i]))
            {
                newString.Insert(i + offset, ' ');
                offset++;
            }
        }

        return newString.ToString();
    }

    public static string Cappitalized(this string input) 
    {
        char[] chars = input.ToCharArray();
        chars[0] = Char.ToUpper(chars[0]);

        return new string(chars);
    }
}