FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ValiBot-test/ValiBot-test.csproj", "ValiBot-test/ValiBot-test.csproj"]
RUN dotnet restore "ValiBot-test/ValiBot-test.csproj"
COPY . .
WORKDIR "/src/ValiBot-test"
RUN dotnet build "ValiBot-test.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ValiBot-test.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ValiBot-test.dll"]
