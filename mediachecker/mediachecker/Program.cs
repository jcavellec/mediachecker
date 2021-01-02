using System;
using System.Diagnostics;
using System.Linq;
using mediachecker.Helpers;
using mediachecker.Models;
using mediachecker.Services;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Serilog;
using Serilog.Core;

namespace mediachecker
{
    class Program
    {
        private static Logger _logger;
        private static DataBaseService _dbService;

        static void Main(string[] args)
        {
            var chrono = new Stopwatch();
            chrono.Start();
            var generateExcel = args[0];
            
            // Arguments must be true or false, for excel generation
            if (generateExcel == null)
            {
                throw new ArgumentNullException();
            }
            
            ConfigureLogger();
            _logger.Information("Starting MediaChecker ...");

            // Initialization
            var configuration = GetConfiguration();
            var connectData = GetConnectionData(configuration);
            _dbService = new DataBaseService(connectData);
            
            _logger.Information("Retrieving files ...");
            var mediaFiles = FileHelper.GetContentFolder(configuration.GetSection("InputFolder").Value);

            // TODO
            // FileHelpers.GetMetaDataFromList(mediaFiles);


            if (generateExcel.Equals("true"))
            {
                // Generate Excel file for with the result list
                _logger.Information("Generating Excel File ...");
                ExcelGeneratorService.GenerateExcelFile(mediaFiles, configuration.GetSection("ExcelOuputFolder").Value);
            }
            
            try
            {
                _logger.Information("Saving documents to db ...");

                // Avoid duplication
                var objCollection = _dbService.Collection;

                foreach (var file in mediaFiles
                    .ToList()
                    .Where(file => objCollection
                        .FindSync(x => x.Name == file.Name)
                        .Any()))
                {
                    mediaFiles.Remove(file);
                }

                if (mediaFiles.Count > 0)
                {
                    _dbService.Add(mediaFiles);
                }
                chrono.Stop();
                TimeSpan ts = chrono.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                _logger.Information($"Stop Watch = {elapsedTime}");
            }
            catch (MongoException e)
            {
                _logger.Error(e.Message);
            }

            _logger.Information("Done !");
        }

        private static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        private static ConnectionDataModel GetConnectionData(IConfiguration config)
        {
            return new ConnectionDataModel
            {
                ConnectionString = config.GetSection("ConnectionString").Value,
                DataBaseName = config.GetSection("DataBaseName").Value,
                CollectionName = config.GetSection("CollectionName").Value
            };
        }

        private static void ConfigureLogger()
        {
            _logger = new LoggerConfiguration()
#if DEBUG
                .WriteTo.Console()
#endif
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}