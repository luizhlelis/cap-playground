#!/usr/bin/env bash

dotnet ef database update --project ${API_PROJECT} --startup-project ${API_PROJECT} --context ${API_SQL_CONTEXT_CLASS} --verbose

dotnet ef database update --project ${CONSUMER_PROJECT} --startup-project ${CONSUMER_PROJECT} --context ${CONSUMER_SQL_CONTEXT_CLASS} --verbose
