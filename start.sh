#!/bin/bash

if [ ! -d "./LibreTranslate/.git" ]; then
    echo "Cloning LibreTranslate repository..."
    git clone https://github.com/LibreTranslate/LibreTranslate
fi

echo "Starting Docker Compose..."
gnome-terminal -- bash -c "docker-compose up --build; exec bash" || \
x-terminal-emulator -e "bash -c 'docker-compose up --build; exec bash'" || \
xterm -e "bash -c 'docker-compose up --build; exec bash'"

echo "Waiting for services to start... (the first time may take a couple of minutes after the build)"
sleep 10

echo "Opening localhost:4200 in the default browser..."
xdg-open http://localhost:4200 || open http://localhost:4200 || start http://localhost:4200
