using Newtonsoft.Json;
using ReleaseRetention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseRetention
{
    public class ReleaseRetentionRelease
    {
        // Get the directory of the currently executing assembly.
        public static string getDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        public static List<ConsoleLogStatements> GetRelease(List<Deployments> deployments, List<Releases> releases)
        {
            List<ConsoleLogStatements> ConsoleLogStatementsList = new List<ConsoleLogStatements>();
            try
            {
                if (deployments != null && deployments.Count() > 0)
                {
                    if (releases != null && releases.Count() > 0)
                    {
                        // Ensure deployments list contains unique ReleaseId.
                        deployments = deployments.DistinctBy(x => x.ReleaseId).ToList();

                        // Identify deployments with ReleaseIds not found in the releases list.
                        var releasesNotFoud = deployments.Where(x => !releases.Any(l => (l.Id == x.ReleaseId))).ToList();

                        if (releasesNotFoud != null && releasesNotFoud.Count() > 0)
                        {
                            for (int i = 0; i < releasesNotFoud.Count(); i++)
                            {
                                if (i == 0)
                                {
                                    Console.WriteLine("| Test Case 7: To check releaseId exist or not in Release json| \n");
                                    Console.WriteLine("##### Result: - \n");
                                }
                                ConsoleLogStatements ConsoleLogStatements = new ConsoleLogStatements();
                                ConsoleLogStatements.Messages = "Cannot find " + releasesNotFoud[i].ReleaseId + "\n";
                                ConsoleLogStatementsList.Add(ConsoleLogStatements);
                            }
                            if (ConsoleLogStatementsList.Count() > 0)
                            {
                                Console.WriteLine("##### Result: -\n");
                                ConsoleLogStatementsList.ForEach(ConsoleLogStatementses => Console.WriteLine(ConsoleLogStatementses.Messages + "\n"));
                            }
                        }

                        // Load projects from the JSON file.
                        string projectsJson = System.IO.File.ReadAllText(getDirectory + "/JsonFile/Projects.json");
                        var objProjects = JsonConvert.DeserializeObject<List<Projects>>(projectsJson);
                        if (objProjects != null && objProjects.Count() > 0)
                        {

                            deployments = deployments.DistinctBy(x => x.ReleaseId).ToList();

                            // Identify releases with ProjectIds not found in the projects list.
                            var projectsNotFoud = releases.Where(x => !objProjects.Any(l => (l.Id == x.ProjectId))).ToList();

                            if (projectsNotFoud != null && projectsNotFoud.Count() > 0)
                            {
                                for (int i = 0; i < projectsNotFoud.Count(); i++)
                                {
                                    if (i == 0)
                                    {
                                        Console.WriteLine("| Test Case 8: To check projectId exist or not in Project.json | \n");                                        
                                    }
                                    ConsoleLogStatements ConsoleLogStatements = new ConsoleLogStatements();
                                    ConsoleLogStatements.Messages = "Cannot find " + projectsNotFoud[i].ProjectId + "\n";
                                    ConsoleLogStatementsList.Add(ConsoleLogStatements);
                                }

                                if (ConsoleLogStatementsList.Count() > 0)
                                {
                                    Console.WriteLine("##### Result: -\n");
                                    ConsoleLogStatementsList.ForEach(ConsoleLogStatementses => Console.WriteLine(ConsoleLogStatementses.Messages + "\n"));
                                   
                                }
                            }
                        }

                        // Get the most recent deployment.
                        var objDeployments = deployments.OrderByDescending(x => x.DeployedAt).FirstOrDefault();

                        if (objDeployments != null)
                        {
                            // Get the most recent release associated with the recent deployment.
                            var objReleases = releases.Where(x => x.Id == objDeployments.ReleaseId).OrderByDescending(x => x.Created).FirstOrDefault();
                            if (objReleases != null)
                            {
                                var projectObj = objProjects.Where(x => x.Id == objReleases.ProjectId).FirstOrDefault();

                                if (projectObj != null)
                                {
                                    ConsoleLogStatementsList = new List<ConsoleLogStatements>();
                                    Console.WriteLine("| Test Case 9: To check project deployed to which version | \n");
                                    Console.WriteLine("##### Result: - \n");

                                    ConsoleLogStatements ConsoleLogStatements = new ConsoleLogStatements();
                                    ConsoleLogStatements.Messages = "Project Name: " + projectObj.Name + " with Version Id: " + objReleases.Version + " and deployment and release Ids " + objDeployments.Id + ", " + objReleases.Id + " respectively \n";
                                    ConsoleLogStatementsList.Add(ConsoleLogStatements);
                                }
                                else
                                {
                                    ConsoleLogStatements ConsoleLogStatements = new ConsoleLogStatements();
                                    ConsoleLogStatements.Messages = objDeployments.Id + " This deployment and this releases " + objReleases.Id + " cannot be found " + objReleases.ProjectId + "\n";
                                    ConsoleLogStatementsList.Add(ConsoleLogStatements);
                                }
                            }
                            else
                            {
                                // If no release found for the recent deployment's ReleaseId. 
                                ConsoleLogStatements ConsoleLogStatements = new ConsoleLogStatements();
                                ConsoleLogStatements.Messages = objDeployments.Id + " This deployment cannot be found " + objDeployments.ReleaseId + "\n";
                                ConsoleLogStatementsList.Add(ConsoleLogStatements);
                            }
                        }
                        else
                        {
                            // If no recent deployment found.
                            ConsoleLogStatements ConsoleLogStatements = new ConsoleLogStatements();
                            ConsoleLogStatements.Messages = " This deployment cannot be found \n";
                            ConsoleLogStatementsList.Add(ConsoleLogStatements);
                        }
                    }
                    else
                    {
                        // If releases list is null or empty. 
                        ConsoleLogStatements ConsoleLogStatements = new ConsoleLogStatements();
                        ConsoleLogStatements.Messages = "release file is blank \n";
                        ConsoleLogStatementsList.Add(ConsoleLogStatements);
                    }
                }
                return ConsoleLogStatementsList;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur.
                ConsoleLogStatements ConsoleLogStatements = new ConsoleLogStatements();
                ConsoleLogStatements.Messages = ex.Message;
                ConsoleLogStatementsList.Add(ConsoleLogStatements);
                ConsoleLogStatementsList.DistinctBy(x => x.Id);
                return ConsoleLogStatementsList;
            }
        }
    }
}