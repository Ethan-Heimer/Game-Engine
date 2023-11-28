using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class StringSpacer
{
    public static string Space(string input)
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
}