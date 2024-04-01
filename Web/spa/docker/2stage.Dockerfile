FROM node:14-alpine as builder

# LABEL MAINTAINER siritas@gmail.com
LABEL MAINTAINER Jakarin_a<jakarin_a@softsquaregroup.com>

WORKDIR /usr/src/app
COPY package*.json ./
ENV NODE_OPTIONS="--max-old-space-size=4096"
RUN npm install -g @angular/cli@11 \
  && npm ci --loglevel=error --verbose \
  && npm install
COPY . .
RUN npm run build-docker

FROM caddy:2.2.1-alpine as runtime
COPY --from=builder /usr/src/app/dist/ /usr/share/caddy/
COPY --from=builder /usr/src/app/docker/Caddyfile /etc/caddy/Caddyfile
