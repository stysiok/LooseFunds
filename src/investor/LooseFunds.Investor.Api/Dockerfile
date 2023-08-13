FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app
COPY . .
RUN dotnet publish src/investor/LooseFunds.Investor.Api -c release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as app
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_URLS=http://*:80
ENV ASPNETCORE_ENVIRONMENT=docker

ENTRYPOINT dotnet LooseFunds.Investor.Api