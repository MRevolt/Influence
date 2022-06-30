FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Influence.UserApi/Influence.UserApi.csproj", "Influence.UserApi/"]
RUN dotnet restore "Influence.UserApi/Influence.UserApi.csproj"
COPY . .
WORKDIR "/src/Influence.UserApi"
RUN dotnet build "Influence.UserApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Influence.UserApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Influence.UserApi.dll"]
