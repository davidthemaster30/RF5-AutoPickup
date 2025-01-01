rm -rf obj
rm -rf bin

dotnet build RF5AutoPickup.csproj -f net6.0 -c Release

zip -j 'RF5AutoPickup_v1.3.0.zip' './bin/Release/net6.0/RF5AutoPickup.dll' './bin/Release/net6.0/RF5AutoPickup.cfg'