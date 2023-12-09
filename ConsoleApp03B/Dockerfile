FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ConsoleApp3B/ConsoleApp3B.csproj", "ConsoleApp3B/"]
RUN dotnet restore "ConsoleApp3B/ConsoleApp3B.csproj"
COPY . .
WORKDIR "/src/ConsoleApp3B"
RUN dotnet build "ConsoleApp3B.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsoleApp3B.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleApp3B.dll"]
