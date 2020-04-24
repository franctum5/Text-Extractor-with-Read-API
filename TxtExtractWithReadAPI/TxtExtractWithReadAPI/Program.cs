using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TxtExtractWithReadAPI
{
    class Program
    {
        private static HttpClient httpClient = new HttpClient();
        private static string readAPIUri = "/vision/v3.0-preview/read/analyze";
        private static string settingsString;
        private static Dictionary<string, string> settings;
        private static string cogServicesEndpoint;
        private static string cogServicesKey;
        private static string inputFolder;
        private static string outputFolder;

        static void Main(string[] args)
        {
            Console.WriteLine("Starting converstion process...");

            //read settings.json
            try
            {
                settingsString = File.ReadAllText("settings.json");
                settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(settingsString);

                cogServicesEndpoint = settings["CognitiveServicesEndpoint"];
                cogServicesKey = settings["CognitiveServicesKey"];

                inputFolder = settings["inputFolder"];
                outputFolder = settings["outputFolder"];
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: settings.json is missing or not configured correctly");
                Console.WriteLine(ex.Message);
                return;
            }

            //create output folder if it doesn't exists
            Directory.CreateDirectory(settings["outputFolder"]);

            //get all files in input folder
            string[] files = Directory.GetFiles(settings["inputFolder"]);
            Console.WriteLine($"{files.Length} file(s) found.");

            Parallel.ForEach(files, (file) =>
            //foreach (string file in files)
            {
                string fileName = file.Substring(file.LastIndexOf('\\') + 1);
                Console.WriteLine($"{fileName} - Processing...");


                //check supported file types
                string contentType = null;
                if (file.EndsWith("pdf"))
                    contentType = "application/pdf";
                else if (file.EndsWith("png"))
                    contentType = "image/png";
                else if (file.EndsWith("jpg") || file.EndsWith("jpeg"))
                    contentType = "image/jpeg";
                else if (file.EndsWith("bmp"))
                    contentType = "image/bmp";
                else if (file.EndsWith("tif") || file.EndsWith("tiff"))
                    contentType = "image/tiff";
                else
                {
                    Console.WriteLine($"{fileName} - This file type is not supported, skipping it.");
                }

                if (contentType != null)
                {
                    //send the doc to the Read API
                    string operationLocation = null;
                    using (FileStream fs = File.OpenRead(file))
                    {
                        using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, string.Format("{0}{1}", cogServicesEndpoint, readAPIUri)))
                        {
                            requestMessage.Headers.Add("Ocp-Apim-Subscription-Key", cogServicesKey);
                            requestMessage.Content = new StreamContent(fs);
                            requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
                            HttpResponseMessage response = httpClient.SendAsync(requestMessage).GetAwaiter().GetResult();

                            operationLocation = response.Headers.GetValues("Operation-Location").FirstOrDefault();
                        }
                    }

                    //get result from Read API
                    ReadAPIResult result = null;
                    do
                    {
                        Thread.Sleep(3000);
                        using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, operationLocation))
                        {
                            requestMessage.Headers.Add("Ocp-Apim-Subscription-Key", cogServicesKey);

                            HttpResponseMessage response = httpClient.SendAsync(requestMessage).GetAwaiter().GetResult();
                            result = JsonConvert.DeserializeObject<ReadAPIResult>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                        }

                        Console.WriteLine($"{fileName} - Extraction status: {result.status}");
                    }
                    while (result.status != "succeeded" && result.status != "failed");


                    //write output file
                    if (result.status == "succeeded")
                    {
                        Console.Write($"{fileName} - Extraction succeeded, writing output file... ");

                        StringBuilder finalText = new StringBuilder();
                        foreach (Readresult rr in result.analyzeResult.readResults)
                        {
                            foreach (Line l in rr.lines)
                            {
                                finalText.AppendFormat($"{l.text} ");
                            }
                        }

                        using (StreamWriter sw = File.CreateText($"{outputFolder}\\{file.Substring(file.LastIndexOf('\\') + 1)}.txt"))
                        {
                            sw.Write(finalText.ToString());
                        }

                        Console.WriteLine($"{fileName} - Done.");
                    }

                }
            });

        }
    }
}
