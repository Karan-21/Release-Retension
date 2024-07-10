# Release Retension

## Problem Statement
The project is to implement the Release Retention rule which determines which releases to keep. Given a set of **projects**, **environments**, **releases** and **deployments**: determine the releases that should be kept.

## Introduction
- This project reads JSON files containing data about deployments, releases, and environments, processes the data, deserializes it into C# classes, and and then performs various checks to ensure the correctness and consistency of the deployments then outputs the results to the console. The program also handles various exceptions that may occur during the file reading and processing operations.

## Prerequisites, Set-Up and Running the Project.
- Visual Studio 2022
- Installing .NET 8.0 or .NET 7.0 SDK 
- Newtonsoft.Json package
- Built both of the Solutions; `Release Retension` which is using the latest .NET 8.0 and `Unit Testing` which is using NUnit Framework.
- After Building `Run the Release Retension normally like console application` and `Run Unit Test Solution as Test`. 

## Setup
- Clone the repository to your local machine.
- `git clone https://github.com/Karan-21/Release-Retension`
- Ensure you have .NET 8.0 installed on your machine.
- Place the JSON files (Deployments.json, Releases.json, Environments.json, Projects.json) in a folder named **ReleaseRention/bin/Debug/net8.0** at the root of the project directory.
- Build and run the projects using your preferred .NET IDE or command line.

### Code Details
### Main Function
- The Main function is the entry point of the program. It performs the following steps:
1. Reads JSON files for deployments, releases, environments, and projects.
2. Deserializes the JSON data into C# objects.
3. Calls various functions to check the correctness of the deployments. Like: `ReleaseRetentionProjects`, `ReleaseRetentionEnvironments`, and `ReleaseRetentionRelease`.
4. Outputs the results to the console.

### GetProjects Function
- The GetProjects function performs the following steps:
1. Reads `Projects.json` file and deserializes its contents into `List<Projects`>.
2. Processes deployment data (Deployments.json), checks project releases, and performs test cases related to project deployments across environments.
3. Joins the deployment, release, and environment data to check various test cases.
- `Test Case 1`: Checks and logs projects deployed to single environments, retaining the most recent deployment.
- `Test Case 2`: Manages projects with multiple releases deployed to the same environment, retaining the most recent release.
- `Test Case 3`: Handles projects with releases deployed across different environments, retaining the most recent release per environment.
- `Test Case 4`: Counts and displays how many releases are deployed to each environment.
- `Test Case 5`: Identifies and logs the latest version of each project based on release data.
4. Outputs the results of each test case to the console.

### GetEnvironments Function
- The GetEnvironments function performs the following steps:
1. Reads `Environments.json` file and deserializes its contents into `List<Environments>`.
2. Checks if the environments specified in the deployments exist in the environments data.
3. Means compares environment IDs from Deployments.json with Environments.json.
4. Outputs any missing environments to the console.

## GetRelease Function
- The GetRelease function performs the following steps:
1. Processes deployment and release data (Deployments.json and Releases.json), performs checks, logs information about releases and associated projects.
2. Checks for missing releases in the Releases.json file based on deployment data.
3. Validates project associations for each release against Projects.json.
4. Retrieves the latest deployment information and associated release details.
5. Logs detailed information about projects, release versions, and deployment details.

#### Error Handling
- The program handles various exceptions, including:
1. DirectoryNotFoundException: Thrown when the specified directory cannot be found.
2. FileNotFoundException: Thrown when a file with the specified pathname does not exist.
3. JsonReaderException: Thrown when an error occurs while reading JSON text.
4. Exception: Handles all other exceptions.
5. In case of an error, the program outputs the error message to the console

#### Sample JSON Structure
- Here is an example structure for the JSON files:

**Deployments.json**
[
  {
    "Id": "deployment-1",
    "EnvironmentId": "Environment-1",
    "ReleaseId": "Release-1",
    "DeployedAt": "1/1/2000"
  }
]

**Releases.json**
[
  {
    "Id": "Release-1",
    "ProjectId": "Project-1",
    "Version": "1.0.0",
    "Created": "1/1/2000"
  }
]

**Environments.json**
[
  {
    "Id": "Environment-1",
    "Name": "Production"
  }
]

**Projects.json**
[
  {
    "Id": "Project-1",
    "Name": "Project Alpha"
  }
]

- By following the setup and usage instructions, you can run the program to verify the deployment data and ensure its correctness. The detailed function descriptions provided above insights into how the code processes and validates the JSON data.


# Example Outputs
| Test Case 1 |

Project-1 | Environment-1

| ----------- |  -------------- |

`Release-1` (Version: `1.0.0`, Created: `1/1/2000 9:00:00 AM`)  | `Deployment-1` (DeployedAt: `1/1/2000 10:00:00 AM`) (Environment: `Environment-1`)

##### Result: -

`Release-1` kept because it was the most recently deployed to `Environment-1` and `Deployment-1`



| Test Case 1 |

Project-2 | Environment-1

| ----------- |  -------------- |

`Release-5` (Version: `1.0.1-ci1`, Created: `1/1/2000 10:00:00 AM`)  | `Deployment-5` (DeployedAt: `1/1/2000 11:00:00 AM`) (Environment: `Environment-1`)

##### Result: -

`Release-5` kept because it was the most recently deployed to `Environment-1` and `Deployment-5`



| Test Case 2 |

Project-1 | Environment-1

| ----------- |  -------------- |

`Release-1` (Version: `1.0.0`, Created: `1/1/2000 9:00:00 AM`)  | `Deployment-1` (DeployedAt: `1/1/2000 10:00:00 AM`)  (Environment: `Environment-1`)

`Release-2` (Version: `1.0.1`, Created: `1/2/2000 9:00:00 AM`)  | `Deployment-2` (DeployedAt: `1/2/2000 10:00:00 AM`)  (Environment: `Environment-1`)

##### Result: -

`Release-2` kept because it was the most recently deployed to `Environment-1` and `Deployment-2`



| Test Case 2 |

Project-2 | Environment-1

| ----------- |  -------------- |

`Release-5` (Version: `1.0.1-ci1`, Created: `1/1/2000 10:00:00 AM`)  | `Deployment-5` (DeployedAt: `1/1/2000 11:00:00 AM`)  (Environment: `Environment-1`)

`Release-6` (Version: `1.0.2`, Created: `1/2/2000 9:00:00 AM`)  | `Deployment-6` (DeployedAt: `1/2/2000 10:00:00 AM`)  (Environment: `Environment-1`)

`Release-7` (Version: `1.0.3`, Created: `1/2/2000 12:00:00 PM`)  | `Deployment-8` (DeployedAt: `1/2/2000 1:00:00 PM`)  (Environment: `Environment-1`)

`Release-6` (Version: `1.0.2`, Created: `1/2/2000 9:00:00 AM`)  | `Deployment-9` (DeployedAt: `1/2/2000 2:00:00 PM`)  (Environment: `Environment-1`)

##### Result: -

`Release-6` kept because it was the most recently deployed to `Environment-1` and `Deployment-9`



| Test Case 2 |

Project-1 | Environment-2

| ----------- |  -------------- |

`Release-1` (Version: `1.0.0`, Created: `1/1/2000 9:00:00 AM`)  | `Deployment-3` (DeployedAt: `1/2/2000 11:00:00 AM`)  (Environment: `Environment-2`)

##### Result: -

`Release-1` kept because it was the most recently deployed to `Environment-2` and `Deployment-3`



| Test Case 2 |

Project-2 | Environment-2

| ----------- |  -------------- |

`Release-6` (Version: `1.0.2`, Created: `1/2/2000 9:00:00 AM`)  | `Deployment-7` (DeployedAt: `1/2/2000 11:00:00 AM`)  (Environment: `Environment-2`)

##### Result: -

`Release-6` kept because it was the most recently deployed to `Environment-2` and `Deployment-7`



| Test Case 3 |

| Project-1 | Environment-1 | Environment-2 |

| ----------- |  -------------- |

`Release-1` (Version: `1.0.0`, Created: `1/1/2000 9:00:00 AM`)  | `Deployment-1` (DeployedAt: `1/1/2000 10:00:00 AM`) (Environment: `Environment-1`)

`Release-2` (Version: `1.0.1`, Created: `1/2/2000 9:00:00 AM`)  | `Deployment-2` (DeployedAt: `1/2/2000 10:00:00 AM`) (Environment: `Environment-1`)

`Release-1` (Version: `1.0.0`, Created: `1/1/2000 9:00:00 AM`)  | `Deployment-3` (DeployedAt: `1/2/2000 11:00:00 AM`) (Environment: `Environment-2`)

##### Result: -

`Release-1` kept because it was the most recently deployed to `Environment-2` and `Deployment-3`

`Release-2` kept because it was the most recently deployed to `Environment-1` and `Deployment-2`


| Test Case 3 |

| Project-2 | Environment-1 | Environment-2 |

| ----------- |  -------------- |

`Release-5` (Version: `1.0.1-ci1`, Created: `1/1/2000 10:00:00 AM`)  | `Deployment-5` (DeployedAt: `1/1/2000 11:00:00 AM`) (Environment: `Environment-1`)

`Release-6` (Version: `1.0.2`, Created: `1/2/2000 9:00:00 AM`)  | `Deployment-6` (DeployedAt: `1/2/2000 10:00:00 AM`) (Environment: `Environment-1`)

`Release-6` (Version: `1.0.2`, Created: `1/2/2000 9:00:00 AM`)  | `Deployment-7` (DeployedAt: `1/2/2000 11:00:00 AM`) (Environment: `Environment-2`)

`Release-7` (Version: `1.0.3`, Created: `1/2/2000 12:00:00 PM`)  | `Deployment-8` (DeployedAt: `1/2/2000 1:00:00 PM`) (Environment: `Environment-1`)

`Release-6` (Version: `1.0.2`, Created: `1/2/2000 9:00:00 AM`)  | `Deployment-9` (DeployedAt: `1/2/2000 2:00:00 PM`) (Environment: `Environment-1`)

##### Result: -

`Release-6` kept because it was the most recently deployed to `Environment-1` and `Deployment-9`

`Release-6` kept because it was the most recently deployed to `Environment-2` and `Deployment-7`



| Test Case 4: How Many Releases To Environments

| ----------- |  -------------- |

##### Result: -

Environment:- Environment-1
Total Releases:- 7

Releases Id:- Release-1

Releases Id:- Release-2

Releases Id:- Release-5

Releases Id:- Release-6

Releases Id:- Release-7

Releases Id:- Release-6

Releases Id:- Release-8


Environment:- Environment-2
Total Releases:- 2

Releases Id:- Release-1

Releases Id:- Release-6


| Test Case 5: Check Project Release Latest Version |

| ----------- |  -------------- |

##### Result: -

`Project-1` Latest Version is (Version: `1.0.0` | Created: `1/1/2000 9:00:00 AM`)

`Project-2` Latest Version is (Version: `1.0.0` | Created: `1/1/2000 9:00:00 AM`)


| Test Case 6: To check environmentId exist or not in Environment json|

##### Result: -

Cannot find `Environment-3`


| Test Case 8: To check projectId exist or not in Project.json |

##### Result: -

Cannot find `Project-3`


| Test Case 9: To check project deployed to which version |

##### Result: -

Project Name: `Pet Shop` with Version Id: `1.0.3` and deployment and release Ids `Deployment-8`, `Release-7` respectively.