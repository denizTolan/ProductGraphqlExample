FROM mcr.microsoft.com/dotnet/sdk:5.0 AS base
COPY . .

RUN dotnet restore src/Beymen.Product.Tag.Consumer/Beymen.Product.Tag.Consumer.csproj
RUN dotnet publish src/Beymen.Product.Tag.Consumer/Beymen.Product.Tag.Consumer.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS FINAL

# Install the agent
RUN apt-get update && apt-get install -y wget ca-certificates gnupg \
&& echo 'deb http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list \
&& wget https://download.newrelic.com/548C16BF.gpg \
&& apt-key add 548C16BF.gpg \
&& apt-get update \
&& apt-get install -y newrelic-netcore20-agent

# Enable the agent
ENV CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
CORECLR_NEWRELIC_HOME=/usr/local/newrelic-netcore20-agent \
CORECLR_PROFILER_PATH=/usr/local/newrelic-netcore20-agent/libNewRelicProfiler.so \
NEW_RELIC_DISTRIBUTED_TRACING_ENABLED=true \
NEW_RELIC_LABELS=env:production;team:beymen-sf \
NEW_RELIC_APP_NAME=Beymen.Product.Tag.Consumer

WORKDIR /app
COPY --from=base /app .
ENTRYPOINT ["dotnet", "Beymen.Product.Tag.Consumer.dll"]