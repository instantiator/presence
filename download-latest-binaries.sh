#!/bin/bash

# prerequisites
# sudo apt-get install jq
# brew install jq

usage() {
  cat << EOF

Downloads the latest Presence binaries for your system into the current directory.

Options:
    -s <system>      --system <system>                   Which system binaries to retrieve, from: linux-x64, osx-x64, win-x64
    -t <path>        --target-directory <path>           The directory to download the binaries to (optional)
    -h               --help                              Prints this help message and exits

Defaults:
    -s linux-x64
    -t /usr/local/bin

EOF
}

# defaults
TARGET_DIRECTORY=/usr/local/bin
SYSTEM=linux-x64
TMP_DIR=$(mktemp -d)
ZIP_FILENAME=Presence.zip
ZIP_PATH=${TMP_DIR}/${ZIP_FILENAME}
GH_ACCOUNT=instantiator
GH_REPO=presence
GH_FILENAME=Presence.zip

# binaries as an array
BINARIES=("Presence.SocialFormat.Console" "Presence.Posting.Console")

# parameters
while [ -n "$1" ]; do
  case $1 in
  -t | --target-directory)
    shift
    TARGET_DIRECTORY=$1
    ;;
  -s | --system)
    shift
    SYSTEM=$1
    ;;
  -h | --help)
    usage
    exit 0
    ;;
  *)
    echo "Unknown option $1..."
    usage
    exit 1
    ;;
  esac
  shift
done

# if SYSTEM is not linux-x64, osx-x64, or win-x64, then exit
if [ "$SYSTEM" != "linux-x64" ] && [ "$SYSTEM" != "osx-x64" ] && [ "$SYSTEM" != "win-x64" ]; then
  echo "Invalid system: $SYSTEM"
  usage
  exit 1
fi

# remove the trailing slash from DOWNLOAD_DIRECTORY
TARGET_DIRECTORY=$(echo $TARGET_DIRECTORY | sed 's:/*$::')

# get the latest release tag
RELEASE_DATA=$( wget -q -O - https://api.github.com/repos/${GH_ACCOUNT}/${GH_REPO}/releases )
LATEST_RELEASE=$( echo $RELEASE_DATA | jq -r '.[].tag_name' | sort -V | tail -1 )

# get the latest release download URL
LATEST_RELEASE_DOWNLOAD_URL=$( echo $RELEASE_DATA | jq -r ".[] | select(.tag_name==\"${LATEST_RELEASE}\") | .assets[] | select(.name==\"${GH_FILENAME}\") | .browser_download_url" )
echo "Downloading latest release from: ${LATEST_RELEASE_DOWNLOAD_URL}"

# retrieve and unzip into the target (temp) directory
wget -O $ZIP_PATH $LATEST_RELEASE_DOWNLOAD_URL
unzip -o $ZIP_PATH -d $TMP_DIR

# copy the binaries into the target directory
for BINARY in "${BINARIES[@]}"; do
  # establish binary file names (suffix .exe for Windows)
  if [ "$SYSTEM" == "win-x64" ]; then
    BINARY_FILE=("${BINARY}.exe")
  else
    BINARY_FILE=$BINARY
  fi

  echo "Copying from: ${TMP_DIR}/release/${SYSTEM}/${BINARY_FILE}"
  echo "Copying to:   ${TARGET_DIRECTORY}/${BINARY_FILE}"
  cp ${TMP_DIR}/release/${SYSTEM}/${BINARY_FILE} ${TARGET_DIRECTORY}/${BINARY_FILE}
done
