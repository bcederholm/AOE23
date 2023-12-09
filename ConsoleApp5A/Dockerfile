FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ConsoleApp5A/ConsoleApp5A.csproj", "ConsoleApp5A/"]
RUN dotnet restore "ConsoleApp5A/ConsoleApp5A.csproj"
COPY . .
WORKDIR "/src/ConsoleApp5A"
RUN dotnet build "ConsoleApp5A.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsoleApp5A.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleApp5A.dll"]
