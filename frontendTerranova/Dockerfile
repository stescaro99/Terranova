FROM node:18
WORKDIR /app

# Installa le dipendenze
COPY package.json package-lock.json ./
RUN npm install

# Copia il codice sorgente
COPY . .

# Avvia il server di sviluppo Angular
EXPOSE 4200
CMD ["npm", "start"]