using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SimbirsoftTestTask
{
    class Program
    {
        /// <summary>
        /// Logs date, time and exception message to file log.txt 
        /// </summary>
        /// <param name="message">Exception message</param>
        static void Log(string message)
        {
            File.AppendAllText("log.txt", DateTime.Now.ToString() + " | " + message+'\n');
        }
        static void Main(string[] args)
        {
            string site = Console.ReadLine();
            ParserURL myParserURL = new ParserURL(site);

            try
            {
                Dictionary<string, int> words=myParserURL.Parse();
                Console.Clear();
                foreach(KeyValuePair<string,int> word in words.OrderBy(key => key.Value))
                {
                    Console.WriteLine("{0}: {1}",word.Key,word.Value);
                }

                Console.ReadKey();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                Log(e.Message);
            }
            catch(WebException e)
            {
                Console.WriteLine(e.Message);
                Log(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Log(e.Message);
            }
        }
    }
}
