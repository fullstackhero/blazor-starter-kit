#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_URLS=https://+:5005;http://+:5006
WORKDIR /app
EXPOSE 5005
EXPOSE 5006

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /
COPY ["src/Server/Server.csproj", "src/Server/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Application.Abstractions/Application.Abstractions.csproj", "src/Application.Abstractions/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Contracts/Contracts.csproj", "src/Contracts/"]
COPY ["src/Shared/Shared.csproj", "src/Shared/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Client/Client.csproj", "src/Client/"]
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]
RUN dotnet restore "src/Server/Server.csproj" --disable-parallel
COPY . .
WORKDIR "src/Server"
RUN dotnet build "Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
WORKDIR /app/Files
WORKDIR /app
ENTRYPOINT ["dotnet", "CleanBlazor.Server.dll"]
