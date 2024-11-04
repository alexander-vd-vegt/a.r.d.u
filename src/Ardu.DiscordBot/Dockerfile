FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5077

ENV ASPNETCORE_URLS=http://+:5077

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Services/Ardu.DiscordBot/Ardu.DiscordBot.csproj", "Services/Ardu.DiscordBot/"]
RUN dotnet restore "Services/Ardu.DiscordBot/Ardu.DiscordBot.csproj"
COPY . .
WORKDIR "/src/Services/Ardu.DiscordBot"
RUN dotnet build "Ardu.DiscordBot.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Ardu.DiscordBot.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ardu.DiscordBot.dll"]
