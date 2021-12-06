#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["testapi.csproj", "testapi1/"]
RUN dotnet restore "testapi1/testapi.csproj"
COPY . testapi1
WORKDIR "/src/testapi1"
RUN dotnet build "testapi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "testapi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "testapi.dll","urls","http://+:80;https://+:443"]