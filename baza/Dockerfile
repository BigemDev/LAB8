# Use the official MySQL base image
FROM mysql:8.0

# Set environment variables
ENV MYSQL_ROOT_PASSWORD=rootpassword
ENV MYSQL_DATABASE=login_demo
ENV MYSQL_USER=appuser
ENV MYSQL_PASSWORD=apppassword

# Copy the schema and seed SQL into the image
COPY init.sql /docker-entrypoint-initdb.d/

# Expose MySQL port
EXPOSE 3306
