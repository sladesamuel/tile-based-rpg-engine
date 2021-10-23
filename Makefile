SHELL := /bin/bash

.PHONY: build
build:
	dotnet build SandboxRpg/

.PHONY: start
start:
	dotnet run --project SandboxRpg/
