using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseRetention
{
    public class ReleaseRetentionEnvironments
    {
        // Get the directory of the currently executing assembly.
        public static string getDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        public static List<ConsoleLogStatements> GetEnvironments(List<Deployments> deployments)
        {
            List<ConsoleLogStatements> ConsoleLogStatementsList = new List<ConsoleLogStatements>();
            try
            {
                // Check if deployments list is not null and contains items.
                if (deployments != null && deployments.Count() > 0)
                {
                    // Read Environments.json file data.
                    string environmentsJson = System.IO.File.ReadAllText(getDirectory + "/JsonFile/Environments.json");

                    // Deserialize the JSON data into a list of Environments objects.
                    var objEnvironments = JsonConvert.DeserializeObject<List<Environments>>(environmentsJson);

                    // Check if objEnvironments is not null and contains items.
                    if (objEnvironments != null && objEnvironments.Count() > 0)
                    {
                        // Get distinct deployments based on EnvironmentId.
                        deployments = deployments.DistinctBy(x => x.EnvironmentId).ToList();

                        // Find environments not found in Environments.json file. 
                        var environmentsnotfoud = deployments.Where(x => !objEnvironments.Any(l => (l.Id == x.EnvironmentId))).ToList();

                        if (environmentsnotfoud != null && environmentsnotfoud.Count() > 0)
                        {
                            for (int i = 0; i < environmentsnotfoud.Count(); i++)
                            {
                                if (i == 0) 
                                {
                                    Console.WriteLine("| Test Case 6: To check environmentId exist or not in Environment json| \n");
                                    Console.WriteLine("##### Result: - \n");
                                }
                                ConsoleLogStatements ConsoleLogStatements = new ConsoleLogStatements();
                                ConsoleLogStatements.Messages = "Cannot find " + environmentsnotfoud[i].EnvironmentId + "\n";
                                ConsoleLogStatementsList.Add(ConsoleLogStatements);
                            }
                        }
                    }
                    else
                    {
                        // If environments in the environments.json are not found. 
                        ConsoleLogStatements ConsoleLogStatements = new ConsoleLogStatements();
                        ConsoleLogStatements.Messages = "Environment file is blank";
                        ConsoleLogStatementsList.Add(ConsoleLogStatements);
                    }
                }
                return ConsoleLogStatementsList;
            }
            catch (DirectoryNotFoundException ex)
            {
                // Log a message if a directory is not found.
                ConsoleLogStatements ConsoleLogStatements = new ConsoleLogStatements();
                ConsoleLogStatements.Messages = ex.Message;
                ConsoleLogStatementsList.Add(ConsoleLogStatements);
                ConsoleLogStatementsList.DistinctBy(x => x.Id);
                return ConsoleLogStatementsList;
            }
            catch (Exception ex)
            {
                // Log a message for any other exceptions.
                ConsoleLogStatements ConsoleLogStatements = new ConsoleLogStatements();
                ConsoleLogStatements.Messages = ex.Message;
                ConsoleLogStatementsList.Add(ConsoleLogStatements);
                ConsoleLogStatementsList.DistinctBy(x => x.Id);
                return ConsoleLogStatementsList;
            }
        }
    }
}
