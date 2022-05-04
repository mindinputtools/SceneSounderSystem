FROM mcr.microsoft.com/dotnet/runtime:6.0-focal AS base
RUN apt update && apt upgrade -y
RUN apt install libespeak-ng1 libespeak-ng-libespeak1 espeak-ng espeak-ng-espeak libopenal1 pulseaudio apulse -y
WORKDIR /app
#RUN cp /usr/lib/aarch64-linux-gnu/libespeak* /app/
#RUN ln -sf libespeak-ng.so.1 libespeak-ng.so
# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 1000 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
#COPY ["libs/MITAudioLib/MITAudioLib.csproj", "libs/MITAudioLib/"]
#COPY ["libs/ESpeakSynthSharp/ESpeakSynthSharp.csproj", "libs/ESpeakSynthSharp/"]
COPY ./nuget.config .
COPY packages/* ./packages/
#RUN dotnet pack -o packages ./libs
COPY ["tests/HelloESpeak/HelloESpeak.csproj", "tests/HelloESpeak/"]
RUN dotnet restore "tests/HelloESpeak/HelloESpeak.csproj"
COPY . .
WORKDIR "/src/tests/HelloESpeak"
RUN dotnet build "HelloESpeak.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HelloESpeak.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HelloESpeak.dll"]
