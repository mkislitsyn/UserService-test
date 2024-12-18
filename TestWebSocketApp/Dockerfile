FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /

COPY TestWebSocketApp/TestWebSocketApp/TestWebSocketApp.csproj TestWebSocketApp/
RUN dotnet restore TestWebSocketApp/TestWebSocketApp.csproj

COPY TestWebSocketApp/TestWebSocketApp/ TestWebSocketApp/
WORKDIR /TestWebSocketApp
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/runtime:9.0

WORKDIR /app

COPY --from=build /app .

ENTRYPOINT ["dotnet", "TestWebSocketApp.dll"]