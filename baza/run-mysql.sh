#!/bin/bash

# === CONFIGURATION ===
IMAGE_NAME="custom-mysql"
CONTAINER_NAME="mysql-login"
DATA_DIR="./mysql-data"
PORT="3306"

# === CHECKS ===

# 1. Check if Docker is installed
if ! command -v docker &> /dev/null; then
    echo "Docker is not installed. Please install Docker first."
    exit 1
fi

# 2. Check if Docker daemon is running
if ! docker info &> /dev/null; then
    echo "Docker daemon is not running. Please start Docker."
    exit 1
fi

# 3. Create data directory if missing
if [ ! -d "$DATA_DIR" ]; then
    echo "Creating data directory at $DATA_DIR"
    mkdir -p "$DATA_DIR"
fi

# 4. Build the Docker image
echo "Building Docker image: $IMAGE_NAME"
docker build -t "$IMAGE_NAME" .

# 5. Stop and remove existing container if exists
if docker ps -a --format '{{.Names}}' | grep -Eq "^$CONTAINER_NAME\$"; then
    echo "🗑Removing existing container: $CONTAINER_NAME"
    docker stop "$CONTAINER_NAME" && docker rm "$CONTAINER_NAME"
fi

# 6. Run the container with volume mount
echo "Running container: $CONTAINER_NAME"
docker run -d \
    --name "$CONTAINER_NAME" \
    -p "$PORT:3306" \
    -v "$(pwd)/mysql-data:/var/lib/mysql" \
    "$IMAGE_NAME"

# 7. Show status
echo "MySQL container '$CONTAINER_NAME' is running on port $PORT"
