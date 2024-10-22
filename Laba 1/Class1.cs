using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    public struct GeneticData
    {
        public string name;
        public string organism;
        public string formula;

        public string Encoding(string formula)
        {
            string encoded = String.Empty;
            for (int i = 0; i < formula.Length; i++)
            {
                char letter = formula[i];
                int count = 1;

                while (i < formula.Length - 1 && formula[i] == formula[i + 1])
                {
                    count++;
                    i++;
                }

                if (count > 2) encoded = encoded + count + letter;
                if (count == 1) encoded += letter;
                if (count == 2) encoded = encoded + letter + letter;

            }
            return encoded;
        }
        public static string Decoding(string formula)
        {
            string decoded = String.Empty;
            for (int i = 0; i < formula.Length; i++)
            {
                if (char.IsDigit(formula[i]))
                {
                    char letter = formula[i + 1];
                    int digit = formula[i] - '0';
                    for (int j = 0; j < digit - 1; j++)
                        decoded += letter;
                }
                else
                    decoded += formula[i];

            }

            return decoded;
        }
    }
}
