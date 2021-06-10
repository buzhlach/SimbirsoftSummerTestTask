using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimbirsoftTestTask
{
    class ParserURL
    {
        /// <summary>
        /// A string containing the site path
        /// </summary>
        private string site;

        /// <summary>
        /// Empty constructor with Google path
        /// </summary>
        public ParserURL()
        {
            this.site = @"https://www.google.com/";
        }

        /// <summary>
        /// Constructor with site path
        /// </summary>
        /// <param name="site">Full path to the site</param>
        public ParserURL(string site)
        {
            this.site = site;
        }

        /// <summary>
        /// Parse Html page with regular expressions and split(SEPARATORS array)
        /// </summary>
        /// <returns>Dictionary with keys = words, values = counts</returns>
        public Dictionary<string, int> Parse()
        {
            char[] SEPARATORS = { ' ', ',', '.', '!', '?', '\"', '\'', ';', ':', ']', '[', '(', ')', '\n', '\r', '\t', '+', '#', '&' };

            WebClient client = new WebClient();
            Stream inStream = client.OpenRead(site);
            StreamReader reader = new StreamReader(inStream);

            Dictionary<string, int> result = new Dictionary<string, int>();
            string inputLine;
            StringBuilder s = new StringBuilder();

            while((inputLine = reader.ReadLine()) != null)
            {
                Regex regex = new Regex(">[^<]+(</script)|" +
                    ">[^<]+(</style)|" +
                    ">[^<]+< |" +
                    ">[^<]+<=|" +
                    ">[^<]+<");
                MatchCollection matches = regex.Matches(inputLine);
                foreach (Match match in matches)
                {
                    String subInputLine = match.Value;
                    if ((!subInputLine.Contains("/script")) &&
                            (!subInputLine.Contains("< ")) &&
                            (!subInputLine.Contains("<=")) &&
                            (!subInputLine.Contains("/style")))
                    {
                        subInputLine = subInputLine.Substring(1, subInputLine.Length - 2);
                        s.Append(subInputLine + "\n");
                        foreach (String word in subInputLine.Split(SEPARATORS))
                        {
                            if (!word.Equals(""))
                            {
                                if(result.TryGetValue(word.ToUpper(), out int getCount))
                                {
                                    result[word.ToUpper()]++;
                                }
                                else
                                {
                                    result.Add(word.ToUpper(), 1);
                                }
                            }
                        }
                    }
                }
            }
            reader.Close();
            inStream.Close();
            return result;
        }
    }
}
