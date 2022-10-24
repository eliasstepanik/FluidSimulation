FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PixelEngine/PixelEngine.csproj", "PixelEngine/"]
RUN dotnet restore "PixelEngine/PixelEngine.csproj"
COPY . .
WORKDIR "/src/PixelEngine"
RUN dotnet build "PixelEngine.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PixelEngine.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PixelEngine.dll"]
