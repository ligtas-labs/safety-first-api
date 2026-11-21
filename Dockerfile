FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore SafetyFirst.slnx && dotnet build SafetyFirst.slnx -c Release --no-restore && dotnet test SafetyFirst.slnx -c Release --no-build && dotnet publish src/SafetyFirst.Api -c Release --no-build -o /out
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /out .
USER $APP_UID
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "SafetyFirst.Api.dll"]
