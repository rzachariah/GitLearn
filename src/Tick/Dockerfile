FROM microsoft/aspnet

RUN apt-get update && \
    apt-get -y install curl && \
    curl -sL https://deb.nodesource.com/setup | sudo bash - && \
    apt-get -y install python build-essential nodejs

COPY . /app

WORKDIR /app

ENTRYPOINT ["bash", "kestrel"]