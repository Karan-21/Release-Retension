using ReleaseRetention;
using Newtonsoft.Json;
using System;
using System.Buffers;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

public class Program
{
    // Get the directory of the currently executing assembly.
    public static string getDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

    public static void Main()
    {
        // Initialize a list to store console log statements.
        List<ConsoleLogStatements> ConsoleLogStatementses = new List<ConsoleLogStatements>();

        ConsoleLogStatements ConsoleLogStatements = new ConsoleLogStatements();
        try
        {
            try
            {
                // Read Deployments.json file data. 
                string getDeploymentsJson = System.IO.File.ReadAllText(getDirectory + "/JsonFile/Deployments.json");

                // Deserialize the JSON data into a list of Deployments objects. 
                var objdeployments = JsonConvert.DeserializeObject<List<Deployments>>(getDeploymentsJson);

                // Read Releases.json file data. 
                string GetReleasessJson = System.IO.File.ReadAllText(getDirectory + "/JsonFile/Releases.json");

                // Deserialize the JSON data into a list of Releases objects. 
                var objReleases = JsonConvert.DeserializeObject<List<Releases>>(GetReleasessJson);

                // Read Environments.json file data. 
                string environmentsJson = System.IO.File.ReadAllText(getDirectory + "/JsonFile/Environments.json");

                // Deserialize the JSON data into a list of Environments objects. 
                var objEnvironments = JsonConvert.DeserializeObject<List<Environments>>(environmentsJson);

                List<Deployments> deploymentsList = new List<Deployments>();

                // Check if objdeployments is not null and contains items.
                if (objdeployments != null && objdeployments.Count > 0)
                {
                    // Get projects based on the deployments, releases, and environments data.
                    ConsoleLogStatementses = ReleaseRetentionProjects.GetProjects(objdeployments, objReleases, objEnvironments);

                    // Print messages if any are found.
                    if (ConsoleLogStatementses.Count() > 0)
                    {
                        ConsoleLogStatementses.ForEach(ConsoleLogStatementses => Console.WriteLine(ConsoleLogStatementses.Messages + "\n"));
                    }

                    // Check if deployment files according to the environment exist.
                    ConsoleLogStatementses = ReleaseRetentionEnvironments.GetEnvironments(objdeployments);

                    // Check if deployment files according to the release exist and if the release files check if the project exists.
                    if (ConsoleLogStatementses.Count() > 0)
                    {
                        ConsoleLogStatementses.ForEach(ConsoleLogStatementses => Console.WriteLine(ConsoleLogStatementses.Messages + "\n"));
                    }

                    //GetRelease function will check Deployment file according to Release exist or not and Release file will check if Project exist or not 
                    ConsoleLogStatementses = ReleaseRetentionRelease.GetRelease(objdeployments, objReleases);

                    // Print messages if any are found.
                    if (ConsoleLogStatementses.Count() > 0)
                    {
                        ConsoleLogStatementses.ForEach(ConsoleLogStatementses => Console.WriteLine(ConsoleLogStatementses.Messages + "\n"));
                    }
                }

                else
                {
                    // If the deployment file is blank, log a message.
                    ConsoleLogStatements = new ConsoleLogStatements();
                    ConsoleLogStatements.Messages = "Deployment file is blank  ";
                    ConsoleLogStatementses.Add(ConsoleLogStatements);
                    if (ConsoleLogStatementses.Count() > 0)
                    {
                        ConsoleLogStatementses.ForEach(ConsoleLogStatementses => Console.WriteLine(ConsoleLogStatementses.Messages + "\n"));
                    }
                }

                // Wait for user input before closing the console.
                Console.Read();
            }

            catch (DirectoryNotFoundException ex)
            {
                // Log a message if a directory is not found.
                ConsoleLogStatements = new ConsoleLogStatements();
                ConsoleLogStatements.Messages = ex.Message;
                ConsoleLogStatementses.Add(ConsoleLogStatements);
                if (ConsoleLogStatementses.Count() > 0)
                {
                    ConsoleLogStatementses.ForEach(ConsoleLogStatementses => Console.WriteLine(ConsoleLogStatementses.Messages + "\n"));
                }
            }
        }

        catch (FileNotFoundException ex)
        {
            // Log a message if a file is not found.
            ConsoleLogStatements = new ConsoleLogStatements();
            ConsoleLogStatements.Messages = ex.Message;
            ConsoleLogStatementses.Add(ConsoleLogStatements);
            ConsoleLogStatementses = ConsoleLogStatementses.DistinctBy(s => s.Messages).Distinct().ToList();
            if (ConsoleLogStatementses.Count() > 0)
            {
                ConsoleLogStatementses.ForEach(ConsoleLogStatementses => Console.WriteLine(ConsoleLogStatementses.Messages + "\n"));
            }
        }

        catch (JsonReaderException ex)
        {
            // Log a message if there is an error reading JSON.
            ConsoleLogStatements = new ConsoleLogStatements();
            ConsoleLogStatements.Messages = ex.Message;
            ConsoleLogStatementses = ConsoleLogStatementses.DistinctBy(s => s.Messages).Distinct().ToList();
            if (ConsoleLogStatementses.Count() > 0)
            {
                ConsoleLogStatementses.ForEach(ConsoleLogStatementses => Console.WriteLine(ConsoleLogStatementses.Messages + "\n"));
            }

        }

        catch (Exception ex)
        {
            // Log a message for any other exceptions.
            ConsoleLogStatements = new ConsoleLogStatements();
            ConsoleLogStatements.Messages = ex.Message;
            ConsoleLogStatementses = ConsoleLogStatementses.DistinctBy(s => s.Messages).Distinct().ToList();
            ConsoleLogStatementses.Add(ConsoleLogStatements);
        }
    }
}