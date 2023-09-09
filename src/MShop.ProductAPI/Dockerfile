#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/MShop.ProductAPI/MShop.ProductAPI.csproj", "src/MShop.ProductAPI/"]
COPY ["src/MShop.Application/MShop.Application.csproj", "src/MShop.Application/"]
COPY ["src/MShop.Business/MShop.Business.csproj", "src/MShop.Business/"]
COPY ["src/Mshop.Chache/MShop.Cache.csproj", "src/Mshop.Chache/"]
COPY ["src/MShop.Messaging/MShop.Messaging.csproj", "src/MShop.Messaging/"]
COPY ["src/MShop.Repository/MShop.Repository.csproj", "src/MShop.Repository/"]
RUN dotnet restore "src/MShop.ProductAPI/MShop.ProductAPI.csproj"
COPY . .
WORKDIR "/src/src/MShop.ProductAPI"
RUN dotnet build "MShop.ProductAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MShop.ProductAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MShop.ProductAPI.dll"]