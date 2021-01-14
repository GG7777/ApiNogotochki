FROM node:15

WORKDIR /web

RUN npm install -g serve

COPY package*.json ./

RUN npm install

COPY . .

RUN npm run build

ENTRYPOINT ["serve", "-s", "build", "-l", "80"]