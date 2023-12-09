FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ConsoleApp4B/ConsoleApp4B.csproj", "ConsoleApp4B/"]
RUN dotnet restore "ConsoleApp4B/ConsoleApp4B.csproj"
COPY . .
WORKDIR "/src/ConsoleApp4B"
RUN dotnet build "ConsoleApp4B.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsoleApp4B.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleApp4B.dll"]
