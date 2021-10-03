SHELL := /bin/bash

.PHONY: build
build:
	dotnet build SandboxRpg/

start:
	dotnet run --project SandboxRpg/
