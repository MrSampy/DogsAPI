FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

WORKDIR /app

COPY . .

RUN dotnet restore

RUN dotnet publish -c Release -o /app/build/API

FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime

WORKDIR /app

EXPOSE 7451

EXPOSE 5043

EXPOSE 80

ENV TZ=Europe/Kiev

COPY --from=build /app/build/API .

CMD ["dotnet", "API.dll"]