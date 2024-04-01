#! /usr/bin/env bash
set -e

REGISTRY="registry.gitlab.com/up-trainee-2020-2021/team-process-manage-control"
IMAGE_NAME="tpmc-front"
COMMIT_SHORT_SHA="$(git rev-parse --short HEAD)"

cd Web/spa;

docker build --rm -f docker/2stage.Dockerfile -t ${IMAGE_NAME} -t ${REGISTRY}/${IMAGE_NAME} .