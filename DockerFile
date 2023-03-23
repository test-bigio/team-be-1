FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BigioHrServices.csproj", "./"]
RUN dotnet restore "./BigioHrServices.csproj"
COPY . .
RUN dotnet build "BigioHrServices.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "BigioHrServices.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BigioHrServices.dll"]