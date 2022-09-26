#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Uceme.API/Uceme.API.csproj", "Uceme.API/"]
COPY ["Uceme.Foundation/Uceme.Foundation.csproj", "Uceme.Foundation/"]
COPY ["Uceme.Model/Uceme.Model.csproj", "Uceme.Model/"]
COPY ["Uceme.Library/Uceme.Library.csproj", "Uceme.Library/"]
RUN dotnet restore "Uceme.API/Uceme.API.csproj"
COPY . .
WORKDIR "/src/Uceme.API"
RUN dotnet build "Uceme.API.csproj" -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "Uceme.API.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Uceme.API.dll"]
