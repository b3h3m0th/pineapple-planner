<a id="readme-top"></a>

<br />
<div align="center">
  <a href="https://github.com/b3h3m0th/pineapple-planner">
    <img src="PineapplePlanner/PineapplePlanner.Wpf/wwwroot/images/pineapple_planner_logo.png" alt="Logo" width="80" height="80">
  </a>
  <h3 align="center">PineapplePlanner</h3>
  <p align="center">Your tasks, served fresh</p>
</div>

<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#contributors">Contributors</a></li>
    <li><a href="#license">License</a></li>
  </ol>
</details>

## About The Project

The PineapplePlanner is a task management application.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Built With

- [![C#][C#]][C#-url]
- [![Blazor][Blazor]][Blazor-url]

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Getting Started

To get a local copy up and running follow these simple steps.

### Prerequisites

- .NET 8.0

### Installation

1. Clone the repo
   ```sh
   git clone https://github.com/b3h3m0th/pineapple-planner.git
   ```
2. Add your Firebase service account key `firebase-adminsdk-key.json` to the root of the `PineapplePlanner.Wpf` project. You can generate a service account key in the Firebase console > Project settings > Service accounts > Generate new private key.
3. Add your Firebase web config `firebase-config.json` to the `wwwroot` folder of the `PineapplerPlanner.Wpf` project. You can find the web config in the Firebase console > Project settings > General > Your apps.
4. Add `appsettings.json` according to the `appsettings.example.json` to the root of the `PineapplePlanner.WPF` project. You can generate a Google generative AI API key here: https://aistudio.google.com/app/apikey.
5. Open `PineapplePlanner.sln` in Visual Studio.
6. In Visual Studio configure `PineapplePlanner.Wpf` as startup project.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Usage

TBA

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Contributors

<a href="https://github.com/b3h3m0th/pineapple-planner/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=b3h3m0th/pineapple-planner" alt="contrib.rocks image" />
</a>

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## License

Distributed under the MIT License. See `LICENSE` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

[Blazor]: https://img.shields.io/badge/blazor-%235C2D91.svg?style=for-the-badge&logo=blazor&logoColor=white
[Blazor-url]: https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor
[C#]: https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white
[C#-url]: https://learn.microsoft.com/en-us/dotnet/csharp/
