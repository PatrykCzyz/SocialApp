FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

EXPOSE 80
EXPOSE 443

COPY TwitterMvc.sln .
COPY TwitterMvc.csproj .
RUN dotnet restore

COPY . ./publish/
WORKDIR /app/publish
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/publish/out ./
ENTRYPOINT ["dotnet", "TwitterMvc.dll"]