FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AuthService.csproj", "./"]
COPY ["../common/Common.csproj", "../common/"]
RUN dotnet restore "AuthService.csproj"
COPY . .
RUN dotnet publish "AuthService.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "AuthService.dll"]