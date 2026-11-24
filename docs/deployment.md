# Ubuntu deployment and sprint release

Use the existing 1 vCPU / 2 GB Ubuntu VPS as a pilot. Build the web and API on GitHub-hosted runners. Host PostgreSQL on Neon. The mobile app runs on Android. On this size, use one active VPS environment initially and a separate disposable Neon branch for integration tests. Confirm memory and disk under a representative load before scheduling evaluations. The planned 2 vCPU / 4 GB upgrade can support another environment if measurements allow it.

1. Run `uname -m`, `lsb_release -ds`, `free -h`, `df -h` and `ss -lntp` on the VPS. Set GitHub variable IMAGE_PLATFORM to linux/amd64 for x86_64 or linux/arm64 for aarch64. Reserve loopback port 3000 for web and 8080 for API. For an existing stack, check both ports are free, install the updated Compose file and release script together, recreate the services with the current image pair, then validate and reload Caddy with the updated upstreams. If another proxy owns 80/443, add the two virtual hosts to that proxy.
2. Install Docker Engine from its official Ubuntu apt repository and the docker-compose-plugin package. Verify `docker version` and `docker compose version`. Install Caddy using its official Ubuntu instructions if the VPS has no existing reverse proxy. Point both provided DNS names to the VPS IPv4 address. Add AAAA only if IPv6 works. Allow HTTP and HTTPS through the provider firewall; keep SSH restricted to the deployment path you can operate.
3. Create a Neon project in an appropriate region and separate development and production credentials. Use the Neon connection details to construct an Npgsql connection string with Host, Database, Username, Password and SSL Mode=VerifyFull. Use the pooled hostname for normal app traffic and the direct endpoint for migrations requiring a session. Store the runtime secret on the server. Keep the migration credential separate. Confirm the actual DbContext uses GetConnectionString("DefaultConnection") when it is introduced.
4. Create a deploy account with narrowly managed deployment access. Access to the Docker socket is privileged. Give GitHub an environment-specific SSH key. Store its private key as DEPLOY_SSH_KEY, the server public host-key line verified via the provider console as DEPLOY_KNOWN_HOSTS, and the hostname as VPS_HOST. For private GHCR packages log the deploy user into ghcr.io with a read-packages credential. Give each repository Actions permission to publish its own package.
5. Install compose.yaml and release.sh in /opt/safety-first and make the script executable. Ensure deploy can write the lock and release record. Create /opt/safety-first/.env with mode 600 containing NEON_CONNECTION_STRING. Protect it from other users. Configure the two reverse proxy routes from Caddyfile, validate the configuration, then reload the proxy. Verify HTTPS after DNS resolves.
6. Before the first rollout verify GET /health/ready with a disposable Neon connection. The implemented route returns 200 when the configured connection opens and 503 when configuration is absent or Npgsql cannot connect. Configure forwarded HTTPS headers for the trusted proxy and browser CORS for the exact web origin. Wire web requests to import.meta.env.VITE_API_BASE_URL. Configure a development API URL for staging builds (STAGING_API_URL). The Docker build argument is public. Run a web-to-API request on the real browser and an Android device before calling integration complete.
7. The API and web repositories contain their own Dockerfiles and image workflows. PRs build on GitHub. Pushing to development or main builds and publishes a SHA-tagged image and reports its digest. On a release, take the tested **main** digests from both summaries. Configure the API repository production environment for protected main and an available reviewer gate. Dispatch Deploy sprint release on main with the two digests. The runner connects by SSH; the host pulls the pair, starts it, checks both loopback endpoints and records the last successful pair. Verify both public origins after deploy. If this is the first release there is no previous pair to restore.
8. Run migrations as a separate reviewed release step once the project has migrations. Test against a Neon development branch, verify restore capability, apply only backward-compatible schema changes while the old version is serving, then deploy. Rehearse rollback by dispatching the previous known-good digest pair. An image rollback does not reverse a database change. After release check proxy/Compose logs, HTTP errors, memory, disk growth and restore access. Run `docker compose --env-file .env -f compose.yaml logs --tail 100` with the release image variables exported. Keep logs free of participant data.

Official references: https://docs.docker.com/engine/install/ubuntu/ • https://caddyserver.com/docs/install • https://neon.com/docs/connect/connection-pooling • https://docs.github.com/en/actions/tutorials/publish-packages/publish-docker-images

## Automatic development deployment

On a successful push to `development`, GitHub builds the image, publishes
it to GHCR, and updates the corresponding service by immutable digest.
Pull requests build images for review. The manual main release still accepts
a full image pair.

### Set up the VPS once

1. Use a VPS stack dedicated to development for automatic deployments. If
   production shares the VPS, give it a separate Compose project, directory,
   ports, and domains before enabling automatic deployment. The development
   SSH command and the existing manual release both target `/opt/safety-first` on their configured hosts. Do not point the production workflow at the development stack.
2. Install the merged Compose file, Caddy routes, and executable `release.sh`
   from the API repository. Follow the setup above for Docker, registry
   access, the deploy account, and the server-side Neon connection string.
   Configure `STAGING_API_URL` in the web repository for the development API.
3. From the VPS, run `/opt/safety-first/release.sh <web-digest-ref> <api-digest-ref>`
   as the deploy user once, using successful **development** image references
   from the two Container workflow summaries. This checks both services and
   records the initial pair in `current-release`. Verify the public web URL
   and API readiness through Caddy.
4. In both GitHub repositories, create the `development` environment and
   restrict it to the `development` branch if supported by your plan. Set
   `DEPLOY_SSH_KEY` and `DEPLOY_KNOWN_HOSTS` as secrets and `VPS_HOST` as a
   variable. Set the repository variable `DEPLOY_DEVELOPMENT` to `true` after
   both repositories and the VPS are ready. A subsequent merge triggers
   deployment.

The VPS serializes web and API updates with the same lock. A failed readiness
check restores the previously recorded image pair. To pause automation,
remove the enable variable from both repositories. To roll back to a known
good pair, run the same two-image command on the development VPS.
