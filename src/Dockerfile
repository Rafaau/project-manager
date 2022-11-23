#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["ProjectManager.Web/ProjectManager.Web.csproj", "src/ProjectManager.Web/"]
COPY ["ProjectManager.Infrastructure/ProjectManager.Infrastructure.csproj", "src/ProjectManager.Infrastructure/"]
COPY ["ProjectManager.Core/ProjectManager.Core.csproj", "src/ProjectManager.Core/"]
COPY ["ProjectManager.SharedKernel/ProjectManager.SharedKernel.csproj", "src/ProjectManager.SharedKernel/"]
WORKDIR /src/src
RUN dotnet restore ./ProjectManager.Web/ProjectManager.Web.csproj
COPY . .
WORKDIR "ProjectManager.Web"
RUN dotnet build "ProjectManager.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectManager.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectManager.Web.dll"]