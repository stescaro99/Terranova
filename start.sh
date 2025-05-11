#!/bin/bash

if [ ! -d "./LibreTranslate" ]; then
    echo "Cloning LibreTranslate repository..."
    git clone https://github.com/LibreTranslate/LibreTranslate
fi

echo "Starting Docker Compose..."
docker-compose up --build