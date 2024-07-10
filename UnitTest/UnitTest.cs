using Newtonsoft.Json;
using ReleaseRetention;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        // Get the directory of the currently executing assembly.
        public static string getDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        [TestMethod]
        public void TestMethod()
        {
            // Call the Main method of the Program class.
            Program.Main();
        }

        [TestMethod]
        public void TestMethod1()
        {
            // Read Deployments JSON file data.
            string getDeploymentsJson = System.IO.File.ReadAllText(getDirectory + "/JsonFile/Deployments.json");

            // Deserialize the JSON data into a list of Deployments objects. 
            var objdeployments = JsonConvert.DeserializeObject<List<Deployments>>(getDeploymentsJson);

            // Read Releases JSON file data.
            string GetReleasessJson = System.IO.File.ReadAllText(getDirectory + "/JsonFile/Releases.json");

            // Deserialize the JSON data into a list of Releases objects. 
            var objReleases = JsonConvert.DeserializeObject<List<Releases>>(GetReleasessJson);

            // Read Environments JSON file data.
            string environmentsJson = System.IO.File.ReadAllText(getDirectory + "/JsonFile/Environments.json");

            // Deserialize the JSON data into a list of Environments objects.
            var objEnvironments = JsonConvert.DeserializeObject<List<Environments>>(environmentsJson);

            // Call the GetProjects method with the deserialized data.
            ReleaseRetentionProjects.GetProjects(objdeployments, objReleases, objEnvironments);
        }
        public void TestMethod2()
        {
            // Read Deployments JSON file data.
            string getDeploymentsJson = System.IO.File.ReadAllText(getDirectory + "/JsonFile/Deployments.json");

            // Deserialize the JSON data into a list of Deployments objects.
            var objdeployments = JsonConvert.DeserializeObject<List<Deployments>>(getDeploymentsJson);

            // Call the GetEnvironments method with the deserialized data.
            ReleaseRetentionEnvironments.GetEnvironments(objdeployments);
        }

        public void TestMethod3()
        {
            // Read Deployments JSON file data.
            string getDeploymentsJson = System.IO.File.ReadAllText(getDirectory + "/JsonFile/Deployments.json");

            // Deserialize the JSON data into a list of Deployments objects.
            var objdeployments = JsonConvert.DeserializeObject<List<Deployments>>(getDeploymentsJson);

            // Read Releases JSON file data.
            string GetReleasessJson = System.IO.File.ReadAllText(getDirectory + "/JsonFile/Releases.json");

            // Deserialize the JSON data into a list of Releases objects.
            var objReleases = JsonConvert.DeserializeObject<List<Releases>>(GetReleasessJson);

            // Call the GetRelease method with the deserialized data.
            ReleaseRetentionRelease.GetRelease(objdeployments, objReleases);
        }

    }
}