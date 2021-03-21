<!-- PROJECT LOGO -->
<br />
<p align="center">
  <a href="https://github.com/blazorhero/CleanArchitecture">
    <img src="https://codewithmukesh.com/wp-content/uploads/2021/03/BlazorHeroBanner.png" alt="Blazor Hero">
  </a>
  <h3 align="center">BlazorHero - Clean Architecture Template</h3>
  <p align="center">
    Open Sourced Solution Template For Blazor Web-Assembly 5.0 built with MudBlazor Components
    <br />
    <a href="https://codewithmukesh.com/blog/blazor-hero-quick-start-guide/"><strong>Read the Documentation »</strong></a>
    <br />
    <br />
    <a href="https://github.com/blazorhero/CleanArchitecture/issues">Report Bug</a>
    ·
    <a href="https://github.com/blazorhero/CleanArchitecture/issues">Request Feature</a>
  </p>
</p>

## About The Project :zap:

Clean Architecture Solution Template for ASP.NET Core 5.0. Built with Onion/Hexagonal Architecture and incorporates the most essential Packages your projects will ever need. Includes both WebApi and Web(MVC) Projects.

### Tech Stack :muscle:

- Blazor WebAssembly 5.0 - ASP.NET Core Hosted Model
- [Entity Framework Core 5.0](https://docs.microsoft.com/en-us/ef/core/)

# Getting Started - Complete Documentation :rocket:

Getting started with Blazor Hero – A Clean Architecture Template built for Blazor WebAssembly using MudBlazor Components. This project will make your Blazor Learning Process much easier than you anticipate. Blazor Hero is meant to be an Enterprise Level Boilerplate, which comes free of cost, completely open sourced. 

The provided documentation / guide will get you started with BlazorHero in no-time. It provides a complete walkthrough about the project with to-the-point guides and notes.

<a href="https://codewithmukesh.com/blog/blazor-hero-quick-start-guide/"><strong>Read the Quick Start Guide</strong></a>

# Video Preview
Here is a quick video preview of this Open Soure Project - (https://www.facebook.com/codewithmukesh/posts/269621381402304)

## Version 1.0.1 - Current Release - Features

- [x] Mudblazor Component Library
- [x] .NET 5.0+
- [x] Blazor Web-Assembly: ASP.NET Core Hosted
- [x] Onion Architecture
- [x] Persistent Dark Mode (Local Storage)
- [x] Service-Based Approach
- [x] MediatR at API Level
- [x] AutoMapper
- [x] API Versioning
- [x] JWT Authentication
- [x] Serilog - Server-Side Only*
- [x] Complete User Management
- [x] Profile Picture
- [x] Clean Fluid UI
- [x] CRUD Functionalities
- [x] Custom Error Pages
- [x] Localization
- [x] Role Management
- [x] User-Role Management
- [x] Swagger
- [x] Middlewares
- [x] Favicon
- [x] Default User & Role Seeding
- [x] Dynamic Service Registration (WASM)
- [x] Auto DB Migrations
- [x] Paginated API Responses
- [x] Polly - Retry Pattern for HttpClient
- [x] Shared Email Service
- [x] Hangfire Implementation
- [x] Custom API Response for 500,401,403
- [x] Specification Pattern
- [x] Permission Based UI Rendering
- [x] Refresh Tokens
- [x] HTTP Interceptor
- [x] Logout User if Refresh Token Fails


# Features To Be Included

## Version 2.0 - Down the Road

- [ ] Breadcrumbs
- [ ] Caching
- [ ] Chat
- [ ] Notifications System using SignalR
- [ ] Charts
- [ ] PDF Downloads
- [ ] Theme Manager
- [ ] File Upload
- [ ] Import / Export to Excel
- [ ] Social Auth - Facebook, Google
- [ ] Audit Trails
- [ ] SEO

## Contribution Needed

- [ ] Need someone to add in the API Documentation for Swagger.
- [ ] Need someone to implement localization throughout every Razor Component of the solution under the WASM(Client) Project. You can take the Pages/Authentication/Login.razor as the point of reference. It is as simple as adding `@inject Microsoft.Extensions.Localization.IStringLocalizer<Login> localizer` to every page, changing the texts to `@localizer["Text Here"]` and finally adding resx files to the Resources Folder as per the folder structure.
- [ ] Need few contributors to add in various language transalations as per the implemented Location. I got time to only add a few transalations for French as of now.
- [ ] Need a UI contributor to look at the UX/UI of the entire project
- [ ] Need someone to buildup a cool Material Logo for BlazorHero (BH):D Do contact me on LinkedIn (https://www.linkedin.com/in/iammukeshm/).
- [ ] And finally, Stars from everyone! :D
