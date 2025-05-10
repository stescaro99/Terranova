all: clone up open

up:
    docker-compose up -d

open:
    start http://localhost:4200

run: up open

clone:
    git clone https://github.com/LibreTranslate/LibreTranslate

.PHONY: up open clone all