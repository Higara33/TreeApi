FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TreeService.csproj", "./"]
COPY ["../common/Common.csproj", "../common/"]
RUN dotnet restore "TreeService.csproj"
COPY . .
RUN dotnet publish "TreeService.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "TreeService.dll"]