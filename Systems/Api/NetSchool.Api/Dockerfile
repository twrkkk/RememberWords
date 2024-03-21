#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Systems/Api/NetSchool.Api/NetSchool.Api.csproj", "Systems/Api/NetSchool.Api/"]
COPY ["Data/NetSchool.Context.Migrations/NetSchool.Context.Migrations.MSSQL/NetSchool.Context.Migrations.MSSQL.csproj", "Data/NetSchool.Context.Migrations/NetSchool.Context.Migrations.MSSQL/"]
COPY ["Data/NetSchool.Context/NetSchool.Context.csproj", "Data/NetSchool.Context/"]
COPY ["Services/NetSchool.Services.Settings/NetSchool.Services.Settings.csproj", "Services/NetSchool.Services.Settings/"]
COPY ["Shared/NetSchool.Common/NetSchool.Common.csproj", "Shared/NetSchool.Common/"]
COPY ["Data/NetSchool.Context.Entities/NetSchool.Context.Entities.csproj", "Data/NetSchool.Context.Entities/"]
COPY ["Data/NetSchool.Context.Migrations/NetSchool.Context.Migrations.PgSql/NetSchool.Context.Migrations.PgSql.csproj", "Data/NetSchool.Context.Migrations/NetSchool.Context.Migrations.PgSql/"]
COPY ["Data/NetSchool.Context.Seeder/NetSchool.Context.Seeder.csproj", "Data/NetSchool.Context.Seeder/"]
COPY ["Services/NetSchool.Services.UserAccount/NetSchool.Services.UserAccount.csproj", "Services/NetSchool.Services.UserAccount/"]
COPY ["Services/NetSchool.Services.Actions/NetSchool.Services.Actions.csproj", "Services/NetSchool.Services.Actions/"]
COPY ["Services/NetSchool.Services.EmailSender/NetSchool.Services.EmailSender.csproj", "Services/NetSchool.Services.EmailSender/"]
COPY ["Services/NetSchool.Services.RabbitMq/NetSchool.Services.RabbitMq.csproj", "Services/NetSchool.Services.RabbitMq/"]
COPY ["Services/NetSchool.Services.CardCollections/NetSchool.Services.CardCollections.csproj", "Services/NetSchool.Services.CardCollections/"]
COPY ["Services/NetSchool.Services.PdfGenerator/NetSchool.Services.PdfGenerator.csproj", "Services/NetSchool.Services.PdfGenerator/"]
COPY ["Services/NetSchool.Services.Filters/NetSchool.Services.Filters.csproj", "Services/NetSchool.Services.Filters/"]
COPY ["Services/NetSchool.Services.Logger/NetSchool.Services.Logger.csproj", "Services/NetSchool.Services.Logger/"]
COPY ["Services/NetSchool.Services.LoadCustomAssembly/NetSchool.Services.LoadCustomAssembly.csproj", "Services/NetSchool.Services.LoadCustomAssembly/"]
COPY ["Services/NetSchool.Services.Scheduler/NetSchool.Services.Scheduler.csproj", "Services/NetSchool.Services.Scheduler/"]
COPY ["Systems/Api/NetSchool.Api.Settings/NetSchool.Api.Settings.csproj", "Systems/Api/NetSchool.Api.Settings/"]
RUN dotnet restore "./Systems/Api/NetSchool.Api/./NetSchool.Api.csproj"
COPY . .
WORKDIR "/src/Systems/Api/NetSchool.Api"
RUN dotnet build "./NetSchool.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NetSchool.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
RUN date >> /app/publish/.env

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetSchool.Api.dll"]