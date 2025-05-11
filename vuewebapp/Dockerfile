# Stage 1: Build
FROM node:20 as build

# Set the working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY package.json package.json
COPY package-lock.json package-lock.json

# Install dependencies
RUN npm install

# Copy the rest of the application files
COPY . .

# Build the application
RUN npm run build

# Stage 2: Serve with Nginx
FROM nginx:alpine

# Copy the built frontend files from the build stage
COPY --from=build /app/dist /usr/share/nginx/html

# Copy the nginx.conf file to the container
COPY nginx.conf /etc/nginx/nginx.conf

# Expose the default Nginx port
EXPOSE 80

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]