rm -rf obj
rm -rf bin
#dotnet build RF5AutoPickup_v577.csproj -c Release
dotnet build RF5AutoPickup.csproj -f netstandard2.1 -c Release

zip -j 'RF5AutoPickup-v577_v1.1.1.zip' './bin/Release/netstandard2.1/RF5AutoPickup.dll' './bin/Release/netstandard2.1/RF5AutoPickup.cfg'

dotnet build RF5AutoPickup.csproj -f net6.0 -c Release

zip -j 'RF5AutoPickup_v1.1.1.zip' './bin/Release/net6.0/RF5AutoPickup.dll' './bin/Release/net6.0/RF5AutoPickup.cfg'
