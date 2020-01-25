FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["TeamA.ProductManagementAPI/TeamA.ProductManagementAPI.csproj", "TeamA.ProductManagementAPI/"]
COPY ["TeamA.Models/TeamA.Models.csproj", "TeamA.Models/"]
COPY ["TeamA.Data/TeamA.Data.csproj", "TeamA.Data/"]
COPY ["TeamA.Services/TeamA.Services.csproj", "TeamA.Services/"]
RUN dotnet restore "TeamA.ProductManagementAPI/TeamA.ProductManagementAPI.csproj"
COPY . .
WORKDIR "/src/TeamA.ProductManagementAPI"
RUN dotnet build "TeamA.ProductManagementAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TeamA.ProductManagementAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TeamA.ProductManagementAPI.dll"]