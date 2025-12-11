#!/bin/bash
set -e

echo "🚀 Starting environment setup..."

# Update system
sudo apt-get update && sudo apt-get upgrade -y

# Install prerequisites
sudo apt-get install -y ca-certificates curl gnupg lsb-release apt-transport-https software-properties-common

# -------------------------------
# Install Docker Engine + Compose
# -------------------------------
echo "📦 Installing Docker..."
sudo mkdir -p /etc/apt/keyrings
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg

echo \
  "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] \
  https://download.docker.com/linux/ubuntu \
  $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

sudo apt-get update
sudo apt-get install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin

# Add current user to docker group
sudo usermod -aG docker $USER

# -------------------------------
# Install .NET 10 SDK
# -------------------------------
echo "🟦 Installing .NET 10 SDK..."
curl -sSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 10.0 --install-dir "$HOME/.dotnet"
rm dotnet-install.sh

# Add .NET to PATH for current session
export PATH="$HOME/.dotnet:$PATH"

# Add .NET to PATH permanently (add to shell config)
if [[ ":$PATH:" == *":$HOME/.dotnet:"* ]]; then
  echo "✅ .NET already in PATH"
else
  if [ -f "$HOME/.bashrc" ]; then
    echo 'export PATH="$HOME/.dotnet:$PATH"' >> "$HOME/.bashrc"
    echo "ℹ️  Added .NET to ~/.bashrc"
  fi
  if [ -f "$HOME/.zshrc" ]; then
    echo 'export PATH="$HOME/.dotnet:$PATH"' >> "$HOME/.zshrc"
    echo "ℹ️  Added .NET to ~/.zshrc"
  fi
fi

# -------------------------------
# Verify installations
# -------------------------------
echo "✅ Verifying installations..."
docker --version
docker compose version
dotnet --version

echo "🎉 Setup complete! Please restart your shell or run 'exec su -l $USER' to apply docker group changes."
