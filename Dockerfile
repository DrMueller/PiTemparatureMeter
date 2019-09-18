#!/bin/bash
FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch-arm32v7

RUN apt-get update && apt-get install -y curl
WORKDIR /tmp

WORKDIR /App
COPY ./DockerContent .

ENTRYPOINT ["dotnet", "Mmu.Ptm.dll"]