#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MShop.ProductAPI/MShop.ProductAPI.csproj", "MShop.ProductAPI/"]
RUN dotnet restore "MShop.ProductAPI/MShop.ProductAPI.csproj"
COPY . .
WORKDIR "/src/MShop.ProductAPI"
RUN dotnet build "MShop.ProductAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MShop.ProductAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MShop.ProductAPI.dll"]