dotnet publish -o ./app --os linux --self-contained true -p:DebugType=None -p:DebugSymbols=false
mv ./app/WorldOfZuul ./app/WorldOfZuul-linux-x64
dotnet publish -o ./app --os win --self-contained true -p:DebugType=None -p:DebugSymbols=false
mv ./app/WorldOfZuul.exe ./app/WorldOfZuul-win-x64.exe
dotnet publish -o ./app --os osx --self-contained true -p:DebugType=None -p:DebugSymbols=false
mv ./app/WorldOfZuul ./app/WorldOfZuul-mac
cp -r ./assets ./app
zip -r app.zip ./app