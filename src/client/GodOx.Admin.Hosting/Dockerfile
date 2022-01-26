#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/hosting/GodOx.Admin.Hosting/GodOx.Admin.Hosting.csproj", "src/hosting/GodOx.Admin.Hosting/"]
COPY ["src/module/GodOx.Shop.API/GodOx.Shop.API.csproj", "src/module/GodOx.Shop.API/"]
COPY ["src/GodOx.Share.BaseController/GodOx.Share.BaseController.csproj", "src/GodOx.Share.BaseController/"]
COPY ["src/GodOx.Share.Domain/GodOx.Share.Domain.csproj", "src/GodOx.Share.Domain/"]
COPY ["src/GodOx.Share.Models/GodOx.Share.Models.csproj", "src/GodOx.Share.Models/"]
COPY ["src/GodOx.Share.Infrastructure/GodOx.Share.Infrastructure.csproj", "src/GodOx.Share.Infrastructure/"]
COPY ["src/framework/core/GodOx.ModuleCore/GodOx.ModuleCore.csproj", "src/framework/core/GodOx.ModuleCore/"]
COPY ["src/module/GodOx.Sys.API/GodOx.Sys.API.csproj", "src/module/GodOx.Sys.API/"]
COPY ["src/module/GodOx.Cms.API/GodOx.Cms.API.csproj", "src/module/GodOx.Cms.API/"]
RUN dotnet restore "src/hosting/GodOx.Admin.Hosting/GodOx.Admin.Hosting.csproj"
COPY . .
WORKDIR "/src/src/hosting/GodOx.Admin.Hosting"
RUN dotnet build "GodOx.Admin.Hosting.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GodOx.Admin.Hosting.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GodOx.Admin.Hosting.dll"]