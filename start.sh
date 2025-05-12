#!/bin/bash

if [ ! -d "./LibreTranslate" ]; then
    echo "Cloning LibreTranslate repository..."
    git clone https://github.com/LibreTranslate/LibreTranslate
fi

echo "Starting Docker Compose..."
docker-compose up --build -d

echo "Waiting for services to start... (the first time may take a couple of minutes after the build)"
sleep 10

echo "Opening localhost:4200 in the default browser..."
xdg-open http://localhost:4200 || open http://localhost:4200 || start http://localhost:4200
