#! /usr/bin/env bash
set -e

REGISTRY="registry.gitlab.com/up-trainee-2020-2021/team-process-manage-control"
IMAGE_NAME="tpmc-api"
TAG_DATETIME=$(date +%Y%m%d)
COMMIT_SHORT_SHA="$(git rev-parse --short HEAD)"

docker build --rm -f "docker/linux.Dockerfile" -t ${IMAGE_NAME} \
-t ${IMAGE_NAME}:${COMMIT_SHORT_SHA} \
-t ${IMAGE_NAME}:${TAG_DATETIME} \
-t ${REGISTRY}/${IMAGE_NAME} \
-t ${REGISTRY}/${IMAGE_NAME}:${COMMIT_SHORT_SHA} \
-t ${REGISTRY}/${IMAGE_NAME}:${TAG_DATETIME} .
