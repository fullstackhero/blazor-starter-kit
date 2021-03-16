<!-- PROJECT LOGO -->
<br />
<p align="center">
  <a href="https://github.com/blazorhero/CleanArchitecture">
    <img src="https://codewithmukesh.com/wp-content/uploads/2021/02/Wide-Logo.jpg" alt="Logo">
  </a>

  <h3 align="center">BlazorHero - Clean Architecture Template</h3>

  <p align="center">
    For Blazor Web-Assembly
    <br />
    <a href=""><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://github.com/blazorhero/CleanArchitecture/issues">Report Bug</a>
    ·
    <a href="https://github.com/blazorhero/CleanArchitecture/issues">Request Feature</a>
  </p>
</p>

# Video Preview
Here is a quick video preview of this Open Soure Project - (https://www.facebook.com/codewithmukesh/posts/269621381402304)

# Features To Be Included

## Version 1.0 - Upcoming Release

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
- [ ] CRUD Functionalities
- [x] Custom Error Pages
- [x] Localization
- [x] Role Management
- [x] User-Role Management
- [x] Swagger
- [x] Middlewares
- [x] Favicon
- [ ] Fluent Validation
- [x] Default User & Role Seeding
- [x] Dynamic Service Registration (WASM)
- [x] Auto DB Migrations
- [x] Paginated API Responses
- [ ] 403 Redirects
- [ ] Polly - Retry Pattern for HttpClient
- [x] Shared Email Service
- [x] Hangfire Implementation
- [x] Custom API Response for 500,401,403

## Version 2.0 - Down the Road

- [ ] Caching
- [ ] Chat
- [ ] Notifications System using SignalR
- [ ] Charts
- [ ] PDF Downloads
- [ ] Theme Manager
- [ ] File Upload
- [ ] Import / Export to Excel
- [ ] Permission Based UI Rendering
- [ ] Social Auth - Facebook, Google
- [ ] Audit Trails

## Contribution Needed

- [ ] Need someone to implement localization throughout every Razor Component of the solution under the WASM(Client) Project. You can take the Pages/Authentication/Login.razor as the point of reference. It is as simple as adding `@inject Microsoft.Extensions.Localization.IStringLocalizer<Login> localizer` to every page, changing the texts to `@localizer["Text Here"]` and finally adding resx files to the Resources Folder as per the folder structure.
- [ ] Need few contributors to add in various language transalations as per the implemented Location. I got time to only add a few transalations for French as of now.
- [ ] Need a UI contributor to look at the UX/UI of the entire project
- [ ] Need someone to buildup a cool Material Logo for BlazorHero (BH):D Do contact me on LinkedIn (https://www.linkedin.com/in/iammukeshm/).
- [ ] And finally, Stars from everyone! :D
