FROM node:18 AS build
WORKDIR /app

# Install dependencies
COPY package.json package-lock.json ./
RUN npm install

# Copy application files and build
COPY . .
RUN npm run build --prod
# temporaneamente per debug
RUN ls /app/dist/frontend-terranova && sleep 60

# Serve the application
FROM nginx:alpine
COPY --from=build /app/dist/frontend-terranova /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/frontend-terranova/. /usr/share/nginx/html/
COPY --from=build index.html /usr/share/nginx/html/
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]