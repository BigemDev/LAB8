#!/bin/bash

IMAGE_NAME="custom-mysql"
CONTAINER_NAME="mysql-login"
DATA_DIR="./mysql-data"
PORT="3306"


if ! command -v docker &> /dev/null; then
    echo "Docker is not installed. Please install Docker first."
    exit 1
fi

if ! docker info &> /dev/null; then
    echo "Docker daemon is not running. Please start Docker."
    exit 1
fi

if [ ! -d "$DATA_DIR" ]; then
    echo "Creating data directory at $DATA_DIR"
    mkdir -p "$DATA_DIR"
fi

echo "Building Docker image: $IMAGE_NAME"
docker build -t "$IMAGE_NAME" .

if docker ps -a --format '{{.Names}}' | grep -Eq "^$CONTAINER_NAME\$"; then
    echo "🗑Removing existing container: $CONTAINER_NAME"
    docker stop "$CONTAINER_NAME" && docker rm "$CONTAINER_NAME"
fi

echo "Running container: $CONTAINER_NAME"
docker run -d \
    --name "$CONTAINER_NAME" \
    -p "$PORT:3306" \
    -v "$(pwd)/mysql-data:/var/lib/mysql" \
    "$IMAGE_NAME"

echo "MySQL container '$CONTAINER_NAME' is running on port $PORT"
