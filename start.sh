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
docker-compose up --build