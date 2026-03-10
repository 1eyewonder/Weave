#!/usr/bin/env bash
# -*- coding: utf-8 -*-

set -eu
set -o pipefail

if git rev-parse --git-dir > /dev/null 2>&1; then
  echo "Configuring git hooks path..."
  git config core.hooksPath hooks
fi

echo "Restoring dotnet tools..."
dotnet tool restore

echo "Restoring Paket dependencies..."
dotnet paket restore

FAKE_DETAILED_ERRORS=true 
dotnet run --project ./build/Weave.Build.fsproj -- -t "$@"