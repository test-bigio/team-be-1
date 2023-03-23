# API HR siblue
Api service for Siblue HR department

## Getting started
1. To get started with the project, you'll need to have .NET Core installed. Once you've cloned the repository
2. Set database connection string in your environment variable
    ```
   ConnectionStrings__DefaultConnection=<db connection string>
    ```
3. run the following commands to set up the database:
    ```
    dotnet ef database update
    ```

Then, to start the API, run:
    ```
    dotnet run
    ```

## Running With Docker
1. Build Docker Image
    ```
    docker build . -t hr-service-api:<TAG>
    ```
2. Run Docker Container
    ```
    docker run -p 8088:80 --name hr-service-api-container hr-service-api:<TAG>
    ```
