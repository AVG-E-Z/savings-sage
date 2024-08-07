##Robitól
#FROM node:22-alpine
##ARG VITE_APP_TITLE
##ENV VITE_APP_TITLE=$VITE_APP_TITLE
#EXPOSE 80
#WORKDIR /app
#COPY . .
#RUN npm install
#RUN npm run build
#CMD [ "npm", "run", "preview", "--", "--host", "0.0.0.0" ]


# Stage 1: Build the React app
FROM node:alpine3.11 AS builder
WORKDIR /app

# Copy package.json and package-lock.json
COPY Frontend/package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application code
COPY  Frontend/. .

# Build the application
RUN npm run build

## Stage 2: Serve the static files using nginx
#FROM nginx:alpine
#
## Copy custom nginx configuration
#COPY Frontend/nginx.conf /etc/nginx/conf.d/default.conf
#
## Copy build files from the previous stage
#COPY --from=builder /app/dist /usr/share/nginx/html

# Expose port 80 to the outside world
EXPOSE 80

# Run nginx in the foreground
#CMD ["nginx", "-g", "daemon off;"]
#Robitól
CMD [ "npm", "run", "preview", "--", "--host", "0.0.0.0" ]
