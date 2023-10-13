#!/bin/bash

# Expect a branch name as the first argument
branch_name=$1
# nlog
default_nlog_config_dirname="ETHTPS.API.Gateway"
default_nlog_config_filename="nlog.example.config"
nlog_config_filename="nlog.config"
# runner
default_runner_config_dirname="ETHTPS.Runner"
default_runner_config_filename="Startup.example.json"
runner_config_filename="Startup.json"

# Check if the branch name is 'dev' or 'main'
if [ "$branch_name" == "dev" ] || [ "$branch_name" == "main" ]; then
  # repository
  git fetch --all > /dev/null 2>&1
  git pull origin $branch_name > /dev/null 2>&1
  git checkout $branch_name > /dev/null 2>&1
  # nlog
  if [ ! -f "$nlog_config_filename" ]; then
    echo "nlog.config missing. Using default configuration."
    cp ./ETHTPS.API.Gateway/nlog.example.config ./nlog.config
  fi

  # .env
  rm ./.env
  if [ "$branch_name" == "dev" ]; then
    echo "ETHTPS_ENV=DEBUG" > .env
  else
    echo "ETHTPS_ENV=PRODUCTION" >> .env
  fi

  # runner
  if [ ! -f "$default_runner_config_dirname/$runner_config_filename" ]; then
    echo "Startup.json missing. Using default configuration."
    cp ./$default_runner_config_dirname/$default_runner_config_filename ./$default_runner_config_dirname/$runner_config_filename
  fi

  echo "Done. Branch '$branch_name' is ready to build."

else
  # Display an error message
  echo "Error: Invalid branch name. Expecting 'dev' or 'main'."
  exit 1
fi
