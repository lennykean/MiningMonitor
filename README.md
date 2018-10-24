# Mining Monitor

Mining Monitor is monitoring tool for ethereum mining rigs.

![Monitoring Page](./images/MonitoringScreen-1.png "Monitoring Page")


### Features

* Real time monitoring
* Mobile-friendly web UI
* Custom Alerts
* Alert actions
* Multiple installation modes
  * Docker 
  * Windows Service

## Installation

### Windows
Mining monitor can be installed as a windows service. Download the latest installer from the [releases](https://github.com/lennykean/MiningMonitor/releases) page.

The Mining Monitor web UI can be accessed on port 8506, http://[machinename]:8506

### Linux (Docker)
Mining Monitor is available as a docker container

#### Port
The Mining Monitor web UI runs on port 80 inside the container. To access the web UI, the port must be exposed from the host machine. See example below.

#### Data
Data is stored in /app/data. To persist Mining Monitor data, a volume must be mounted to the host machine, see example below.

#### Docker Command
`docker run -d -p 80:80 -v /data:/app/data lennykean/miningmonitor:latest`

## Monitoring

The monitoring page will auto-update with the last hour of data, including a graph of GPU Hashrate, Temperature, and Fanspeed. There is also a time-travel option to see the same data at any specified point in time.

## Alerting

Optional alerts can triggered when metrics fall outside of a specified range, or if a metric falls outside of a specified range for longer than a specified amount of time. The metrics include Rig Hashrate, Rig Connectivity, GPU hashrate, GPU temperature, and GPU fanspeed. 

### Actions

Alert actions can be configured to take an action when an alert is triggered. The actions that can be taken include disabling the affected GPU, or all GPUS, resetting the mining rig, or calling a webhook.

## Remote Data Collection

Multiple installations of mining monitor can be configured to send data to one central instance. This is useful for safely exposing your mining data outside of your local network. Data collector settings are available in the admin panel.

## Configuration

### Mongo Database

By default, Mining Monitor uses an in-process database. MongoDB can be used optionally be used instead by setting the environement variable `use_mongo` to `true`. The mongo connection string must also be set by using the environment variable `ConnectionStrings::miningmonitor` 

### Data Collector

To configure an installation as a data collector, set the data collector flag in the admin panel, and provide the remote server URL. On the remote server, the data collector must be marked as approved from the admin panel's data collector list.

### Authentication

From the admin panel, authentication can be enabled. At least one user must be created before authentication can be enabled. 
