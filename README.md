# httpclient-request

.net core console application used to make http request using `HttpClient` created via `HttpClientFactory`

## Running

**Using docker run command**

`docker run --rm -e HttpRunner__Url=https://httpbin.org/response-headers?freeform=hello christianacca/httpclient-request`

**Using docker-compose**

* download [docker-compose.yml](docker-compose.yml)
* run: `docker-compose up --abort-on-container-exit`

## Supported settings

See [HttpRunnerSettings.cs](src/httpclient-request/HttpRunnerSettings.cs) for an explanation of each setting that can be
supplied

See [appsttings.json](src/httpclient-request/appsttings.json) `HttpRunner` for the default values for these settings.

To override these defaults you can use:
* docker config (swarm mode only) to supply an appsettings.json or appsettings-overrides.json file
* docker volume to supply an appsettings.json or appsettings-overrides.json file
* docker environment variable. See [docker-compose.yml](docker-compose.yml) for an example

## Build / Publish

* Build only: `./build.ps1`
* Build + publish: `./build.ps1 -Tag n.n.n -Publish -Credential christianacca`
    * replace 'n.n.n' with the sematic version that describes the change EG: 1.0.0