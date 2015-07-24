FROM microsoft/aspnet

COPY . /app

WORKDIR /app/approot/src/Tick

ENTRYPOINT ["dnx", ".", "kestrel"]