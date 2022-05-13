#!/bin/bash
# dotnet clean some
dotnet nuget locals all -c
rm -f ./packages/*.nu*
dotnet pack -o packages libs/MITAudioLib
dotnet pack -o packages libs/ESpeakSynthSharp
dotnet pack -o packages libs/SpeechClient
