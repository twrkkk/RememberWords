FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Systems/Web/NetSchool.Web/NetSchool.Web.csproj Systems/Web/NetSchool.Web/
RUN dotnet restore Systems/Web/NetSchool.Web/NetSchool.Web.csproj
COPY . .

RUN dotnet publish Systems/Web/NetSchool.Web/NetSchool.Web.csproj -c Release -o /app/publish
