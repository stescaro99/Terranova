# Terranova

1. Pianificazione del progetto


Definire l'architettura generale (frontend e backend separati).


~~Creare un repository Git (GitHub/GitLab) per il versionamento del codice.~~


Preparare una documentazione iniziale con le specifiche tecniche e funzionali.


# 2. Sviluppo del Backend (C# con ASP.NET Core e SQL Server)


~~2.1 Setup dell’ambiente~~


~~Installare Visual Studio 2022 Community con il workload ASP.NET e sviluppo web.~~


~~Installare SQL Server Express e SQL Server Management Studio (SSMS).~~


~~Creare un repository Git per il backend.~~


2.2 Creazione del progetto


Avviare un nuovo progetto ASP.NET Core Web API.


Strutturare il progetto in modo modulare:


Models: Definire le entità (User, Cocktail, Ingredient).


DTOs: Creare i Data Transfer Objects per evitare esposizione diretta delle entità.


Controllers: Implementare i controller API (AuthController, CocktailController, UserController).


Services: Creare servizi per la business logic.


Repositories: Implementare pattern Repository per la gestione del database.


2.3 Database e connessione


Configurare Entity Framework Core con SQL Server.


Creare il DbContext e le migrazioni (dotnet ef migrations add InitialCreate).


Definire le entità:


User (Id, Email, PasswordHash, Preferenze)


Cocktail (Id, Nome, Ingredienti, Categoria, Immagine)


FavoriteCocktail (UserId, CocktailId)


2.4 Implementazione API


Autenticazione JWT per la gestione degli utenti (login, registrazione, token JWT).


Gestione cocktail:


Ottenere cocktail tramite TheCocktailDB API.


Salvare cocktail nei preferiti dell’utente.


Ricerca cocktail per nome, ingredienti, tipo.


Gestione utenti:


Visualizzazione area personale.


Modifica preferenze.


Consensi GDPR.


2.5 Logging e Debugging


Stampare nel terminale:


Log di avvio del backend.


Richieste ricevute ed errori.


Utilizzare Serilog per log strutturati.


2.6 Test e documentazione API


Scrivere test unitari con xUnit.


Testare le API con Postman.


Generare documentazione API con Swagger.




# 3. Sviluppo del Frontend (TypeScript con React)


3.1 Setup dell’ambiente


Installare Node.js e npm.


Creare un progetto React con TypeScript:


bash


Copy


Edit


npx create-react-app cocktail-debacle --template typescript


Configurare Git per il frontend.


3.2 Struttura del progetto


src/components: Componenti UI (CocktailCard.tsx, SearchBar.tsx, AuthForm.tsx).


src/pages: Pagine principali (Home.tsx, Login.tsx, Profile.tsx).


src/api: Funzioni per chiamare le API (auth.ts, cocktails.ts).


src/context: Context API per la gestione dello stato utente.


3.3 Implementazione delle funzionalità


Autenticazione:


Form di login/registrazione con validazione.


Gestione del token JWT (salvataggio in localStorage).


Ricerca cocktail:


Input per nome, ingredienti, tipo.


Chiamate API per ottenere i cocktail.


Gestione preferiti:


Pagina "I miei cocktail preferiti".


Aggiunta/rimozione di cocktail nei preferiti.


Profilazione utente:


Modifica preferenze.


Consenso alla profilazione.


3.4 UI e miglioramenti


Usare Tailwind CSS per uno stile moderno e responsivo.


Implementare React Router per la navigazione.


Creare componenti riutilizzabili (Button, Modal, Card).


3.5 Test e ottimizzazione


Scrivere test unitari con Jest e React Testing Library.


Usare Vite per un ambiente di sviluppo veloce.


Ottimizzare il codice con Lazy Loading e React Query.


4. Deployment


Backend: Pubblicare su Azure App Service o Docker.


Database: Deploy su Azure SQL Server o AWS RDS.


Frontend: Deploy su Vercel o Netlify.
