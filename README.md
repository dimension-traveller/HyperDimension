# Hyper Dimension

A personal space for you to store your thoughts, ideas, articles, and more.

This is the backend project.

Also, this project is a playground for me to learn new technologies, architectures, and patterns.

## Features

- [ ] Content Management
  - [ ] Content Types
    - [x] Article (Blog posts)
    - [x] Page (Static pages)
    - [x] Note (Personal notes)
    - [x] Friends (Links to friends' websites)
    - [x] Project (Links to projects)
  - [x] Organization
    - [x] Tags
    - [x] Categories
    - [x] Collections
- [ ] Web Site
  - [ ] RSS Feed
  - [ ] Sitemap
- [ ] Notifications
  - [ ] Email
  - [ ] Push Notification

## Technologies

- [x] Identity
  - [x] Authentication
    - [x] Local
      - [x] Database
      - [ ] (Delayed) LDAP
    - [x] 2FA (TOTP and Email)
    - [x] WebAuthn
    - [x] External Provider
      - [x] OpenID Connect (and OAuth2)
  - [x] Authorization
    - [x] Role-based
    - [x] Permission-based
- [x] Search
  - [ ] (Abandoned) PGroonga (PostgreSQL Extension)
  - [x] Elasticsearch
  - [x] MeiliSearch
  - [x] Algolia
- [x] Cache
  - [x] Redis
  - [x] Distributed Memory Cache
- [x] Database
  - [x] SQLite
  - [x] Microsoft SQL Server
  - [x] MySQL/MariaDB
  - [x] PostgreSQL
- [x] Storage
  - [x] Amazon S3 Compatible
  - [x] Local File System
- [x] Configuration
  - [x] YAML/YML
  - [x] JSON
  - [x] TOML
- [x] Observability
  - [x] Logging (Open Telemetry)
  - [x] Metrics (Prometheus)
  - [x] Tracing (Open Telemetry)
  - [x] Health Check
- [ ] Notifications
  - [ ] Email
  - [ ] Server Chan
  - [ ] WebHooks

## Development

### Prerequisites

- .NET SDK 8.0
- A supported database server (SQLite by default)
- A supported cache server (Memory by default)
- A supported storage server (File System by default)
- A supported configuration file (YAML by default)
- A supported search server (None by default)
- A supported identity server (None by default, use local database)

### Scripts

All scripts are located in the `scripts` folder.

- `Add-Migration.ps1` - Add a new migration, provide a name as the first argument.

### Configuration

The configuration file is located in the API project with name `appsettings.yaml`.

You can use any supported configuration file format, be careful that if you choose to use YAML format, the file extension must be `.yaml`, `.yml` will not work.

By default, `appsettings.yaml` is required. In development environment, you can use `appsettings.development.yaml` to override the default settings.
