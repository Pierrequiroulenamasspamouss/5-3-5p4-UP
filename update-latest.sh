#!/bin/bash
set -e

########################################

# CONFIGURATION

########################################
GITHUB_USER="Pierrequiroulenamasspamouss"
REPO_NAME="5-3-5p4-UP"
BRANCH="main"
SERVER_DIR="SERVER"
TARGET_DIR="/opt/minions"

# Use environment variable for token (safer)

# export GITHUB_TOKEN=7JxNAU4242CvdU51kDNwqVdbSY6o7m3o4a9i

REPO_URL="https://${GITHUB_TOKEN}@github.com/${GITHUB_USER}/${REPO_NAME}.git"

########################################

# PRECHECKS

########################################
if ! command -v git &> /dev/null; then
echo "Git not found, installing..."
sudo dnf install -y git-all || sudo apt-get install -y git
fi

if ! command -v rsync &> /dev/null; then
echo "rsync not found, installing..."
sudo dnf install -y rsync || sudo apt-get install -y rsync
fi

########################################

# CREATE STAGING

########################################
STAGING_DIR=$(mktemp -d)
echo "Using staging directory: $STAGING_DIR"

########################################

# CLONE REPO

########################################
echo "Cloning repository..."
git clone --branch "$BRANCH" --single-branch --depth 1 
"$REPO_URL" "$STAGING_DIR/repo"

if [ ! -d "$STAGING_DIR/repo/$SERVER_DIR" ]; then
echo "Error: SERVER directory not found!"
rm -rf "$STAGING_DIR"
exit 1
fi

########################################

# BACKUP IMPORTANT DATA

########################################
echo "Backing up persistent data..."
mkdir -p "$STAGING_DIR/backup"

cp -f "$TARGET_DIR"/*.sqlite "$STAGING_DIR/backup/" 2>/dev/null || true
cp -f "$TARGET_DIR"/*.env "$STAGING_DIR/backup/" 2>/dev/null || true
cp -r "$TARGET_DIR/playerdata" "$STAGING_DIR/backup/" 2>/dev/null || true

########################################

# UPDATE FILES (SAFE SYNC)

########################################
echo "Updating files..."

rsync -av --delete 
--exclude='.git' 
--exclude='node_modules' 
--exclude='*.sqlite' 
--exclude='*.env' 
--exclude='playerdata' 
"$STAGING_DIR/repo/$SERVER_DIR/" "$TARGET_DIR/"

########################################

# RESTORE BACKUPS

########################################
echo "Restoring persistent data..."

cp -f "$STAGING_DIR/backup"/*.sqlite "$TARGET_DIR/" 2>/dev/null || true
cp -f "$STAGING_DIR/backup"/*.env "$TARGET_DIR/" 2>/dev/null || true

if [ -d "$STAGING_DIR/backup/playerdata" ]; then
rm -rf "$TARGET_DIR/playerdata"
cp -r "$STAGING_DIR/backup/playerdata" "$TARGET_DIR/"
fi

########################################

# PERMISSIONS (SAFER DEFAULTS)

########################################
echo "Setting permissions..."

chown -R $(id -u):$(id -g) "$TARGET_DIR" 2>/dev/null || true

# safer than 777 everywhere

find "$TARGET_DIR" -type d -exec chmod 755 {} ;
find "$TARGET_DIR" -type f -exec chmod 644 {} ;

# sqlite needs write access

find "$TARGET_DIR" -name "*.sqlite" -exec chmod 666 {} ;

########################################

# CLEANUP

########################################
rm -rf "$STAGING_DIR"

########################################

# RESTART SERVICE

########################################
echo "Restarting service..."
if systemctl list-units --full -all | grep -Fq "minions.service"; then
    systemctl restart minions
else
    service minions restart
fi

echo "✅ Update completed successfully"
exit 0
