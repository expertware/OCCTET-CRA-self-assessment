	# OCCTET-CRA-self-assessment

OCCTET is a free, open-source self-assessment platform that helps organisations — especially SMEs — measure their preparedness against the EU Cyber Resilience Act (CRA). The project is part of the Open CyberSecurity Compliance Toolkit (OCCTET) and is funded by the EU Digital Europe Programme.

## What this repository contains
- Self-assessment web app (frontend) located at `exp.Frontend`
- Backend API and services located at `exp.Backend`
- UI components and static assets for the public-facing site and assessment flows

## Quick start — run the frontend locally

Prerequisites
- Node.js (recommended 16+)
- npm (or use an alternative package manager like `pnpm`/`yarn`)

Steps
1. Clone the repository
   - `git clone https://github.com/expertware/OCCTET-CRA-self-assessment.git`
2. Start the frontend
   - `cd OCCTET-CRA-self-assessment/exp.Frontend`
   - `npm install`
   - `npm run dev`
3. The Vite dev server will start (by default on `http://localhost:5173/`) and hot-reload on changes.

Build for production
- From `exp.Frontend` run:
  - `npm run build`
- The production build output will be produced in the configured `dist` folder.


# OCCTET — Backend (exp.Backend)

This document describes the backend part of the repository, how it is configured and how to run it locally for development.

## Overview
The backend is an ASP.NET Core 8 application that provides authentication, survey management, email delivery, and persistence to PostgreSQL. Key features:
- ASP.NET Core 8 Web API with Identity (EF Core + PostgreSQL)
- Azure Key Vault backed secrets 
- JWT authentication 
- Services: email, surveys, organisations, reports, copilot agent, repositories, etc.
- Email templating using files under `wwwroot/EmailTemplates`
- Data protection keys persisted to disk (configured under `exp.Backend`)
- Rate limiting (sliding window)
- Serilog file logging

## Prerequisites
- .NET 8 SDK
- PostgreSQL (for local DB runs or migrations)
- Azure Key Vault 
- Visual Studio 2022 or VS Code (optional)
- Node/NPM only required for frontend work

## Important paths & files
- Backend project: `exp.Backend` (run and debug this project)
- Email templates: `wwwroot/EmailTemplates` (templates referenced by `EmailService`)
- DataProtection keys (development): `..\exp.Backend\DataProtectionKeys`
- Serilog file: `logs/myapp.txt`

## Configuration & Secrets
Production expects secrets from Azure Key Vault (see `Program.cs`). The application reads these Key Vault secrets (examples):
- `ConnectionString`
- `JWTTokenSecret`
- `SmtpHost`, `SmtpPort`, `SmtpUser`, `SmtpPass`, `EmailFrom`
- `ApplicationUrl`, `EmailContact`, `RecaptchaSecretKey`, `AgentCopilotBearer`

Locally you can provide the same values using:
- environment variables, or
- `appsettings.Development.json`, or
- `dotnet user-secrets` 

Example `appsettings.Development.json` (place in `exp.Backend`):

## Contributing
Contributions are welcome. To contribute:
1. Fork the repository
2. Implement your change on a topic branch
3. Open a pull request with a clear description of the change and any manual testing steps

If you plan larger feature work or architecture changes, open an issue first to discuss the approach.

## Issues & support
- Use the repository Issues for bug reports, feature requests, and questions.
- For project-related inquiries, refer to the project website: https://occtet.eu

## EU Funding Acknowledgement
The OCCTET project (Grant Agreement No. 101190474) is funded by the European Union under the Digital Europe Programme.
Funded by the European Union. Views and opinions expressed are however those of the author(s) only and do not necessarily reflect those of the European Union or the European Cybersecurity Industrial, Technology and Research Competence Centre. Neither the European Union nor the granting authority can be held responsible for them.

<p align="start">
  <img src="docs/eu_co_funded_en.png" width="400" style="vertical-align: top; margin-right: 200px;">
  <img src="docs/ecc.png" width="300" style="vertical-align: top">
</p>