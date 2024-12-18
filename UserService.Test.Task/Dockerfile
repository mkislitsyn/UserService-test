FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY  UserService.Test.Task/UserService.Test.Task.sln ./
COPY  UserService.Test.Task/src ./src
COPY  UserService.Test.Task/tests ./tests


RUN dotnet restore  UserService.Test.Task.sln

RUN dotnet build  UserService.Test.Task.sln -c Release --no-restore

RUN dotnet publish src/UserService.Web/UserService.Web.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app

RUN apt-get update && apt-get install -y curl

COPY --from=build /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80 \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 3000

ENTRYPOINT ["dotnet", "UserService.Web.dll"]