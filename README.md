# datadog-dotnet7-tracing-helloworld

## What is this?

A basic app showing how to integrate the Datadog Tracer into a .Net 7 application.

## Warnings

Running this will create APM traces within your Datadog account, which could be billable. 

This is a sandbox app for illustrative purposes only, do not run in production.

## Instructions

This spins up a dotnet 7 application on port 8080. 

The agent is containerized. Configuration can be set in the `sandbox.docker.env` file, an example is provided, so you can simply rename it.

```DD_API_KEY=<Your API Key>```

Note: The file itself is in the `.gitignore` settings, so you don't accidentally commit and push your API key to a public repo.

This is where the agent will read the API key.

### Are you on an M1 machine?

The architecture can be changed in the docker-compose.yaml file under `services.simple-dotnet.build.args.ARCH` (`arm64` for arm (ie. an M1 Macbook), and `amd64` for amd).

Launch with `./run.sh`. This runs the application in detached mode.

Then connect to:
[http://localhost:8080](http://localhost:8080).

Hit the various endpoints to generate some traces, and then you should see them in your Datadog account.

The version of the agent in use is 7.41.1. It can be changed in the `docker-compose.yaml` file under `services.datadog-simple-dotnet.image`.

The version of the tracer in use is 2.20.0, and the tracer used is for arm architecture. The version can be changed in the `docker-compose.yaml` file under `services.simple-dotnet.build.args.VERSION`. 

You can run an interactive shell on the container with:

```docker exec -it simple-dotnet sh```

You can set the tracer to debug by changing `DD_TRACE_DEBUG` to `true` in the `docker-compose.yaml` under `simple-dotnet.environment`. 

To find the logs, you first need to exec inside the container with `docker exec -it simple-dotnet bash` and then search under `/var/log/datadog/dotnet/`.

The content should look as follow:
```
root@41bda1846f43:/var/log/datadog/dotnet# ls
DD-DotNet-Profiler-Native-dotnet-1.log       dotnet-native-loader-dotnet-1.log       dotnet-tracer-loader-dotnet-1.log       dotnet-tracer-managed-dotnet-20230320.log      dotnet-tracer-native-dotnet-47.log
DD-DotNet-Profiler-Native-dotnet-47.log      dotnet-native-loader-dotnet-47.log      dotnet-tracer-loader-dotnet-47.log      dotnet-tracer-managed-dotnet-app-20230320.log  dotnet-tracer-native-dotnet_app-82.log
DD-DotNet-Profiler-Native-dotnet_app-82.log  dotnet-native-loader-dotnet_app-82.log  dotnet-tracer-loader-dotnet-app-82.log  dotnet-tracer-native-dotnet-1.log
```

## Endpoints

Endpoints are defined in the `Program.cs` file:
* `/`: this endpoints returns a hello world, and generate a trace with no custom instrumentation.

* `/add-tag`: here, using custom instrumentation, we add a tag to the active span, with the date.

* `/exception`: here we add an exception on the current span.

* `/manual-span`: we create two manual spans in the trace, a parent and a child one.

## Tear down

Run `docker-compose down`
