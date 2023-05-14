FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src

COPY ["RSSCargo.PL/RSSCargo.PL.csproj", "RSSCargo.PL/"]
COPY ["RSSCargo.BLL/RSSCargo.BLL.csproj", "RSSCargo.BLL/"]
COPY ["RSSCargo.DAL/RSSCargo.DAL.csproj", "RSSCargo.DAL/"]

RUN dotnet restore "RSSCargo.PL/RSSCargo.PL.csproj"

COPY . .

WORKDIR /src/RSSCargo.PL

RUN dotnet publish "RSSCargo.PL.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS release

WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "RSSCargo.PL.dll"]
