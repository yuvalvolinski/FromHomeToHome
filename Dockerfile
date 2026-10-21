# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy backend and shared project
COPY FromHomeToHome.Core/ FromHomeToHome.Core/
COPY FromHomeToHome.Backend/ FromHomeToHome.Backend/

# Publish backend with its dependencies
RUN dotnet publish FromHomeToHome.Backend/FromHomeToHome.Backend.csproj \
    -c Release \
    -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /app/publish .

# Configure HTTP port
ENV ASPNETCORE_URLS=http://0.0.0.0:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "FromHomeToHome.Backend.dll"]