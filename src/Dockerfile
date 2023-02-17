FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore "ProjectManager.Web"
RUN dotnet restore "ProjectManager.Core"
RUN dotnet restore "ProjectManager.Infrastructure"
RUN dotnet restore "ProjectManager.SharedKernel"
# Build and publish a release
RUN dotnet publish "ProjectManager.Web" -c Release -o out
RUN dotnet publish "ProjectManager.Core" -c Release -o out
RUN dotnet publish "ProjectManager.Infrastructure" -c Release -o out
RUN dotnet publish "ProjectManager.SharedKernel" -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
COPY ["cert.pfx", "/https/cert.pfx"]
ENTRYPOINT ["dotnet", "ProjectManager.Web.dll"]
