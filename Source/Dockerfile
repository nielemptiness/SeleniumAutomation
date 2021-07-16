FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SeleniumAutomationExample/SeleniumAutomationExample.csproj", "SeleniumAutomationExample/"]
RUN dotnet restore "SeleniumAutomationExample/SeleniumAutomationExample.csproj"
COPY . .
WORKDIR "/src/SeleniumAutomationExample"
RUN dotnet build "SeleniumAutomationExample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SeleniumAutomationExample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SeleniumAutomationExample.dll"]
