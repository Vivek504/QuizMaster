FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /app

COPY QuizPortal/QuizPortal.csproj /app/QuizPortal/
COPY QuizPortalConsumer/QuizPortalConsumer.csproj /app/QuizPortalConsumer/

RUN dotnet restore QuizPortalConsumer/QuizPortalConsumer.csproj

COPY . .

RUN dotnet build QuizPortalConsumer/QuizPortalConsumer.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish QuizPortalConsumer/QuizPortalConsumer.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0-bookworm-slim-amd64 AS runtime
WORKDIR /app
COPY --from=publish /app/publish ./

RUN apt update
RUN apt install -y ffmpeg

RUN mkdir /app/video_storage

EXPOSE 80

ENTRYPOINT ["dotnet", "QuizPortalConsumer.dll"]