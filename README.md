# cloudtrader-traders

Traders API for the CloudTrader project.

## Running locally with Visual Studio

Open the project from the solution file, run, and access the api at `http://localhost:5999`.

## Running with docker

Build the image

`docker build . -t cloudtrader-traders:latest`

Start the container

`docker run -p 5999:80 cloudtrader-traders:latest`

Access the api at `http://localhost:5999`

### Swagger UI

Once running, access Swagger UI at http://localhost:5999/swagger.

## Running unit tests

Run `dotnet test`
