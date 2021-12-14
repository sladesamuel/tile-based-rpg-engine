SHELL := /bin/bash

.PHONY: clean
clean:
	dotnet clean

.PHONY: build
build:
	dotnet build

.PHONY: start
start:
	dotnet run --project TileBasedRpg.Sandbox/
