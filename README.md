# team-be-1

## Build Docker Image
docker build . -t hr-service-api:{TAG}


## Run Docker Container
docker run -p 8088:80 --name hr-service-api-container hr-service-api:{TAG}