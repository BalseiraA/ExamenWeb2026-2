# MiApp API

Este proyecto es una API desarrollada con **ASP.NET Core**, organizada en varias capas y preparada para ejecutarse de manera local usando **.NET SDK**, **Visual Studio Code** y una colección de pruebas en **Bruno**.

---

## 1. Requisitos previos

Antes de ejecutar el proyecto, es necesario instalar las siguientes herramientas:

* Visual Studio Code
* .NET SDK 10
* Git
* Extensión C# Dev Kit para Visual Studio Code
* Extensión C# para Visual Studio Code
* Extensión Bruno para Visual Studio Code o la aplicación de Bruno
* Extensión SQLite Viewer, opcional, para revisar la base de datos local

> Importante: se debe instalar el **.NET SDK**, no solamente el **.NET Runtime**.
> El Runtime solo permite ejecutar aplicaciones ya compiladas, mientras que el SDK permite restaurar dependencias, compilar y ejecutar el proyecto desde terminal.

---

## 2. Obtener el proyecto desde GitHub

Como el proyecto ya está en GitHub, no es necesario descomprimir manualmente el archivo `bruno.zip`.

Para obtener el proyecto, se debe clonar el repositorio desde GitHub.

Abrir una terminal en la carpeta donde se quiera guardar el proyecto y ejecutar:

```bash
git clone https://github.com/BalseiraA/ExamenWeb2026-2.git
```

Después, entrar a la carpeta del proyecto:

```bash
cd ExamenWeb2026-2
```

La estructura general del proyecto debe verse de forma similar a la siguiente:

```text
bruno/
MiApp.API/
MiApp.Application/
MiApp.Domain/
MiApp.Infrastructure/
MiApp.slnx
.gitignore
```

La carpeta principal del proyecto es la que contiene el archivo:

```text
MiApp.slnx
```

Esa es la carpeta que se debe abrir en Visual Studio Code.

---

## 3. Instalar .NET SDK 10

El proyecto está configurado para trabajar con:

```xml
<TargetFramework>net10.0</TargetFramework>
```

Por eso es necesario instalar **.NET SDK 10**.

Al descargar .NET desde la página oficial de Microsoft (https://dotnet.microsoft.com/es-es/download/dotnet/10.0), se debe elegir la opción:

```text
.NET SDK 10 x64
```
No se debe elegir únicamente:

```text
.NET Runtime
ASP.NET Core Runtime
Desktop Runtime
```

El instalador correcto normalmente tiene un nombre parecido a:

```text
dotnet-sdk-10.0.x-win-x64.exe
```

Después de descargarlo:

1. Ejecutar el instalador.
2. Aceptar los permisos de administrador.
3. Presionar **Install**.
4. Esperar a que finalice.
5. Cerrar el instalador.

---

## 4. Verificar la instalación de .NET

Después de instalar el SDK, se deben cerrar y volver a abrir las terminales, incluyendo:

* PowerShell
* CMD
* Terminal de Visual Studio Code
* Visual Studio Code, si estaba abierto

Luego, abrir una nueva terminal y ejecutar:

```bash
dotnet --version
```

La respuesta debe mostrar una versión de .NET 10, por ejemplo:

```bash
10.0.x
```

Si aparece el mensaje:

```text
No .NET SDKs were found.
```

significa que se instaló solamente el Runtime o que el SDK no está correctamente agregado al PATH del sistema.

---

## 5. Corregir posible problema de PATH

Si después de instalar el SDK el comando `dotnet --list-sdks` no muestra ningún SDK, se puede revisar qué ejecutable está usando Windows con:

```bash
where.exe dotnet
```

Lo ideal es que aparezca una ruta como:

```text
C:\Program Files\dotnet\dotnet.exe
```

Si aparece una ruta de 32 bits como:

```text
C:\Program Files (x86)\dotnet\dotnet.exe
```

puede haber un conflicto de rutas.

Para corregirlo:

1. Buscar en Windows: **Editar las variables de entorno del sistema**.
2. Entrar a **Variables de entorno**.
3. En **Variables del sistema**, seleccionar `Path`.
4. Presionar **Editar**.
5. Verificar que exista esta ruta:

```text
C:\Program Files\dotnet\
```

6. Si también existe:

```text
C:\Program Files (x86)\dotnet\
```

asegurarse de que la ruta de 64 bits aparezca antes:

```text
C:\Program Files\dotnet\
```

7. Guardar los cambios.
8. Cerrar y volver a abrir la terminal.
9. Ejecutar de nuevo:

```bash
dotnet --list-sdks
dotnet --version
```

---

## 6. Instalar extensiones en Visual Studio Code

Abrir Visual Studio Code e instalar las siguientes extensiones:

```text
C# Dev Kit
C#
Bruno
SQLite Viewer
```

La extensión **C# Dev Kit** ayuda a trabajar con proyectos C# y .NET dentro de Visual Studio Code.

La extensión **Bruno** sirve para abrir y ejecutar la colección de peticiones incluida en el proyecto.

La extensión **SQLite Viewer** es opcional, pero puede ser útil para revisar la base de datos local generada por la aplicación.

---

## 7. Abrir el proyecto en Visual Studio Code

No se debe abrir únicamente la carpeta `MiApp.API`.

Se debe abrir la carpeta raíz del proyecto, es decir, la carpeta donde se encuentra:

```text
MiApp.slnx
```

Desde Visual Studio Code:

```text
File > Open Folder
```

Seleccionar la carpeta principal del repositorio clonado.

También se puede abrir desde terminal con:

```bash
code .
```

Este comando debe ejecutarse dentro de la carpeta raíz del proyecto.

---

## 8. Revisar la estructura del proyecto

Una vez abierto en Visual Studio Code, la estructura esperada es:

```text
bruno/
MiApp.API/
MiApp.Application/
MiApp.Domain/
MiApp.Infrastructure/
MiApp.slnx
.gitignore
```

Descripción general de las carpetas:

```text
MiApp.API
```

Contiene el arranque de la API, controladores, configuración general, CORS, JWT y conexión con la base de datos.

```text
MiApp.Application
```

Contiene la lógica de aplicación, validaciones y casos de uso.

```text
MiApp.Domain
```

Contiene entidades, roles e interfaces principales del dominio.

```text
MiApp.Infrastructure
```

Contiene la configuración de Entity Framework Core, SQLite, repositorios, hasheo de contraseñas y generación de tokens.

```text
bruno
```

Contiene la colección de peticiones para probar la API desde Bruno.

---

## 9. Restaurar dependencias del proyecto

Abrir una terminal en Visual Studio Code:

```text
Terminal > New Terminal
```

Asegurarse de estar en la carpeta raíz del proyecto, donde está `MiApp.slnx`.

Luego ejecutar:

```bash
dotnet restore MiApp.slnx
```

Este comando descarga las dependencias necesarias para compilar y ejecutar el proyecto.

---

## 10. Compilar el proyecto

Después de restaurar las dependencias, ejecutar:

```bash
dotnet build MiApp.slnx
```

Si todo está configurado correctamente, debe aparecer un mensaje similar a:

```text
Build succeeded.
```

Si aparece un error relacionado con `net10.0`, se debe revisar que el SDK instalado sea .NET 10.

---

## 11. Ejecutar la API

Para ejecutar el proyecto, usar el siguiente comando desde la raíz del proyecto:

```bash
dotnet run --project MiApp.API/MiApp.API.csproj --launch-profile http
```

Si la API se levanta correctamente, la terminal mostrará un mensaje parecido a:

```text
Now listening on: http://localhost:5223
Application started. Press Ctrl+C to shut down.
Hosting environment: Development
```

Esto significa que la API ya está corriendo localmente en:

```text
http://localhost:5223
```

Mientras esa terminal permanezca abierta, la API seguirá activa.

No se deben escribir más comandos en esa misma terminal mientras la API esté corriendo. Para ejecutar otros comandos, se debe abrir una terminal nueva.

---

## 12. Base de datos local

El proyecto usa SQLite como base de datos local. La cadena de conexión está configurada para generar un archivo llamado:

```text
miapp.db
```

Cuando la aplicación se ejecuta por primera vez, se crea la base de datos local y se insertan usuarios iniciales mediante el seeder del proyecto.

La base de datos local no necesita crearse manualmente antes de ejecutar el proyecto.

---

## 13. Detener la API

Para detener la API, regresar a la terminal donde se está ejecutando el servidor y presionar:

```text
Ctrl + C
```

Después de eso, la API dejará de estar disponible en:

```text
http://localhost:5223
```

---

## 14. Resumen de comandos principales

Desde la raíz del proyecto:

```bash
git clone URL_DEL_REPOSITORIO
cd NOMBRE_DEL_REPOSITORIO
dotnet --version
dotnet --list-sdks
dotnet restore MiApp.slnx
dotnet build MiApp.slnx
dotnet run --project MiApp.API/MiApp.API.csproj --launch-profile http
```

---

## 15. Estado esperado antes de probar requests en Bruno

Antes de comenzar a probar peticiones en Bruno, se debe confirmar lo siguiente:

* El repositorio fue clonado correctamente desde GitHub.
* Visual Studio Code abrió la carpeta raíz donde está `MiApp.slnx`.
* Está instalado .NET SDK 10.
* El comando `dotnet --list-sdks` muestra un SDK versión 10.
* Las dependencias fueron restauradas con `dotnet restore`.
* El proyecto compila correctamente con `dotnet build`.
* La API está corriendo con `dotnet run`.
* La terminal muestra:

```text
Now listening on: http://localhost:5223
```

Cuando todo lo anterior se cumple, el entorno local ya está preparado para comenzar las pruebas de requests con Bruno.
