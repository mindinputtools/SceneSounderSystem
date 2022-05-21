FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
RUN apt update && apt upgrade -y
RUN apt install pulseaudio apulse -y
# Cleanup
RUN apt clean -y
RUN rm -rf /var/lib/apt/lists/*

WORKDIR /app
EXPOSE 8080
# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 1000 --disabled-password --gecos "" pi && chown -R pi /app
USER pi

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ./nuget.config .
COPY packages/* ./packages/
COPY ["services/SystemApi/SystemApi.csproj", "services/SystemApi/"]
RUN dotnet restore "services/SystemApi/SystemApi.csproj"
COPY . .
WORKDIR "/src/services/SystemApi"
RUN dotnet build "SystemApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SystemApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SystemApi.dll"]
