FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ConsoleApp6A/ConsoleApp6A.csproj", "ConsoleApp6A/"]
RUN dotnet restore "ConsoleApp6A/ConsoleApp6A.csproj"
COPY . .
WORKDIR "/src/ConsoleApp6A"
RUN dotnet build "ConsoleApp6A.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsoleApp6A.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleApp6A.dll"]
