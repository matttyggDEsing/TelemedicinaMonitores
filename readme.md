# 🩺 TelemedicinaMonitores

> Monitoreo en tiempo real de pacientes con sensores biomédicos  
> 🛠️ Desarrollado con .NET 8, SignalR, EF Core, Chart.js y Bootstrap.

![telemedicina-banner](https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExdTJtZGE1OGV6YmM1bWQ0YzVhdTg4c3F0eGNkN2ttM2h0Z3k5bTQ0MiZlcD12MV9naWZzX3NlYXJjaCZjdD1n/kBUuBHsxKxW5Et0RM8/giphy.gif)

---

## 📡 ¿Cómo funciona?

### 1. Conexión de monitores
- Simuladores de pacientes se conectan al backend vía HTTP + SignalR.
- Cada uno envía datos simulados de sensores como glucosa, presión arterial, etc.

```plaintext
Simulador ─▶ POST /api/monitor/{monitorId}
          └▶ WebSocket: /monitorHub
```

---

### 2. Asignación de pacientes
- El backend relaciona automáticamente `monitorId` con un paciente registrado.
- Se identifica nombre, ubicación y sensores activos.

```csharp
var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.MonitorId == monitorId);
```

---

### 3. Historial + Alarmas

- Todos los valores se guardan en memoria (o futuro: en BD).
- Se comprueban umbrales configurados:
  - Si se exceden → alarma visual + registro.

```csharp
if (value > alarm.Max || value < alarm.Min)
{
    // ⚠️ Activar alerta
}
```

![alarmas-demo](https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExdnZsaWs4c3Q5OGpva2R3NWFlMmFqaXprN2ptYTZlMnN2dm9wYTZ0MiZlcD12MV9naWZzX3NlYXJjaCZjdD1n/dZ4rM2ogUNyf8/giphy.gif)

---

### 4. Visualización Web

#### Panel Principal:
- Tarjetas por paciente: nombre, ubicación, gráfico de último sensor, estado actual.

#### Vista Detallada:
- Historial completo por sensor.
- Gráficos dinámicos (Chart.js).
- Alarmas destacadas.

![dashboard-demo](https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExbHdlZ3gzNTZmcTNvd2duMHRscm91dWhqdmZmOTM5NWI2ZXM2YXNidSZlcD12MV9naWZzX3NlYXJjaCZjdD1n/yGhzECIOH5Krc8Nxl5/giphy.gif)

---

## 🧩 Arquitectura

- **Backend:** ASP.NET Core 8 + EF Core + SignalR
- **Frontend:** Razor Pages + Bootstrap + Chart.js
- **Datos:** Simulador externo que envía señales biomédicas
- **Comunicación:** WebSockets en tiempo real (SignalR)

---

## 🛠️ Cómo ejecutar

```bash
# 1. Restaurar paquetes
dotnet restore

# 2. Aplicar migraciones
dotnet ef database update

# 3. Correr el servidor
dotnet run

# (Opcional) Ejecutar simulador
dotnet run --project SimuladorSensores
```

---

## 💡 Ideas futuras

- 💾 Guardar historial en base de datos
- 📱 Aplicación móvil (Xamarin o MAUI)
- 🔐 Login y perfiles (médico, admin)
- 📊 Panel estadístico general
- 📥 Exportar historial a PDF / CSV
- 🧠 Análisis inteligente de patrones (IA)
- 🌍 Modo multiclínica con grupos SignalR

---

## 👤 Autor

**Maty Anderegg**  
📍 Argentina  
📧 [tuemail@ejemplo.com]  
📱 343 xxx xxxx

---

¡Gracias por visitar este proyecto!  
Si te gustó o querés colaborar, ¡bienvenido! 🙌
