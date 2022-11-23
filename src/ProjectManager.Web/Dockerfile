#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["ProjectManager.Web.csproj", "src/ProjectManager.Web/"]
WORKDIR ../src
COPY ["ProjectManager.Infrastructure/ProjectManager.Infrastructure.csproj", "src/ProjectManager.Infrastructure/"]
COPY ["src/ProjectManager.Core/ProjectManager.Core.csproj", "src/ProjectManager.Core/"]
COPY ["src/ProjectManager.SharedKernel/ProjectManager.SharedKernel.csproj", "src/ProjectManager.SharedKernel/"]
RUN dotnet restore "src/ProjectManager.Web/ProjectManager.Web.csproj"
COPY . .
WORKDIR "/src/src/ProjectManager.Web"
RUN dotnet build "ProjectManager.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectManager.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectManager.Web.dll"]