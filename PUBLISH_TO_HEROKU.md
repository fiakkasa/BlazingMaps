# Blazing Maps

App URL @ https://blazingmaps.herokuapp.com/

## To get started you will need

- A heroku account
- The heroku cli https://devcenter.heroku.com/articles/heroku-cli
- Docker
- dotnet core sdk

## Steps

- Create your .Net app with docker support
- Ensure your app can be built and runs using the default docker setup
- Copy your DOCKER file to the root directory of the solution
- Modify the copied DOCKER file
  - comment out the ENTRYPOINT line
  - add CMD ASPNETCORE_URLS=http://\*:$PORT dotnet BlazingMaps.dll after the commented out ENTRYPOINT line
- Create an app on heroku
- Add any variables the app requires in the heroku panel
- Ensure you are logged in to heroku using both
  - heroku auth:login
  - heroku container:login
- Push the docker image
- Enjoy!

## Dockerization and Pushing to Heroku

Using Powershell build the image and test it.

If your app requires enviromental variables, ensure they are in place.

```powershell
PS > docker build -t BlazingMaps .
PS > docker run -d -p 8080:80 --name BlazingMaps BlazingMaps
PS > explorer http://localhost:8080
PS > docker rm --force BlazingMaps
```

Assuming your are signed in to Heroku, publish away!

```powershell
PS > heroku container:push -a blazing-maps web
PS > heroku container:release -a blazing-maps web
```

Doing it in one go!

```powershell
PS > docker build -t BlazingMaps .;heroku container:push -a blazing-maps web;heroku container:release -a blazing-maps web
```

## Resources

- https://dev.to/alrobilliard/deploying-net-core-to-heroku-1lfe
