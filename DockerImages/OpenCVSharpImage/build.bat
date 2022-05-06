echo off
docker buildx build --platform linux/arm64,linux/amd64 -t mindinputtools/opencv-csharp-sdk --push -f sdk.Dockerfile .
docker buildx build --platform linux/arm64,linux/amd64 -t mindinputtools/opencv-csharp-base --push .
