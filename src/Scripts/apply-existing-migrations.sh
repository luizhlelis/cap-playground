#!/usr/bin/env bash

dotnet ef database update --project ${PROJECT} --startup-project ${STARTUP_PROJECT} --context ${SQL_CONTEXT_CLASS} --verbose
