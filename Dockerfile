FROM mcr.microsoft.com/dotnet/sdk:5.0 AS projects-env
WORKDIR /src
# copy csproj and restore as distinct layers
COPY *.sln .
COPY src/Backend/MabelBookshelf/*.csproj ./src/Backend/MabelBookshelf/
COPY src/Backend/MabelBookshelf.Bookshelf.Application/*.csproj ./src/Backend/MabelBookshelf.Bookshelf.Application/
COPY src/Backend/MabelBookshelf.Bookshelf.Domain/*.csproj ./src/Backend/MabelBookshelf.Bookshelf.Domain/
COPY src/Backend/MabelBookshelf.Bookshelf.Infrastructure/*.csproj ./src/Backend/MabelBookshelf.Bookshelf.Infrastructure/

#
RUN dotnet restore src/Backend/MabelBookshelf/MabelBookshelf.csproj

# copy everything else and build app
COPY src/Backend/MabelBookshelf/. ./src/Backend/MabelBookshelf/
COPY src/Backend/MabelBookshelf.Bookshelf.Application/. ./src/Backend/MabelBookshelf.Bookshelf.Application/
COPY src/Backend/MabelBookshelf.Bookshelf.Domain/. ./src/Backend/MabelBookshelf.Bookshelf.Domain/ 
COPY src/Backend/MabelBookshelf.Bookshelf.Infrastructure/. ./src/Backend/MabelBookshelf.Bookshelf.Infrastructure/ 

WORKDIR src/Backend/MabelBookshelf
RUN dotnet publish MabelBookshelf.csproj -c Debug --no-restore -o /out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS run-env
WORKDIR /app
COPY --from=projects-env /out .
ENTRYPOINT ["dotnet", "MabelBookshelf.dll"]