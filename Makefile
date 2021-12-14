SHELL := /bin/bash

.PHONY: clean
clean:
	dotnet clean

.PHONY: build
build:
	dotnet build

.PHONY: test
test:
	dotnet test

.PHONY: start
start:
	dotnet run --project TileBasedRpg.Sandbox/
