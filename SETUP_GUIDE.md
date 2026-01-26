# 🚗 GASCAR - Smart Parking & Charging System

## 📋 Problemi Risolti

✅ **Namespace mancanti** - Aggiunti a DTO e Controller  
✅ **Servizi non registrati** - `MWBotService` e `PaymentService` registrati nel DI  
✅ **JWT Key mismatch** - Sincronizzate le chiavi (ora leggono da config)  
✅ **Database vuoto** - Aggiunte MWBot, Tariff e 20 ParkingSpot  
✅ **CORS restrittivo** - Aperto a multiple porte (5213, 5174)

---

## 🚀 Come Avviare il Progetto

### **Prerequisiti**
- .NET 8.0+ / .NET 10.0
- Visual Studio 2022 o VS Code con C# extension

### **1. Avviare l'API (Porta 5184)**
```bash
cd GASCAR.API
dotnet restore
dotnet run
```
Swagger sarà disponibile su: `http://localhost:5184/swagger`

### **2. Avviare il Client Web (Porta 5213)**
```bash
cd GASCAR.Web
dotnet restore
dotnet run
```
L'app sarà disponibile su: `http://localhost:5213`

---

## 🔐 Autenticazione JWT

**Configurazione corrente:**
- **Key**: `SUPER_SECRET_KEY_123456_THIS_MUST_BE_AT_LEAST_32_CHARS_LONG`
- **Issuer**: `SmartParking`
- **Audience**: `SmartParkingUsers`
- **Durata token**: 3 ore

**Per cambiare la chiave**: Modificare `appsettings.json` nella sezione `Jwt:Key`

---

## 📊 Database Seed Automatico

Al primo avvio, il DB viene popolato con:
- **1 MWBot** (robot di carica) disponibile
- **1 Tariff** con costi: €5/ora parcheggio, €0.35/kWh carica
- **20 ParkingSpot** liberi

---

## 🧪 API Endpoints Principali

### **Autenticazione**
```
POST /api/auth/register  - Registra nuovo utente
POST /api/auth/login     - Login e ricevi JWT token
```

### **Parcheggio**
```
GET  /api/parking/spots  - Vedi tutti i posti liberi
POST /api/parking/enter  - Entra in parcheggio
POST /api/parking/exit   - Esci dal parcheggio
```

### **Ricarica**
```
POST /api/charging/request      - Richiedi ricarica
GET  /api/charging/status/{carId} - Status ricarica
```

### **Admin** (Authorized Only)
```
GET  /api/admin/parking-status  - Status complessivo
PUT  /api/admin/tariffs         - Modifica tariffe
GET  /api/admin/payments        - Report pagamenti
```

---

## 🐛 Note Importanti

1. **InMemoryDatabase**: I dati si perdono al riavvio. Per persistenza, cambiare in [appsettings.json](GASCAR.API/appsettings.json):
   ```json
   "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=SmartParkingDB;..."
   ```

2. **BCrypt**: Il progetto usa BCrypt per hash password. Assicurare che `BCrypt.Net.BCrypt` sia installato:
   ```bash
   dotnet add package BCrypt.Net-Core
   ```

3. **Token Authorization**: Tutti gli endpoint protetti richiedono header:
   ```
   Authorization: Bearer <token>
   ```

4. **CORS**: Configurato per localhost:5213 e localhost:5174

---

## 📝 File Chiave Modificati

- [Program.cs](GASCAR.API/Program.cs) - Configurazione DI, JWT, Seeding
- [appsettings.json](GASCAR.API/appsettings.json) - Config JWT e DB
- [DbSeeder.cs](GASCAR.API/Data/DbSeeder.cs) - Popolamento dati iniziali
- [Controllers/*](GASCAR.API/Controllers/) - Namespace aggiunto
- [Models/Auth/*](GASCAR.API/Models/Auth/) - Namespace aggiunto

---

## ✨ Prossimi Miglioramenti Consigliati

1. Spostare chiavi JWT in `appsettings.Development.json`
2. Aggiungere validazione input su tutti gli endpoint
3. Implementare logging strutturato (Serilog)
4. Aggiungere test unitari
5. Usare Tariff dal DB invece di default hardcoded
6. Migrare da InMemoryDatabase a SQL Server

---

**Status**: ✅ Pronto per il testing in locale
