# .NET OpenTelemetry sample app

This app exports traces, metrics as well as logs via OpenTelemetry exporter to an OpenTelemetry collector such as New Relic.

Note: OTLP exporter for logs is in mixed support state for now. See link to [docs](https://opentelemetry.io/docs/instrumentation/net/) for up-to-date information.

The OTLP exporter leverages some environment variables (as shown in [env-vars.sh](/env-vars.sh)) to configure the OTLP collector backend:

```shell
export OTEL_EXPORTER_OTLP_ENDPOINT=https://otlp.nr-data.net
export OTEL_EXPORTER_OTLP_PROTOCOL=http/protobuf
export OTEL_EXPORTER_OTLP_HEADERS='api-key=NR_LICENSE_KEY'
```