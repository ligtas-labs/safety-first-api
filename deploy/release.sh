#!/usr/bin/env bash
set -euo pipefail
cd /opt/safety-first
exec 9>.deploy.lock
flock -x 9
if [[ $# != 2 ]]; then
  echo "Usage: release.sh <web-image> <api-image> or release.sh <web|api> <image>" >&2
  exit 2
fi

case "$1" in
  web|api)
    if [[ ! -f current-release ]]; then
      echo "Deploy an initial web and API image pair before updating one service." >&2
      exit 1
    fi
    read -r web api < current-release
    if [[ "$1" == web ]]; then
      web=$2
    else
      api=$2
    fi
    ;;
  *)
    web=$1
    api=$2
    ;;
esac
[[ "$web" =~ ^ghcr.io/ligtas-labs/safety-first-web@sha256:[0-9a-f]{64}$ ]]
[[ "$api" =~ ^ghcr.io/ligtas-labs/safety-first-api@sha256:[0-9a-f]{64}$ ]]
export WEB_IMAGE="$web" API_IMAGE="$api"
compose=(docker compose --env-file .env -f compose.yaml)
"${compose[@]}" pull
check_http() {
  local url=$1 code
  for attempt in {1..12}; do
    code=$(curl --silent --output /dev/null --write-out '%{http_code}' --max-time 5 "$url") || code=000
    [[ "$code" == 200 ]] && return 0
    sleep 2
  done
  return 1
}
if ! ( "${compose[@]}" up -d && check_http http://127.0.0.1:3000/ && check_http http://127.0.0.1:8080/health/ready ); then
  if [[ -f current-release ]]; then
    read -r WEB_IMAGE API_IMAGE <current-release
    "${compose[@]}" up -d
  fi
  exit 1
fi
printf '%s %s\n' "$web" "$api" > current-release.new
mv current-release.new current-release
