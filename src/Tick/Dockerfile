FROM microsoft/aspnet

RUN apt-get install nodejs

COPY . /app

WORKDIR /app

ENTRYPOINT ["bash", "kestrel"]