FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ConsoleApp15B/ConsoleApp15B.csproj", "ConsoleApp15B/"]
RUN dotnet restore "ConsoleApp15B/ConsoleApp15B.csproj"
COPY . .
WORKDIR "/src/ConsoleApp15B"
RUN dotnet build "ConsoleApp15B.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsoleApp15B.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleApp15B.dll"]
