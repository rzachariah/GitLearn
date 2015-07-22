#From Lastest Node js
FROM node:latest		

#Install Frisby
RUN npm install -g frisby

#Install jasmine
RUN npm install -g jasmine-node

#Set Path
ENV NODE_PATH /usr/local/lib/node_modules/

#Copy test file to app folder
COPY . /app

#Set working directory to app
WORKDIR /app

#Run test
ENTRYPOINT ["jasmine-node", "apitest_spec.js"]
