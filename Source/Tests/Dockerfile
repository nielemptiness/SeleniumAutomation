FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Source/Tests/Tests.csproj", "Source/Tests/"]
COPY ["Source/Core/Core.Base/Core.Base.csproj", "Source/Core/Core.Base/Core.Base.csproj"]
RUN dotnet restore "Source/Tests/Tests.csproj"
COPY . .
WORKDIR "/src/Source/Tests/"
RUN dotnet build "Tests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tests.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "test", "Tests.dll"]
