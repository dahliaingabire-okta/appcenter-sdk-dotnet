#!/bin/bash
set -e

# Replace version in all the assemblies
find SDK -name AssemblyInfo.cs | xargs sed -E -i '' "s/(AssemblyInformationalVersion\(\")(.*)\"\)/\1$1-SNAPSHOT\")/g"
find SDK -name AssemblyInfo.cs | xargs sed -E -i '' "s/(AssemblyFileVersion\(\")(.*)\"\)/\1$1.0\")/g"
