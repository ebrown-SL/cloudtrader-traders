FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /sln

# Restore solution
COPY ./CloudTrader.Traders.sln ./
COPY ./src/CloudTrader.Traders.Api/CloudTrader.Traders.Api.csproj  ./src/CloudTrader.Traders.Api/CloudTrader.Traders.Api.csproj
COPY ./src/CloudTrader.Traders.Data/CloudTrader.Traders.Data.csproj  ./src/CloudTrader.Traders.Data/CloudTrader.Traders.Data.csproj
COPY ./src/CloudTrader.Traders.Models/CloudTrader.Traders.Models.csproj  ./src/CloudTrader.Traders.Models/CloudTrader.Traders.Models.csproj
COPY ./src/CloudTrader.Traders.Service/CloudTrader.Traders.Service.csproj  ./src/CloudTrader.Traders.Service/CloudTrader.Traders.Service.csproj

COPY ./test/CloudTrader.Traders.Models.Tests/CloudTrader.Traders.Models.Tests.csproj  ./test/CloudTrader.Traders.Models.Tests/CloudTrader.Traders.Models.Tests.csproj
COPY ./test/CloudTrader.Traders.Service.Tests/CloudTrader.Traders.Service.Tests.csproj  ./test/CloudTrader.Traders.Service.Tests/CloudTrader.Traders.Service.Tests.csproj

RUN dotnet restore

# Build solution
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /sln
COPY --from=build-env /sln/out .

EXPOSE 80

ENTRYPOINT ["dotnet", "CloudTrader.Traders.dll"]
