using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace lab1
{

    class Program
    {   
        static List<GeneticData> data;
        static List<string> resultData = new List<string>();
        static string divideline = "--------------------------------------------------------------------------";
        static string genedata = "genedata.0.txt";

        static void SaveDataInFile(string filename, List<string> savedData)
        {
            File.AppendAllLines(filename, savedData);
        }
        static string GetFormula(string proteinName)
        {
            string formula = String.Empty;

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].name.Equals(proteinName))
                {
                    formula = data[i].formula;
                    return formula;
                }
            }
            return null;
        }
        static List<GeneticData> ReadGeneticData(string fileName)
        {
            data = new List<GeneticData>();
            StreamReader reader = new StreamReader(fileName);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] fragments = line.Split('\t');
                GeneticData protein = new GeneticData();
                protein.name = fragments[0];
                protein.organism = fragments[1];
                protein.formula = GeneticData.Decoding(fragments[2]);
                //protein.Decoding(fragments[2]);
                data.Add(protein);
            }
            return data;
        }
        static void ReadHandleCommand(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);

            File.WriteAllText(genedata, String.Empty);

            int counter = 1;

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] command = line.Split('\t');
                if (command[0].Equals("search"))
                {
                    string decoded = GeneticData.Decoding(command[1]);
                    resultData.Add($"{counter.ToString("D3")}    {"search"}  {decoded}");
                    resultData.Add($"{"organism"}                {"protein"}");

                    int index = Search(decoded);

                    if (index != -1)
                        resultData.Add($"{data[index].organism}  {data[index].name}");
                    else
                        resultData.Add("NOT FOUND");
                }
                if (command[0].Equals("diff"))
                {
                    string formula1 = GetFormula(command[1]);
                    string formula2 = GetFormula(command[2]);

                    resultData.Add($"{counter.ToString("D3")}    {"diff"}    {command[1]}    {command[2]}");
                    resultData.Add($"{"amino-acids difference: "}");

                    if (formula1 != null && formula2 != null)
                    {
                        int diff_number = Diff(formula1, formula2);

                        resultData.Add(diff_number.ToString());
                    }
                    else
                    {
                        if (formula1 == null)
                            resultData.Add($"{"Missing:"}   {command[1]}");

                        else if (formula2 == null)
                            resultData.Add($"{"Missing:"}   {command[2]}");
                        else
                            resultData.Add($"{"Missing:"}   {command[1]}  {command[2]}");
                    }
                }
                if (command[0].Equals("mode"))
                {
                    string formula = GetFormula(command[1]);
                    resultData.Add($"{counter.ToString("D3")}    {"mode"}    {command[1]}");
                    resultData.Add("amino-acidoccurs: ");
                    if (formula != null)
                    {
                        string str = Mode(formula);
                        resultData.Add(str);
                    }
                    else
                    {
                        resultData.Add($"{"Missing:"}   {command[1]}");
                    }
                }

                resultData.Add(divideline);
                SaveDataInFile(genedata, resultData);
                resultData.Clear();
                counter++;
            }
            reader.Close();

        }
        static int Search(string amino_acid)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].formula.Contains(amino_acid))
                    return i;
            }
            return -1;
        }
        static int Diff(string formula1, string formula2)
        {
            formula1 = GeneticData.Decoding(formula1);
            formula2 = GeneticData.Decoding(formula2);

            int counter = 0;
            int minLenght = Math.Min(formula1.Length, formula2.Length);

            counter += Math.Abs(formula1.Length - formula2.Length);

            for (int i = 0; i < minLenght; i++)
            {
                if (formula1[i] != formula2[i])
                    counter++;
            }
            return counter;
        }
        static string Mode(string protein)
        {
            string result = String.Empty;
            int maxCount = 0;
            char letter, maxLetter = ' ';
            string formula = GeneticData.Decoding(protein);
            for (int i = 0; i < formula.Length; i++)
            {
                letter = formula[i];
                int count = 1;
                for (int j = i + 1; j < formula.Length; j++)
                {
                    if (letter == formula[j])
                    {
                        count++;
                    }
                }
                if (count > maxCount)
                {
                    maxCount = count;
                    maxLetter = letter;
                }
            }
            result = maxLetter.ToString() + "\t" + maxCount.ToString();
            return result;
        }
        static void Main(string[] args)
        {
            string file1 = "sequences.0.txt";
            string file2 = "commands.0.txt";
            data = ReadGeneticData(file1);

            ReadHandleCommand(file2);

            GeneticData data1 = new GeneticData();
            data1.formula = "AXCVDFGHHFJFJ3GH6hJKJ";
            data1.Encoding("trjbndmkskdmfbdnjskmd");

        }
    }
}