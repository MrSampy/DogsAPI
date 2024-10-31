# DogsAPI

[![.github/workflows/dotnet.yml](https://github.com/MrSampy/DogsHouseService/actions/workflows/dotnet.yml/badge.svg)](https://github.com/MrSampy/DogsHouseService/actions/workflows/dotnet.yml)

## Key Features
- Onion Architecture: The project is structured following Onion Architecture principles, which promotes separation of concerns, maintainability, and testability. It organizes code into distinct layers (e.g., Domain, Services, and Infrastructure).
- Database Configuration: The project can be run with either an in-memory database (for testing) or a PostgreSQL database (for production). This can be configured in appsettings.json with the setting "UseInMemoryDatabase": false.

## Running the Project
The project can be run in two ways based on the database configuration:

In-Memory Database:

1. Set "UseInMemoryDatabase": true in appsettings.json.
Run the project directly using Visual Studio or dotnet run from the terminal.
PostgreSQL Database with Docker:

2. Set "UseInMemoryDatabase": false in appsettings.json.
Make sure Docker is installed and running.
In the root directory, run the following command to start the PostgreSQL database and the API:
```console
docker-compose up --build
```

## Testing the API
You can test the API using the following curl commands:
- Get all dogs:
```console
curl -X GET http://localhost:5043/dogs
```
- Get dogs with pagination:
```console
curl -X GET "http://localhost:5043/dogs?pageNumber=1&pageSize=10"
```
- Get dogs sorted by attribute (e.g., tail length):
```console
curl -X GET "http://localhost:5043/dogs?attribute=tail_length&order=asc"
```
- Add a new dog:
```console
curl -X POST http://localhost:5043/dog -H "Content-Type: application/json" -d "{\"name\": \"Doggy\", \"color\": \"red^&amber\", \"tail_length\": 173, \"weight\": 33}"
```
