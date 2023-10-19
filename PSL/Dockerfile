FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PSL/PSL.csproj", "PSL/"]
RUN dotnet restore "PSL/PSL.csproj"
COPY . .
WORKDIR "/src/PSL"
RUN dotnet build "PSL.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PSL.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PSL.dll"]
