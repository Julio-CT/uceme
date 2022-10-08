#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Uceme.UI/Uceme.UI.csproj", "Uceme.UI/"]
COPY ["Uceme.Foundation/Uceme.Foundation.csproj", "Uceme.Foundation/"]
COPY ["Uceme.Model/Uceme.Model.csproj", "Uceme.Model/"]
COPY ["Uceme.Library/Uceme.Library.csproj", "Uceme.Library/"]
RUN dotnet restore "Uceme.UI/Uceme.UI.csproj"
COPY . .
WORKDIR "/src/Uceme.UI"
RUN dotnet build "Uceme.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Uceme.UI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Uceme.UI.dll"]