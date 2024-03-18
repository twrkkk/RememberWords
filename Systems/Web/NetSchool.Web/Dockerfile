FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Systems/Web/NetSchool.Web/NetSchool.Web.csproj", "Systems/Web/NetSchool.Web/"] .
RUN dotnet restore "./Systems/Web/NetSchool.Web/NetSchool.Web.csproj"
COPY . .
RUN dotnet build "./Systems/Web/NetSchool.Web/NetSchool.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Systems/Web/NetSchool.Web/NetSchool.Web.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf