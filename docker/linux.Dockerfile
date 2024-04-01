FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build-env

# LABEL MAINTAINER SiritasDho<dahoba@gmail.com>
LABEL MAINTAINER Jakarin_a<jakarin_a@softsquaregroup.com>
LABEL description="TPMC Web App"

WORKDIR /source

COPY Application/*.csproj ./Application/

COPY Domain/*.csproj ./Domain/

COPY Infrastructure/*.csproj ./Infrastructure/

COPY Persistense/*.csproj ./Persistense/

COPY Web/*.csproj ./Web/

COPY *.sln ./

RUN dotnet restore

# Copy everything else and build
COPY . ./

RUN dotnet publish -c release -o /app  --no-restore
# RUN dotnet publish -c debug -o /app  --no-restore

# COPY Web/certs /app/certs

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine as runtime

RUN apk add --update tzdata icu-libs && \
    rm -rf /var/lib/apt/lists/* && rm /var/cache/apk/*

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=th_TH.UTF-8 \
    LANG=th_TH.UTF-8

WORKDIR /app

COPY --from=build-env /app ./

EXPOSE 80

ENTRYPOINT ["dotnet", "Web.dll"]