FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5296

ENV ASPNETCORE_URLS=http://+:5296

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY "Ardu.Common/Ardu.Common.csproj" "Ardu.Common/"
COPY "arduhostsupervisor/arduhostsupervisor.csproj" "arduhostsupervisor/"
RUN dotnet restore "arduhostsupervisor/arduhostsupervisor.csproj" 
COPY . .
WORKDIR "/src/arduhostsupervisor"
RUN dotnet build "arduhostsupervisor.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "arduhostsupervisor.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "arduhostsupervisor.dll"]
