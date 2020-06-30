# cloudtrader-traders

Traders API for the CloudTrader project.

## Running locally with Visual Studio

Open the project from the solution file, run, and access the api at `https://localhost:1182`.

## Running unit tests

Run `dotnet test`

## Running with docker

Build the image

`docker build . -t cloudtrader-traders:latest`

Start the container

`docker run -p 1182:80 cloudtrader-traders:latest`

Access the api at `http://localhost:1182`
