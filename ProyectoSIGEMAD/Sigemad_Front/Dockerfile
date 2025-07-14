# Stage 1: Build the Angular application
FROM docker-registry.ns-sigemad-pipeline.svc.cluster.local:5000/node:18-alpine AS build
WORKDIR /app

# Copy package files and install all dependencies
COPY package.json package-lock.json ./
RUN npm ci

# Copy the rest of the source code into the container
COPY . . 

# Build the application in production mode
RUN npm run build

# Stage 2: Serve the application with Nginx
FROM docker-registry.ns-sigemad-pipeline.svc.cluster.local:5000/nginx:1.21.6
WORKDIR /usr/share/nginx/html

# Remove default Nginx index.html and copy compiled files
RUN rm -rf ./*
COPY --from=build /app/dist/sigemad/browser/ .

# Copy custom Nginx configuration
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expose the necessary port for Kubernetes
EXPOSE 80

# Default command to start Nginx
CMD ["nginx", "-g", "daemon off;"]
