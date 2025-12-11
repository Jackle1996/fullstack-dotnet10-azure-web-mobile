# fullstack-dotnet10-azure-web-mobile
Full‑stack .NET 10 project showcasing web, mobile, and cloud engineering with Azure integration. Demonstrates backend, frontend, and cross‑platform development, secure authentication, database persistence, and modern deployment practices for a professional portfolio.

## Local development setup

This repository includes a helper script, `dev-setup.sh`, that prepares an Ubuntu-based development environment for the project. The script automates common machine setup tasks so you can get up and running quickly.

### What `dev-setup.sh` does
- Installs common prerequisites (certificates, `curl`, `gnupg`, etc.)
- Installs Docker Engine and Docker Compose plugins and adds the current user to the `docker` group
- Installs the .NET 10 SDK using Microsoft's official `dotnet-install.sh` script
- Configures the `PATH` environment variable to include .NET
- Verifies the installed tools (`docker --version`, `docker compose version`, `dotnet --version`)

### How to use
1. Open a terminal on an Ubuntu 24.04 machine or WSL2 instance.
2. Make the script executable: `chmod +x dev-setup.sh`
3. Run the script: `./dev-setup.sh`
4. After the script completes, restart your shell or run `exec su -l $USER` to apply all changes.

### Notes and recommendations
- The script requires `sudo` privileges for Docker and system package installation.
- .NET SDK is installed in `~/.dotnet` (user-specific, no `sudo` needed).
- To uninstall .NET, simply remove the `~/.dotnet` directory: `rm -rf ~/.dotnet`
- The script automatically adds .NET to your shell's `PATH` by updating `~/.bashrc` and/or `~/.zshrc`.
- After running the script you may need to restart your shell or run `exec su -l $USER` to apply the `docker` group changes.
