FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY "TTS.sln" "TTS.sln"
COPY ["TTS.Web/TTS.Web.csproj", "TTS.Web/"]
COPY ["TTS.BLL/TTS.BLL.csproj", "TTS.BLL/"]
COPY ["TTS.DAL/TTS.DAL.csproj", "TTS.DAL/"]
COPY ["TTS.Shared/TTS.Shared.csproj", "TTS.Shared/"]
RUN dotnet restore "TTS.sln"
COPY . .
WORKDIR "/src/TTS.Web"
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TTS.Web.dll"]