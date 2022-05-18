FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
RUN apt update && apt upgrade -y
RUN apt install libgdiplus -y
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
COPY ["services/ObjDetectYOLO/ObjDetectYOLO.csproj", "services/ObjDetectYOLO/"]
RUN dotnet restore "services/ObjDetectYOLO/ObjDetectYOLO.csproj"
COPY . .
WORKDIR "/src/services/ObjDetectYOLO"
RUN dotnet build "ObjDetectYOLO.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ObjDetectYOLO.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY services/ObjDetectYOLO/Assets .
ENTRYPOINT ["dotnet", "ObjDetectYOLO.dll"]
