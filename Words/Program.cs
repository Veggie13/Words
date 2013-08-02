#define STATE4

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace Words
{
    class Program
    {
        static string ToBase6(double value)
        {
            if (value == 0)
                return "0";

            string result = "";
            if (value < 0)
            {
                result = "-";
                value *= -1;
            }

            int nIntChars = (int)Math.Ceiling(Math.Log(value, 6));
            if (nIntChars == 0)
            {
                int c = (int)value;
                result += c.ToString();
                value -= c;
            }
            for (int ex = nIntChars - 1; ex >= 0; ex--)
            {
                int c = (int)(value / Math.Pow(6, ex));
                if (c == 6)
                    result += "10";
                else
                    result += c.ToString();
                value -= c * Math.Pow(6, ex);
            }

            if (value > 0)
                result += ".";
            int charCount = 0;
            while (value > 0)
            {
                if (++charCount > 4) break;
                value *= 6;
                int c = (int)value;
                result += c.ToString();
                value -= c;
            }

            return result;
        }

        static void Main(string[] args)
        {
#if true
            StreamReader wordReader = new StreamReader(@"H:\keycluedrop\Dropbox\Alien word list possible 1.txt");
            Dictionary<string, string> dict = new Dictionary<string,string>();
            while (!wordReader.EndOfStream)
            {
                string stuff = wordReader.ReadLine();
                string[] parts = stuff.Split('\t');
                dict[parts[0]] = parts[1];
            }
            wordReader.Close();
#endif

#if STATE2
            //StreamReader reader = new StreamReader(@"H:\keycluedrop\Dropbox\Capn Log 2.txt");
            StreamReader reader = new StreamReader(@"H:\keycluedrop\Dropbox\Alien Target Text 2.txt");
            string content = reader.ReadToEnd();
            reader.Close();
            string[] words = content.Split(' ');
#if true
            Dictionary<string, int> usage = new Dictionary<string, int>();
            foreach (string word in words)
            {
                double t = 0;
                if (double.TryParse(word, out t))
                    continue;
                if (dict.ContainsKey(word))
                    continue;
                if (usage.ContainsKey(word))
                    usage[word]++;
                else
                    usage[word] = 1;
            }
            List<string> sorted = new List<string>(usage.Keys);
            sorted.Sort(delegate(string a, string b)
            {
                int result = usage[b].CompareTo(usage[a]);
                if (result != 0)
                    return result;
                result = b.Length.CompareTo(a.Length);
                if (result != 0)
                    return result;/**/
                return b.CompareTo(a);
            });

#if false
            //TextWriter outer = Console.Out;
            using (TextWriter outer = new StreamWriter(@"H:\keycluedrop\Dropbox\Alien Target Words.txt"))
            {
                outer.WriteLine("{0} words:", sorted.Count);
                foreach (string word in sorted)
                {
                    outer.WriteLine("{0} - {1}", usage[word], word);
                }
                outer.Flush();
            }
#endif
#endif
#endif

#if false
            StreamReader alphaReader = new StreamReader(@"H:\keycluedrop\Dropbox\Alien Alphabet Order.txt");
            List<string> alphabet = new List<string>();
            while (!alphaReader.EndOfStream)
                alphabet.Add(alphaReader.ReadLine());
            alphaReader.Close();
#endif

#if STATE2
            Random r = new Random();
            //List<string> alienWords = new List<string>();
            foreach (string word in words)
            {
                if (dict.ContainsKey(word))
                    continue;
                int len = 4;// (int)Math.Round(3.25 + r.NextDouble()); //+r.Next(-4, 3) / 2;
                /*
                if (word.Length > 9)
                    len = 8;
                else if (word.Length > 6)
                    len = (int)Math.Round(1.5d + (word.Length / 2d) + 2 * r.NextDouble());
                else if (word.Length > 3)
                    len = 4 + (int)Math.Round(r.NextDouble());
                else
                    len = 4;*/

                string alien;
                do
                {
                starter:
                    alien = "";
                    for (int i = 0; i < len; i++)
                    {
                        int cons = (int)Math.Round(6.49 * r.NextDouble());
                        int vowl = (int)Math.Round(6.49 * r.NextDouble());
                        if (cons == 0 && vowl == 0) goto starter;

                        bool pos = (cons == 0) || (vowl == 0) || (r.Next(0, 3) > 0);
                        alien += string.Format("{0}{1}{2}", pos ? "+" : "-", cons, vowl);
                    }
                } while (false && dict.ContainsValue(alien));
                dict[word] = alien;
                //alienWords.Add(alien);
            }
            /*
            alienWords.Sort(delegate(string a, string b)
            {
                int l = Math.Min(a.Length, b.Length);
                for (int i = 0; i < l; i += 3)
                {
                    int result = alphabet.IndexOf(a.Substring(i, 3)).CompareTo(alphabet.IndexOf(b.Substring(i, 3)));
                    if (result != 0)
                        return result;
                }
                return a.Length.CompareTo(b.Length);
            });
            for (int n = 0; n < words.Length; n++)
                dict[words[n]] = alienWords[n];*/

            StreamWriter writer = new StreamWriter(@"H:\Alien word list 2.txt");
            foreach (string word in dict.Keys)
            {
                writer.WriteLine(string.Format("{0}\t{1}", word, dict[word]));
            }
            writer.Close();
#endif

#if STATE3
            StreamWriter writer = new StreamWriter(@"H:\keycluedrop\Dropbox\Alien DB Coded.txt");
            //StreamWriter writer2 = new StreamWriter(@"H:\keycluedrop\Dropbox\Capn Log Arranged.txt");

            StreamReader reader = new StreamReader(@"H:\keycluedrop\Dropbox\Alien DB Format.txt");
            string output = "", output2 = "";
            while (!reader.EndOfStream)
            {
                string curLine = reader.ReadLine();
                string[] sentences = curLine.Split(",:".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                char[] punc = Array.FindAll(curLine.ToCharArray(), delegate(char c)
                {
                    return c == ',' || c == ':';
                });
                string[][] sentenceWords = new string[sentences.Length][];
                for (int s = 0; s < sentences.Length; s++)
                {
                    sentenceWords[s] = sentences[s].Split(" \t".ToCharArray(), StringSplitOptions.None);
                }

                if (sentences.Length == 0)
                {
                    output += "SPC\r\n";
                    output2 += " \r\n";
                    continue;
                }
                int lineLen = 0;
                for (int s = 0; s < sentences.Length; s++)
                {
                    string[] sentence = sentenceWords[s];
                    string myWord = "", lastWord = "";
                    foreach (string word in sentence)
                    {
                        lastWord = myWord = word;
                        double temp = 0;
                        if (double.TryParse(word, out temp))
                        {
                            myWord = ToBase6(temp);
                            myWord = myWord.Replace("-", "NEG");
                            myWord = myWord.Replace("0", "dd0");
                            myWord = myWord.Replace("1", "dd1");
                            myWord = myWord.Replace("2", "dd2");
                            myWord = myWord.Replace("3", "dd3");
                            myWord = myWord.Replace("4", "dd4");
                            myWord = myWord.Replace("5", "dd5");
                            myWord = myWord.Replace(".", "ddd");
                        }
                        else if (!word.Equals(""))
                        {
                            myWord = dict[word];
                        }

                        if (lineLen + myWord.Length > 36 * 3)
                        {
                            output += "\r\n";
                            output2 += "\r\n";
                            lineLen = 0;
                        }

                        output += myWord;
                        output2 += word;
                        lineLen += myWord.Length;
                        if (lineLen + 3 > 36 * 3)
                        {
                            output += "\r\n";
                            output2 += "\r\n";
                            lineLen = 0;
                        }
                        else
                        {
                            output += "SPC";
                            output2 += " ";
                            lineLen += 3;
                        }
                    }
                    if (lineLen > 0)
                    {
                        output = output.Substring(0, output.Length - 3);
                        output2 = output2.Substring(0, output2.Length - 1);
                        lineLen -= 3;
                    }
                    if (s < punc.Length)
                    {
                        if (lineLen + 3 > 36 * 3)
                        {
                            output = output.Substring(0, output.Length - myWord.Length) + "\r\n" + myWord;
                            output2 = output2.Substring(0, output2.Length - lastWord.Length) + "\r\n" + lastWord;
                            lineLen = myWord.Length;
                        }
                        else if (lineLen == 0)
                        {
                            output = output.Substring(0, output.Length - myWord.Length - 5) + "\r\n" + myWord;
                            output2 = output2.Substring(0, output2.Length - lastWord.Length - 3) + "\r\n" + lastWord;
                            lineLen = myWord.Length;
                        }
                        if (punc[s] == ',')
                        {
                            output += "PRD";
                            output2 += ".";
                        }
                        else if (punc[s] == ':')
                        {
                            output += "CLN";
                            output2 += ":";
                        }
                        lineLen += 3;
                    }
                    if (lineLen + 3 > 36 * 3)
                    {
                        output += "\r\n";
                        output2 += "\r\n";
                        lineLen = 0;
                    }
                    else
                    {
                        output += "SPC";
                        output2 += " ";
                        lineLen += 3;
                    }
                }
                if (lineLen > 0)
                {
                    output = output.Substring(0, output.Length - 3) + "\r\n";
                    output2 = output2.Substring(0, output2.Length - 1) + "\r\n";
                    lineLen = 0;
                }
            }
            writer.Write(output);
            writer.Flush();
            writer.Close();
            //writer2.Write(output2);
            //writer2.Flush();
            //writer2.Close();
#endif

#if STATE4
            StreamReader reader = new StreamReader(@"H:\keycluedrop\Dropbox\Alien DB Coded.txt");

            StreamWriter writer = new StreamWriter(@"H:\keycluedrop\Dropbox\alien\earth.html");
            writer.WriteLine("<html>");
            writer.WriteLine("<body text='green' bgcolor='black'>");

            string output = "";
            int line = 1;
            while (!reader.EndOfStream)
            {
                output = "<br>";// +(line++).ToString() + " ";
                string curLine = reader.ReadLine();
                for (int i = 0; i < curLine.Length / 3; i++)
                {
                    string c = curLine.Substring(3 * i, 3);
                    output += string.Format("<img src='{0}.png'>", c);
                }
                //output += "\r\n";
                writer.WriteLine(output);
                writer.Flush();
            }

            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
            writer.Flush();
            writer.Close();
#endif

            Console.Write("Press any key...");
            Console.Read();
        }
    }
}
