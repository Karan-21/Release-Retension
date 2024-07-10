using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseRetention
{
    public class Result
    {
        public string Id { get; set; }
        public string EnvironmentId { get; set; }
        public string ReleaseId { get; set; }
        public DateTime DeployedAt { get; set; }
        public string Version { get; set; }
        public string ProjectId { get; set; }
        public DateTime Created { get; set; }
    }
}
