FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /sln

# Restore solution
COPY ./CloudTrader.Traders.sln ./
COPY ./CloudTrader.Traders.Api/CloudTrader.Traders.Api.csproj  ./CloudTrader.Traders.Api/CloudTrader.Traders.Api.csproj
COPY ./CloudTrader.Traders.Data/CloudTrader.Traders.Data.csproj  ./CloudTrader.Traders.Data/CloudTrader.Traders.Data.csproj
COPY ./CloudTrader.Traders.Domain/CloudTrader.Traders.Domain.csproj  ./CloudTrader.Traders.Domain/CloudTrader.Traders.Domain.csproj

COPY ./CloudTrader.Traders.Api.Tests/CloudTrader.Traders.Api.Tests.csproj  ./CloudTrader.Traders.Api.Tests/CloudTrader.Traders.Api.Tests.csproj
COPY ./CloudTrader.Traders.Domain.Tests/CloudTrader.Traders.Domain.Tests.csproj  ./CloudTrader.Traders.Domain.Tests/CloudTrader.Traders.Domain.Tests.csproj

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
