echo off
rem dotnet clean some
dotnet nuget locals all -c
del .\packages\*.nu*
dotnet pack -o packages libs\MITAudioLib
dotnet pack -o packages libs\ESpeakSynthSharp
dotnet pack -o packages libs\SpeechClient
dotnet pack -o packages libs\CameraClient
dotnet pack -o packages libs\SystemClient
