FROM mindinputtools/opencv-csharp-base AS base
RUN apt update && apt upgrade -y
RUN apt install v4l-utils ffmpeg -y
# Cleanup
RUN apt clean -y
RUN rm -rf /var/lib/apt/lists/*
WORKDIR /app
EXPOSE 8080
# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 1000 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser
FROM mindinputtools/opencv-csharp-sdk AS build
WORKDIR /src
COPY ./nuget.config .
COPY packages/* ./packages/
COPY ["services/CameraApi/CameraApi.csproj", "services/CameraApi/"]
RUN dotnet restore "services/CameraApi/CameraApi.csproj"
COPY . .
WORKDIR "/src/services/CameraApi"
RUN dotnet build "CameraApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CameraApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CameraApi.dll"]
