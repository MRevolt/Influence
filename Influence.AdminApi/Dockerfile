FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Influence.AdminApi/Influence.AdminApi.csproj", "Influence.AdminApi/"]
RUN dotnet restore "Influence.AdminApi/Influence.AdminApi.csproj"
COPY . .
WORKDIR "/src/Influence.AdminApi"
RUN dotnet build "Influence.AdminApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Influence.AdminApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Influence.AdminApi.dll"]
