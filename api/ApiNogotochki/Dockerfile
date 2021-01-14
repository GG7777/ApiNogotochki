FROM mcr.microsoft.com/dotnet/sdk:3.1

WORKDIR /ApiNogotochki

COPY . .

RUN dotnet build --configuration Release --output ./Release

ENTRYPOINT ["dotnet", "./Release/ApiNogotochki.dll", "--urls", "http://+:5000"]