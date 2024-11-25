# Instrucciones para ejecutar el proyecto

## Prerrequisitos

1. Asegúrate de tener instalados los siguientes componentes:
   - [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
   - [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) o cualquier instancia compatible.
   - [Visual Studio Code](https://code.visualstudio.com/) o un editor compatible.
   - [Postman](https://www.postman.com/downloads/) (opcional para probar la API).

2. Clona el repositorio del proyecto en tu máquina local:
   ```
   git clone https://github.com/DavidZeballos/IDWM_Taller1
   ```

3. Navega al directorio del proyecto:
   ```
   cd src
   ```

## Configuración del proyecto

### Configurar la base de datos

1. Abre el archivo `appsettings.json` en la raíz del proyecto.

2. Localiza la sección `"ConnectionStrings"` y configúrala con los datos de tu servidor de base de datos. Por ejemplo:
   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=IDWM_TallerDB;User Id=SA;Password=<TU_CONTRASEÑA>;"
   }
   ```

   - Asegúrate de que el servidor de base de datos está en funcionamiento y accesible desde tu máquina local.

## Ejecutar las migraciones

1. Navega a la carpeta raíz del proyecto:
   ```
   cd src
   ```

2. Aplica las migraciones para crear las tablas en la base de datos:
   ```
   dotnet ef migrations add InitialMigration -o src/Data/Migrations
   dotnet ef database update
   ```

## Ejecutar el proyecto

1. Inicia el servidor de la API ejecutando:
   ```
   dotnet run
   ```

2. Si todo está configurado correctamente, la API estará disponible en:
   ```
   http://localhost:5230
   ```


## Notas adicionales

- Utiliza Postman o cualquier cliente HTTP para probar los endpoints.