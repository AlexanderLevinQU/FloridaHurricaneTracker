using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FloridaHurricaneTracker.Model
{
    internal class CsvParserHelper
    {

        private readonly string _filePath;
        public CsvParserHelper() 
        {

            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(ConfigParameters.AppSettings)
                .Build();

            _filePath = config[ConfigParameters.Hurdat2FilePath];

            if (string.IsNullOrEmpty(_filePath))
            {
                throw new Exception("CsvParserHelper(): Was not able to read Hurricane Data file.");
            }
        }


        public List<Hurricane> Parse()
        {
            List<Hurricane> floridaHurricanes = new List<Hurricane>();

            return null;
        }

    }
}
