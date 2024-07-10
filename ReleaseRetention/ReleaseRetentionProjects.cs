using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseRetention
{
    public class ReleaseRetentionProjects
    {
        // Get the directory of the currently executing assembly.
        public static string getDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        public static List<ConsoleLogStatements> GetProjects(List<Deployments> deployments, List<Releases> releases, List<Environments> environments)
        {
            List<ConsoleLogStatements> ConsoleLogStatementsList = new List<ConsoleLogStatements>();
            try
            {
                // Check if deployments list is not null and contains items.
                if (deployments != null && deployments.Count() > 0)
                {
                    try
                    {
                        // Get Projects JSON file data.
                        string getProjectsJson = System.IO.File.ReadAllText(getDirectory + "/JsonFile/Projects.json");

                        // Deserialize the JSON data into a list of Projects objects.
                        var objProjects = JsonConvert.DeserializeObject<List<Projects>>(getProjectsJson);

                        // Check if environments list is not null and contains items.
                        if (environments != null && environments.Count() > 0)
                        {
                            #region #### Test Case: 1 Release, Keep 1
                            for (int j = 0; j < environments.Count(); j++)
                            {
                                if (objProjects != null && objProjects.Count() > 0)
                                {
                                    for (int i = 0; i < objProjects.Count(); i++)
                                    {
                                        // Find all matching data based on Project-Environment relationship.  
                                        List<Result> releasesEnvironment = (from dep in deployments
                                                                            join env in environments on dep.EnvironmentId equals env.Id
                                                                            join rel in releases on dep.ReleaseId equals rel.Id
                                                                            where rel.ProjectId == objProjects[i].Id
                                                                            join pro in objProjects on rel.ProjectId equals pro.Id
                                                                            where dep.EnvironmentId == environments[j].Id
                                                                            select new Result
                                                                            {
                                                                                Id = dep.Id,
                                                                                EnvironmentId = dep.EnvironmentId,
                                                                                ReleaseId = dep.ReleaseId,
                                                                                DeployedAt = dep.DeployedAt,
                                                                                Version = rel.Version,
                                                                                ProjectId = rel.ProjectId,
                                                                                Created = rel.Created
                                                                            }).ToList();

                                        if (releasesEnvironment != null && releasesEnvironment.Count() > 0)
                                        {

                                            if (releasesEnvironment.Count() > 1)
                                            {
                                                Console.WriteLine("| Test Case 1 | \n");
                                                Console.WriteLine(releasesEnvironment[0].ProjectId + " | " + releasesEnvironment[0].EnvironmentId + " \n");
                                                Console.WriteLine("| ----------- |  -------------- | \n");
                                                Console.WriteLine("`" + releasesEnvironment[0].ReleaseId + "` (Version: `" + releasesEnvironment[0].Version + "`, Created: `" + releasesEnvironment[0].Created + "`)  | `" + releasesEnvironment[0].Id + "` (DeployedAt: `" + releasesEnvironment[0].DeployedAt + "`) (Environment: `" + releasesEnvironment[0].EnvironmentId + "`) \n");

                                                Console.WriteLine("##### Result: - \n");
                                                Console.WriteLine("`" + releasesEnvironment[0].ReleaseId + "` kept because it was the most recently deployed to `" + releasesEnvironment[0].EnvironmentId + "` and `" + releasesEnvironment[0].Id + "` \n");
                                                Console.WriteLine("\n");
                                            }
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region #### Test Case: 2 Releases deployed to the same environment, Keep 1
                            // Loop through environments and projects to find matching data based on Project-Environment relationship.
                            for (int k = 0; k < environments.Count(); k++)
                            {
                                if (objProjects != null && objProjects.Count() > 0)
                                {
                                    for (int n = 0; n < objProjects.Count(); n++)
                                    {
                                        // Find all matching data based on Project-Environment relationship.  
                                        List<Result> releasesEnvironment = (from dep in deployments
                                                                            join env in environments on dep.EnvironmentId equals env.Id
                                                                            join rel in releases on dep.ReleaseId equals rel.Id
                                                                            where rel.ProjectId == objProjects[n].Id
                                                                            join pro in objProjects on rel.ProjectId equals pro.Id
                                                                            where dep.EnvironmentId == environments[k].Id
                                                                            select new Result
                                                                            {
                                                                                Id = dep.Id,
                                                                                EnvironmentId = dep.EnvironmentId,
                                                                                ReleaseId = dep.ReleaseId,
                                                                                DeployedAt = dep.DeployedAt,
                                                                                Version = rel.Version,
                                                                                ProjectId = rel.ProjectId,
                                                                                Created = rel.Created
                                                                            }).ToList();
                                        if (releasesEnvironment != null && releasesEnvironment.Count() > 0)
                                        {
                                            for (int i = 0; i < releasesEnvironment.Count(); i++)
                                            {
                                                if (i == 0)
                                                {
                                                    Console.WriteLine("| Test Case 2 | \n");
                                                    Console.WriteLine(releasesEnvironment[i].ProjectId + " | " + releasesEnvironment[i].EnvironmentId + " \n");
                                                    Console.WriteLine("| ----------- |  -------------- | \n");
                                                }
                                                Console.WriteLine("`" + releasesEnvironment[i].ReleaseId + "` (Version: `" + releasesEnvironment[i].Version + "`, Created: `" + releasesEnvironment[i].Created + "`)  | `" + releasesEnvironment[i].Id + "` (DeployedAt: `" + releasesEnvironment[i].DeployedAt + "`)  (Environment: `" + releasesEnvironment[i].EnvironmentId + "`)   \n");
                                                Console.WriteLine("\n");
                                            }

                                            if (releasesEnvironment.Count > 0)
                                            {
                                                Console.WriteLine("##### Result: -\n");
                                                var objreleasesdeployed = releasesEnvironment.Where(x => x.EnvironmentId == environments.Where(x => x.Id == environments[k].Id).Select(x => x.Id).FirstOrDefault()).OrderByDescending(x => x.DeployedAt).FirstOrDefault();
                                                Console.WriteLine("`" + objreleasesdeployed.ReleaseId + "` kept because it was the most recently deployed to `" + objreleasesdeployed.EnvironmentId + "` and `" + objreleasesdeployed.Id + "` \n");
                                                Console.WriteLine("\n");
                                            }
                                        }
                                    }
                                }
                            }

                            #endregion


                            #region #### Test Case: 3 Releases deployed to different environments, Keep 1
                            // Loop through projects to find matching data based on Project-Environment relationship.
                            if (objProjects != null && objProjects.Count() > 0)
                            {
                                for (int n = 0; n < objProjects.Count(); n++)
                                {
                                    List<Result> releasesdeployed = (from dep in deployments
                                                                     join env in environments on dep.EnvironmentId equals env.Id
                                                                     join rel in releases on dep.ReleaseId equals rel.Id
                                                                     where rel.ProjectId == objProjects[n].Id
                                                                     join pro in objProjects on rel.ProjectId equals pro.Id
                                                                     select new Result
                                                                     {
                                                                         Id = dep.Id,
                                                                         EnvironmentId = dep.EnvironmentId,
                                                                         ReleaseId = dep.ReleaseId,
                                                                         DeployedAt = dep.DeployedAt,
                                                                         Version = rel.Version,
                                                                         ProjectId = rel.ProjectId,
                                                                         Created = rel.Created
                                                                     }).ToList();

                                    if (releasesdeployed != null && releasesdeployed.Count() > 0)
                                    {
                                        bool isCheckEnvironmentSameOrNot = false;
                                        bool isCheckReleaseSameOrNot = false;
                                        List<ConsoleLogStatements> consoleLogStatementsList = new List<ConsoleLogStatements>();
                                        List<ConsoleLogStatements> consoleLogStatementsTestCase = new List<ConsoleLogStatements>();
                                        string EnvironmentId = string.Empty;

                                        for (int m = 0; m < releasesdeployed.Count(); m++)
                                        {
                                            if (m == 0)
                                            {
                                                EnvironmentId = releasesdeployed[m].EnvironmentId;
                                                Console.WriteLine("| Test Case 3 | \n");
                                                Console.WriteLine("| " + objProjects[n].Id + " | Environment-1 | Environment-2 | \n");
                                                Console.WriteLine("| ----------- |  -------------- | \n");
                                            }
                                            Console.WriteLine("`" + releasesdeployed[m].ReleaseId + "` (Version: `" + releasesdeployed[m].Version + "`, Created: `" + releasesdeployed[m].Created + "`)  | `" + releasesdeployed[m].Id + "` (DeployedAt: `" + releasesdeployed[m].DeployedAt + "`) (Environment: `" + releasesdeployed[m].EnvironmentId + "`)  \n");
                                        }


                                        var objkept = releasesdeployed.OrderByDescending(x => x.DeployedAt).DistinctBy(x => x.EnvironmentId).ToList();
                                        if (objkept != null && objkept.Count > 0)
                                        {
                                            for (int j = 0; j < objkept.Count(); j++)
                                            {
                                                if (j == 0)
                                                {
                                                    Console.WriteLine("##### Result: -\n");
                                                }
                                                Console.WriteLine("`" + objkept[j].ReleaseId + "` kept because it was the most recently deployed to `" + objkept[j].EnvironmentId + "` and `" + objkept[j].Id + "` \n");
                                            }
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region #### Test Case: 4 Test Case 5 How Many Releases To 1 Environment
                            // Loop through environments to get the count of releases for each environment.
                            if (environments != null && environments.Count() > 0)
                            {
                                Console.WriteLine("\n| Test Case 4: How Many Releases To Environments \n");
                                Console.WriteLine("| ----------- |  -------------- | \n");
                                Console.WriteLine("##### Result: - \n");

                                for (int i = 0; i < environments.Count(); i++)
                                {

                                    //Get how many releases according to environmentId
                                    var howManyReleases = deployments.Where(x => x.EnvironmentId == environments[i].Id).ToList();
                                    if (howManyReleases != null && howManyReleases.Count() > 0)
                                    {
                                        for (int j = 0; j < howManyReleases.Count(); j++)
                                        {
                                            if (j == 0)
                                            {
                                                Console.WriteLine("Environment:- " + howManyReleases[j].EnvironmentId);
                                                Console.WriteLine("Total Releases:- " + howManyReleases.Count() + " \n");
                                            }

                                            Console.WriteLine("Releases Id:- " + howManyReleases[j].ReleaseId + " \n");
                                        }
                                    }
                                }
                                Console.WriteLine("\n");
                            }

                            #endregion

                            #region #### Test Case: 5 Check Project Release Latest Version
                            // Loop through projects to find the latest release version for each project.
                            if (objProjects != null && objProjects.Count() > 0)
                            {
                                Console.WriteLine("| Test Case 5: Check Project Release Latest Version | \n");
                                Console.WriteLine("| ----------- |  -------------- | \n");
                                Console.WriteLine("##### Result: - \n");

                                for (int i = 0; i < objProjects.Count(); i++)
                                {
                                    var releasesVersion = releases.DistinctBy(x => x.ProjectId).OrderByDescending(x => x.Created).Where(x => x.ProjectId == objProjects[i].Id).FirstOrDefault();
                                    if (releasesVersion != null)
                                    {
                                        Console.WriteLine("`" + releasesVersion.ProjectId + "` Latest Version is (Version: `" + releasesVersion.Version + "` | Created: `" + releasesVersion.Created + "`)" + "\n");
                                    }
                                }
                                Console.WriteLine("\n");
                            }

                            #endregion
                        }
                        else
                        {
                            // If environments in the projects.json are not found.
                            ConsoleLogStatements ConsoleLogStatements = new ConsoleLogStatements();
                            ConsoleLogStatements.Messages = "Project file is blank";
                            ConsoleLogStatementsList.Add(ConsoleLogStatements);
                        }
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        // Log a message if a directory is not found.
                        ConsoleLogStatements ConsoleLogStatements = new ConsoleLogStatements();
                        ConsoleLogStatements.Messages = ex.Message;
                        ConsoleLogStatementsList.Add(ConsoleLogStatements);
                        ConsoleLogStatementsList.DistinctBy(x => x.Messages);
                        Console.WriteLine(ConsoleLogStatementsList);
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
                // Log a message if a directory is not found.
                ConsoleLogStatements ConsoleLogStatements = new ConsoleLogStatements();
                ConsoleLogStatements.Messages = ex.Message;
                ConsoleLogStatementsList.Add(ConsoleLogStatements);
                ConsoleLogStatementsList.DistinctBy(x => x.Id);
                return ConsoleLogStatementsList;
            }
        }
    }
}
