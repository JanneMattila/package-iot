FROM mcr.microsoft.com/dotnet/core/runtime:3.1.3-alpine3.11 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.201-buster AS build
WORKDIR /src
COPY "PackageDevice.csproj" .
RUN dotnet restore "PackageDevice.csproj"
COPY . .
RUN dotnet build "PackageDevice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PackageDevice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PackageDevice.dll"]
