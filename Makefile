# Variabili
FRONTEND_DIR=frontend
BACKEND_DIR=backend
FRONTEND_BUILD_DIR=$(FRONTEND_DIR)/dist
BACKEND_BUILD_DIR=$(BACKEND_DIR)/bin/Debug/net6.0

# Comandi
.PHONY: all frontend backend clean run

# Compila sia frontend che backend
all: frontend backend

# Compila il frontend
frontend:
    @echo "Compilazione del frontend..."
    cd $(FRONTEND_DIR) && npm install && npm run build

# Compila il backend
backend:
    @echo "Compilazione del backend..."
    cd $(BACKEND_DIR) && dotnet build

# Pulisce i file di compilazione
clean:
    @echo "Pulizia dei file di compilazione..."
    rm -rf $(FRONTEND_BUILD_DIR)
    cd $(BACKEND_DIR) && dotnet clean

# Avvia sia frontend che backend in terminali separati
run: 
    @echo "Avvio del backend in un nuovo terminale..."
    start cmd /k "cd $(BACKEND_DIR) && dotnet run"
    @echo "Avvio del frontend in un nuovo terminale..."
    start cmd /k "cd $(FRONTEND_DIR) && npm start"