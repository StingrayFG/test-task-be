#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:7000
EXPOSE 7000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["test-task-be.csproj", "."]
RUN dotnet restore "./test-task-be.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "test-task-be.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "test-task-be.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "test-task-be.dll"]