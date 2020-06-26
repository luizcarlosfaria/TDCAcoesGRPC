#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["AcoesGRPC/AcoesGRPC.csproj", "AcoesGRPC/"]
RUN dotnet restore "AcoesGRPC/AcoesGRPC.csproj"
COPY . .
WORKDIR "/src/AcoesGRPC"
RUN dotnet build "AcoesGRPC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AcoesGRPC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AcoesGRPC.dll"]