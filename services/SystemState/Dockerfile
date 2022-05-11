FROM mcr.microsoft.com/dotnet/runtime:6.0-focal AS base
RUN apt update && apt upgrade -y
RUN apt install libespeak-ng1 libespeak-ng-libespeak1 espeak-ng espeak-ng-espeak libopenal1 pulseaudio apulse -y
WORKDIR /app
EXPOSE 8080
# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 1000 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ./nuget.config .
COPY packages/* ./packages/
COPY ["services/SystemState/SystemState.csproj", "services/SystemState/"]
RUN dotnet restore "services/SystemState/SystemState.csproj"
COPY . .
WORKDIR "/src/services/SystemState"
RUN dotnet build "SystemState.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SystemState.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SystemState.dll"]
