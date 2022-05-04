echo off
rem dotnet clean some
dotnet nuget locals all -c
del .\packages\*.nu*
dotnet pack -o packages libs\MITAudioLib
dotnet pack -o packages libs\ESpeakSynthSharp
