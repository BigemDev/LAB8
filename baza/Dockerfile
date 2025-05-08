FROM mysql:8.0
ENV MYSQL_ROOT_PASSWORD=rootpassword
ENV MYSQL_DATABASE=login_demo
ENV MYSQL_USER=appuser
ENV MYSQL_PASSWORD=apppassword
COPY init.sql /docker-entrypoint-initdb.d/
EXPOSE 3306
