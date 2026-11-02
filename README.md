🎮 Games-Platform

A distributed platform built around three cooperating components: a sensor simulator, an edge server and a central server.

![Java](https://img.shields.io/badge/Java-17+-ED8B00?logo=openjdk&logoColor=white)
![HTML](https://img.shields.io/badge/HTML-5-E34F26?logo=html5&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-ready-2496ED?logo=docker&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-green)

Games-Platform is a multi-module project where simulated sensors generate data, an edge layer collects and pre-processes it close to the source, and a central server aggregates everything and exposes it through a web interface.



🏗️ Architecture
┌──────────────────┐      ┌──────────────┐      ┌────────────────┐
│ sensor-simulator │ ───▶ │ edge-server  │ ───▶ │ central-server │
│ generates data   │      │ collects and │      │ aggregates and │
│                  │      │ pre-processes│      │ serves the UI  │
└──────────────────┘      └──────────────┘      └────────────────┘


Module	Role
sensor-simulator	Simulates sensors and sends generated data to the edge layer
edge-server	Receives data from the sensors and forwards it to the central server
central-server	Stores/aggregates the data and provides the web front-end

✨ Features
Simulated sensors producing test data
Edge layer between sensors and central system
Central server with web interface
Add your real features here

🛠️ Tech stack
Java – backend modules
HTML – web front-end
Add frameworks/libraries used (e.g. Spring Boot, Maven/Gradle, database)

⚙️ Getting started
Prerequisites
JDK 17 or later (adjust to your version)
Maven or Gradle (adjust to your build tool)
Installation
bash

git clone https://github.com/Coz-And/Games-Platform.git
cd Games-Platform
Run

Start the components in this order:

bash
# 1. Central server
cd central-server
# ./mvnw spring-boot:run   (or your actual command)

# 2. Edge server
cd ../edge-server
# ./mvnw spring-boot:run

# 3. Sensor simulator
cd ../sensor-simulator
# ./mvnw spring-boot:run

Then open http://localhost:8080 (adjust port).

📂 Project structure
Games-Platform/
├── central-server/     # Central server and web front-end
├── edge-server/        # Edge layer
├── sensor-simulator/   # Sensor data generator
└── README.md

🗺️ Roadmap
 Base architecture (sensors → edge → central)
 Docker support (docker compose up)
 Documentation and screenshots
 Automated tests

👤 Author
  Andrea Cozzi – @Coz-And

📄 License

MIT (add a LICENSE file to the repository)
