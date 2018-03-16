using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Shenon_Fano
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        Dictionary<char, double> dictionary = new Dictionary<char, double>();
        Dictionary<char, string> dictionarycode = new Dictionary<char, string>();
        ICollection<char> keys;
        string[] code;
        int m;

        public int Count(string a, TextBox textBox)
        {
            int count1 = 0;
            foreach (Match m in Regex.Matches(textBox.Text, a))
                count1++;
            return count1;
        }

        int BoundCalculate(Dictionary<char,double> dictionary, ICollection<char> keys, int Up, int Down)
        {
            double sumUp = 0;
            double sumDown = 0;
            for (int i = Up; i <= Down-1; i++)
            {
                sumUp += dictionary[keys.ElementAt(i)];
            }

            while (sumUp > sumDown)
            {
                Down -=1;
                sumUp -=dictionary[keys.ElementAt(Down)];
                sumDown +=dictionary[keys.ElementAt(Down)];
            }
            return Down;
        }

        void Sorted()
        {
            double length = Source.Text.Length;

            for (int i = 0; i < Source.Text.Length; i++)
            {
                dictionary[Source.Text[i]] = ((Count(Source.Text[i].ToString(), Source)) / length);
            }
            dictionary = dictionary.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
        void Shennon_Fano(Dictionary<char, double> dictionary, int Up, int Down)
        {
            int n = 0;
            if (Up!=Down)
            {
                n = BoundCalculate(dictionary,keys,Up, Down);
                for (int i = Up; i <= Down; i++)
                {
                    if (i <= n)
                    {
                        code[i] += "0";
                    }
                    else
                        code[i] += "1";
                }
                Shennon_Fano(dictionary,Up, n);
                Shennon_Fano(dictionary, n+1 , Down);
            }
                
        }
        void EncodeMessage()
        {
            for (int i = 0; i < dictionary.Count; i++)
            {
                dictionarycode[keys.ElementAt(i)] = code[i];
                list1.Items.Add(keys.ElementAt(i) + "-" + dictionarycode[keys.ElementAt(i)]);
            }

        }
        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                list1.Items.Clear();

                Sorted();

                keys = dictionary.Keys;
                code = new string[dictionary.Count];

                Shennon_Fano(dictionary, 0, dictionary.Count-1);

                EncodeMessage();

                Repend.Text = "Избыточность источника: " + OriginRedundancy(Enthropy()).ToString();
                LengthCode.Text = "Средняя длина кода: " + LengthCodeCalculate();
                OptimumCode.Text = "Оптимальная длина кода: " + Math.Ceiling(Enthropy());
            }
        }
        double Enthropy()
        {
            double entr = 0;
            double length = Source.Text.Length;

            foreach (char j in keys)
                entr += -(Convert.ToDouble(dictionary[j]) * Math.Log(Convert.ToDouble(dictionary[j]), 2));
            return entr;
        }

        double OriginRedundancy(double entr)
        {
            return Math.Round(1 - ((entr) / Math.Log(keys.Count, 2)),5);
        }

        double LengthCodeCalculate()
        {
            double sum = 0;
            for (int i = 0; i < dictionary.Count; i++)
            {
                if (dictionary.Count == 1)
                    return 1;
                else
                    sum += dictionarycode[keys.ElementAt(i)].Length * dictionary[keys.ElementAt(i)];
            }
            return Math.Round(sum,5);
        }
    }
}

