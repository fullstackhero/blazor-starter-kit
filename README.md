<!-- PROJECT SHIELDS -->

[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

[contributors-shield]: https://img.shields.io/github/contributors/blazorhero/CleanArchitecture.svg?style=flat-square
[contributors-url]: https://github.com/blazorhero/CleanArchitecture/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/blazorhero/CleanArchitecture?style=flat-square
[forks-url]: https://github.com/blazorhero/CleanArchitecture/network/members
[stars-shield]: https://img.shields.io/github/stars/blazorhero/CleanArchitecture.svg?style=flat-square
[stars-url]: https://img.shields.io/github/stars/blazorhero/CleanArchitecture?style=flat-square
[issues-shield]: https://img.shields.io/github/issues/blazorhero/CleanArchitecture?style=flat-square
[issues-url]: https://github.com/blazorhero/CleanArchitecture/issues
[license-shield]: https://img.shields.io/github/license/blazorhero/CleanArchitecture?style=flat-square
[license-url]: https://github.com/blazorhero/CleanArchitecture/blob/master/LICENSE
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=flat-square&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/iammukeshm/

<!-- PROJECT LOGO -->
<br />
<p align="center">
  <a href="https://github.com/blazorhero/CleanArchitecture">
    <img src="https://codewithmukesh.com/wp-content/uploads/2021/03/BlazorHeroBanner-1.png" alt="Blazor Hero">
  </a>
  <h3 align="center">BlazorHero - Clean Architecture Template</h3>
  <p align="center">
    Open Sourced Solution Template For Blazor Web-Assembly 6.0 built with MudBlazor Components
    <br />
    <a href="https://codewithmukesh.com/blog/blazor-hero-quick-start-guide/"><strong>Read the Documentation »</strong></a>
    <br />
    <br />
    <a href="https://github.com/blazorhero/CleanArchitecture/issues">Report Bug</a>
    ·
    <a href="https://github.com/blazorhero/CleanArchitecture/issues">Request Feature</a>
    .
    <a href="https://github.com/blazorhero/CleanArchitecture/issues">Request Documentation</a>
  </p>
</p>

## About The Project :zap:

BlazorHero is a Clean Architecture Solution Template for Blazor Webassembly 6.0 built with MudBlazor Components.

## Complete Overview - Youtube Video 🆕 📈

So, here is an in-depth video that takes you through the BlazorHero Project! Do Like & Subscribe to my Youtube channel! It would be great if you could leave behind your valuable feedback in the comments section of the Video. This helps me reach a much wider audience with time :)

<a href="https://www.youtube.com/watch?v=j7BxKN7HYjk&lc=Ugy1SjDcmP0pboRxJe94AaABAg">Watch it here!</a>

[![Blazor Hero - Clean Architecture Solution Template for Blazor WebAssembly](http://img.youtube.com/vi/j7BxKN7HYjk/0.jpg)](http://www.youtube.com/watch?v=j7BxKN7HYjk "Blazor Hero - Clean Architecture Solution Template for Blazor WebAssembly")

### Tech Stack :muscle:

- Blazor WebAssembly 6.0 - ASP.NET Core Hosted Model
- [Entity Framework Core 6.0](https://docs.microsoft.com/en-us/ef/core/)

# BlazorHero v2.2

- UI Improvements
- Docker Support
- Better Permissions Management
- Code Cleanups
- RTL Support
- Minor Bug Fixes
- Better Project Structure

# What to Expect in BlazorHero 3.0?

- Modular Architecture
- Cleaner Separation Of Code
- Dedicated Documentation Website - [Here](https://blazorhero.github.io/docs/)
- Tutorials to add new entities, controllers
- UI Updates
- Support for PostgreSQL / MySQL - Easy DB Switching
- Theme Manager Integration to change UI Color Palletes / Fonts on the go.
- You can suggest your requirements as well!

# Down the Roadmap

 - Migration to .NET 6
 - Multi Tenancy
 - Better Localization - JSON

# Getting Started 🦸

> **Important**
If you are already using Blazor Hero v1.x, make sure that you drop your existing database and re-update your database using the CLI as there are a couple of new migrations added that might clash with your existing schema. Also, install the latest version of BlazorHero.

The easiest way to get started with Blazor Hero is to install the [NuGet package](https://www.nuget.org/packages/BlazorHero.CleanArchitecture/) and run `dotnet new BlazorHero.CleanArchitecture`:

1. Install the latest [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
2. Install the latest DOTNET & EF CLI Tools by using this command `dotnet tool install --global dotnet-ef` 
3. Install the latest version of Visual Studio IDE 2019 (v16.8 and above) 🚀
4. Open up Command Prompt and run `dotnet new --install BlazorHero.CleanArchitecture` to install the project template
5. Create a folder for your solution and cd into it (the template will use it as project name)
6. Run `dotnet new BlazorHero.CleanArchitecture` to create a new Solution with all the Awesomeness 🕶️ of BlazorHero 🦸

What to do next? Read the [entire guide on my blog](https://codewithmukesh.com/blog/blazor-hero-quick-start-guide/).

## Getting Started with Docker in Windows :rocket:

- Install Docker on Windows via `https://docs.docker.com/docker-for-windows/install/`
- Open up Powershell on Windows and run the following
    - `cd c:\`
    - `dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p securePassword123`
    - `dotnet dev-certs https --trust`
    - Note - Make sure that you use the same password that has been configured in the `docker-compose.yml` file. By default, `securePassword123` is configured.
- 5005 & 5006 are the ports setup to run blazorHero on Docker, so make sure that these ports are free. You could also change the ports in the `docker-compose.yml` and `Server\Dockerfile` files.
- Now navigate back to the root of the BlazorHero Project on your local machine and run the following via terminal - `docker-compose -f 'docker-compose.yml' up --build`
- This will start pulling MSSQL Server Image from Docker Hub if you don't already have this image. It's around 500+ Mbs of download.
- Once that is done, dotnet SDKs and runtimes are downloaded, if not present already. That's almost 200+ more Mbs of download.
- PS If you find any issues while Docker installs the nuget packages, it is most likely that your ssl certificates are not installed properly. Apart from that I also added the `--disable-parallel` in the `Server\Dockerfile`to ensure network issues don't pop-up. You can remove this option to speed up the build process.
- That's almost everything. Once the containers are available, migrations are updated in the MSSQL DB, default data is seeded.
- Browse to https://localhost:5005/ to use your version of BlazorHero !

# Complete Documentation :rocket:

Getting started with Blazor Hero – A Clean Architecture Template built for Blazor WebAssembly using MudBlazor Components. This project will make your Blazor Learning Process much easier than you anticipate. Blazor Hero is meant to be an Enterprise Level Boilerplate, which comes free of cost, completely open sourced. 

The provided documentation / guide will get you started with BlazorHero in no time. It provides a complete walkthrough for the project with to-the-point guides and notes.

<a href="https://codewithmukesh.com/blog/blazor-hero-quick-start-guide/"><strong>Read the Quick Start Guide</strong></a>

# Features

All the completed and the upcoming features are mentioned in the [Features.MD File](https://github.com/blazorhero/CleanArchitecture/blob/master/Features.md)

## Contributing

Contributions are what make the open-source community such an amazing place to be, learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

Here are the few contributions that I would highly appreciate ;)

- [ ] Need someone to add in the API Documentation for Swagger.
- [ ] Need someone to implement localization throughout every Razor Component of the solution under the WASM(Client) Project. You can take the Pages/Authentication/Login.razor as the point of reference. It is as simple as adding `@inject Microsoft.Extensions.Localization.IStringLocalizer<Login> localizer` to every page, changing the texts to `@localizer["Text Here"]` and finally adding resx files to the Resources Folder as per the folder structure.
- [ ] Need few contributors to add in various language translations as per the implemented Location. I got time to only add a few translations for French as of now.
- [ ] Need a UI contributor to look at the UX/UI of the entire project
- [ ] Need someone to buildup a cool Material Logo for BlazorHero (BH):D Do contact me on LinkedIn (https://www.linkedin.com/in/iammukeshm/).
- [ ] And finally, Stars from everyone! :D

## License

Distributed under the MIT License.

## Contact
### Mukesh Murugan

-   Blogs at [codewithmukesh.com](https://www.codewithmukesh.com)
-   Facebook - [codewithmukesh](https://www.facebook.com/codewithmukesh)
-   Twitter - [Mukesh Murugan](https://www.twitter.com/iammukeshm)
-   Twitter - [codewithmukesh](https://www.twitter.com/codewithmukesh)
-   Linkedin - [Mukesh Murugan](https://www.linkedin.com/in/iammukeshm/)

## Support :star:

Has this Project helped you learn something New? or Helped you at work? Do Consider Supporting. 
Here are a few ways by which you can support.

-   Leave a star! :star:
-   Recommend this awesome project to your colleagues. 🥇
-   Leave your feedback / comments regarding this project in the comments section on my blog [Blazor Hero Blog](https://codewithmukesh.com/blog/blazor-hero-quick-start-guide/)
-   Do consider endorsing me on LinkedIn for ASP.NET Core - [Connect via LinkedIn](https://codewithmukesh.com/linkedin) 🦸
-   Or, If you want to support this project on the long run, [consider buying me a coffee](https://www.buymeacoffee.com/codewithmukesh)! ☕

<a href="https://www.buymeacoffee.com/codewithmukesh" target="_blank"><img src="https://codewithmukesh.com/wp-content/uploads/2021/04/bmclogo.jpg" alt="Buy Me A Coffee" width="200"  style="height: 60px !important;width: 200px !important;" ></a>
