# Presentaciones del campus

## 1) Métodos para los tipos de datos STRING

### Repaso

```csharp
string texto = "Hola Mundo";
```

### Nuevos métodos y propiedades

```csharp
int largo = texto.Length;
string porcionTexto = texto.Substring(0, 5);
int posicionSeparacionPalabra = texto.IndexOf(" ");
string nuevoTexto = texto.Replace("Hola", "Chau");
string textoMayuscula = texto.ToUpper();
string textoMinuscula = texto.ToLower();
char letra = texto[0];
```

> Nota: `texto` es un `string`, y cada carácter puede accederse mediante índice (`texto[0]`).

---

## 2) Tipo de dato DateTime

```csharp
DateTime miCumple = new DateTime(2009, 2, 25);
DateTime fechaHoraNacimiento = new DateTime(2009, 2, 25, 9, 20, 0);
DateTime fechaVacia = new DateTime();
DateTime hoy = DateTime.Today;
DateTime hoyYAhora = DateTime.Now;
```

> `DateTime` permite representar fechas y horas en C#.

---

## 3) Colecciones

### Listas y arreglos

- `List<T>`: tamaño dinámico.
- `Array`: tamaño fijo definido al crearlo.

```csharp
List<string> nombres = new List<string>();
nombres.Add("Ana");
nombres.Add("Luis");

string primerNombre = nombres[0];
nombres.Remove("Ana");

foreach (string nombre in nombres)
{
    Console.WriteLine(nombre);
}
```

---

## 4) Dictionary

### Definición

```csharp
Dictionary<int, Alumno> dicAlumnos = new Dictionary<int, Alumno>();
```

### Operaciones comunes

```csharp
dicAlumnos.Add(dni, objAlumno);
Alumno alumno = dicAlumnos[dni];

bool existe = dicAlumnos.ContainsKey(dni);
dicAlumnos.Remove(dni);
```

### Recorrido por claves

```csharp
foreach (int clave in dicAlumnos.Keys)
{
    Console.WriteLine(clave);
    Console.WriteLine(dicAlumnos[clave]);
}
```

### Recorrido por valores

```csharp
foreach (Alumno objAlum in dicAlumnos.Values)
{
    Console.WriteLine(objAlum.Nombre);
}
```

### Iteración completa

```csharp
foreach (KeyValuePair<int, Alumno> alumno in dicAlumnos)
{
    Console.WriteLine(alumno.Key);
    Console.WriteLine(alumno.Value.Nombre);
}
```

También se puede usar `var`:

```csharp
foreach (var alumno in dicAlumnos)
{
    Console.WriteLine(alumno.Key);
    Console.WriteLine(alumno.Value.Nombre);
}
```

---

## 5) Fundamentos de .NET y arquitectura MVC

### ¿Qué es .NET?

.NET es un framework para crear aplicaciones web, desktop y servicios, con soporte en Windows, Linux y macOS.

### MVC

MVC separa la aplicación en tres partes:

- Modelo: representa los datos y la lógica de negocio.
- Vista: presenta la interfaz al usuario.
- Controlador: recibe la petición y decide qué mostrar.

### Estructura de un proyecto MVC

```text
/Controllers
/Models
/Views
/wwwroot
Program.cs
appsettings.json
```

### Controlador básico

```csharp
using Microsoft.AspNetCore.Mvc;

namespace TP4.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
    public IActionResult ListaEstudiantes() => View();
    public IActionResult InfoEstudiante() => View();
}
```

### Modelo básico

```csharp
namespace PrimerProyecto.Models;

public class Auto
{
    public string Patente { get; set; }
    public string Marca { get; set; }

    public Auto(string patente, string marca)
    {
        Patente = patente;
        Marca = marca;
    }
}
```

### Envío de datos a la vista

```csharp
private static Auto miAuto;

public IActionResult Index()
{
    miAuto = new Auto("AAA123", "Chevrolet");
    ViewBag.MiAuto = miAuto;
    return View();
}
```

---

## 6) Vistas, Razor y enlaces dinámicos

### Razor

Razor permite mezclar HTML con código C# dentro de archivos `.cshtml`.

```cshtml
<h1>El nombre del alumno es @ViewBag.NombreAlumno y tiene @ViewBag.Edad años</h1>
```

### `ViewData` y `ViewBag`

```csharp
ViewData["Title"] = "Index";
ViewBag.NombreAlumno = "Carlos";
```

### Enlaces con `Url.Action`

```cshtml
<a href='@Url.Action("InfoEstudiante", "Estudiante")'>Federico Perchuk</a>
<a href='@Url.Action("ListadoEstudiantes", "Estudiante", new { Curso = "4A" })'>Ir al Curso 4A</a>
```

### Diferencia entre `Return View` y `RedirectToAction`

- `return View()` renderiza dentro del mismo request.
- `RedirectToAction()` ejecuta una nueva petición HTTP.

---

## 7) Formularios HTML

### Estructura básica

```html
<form action='@Url.Action("GuardarDatos", "Home")' method="POST" enctype="multipart/form-data">
    <input type="text" name="nombre" />
    <input type="password" name="contrasena" />
    <input type="submit" value="Guardar" />
</form>
```

### Atributos importantes

- `action`: URL de destino.
- `method`: `GET` o `POST`.
- `enctype`: codificación del formulario, por ejemplo `multipart/form-data`.

### Tipos de input

```html
<input type="text" name="nombre" />
<input type="password" name="contrasena" />
<textarea name="descripcionLarga" rows="10" cols="5"></textarea>
<input type="email" name="mail" />
<input type="tel" name="telefono" />
<input type="color" name="colorPelo" />
<input type="url" name="paginaWeb" />
<input type="date" name="fecha" />
<input type="number" name="edad" />
```

### Radio, checkbox y select

```html
<input type="radio" name="color" value="rojo" /> Rojo
<input type="radio" name="color" value="azul" /> Azul
<input type="radio" name="color" value="verde" /> Verde

<input type="checkbox" name="deportes" /> Deportes
<input type="checkbox" name="musica" /> Música
<input type="checkbox" name="cine" /> Cine

<select name="pais">
    <option value="1">Argentina</option>
    <option value="2">España</option>
    <option value="3">México</option>
    <option value="4">Chile</option>
</select>
```

### Input oculto y archivo

```html
<input type="hidden" name="IdPelicula" value="1" />
<input type="file" name="foto" />
```

### Botones

```html
<input type="submit" value="Guardar" />
<input type="reset" value="Borrar Formulario" />
<input type="image" src="/img/guardar.jpg" />
<input type="button" onclick="funcion()" />
```

---

## 8) Recepción de información desde un formulario

### View

```cshtml
<form action="@Url.Action("Login", "Cuenta")" method="post">
    Usuario: <input type="text" name="usuario" />
    Contraseña: <input type="password" name="contrasena" />
    <input type="submit" value="Entrar" />
</form>
```

### Controller

```csharp
public class CuentaController : Controller
{
    [HttpPost]
    public ActionResult Login(string usuario, string contrasena)
    {
        ViewBag.Usuario = usuario;
        return View("Bienvenido");
    }
}
```

> Importante: los atributos `name` de los `input` deben coincidir con los parámetros del controlador.

### Subir archivo

```cshtml
<form action='@Url.Action("SubirArchivo", "Home")' method="post" enctype="multipart/form-data">
    <input type="file" name="archivo" />
    <button type="submit">Subir</button>
</form>
```

```csharp
private readonly IWebHostEnvironment _env;

public HomeController(IWebHostEnvironment env)
{
    _env = env;
}

public IActionResult SubirArchivo(IFormFile archivo)
{
    if (archivo != null && archivo.Length > 0)
    {
        string nombreArchivo = archivo.FileName;
        string rutaCarpeta = Path.Combine(_env.WebRootPath, "imagenes");

        if (!Directory.Exists(rutaCarpeta))
        {
            Directory.CreateDirectory(rutaCarpeta);
        }

        string rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);

        using (var stream = new FileStream(rutaCompleta, FileMode.Create))
        {
            archivo.CopyTo(stream);
        }

        return View("Resultado");
    }

    ViewBag.Mensaje = "No se seleccionó ningún archivo.";
    return View("Index");
}
```

---

## 9) Getters y setters

### Ejemplo clásico

```csharp
class Persona
{
    private string nombre;

    public string DevolverNombre()
    {
        return nombre;
    }

    public void CambiarNombre(string nuevoNombre)
    {
        nombre = nuevoNombre;
    }
}
```

### Propiedades automáticas

```csharp
class Persona
{
    public string Nombre { get; set; }
}
```

```csharp
Persona p = new Persona();
p.Nombre = "Carlos";
Console.WriteLine(p.Nombre);
```

### Solo lectura o solo escritura

```csharp
class Persona
{
    public string Nombre { get; private set; }
}
```

> En este caso, se puede leer `Nombre`, pero solo la propia clase puede cambiarlo.

### Enviar datos del formulario a un objeto

```cshtml
<form action='@Url.Action("Login", "Home")' method="POST">
    <input type="text" name="NombreUsuario" placeholder="Usuario" />
    <input type="password" name="Contrasena" placeholder="Password" />
    <input type="submit" value="Ingresar" />
</form>
```

```csharp
[HttpPost]
public IActionResult Login(Usuario user)
{
    ViewBag.Nombre = user.NombreUsuario;
    ViewBag.Contrasena = user.Contrasena;
    return View("Index");
}
```

> Los nombres de los inputs deben coincidir con los nombres de las propiedades del modelo.

---

## 10) JavaScript

### ¿Qué es JavaScript?

JavaScript es un lenguaje de programación interpretado que permite agregar comportamiento a la web.

### Variables

```javascript
let nombre;
nombre = "Lionel";

const apellido = "Scaloni";
```

### Uso de `let` y `const`

```javascript
function saludar() {
    const nombre = "Ezequiel";
    console.log("Hola " + nombre + "!");

    let apellido = "Perez";
    console.log("Hola " + apellido + "!");

    apellido = "Gonzalez";
    console.log("Hola " + apellido + "!");
}
```

> `const` no se puede reasignar; `let` sí.

### Tipos primitivos

```javascript
let foo = 42;
foo = "bar";
foo = true;
```

### Condicionales

```javascript
if (num1 > num2) {
    console.log("Num 1 es mayor a num 2");
} else if (num1 === num2) {
    console.log("Son iguales");
} else {
    console.log("Num 2 es mayor a num 1");
}
```

### Switch

```javascript
switch (categoria) {
    case 'A':
        console.log(precio * TIPO_A);
        break;
    case 'B':
        console.log(precio * TIPO_B);
        break;
    default:
        console.log("categoría inexistente");
        break;
}
```

### Ciclos

```javascript
for (let i = 0; i < 100; i++) {
    console.log(i);
}

while (!encontrado) {
    // código
}

do {
    // código
} while (!encontrado);
```

### Funciones

```javascript
function sumar(num1, num2) {
    return num1 + num2;
}
```

### DOM

```javascript
const parrafo = document.getElementById('parrafo');
parrafo.innerHTML = "Taller de Programación";
```

### Input text y button

```html
<input type="text" name="NombreEmpleado" value="Juan" id="Nombre" />
<button id="Actualizar" onclick="cambiarColor()" estado="Azul">Cambiar Color</button>
```

```javascript
function cambiarColor() {
    let boton = document.getElementById("Actualizar");
    const objH1 = document.getElementById("MostrarTexto");

    objH1.style.color = (boton.getAttribute("estado") == "Azul" ? 'green' : 'blue');
    boton.setAttribute("estado", (boton.getAttribute("estado") == "Azul" ? 'Verde' : 'Azul'));
}
```

---

## 11) Dapper y SQL Server

### Instalación de paquetes

```bash
dotnet add package Microsoft.Data.SqlClient
dotnet add package Dapper
```

### Archivo `.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net7.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <RootNamespace>TP_Prog</RootNamespace>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Dapper" Version="2.1.66" />
    <PackageReference Include="Microsoft.Data.SqlClient" Version="6.0.1" />
  </ItemGroup>
</Project>
```

### ConnectionString

```csharp
private string _connectionString = @"Server=localhost;Database=NombreBase;Integrated Security=True;TrustServerCertificate=True;";
```

### `Query`

```csharp
public List<Patente> LevantarPatentes()
{
    List<Patente> patentes = new List<Patente>();

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = "SELECT * FROM Patentes";
        patentes = connection.Query<Patente>(query).ToList();
    }

    return patentes;
}
```

### `QueryFirstOrDefault`

```csharp
public Patente LevantarPatente(string patente)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = "SELECT Matricula FROM Patentes WHERE Matricula = @pPatente";
        return connection.QueryFirstOrDefault<Patente>(query, new { pPatente = patente });
    }
}
```

### `Execute` con `INSERT`

```csharp
public void AgregarJugador(Jugador jug)
{
    string query = "INSERT INTO Jugadores (IdEquipo, Nombre, FechaNacimiento) VALUES (@pIdEquipo, @pNombre, @pFechaNacimiento)";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        connection.Execute(query, new
        {
            pIdEquipo = jug.IdEquipo,
            pNombre = jug.Nombre,
            pFechaNacimiento = jug.FechaNacimiento
        });
    }
}
```

### `Execute` con `DELETE`

```csharp
public int EliminarPatente(string patenteAEliminar)
{
    string query = "DELETE FROM Patentes WHERE Matricula = @patenteAEliminar";

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        return connection.Execute(query, new { patenteAEliminar });
    }
}
```

### Stored Procedure con `CommandType.StoredProcedure`

```csharp
public List<Patente> LevantarPatentes(string letraInicial)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string storedProcedure = "TraerPatentesxLetra";

        return connection.Query<Patente>(
            storedProcedure,
            new { Letra = letraInicial },
            commandType: CommandType.StoredProcedure
        ).ToList();
    }
}
```

> Importante: agregar `using System.Data;` para usar `CommandType`.

---

## 12) Sessions en ASP.NET Core

### Configuración

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession();
```

```csharp
app.UseSession();
```

### Guardar y leer datos de sesión

```csharp
HttpContext.Session.SetString("Nombre", nombreUsuario);
HttpContext.Session.SetString("DNI", dni.ToString());

string nombre = HttpContext.Session.GetString("Nombre");
int dni = int.Parse(HttpContext.Session.GetString("DNI"));
```

### Eliminar datos

```csharp
HttpContext.Session.Remove("Usuario");
HttpContext.Session.Clear();
```

---

## 13) SQL: Stored Procedures

### Ejemplo básico

```sql
CREATE PROCEDURE ListarClientes
AS
BEGIN
    SELECT * FROM Clientes
END
GO
```

### Con parámetro

```sql
CREATE PROCEDURE ListarUnCliente
    @Id int
AS
BEGIN
    SELECT * FROM Clientes WHERE IdCliente = @Id
END
GO
```

### Ejecución

```sql
EXEC ListarClientes;
EXEC ListarUnCliente 1;
```

### Insert con parámetro

```sql
CREATE PROCEDURE InsertarNuevoCliente
    @NombreCliente varchar(50),
    @CUIT varchar(50),
    @FotoCliente varchar(50),
    @FechaUltimaCompra date,
    @IdProvincia int
AS
BEGIN
    INSERT INTO Clientes (NombreCliente, CUIT, FotoCliente, FechaUltimaCompra, IdProvincia)
    VALUES (@NombreCliente, @CUIT, @FotoCliente, @FechaUltimaCompra, @IdProvincia)
END
GO
```

### Verificar existencia con IF EXISTS / IF NOT EXISTS

```sql
CREATE PROCEDURE sp_InsertarOModificarGusto
    @gusto NVARCHAR(50),
    @cantidad int
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM GustosHeladeria WHERE IdGusto = @gusto)
    BEGIN
        INSERT INTO GustosHeladeria (IdGusto, Cantidad)
        VALUES (@gusto, @cantidad);
    END
    ELSE
    BEGIN
        UPDATE GustosHeladeria
        SET Cantidad = @cantidad
        WHERE IdGusto = @gusto;
    END
END
```

### Variables internas

```sql
CREATE PROCEDURE CantidadGustosPorSucursal
    @Sucursal1 INT,
    @Sucursal2 INT
AS
BEGIN
    DECLARE @CantidadGustos INT;
    SET @CantidadGustos = 0;

    SELECT @CantidadGustos += COUNT(*)
    FROM GustosHeladeria
    WHERE IdHeladeria = @Sucursal1;

    SELECT @CantidadGustos += COUNT(*)
    FROM GustosHeladeria
    WHERE IdHeladeria = @Sucursal2;

    SELECT @CantidadGustos;
END;
```

---

## 14) JOINs

### INNER JOIN

```sql
SELECT Pedidos.ID_pedido, Pedidos.Fecha, Clientes.Nombre, Clientes.Direccion
FROM Pedidos
INNER JOIN Clientes ON Pedidos.ID_cliente = Clientes.ID_cliente;
```

### LEFT JOIN

```sql
SELECT Clientes.Nombre, Pedidos.ID_pedido, Pedidos.Fecha
FROM Clientes
LEFT JOIN Pedidos ON Clientes.ID_cliente = Pedidos.ID_cliente;
```

### RIGHT JOIN

```sql
SELECT Clientes.Nombre, Pedidos.ID_pedido, Pedidos.Fecha
FROM Clientes
RIGHT JOIN Pedidos ON Clientes.ID_cliente = Pedidos.ID_cliente;
```

---

## 15) DML y DDL básicos

### DML

```sql
SELECT * FROM Clientes WHERE Mail LIKE 'juan%';
INSERT INTO Clientes (mail, telefono) VALUES ('jesi@rodriguez.com', '1266342233');
UPDATE Clientes SET mail = 'nuevomail@gmail.com' WHERE id = 2;
DELETE FROM Clientes WHERE Mail LIKE 'V%';
```

### DDL

```sql
CREATE TABLE Notas (
    nota DOUBLE NOT NULL,
    trimestre INTEGER NOT NULL,
    dni INTEGER REFERENCES Alumnos(dni) ON UPDATE RESTRICT ON DELETE SET NULL,
    codigoMateria INTEGER
);
```

### Restricciones

- `PRIMARY KEY`: identifica de forma única cada fila.
- `NOT NULL`: no acepta valores nulos.
- `AUTO_INCREMENT`: genera un valor consecutivo.
- `FOREIGN KEY` / `REFERENCES`: relaciona una tabla con otra.

---

## 16) Normalización

### 1FN

Una tabla está en 1FN si cada columna contiene un valor atómico y no hay listas repetidas dentro de una misma fila.

### 2FN

Una tabla está en 2FN si está en 1FN y cada atributo no clave depende completamente de la clave primaria.

### 3FN

Una tabla está en 3FN si está en 2FN y no existen dependencias transitivas entre columnas no clave.

---

## 17) Funciones de agregación

```sql
SELECT COUNT(*) FROM clientes;
SELECT SUM(ventas) FROM ordenes;
SELECT AVG(precio) FROM productos;
SELECT MIN(fechaNacimiento) FROM personas;
SELECT MAX(salario) FROM empleados;
```

### GROUP BY

```sql
SELECT categoria, SUM(cantidad)
FROM Ventas
GROUP BY categoria;
```

### HAVING

```sql
SELECT departamento, AVG(salario)
FROM Empleados
GROUP BY departamento
HAVING AVG(salario) > 50000;
```

> `WHERE` filtra filas; `HAVING` filtra grupos ya agregados.

---

## 18) Cierre

Los conceptos más importantes para reforzar son:

- C# y ASP.NET MVC
- HTML + formularios + Razor
- JavaScript + DOM
- Dapper + SQL Server
- Stored procedures, joins y normalización
- Sessions y manejo de estado en web

Este archivo fue revisado y corregido en varios puntos para dejar ejemplos consistentes con la sintaxis real de C#, ASP.NET, JavaScript y SQL.
