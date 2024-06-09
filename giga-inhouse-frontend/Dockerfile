# Use a lightweight web server image
FROM nginx:alpine

# Copy the build output to the web server's root directory
COPY ./dist /usr/share/nginx/html

# Expose the port the server will run on
EXPOSE 80

# Start the web server
CMD ["nginx", "-g", "daemon off;"]
