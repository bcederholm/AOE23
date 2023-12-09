FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ConsoleApp4A/ConsoleApp4A.csproj", "ConsoleApp4A/"]
RUN dotnet restore "ConsoleApp4A/ConsoleApp4A.csproj"
COPY . .
WORKDIR "/src/ConsoleApp4A"
RUN dotnet build "ConsoleApp4A.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsoleApp4A.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleApp4A.dll"]
