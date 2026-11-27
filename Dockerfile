FROM mcr.microsoft.com/dotnet/sdk:10.0-noble AS build
WORKDIR /src

COPY SafetyFirst.slnx ./
COPY src/SafetyFirst.Api/SafetyFirst.Api.csproj src/SafetyFirst.Api/packages.lock.json ./src/SafetyFirst.Api/
COPY tests/SafetyFirst.Api.Tests/SafetyFirst.Api.Tests.csproj tests/SafetyFirst.Api.Tests/packages.lock.json ./tests/SafetyFirst.Api.Tests/
RUN dotnet restore SafetyFirst.slnx --locked-mode

COPY . .
RUN dotnet build SafetyFirst.slnx -c Release --no-restore -p:UseAppHost=false \
    && dotnet test SafetyFirst.slnx -c Release --no-build \
    && dotnet publish src/SafetyFirst.Api -c Release --no-build -p:UseAppHost=false -o /out

FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble-chiseled-extra
WORKDIR /app
COPY --from=build /out/ ./
USER $APP_UID
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "SafetyFirst.Api.dll"]
