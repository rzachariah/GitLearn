FROM microsoft/aspnet

RUN curl -sL https://deb.nodesource.com/setup | bash -
RUN apt-get install -y nodejs
COPY . /app

WORKDIR /app

ENTRYPOINT ["bash", "kestrel"]