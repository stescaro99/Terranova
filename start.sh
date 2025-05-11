#!/bin/bash

# Clona il repository LibreTranslate
if [ ! -d "./LibreTranslate" ]; then
    echo "Cloning LibreTranslate repository..."
    git clone https://github.com/LibreTranslate/LibreTranslate
else
    echo "LibreTranslate directory already exists."
fi

# Avvia Docker Compose con build
echo "Starting Docker Compose..."
docker-compose up --build &

# Attendi 10 secondi
echo "Waiting for services to start..."
sleep 10

# Apri il browser alla pagina del frontend
echo "Opening browser..."
xdg-open http://localhost:4200 2>/dev/null || open http://localhost:4200 2>/dev/null || echo "Please open http://localhost:4200 manually."