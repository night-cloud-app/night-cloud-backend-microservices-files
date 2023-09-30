FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Files.Presentation/Files.Presentation.csproj", "src/Files.Presentation/"]
COPY ["src/Files.Application/Files.Application.csproj", "src/Files.Application/"]
COPY ["src/Files.Domain/Files.Domain.csproj", "src/Files.Domain/"]
COPY ["src/Files.Infrastructure/Files.Infrastructure.csproj", "src/Files.Infrastructure/"]
RUN dotnet restore "src/Files.Presentation/Files.Presentation.csproj"
COPY . .
WORKDIR "/src/src/Files.Presentation"
RUN dotnet build "Files.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Files.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Files.Presentation.dll"]
