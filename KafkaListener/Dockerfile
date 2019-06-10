FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["KafkaListener/KafkaListener.csproj", "KafkaListener/"]
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "KafkaListener/KafkaListener.csproj"
COPY . .
WORKDIR "/src/KafkaListener"
RUN dotnet build "KafkaListener.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "KafkaListener.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "KafkaListener.dll"]