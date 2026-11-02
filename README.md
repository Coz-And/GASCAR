# 🏎️ GASCAR

Book electric cars anywhere in Italy and find charging stations on an interactive map.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-12-239120?logo=csharp&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-ready-2496ED?logo=docker&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-green)

<!-- ![Screenshot](docs/screenshot.png) -->

## 📖 About

GASCAR is a nationwide platform for **booking electric vehicles** across Italy. Users can browse available cars, reserve them, and plan their trips thanks to a **map and a list of charging stations** (*colonnine di ricarica*), so they always know where to recharge.

The project is built as a multi-project .NET solution: a backend API, a web application and a dedicated client, all working on the same data.

## ✨ Features

- 🚗 **Electric car booking** available throughout Italy
- 🗺️ **Interactive map** showing charging stations and vehicles
- 📋 **Charging station list** with search and filters
- 🔌 Charging station details _(add: connector type, power, availability)_
- 🌐 Web interface and dedicated client application
- _Add more features here (user accounts, booking history, payments...)_

## 🏗️ Solution structure

| Project | Role |
|---|---|
| `GASCAR.API` | Backend API: bookings, vehicles, charging stations, business logic |
| `GASCAR.Web` | Web front-end with map and station list |
| `GascarClient` | Client application that connects to the API |

```
GASCAR/
├── GASCAR.API/       # Backend API
├── GASCAR.Web/       # Web application
├── GascarClient/     # Client application
├── GASCAR.sln        # Visual Studio solution
├── global.json       # .NET SDK version pin
├── SETUP_GUIDE.md    # Detailed setup instructions
└── README.md
```

## 💻 Languages

<p>
  <img src="https://skillicons.dev/icons?i=cs,dotnet,html,css,js" />
</p>

| Language | Used for |
|---|---|
| **C#** | API, web back-end and client logic |
| **HTML** | Web pages and views |
| **CSS** | Styling and layout |
| **JavaScript** | Interactive map and front-end behavior |
| **SQL** | Database queries _(remove if not used)_ |

## 🛠️ Tech stack

- **C# / .NET** – API, web and client
- **Maps** – _add the library used (e.g. Leaflet, Google Maps, OpenStreetMap)_
- **Database** – _add (e.g. SQL Server, PostgreSQL)_
- **Visual Studio / VS Code** – development environment

## ⚙️ Getting started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) (the exact version is pinned in `global.json`)
- A database server, if the API requires one _(adjust)_

### Installation

```bash
git clone https://github.com/Coz-And/GASCAR.git
cd GASCAR
dotnet restore
```

### Run

```bash
# API
dotnet run --project GASCAR.API

# Web (in a second terminal)
dotnet run --project GASCAR.Web

# Client (in a third terminal)
dotnet run --project GascarClient
```

For the full configuration (connection strings, ports, environment settings), see **[SETUP_GUIDE.md](SETUP_GUIDE.md)**.

## 🗺️ Roadmap

- [x] Solution structure (API, Web, Client)
- [ ] Charging station map and list
- [ ] Electric car booking
- [ ] Docker support (`docker compose up`)
- [ ] Automated tests
- [ ] Screenshots and documentation

## 👤 Author

**Andrea Cozzi** – [@Coz-And](https://github.com/Coz-And)

## 📄 License

MIT _(add a `LICENSE` file to the repository)_

