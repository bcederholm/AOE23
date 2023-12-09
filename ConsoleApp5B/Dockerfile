FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ConsoleApp5B/ConsoleApp5B.csproj", "ConsoleApp5B/"]
RUN dotnet restore "ConsoleApp5B/ConsoleApp5B.csproj"
COPY . .
WORKDIR "/src/ConsoleApp5B"
RUN dotnet build "ConsoleApp5B.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsoleApp5B.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleApp5B.dll"]
