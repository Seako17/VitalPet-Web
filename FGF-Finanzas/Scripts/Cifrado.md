## Criterio

Se conserva el AES existente de Usuario y del web service. Los métodos de acceso cifran al escribir y descifran al leer. La contraseña sigue siendo un hash SHA-256, nunca reversible.

| Tabla | Campos reversibles |
| --- | --- |
| Usuario | nombre, apellido, usuario, mail, rol_viejo |
| Mascota | nombre, especie, raza, fechaNacimiento |
| Consulta | motivo, fechaHora, estado, diagnostico, fechaCancelacion (ya estaban cifrados) |
| Atencion | diagnostico, observaciones (ya estaban cifrados) |
| Evento | modulo, evento, fechaHora |
| Rol y Familia | nombre, descripcion |
| Permiso | descripcion; nombre es el código técnico utilizado para acceder a pantallas |
| Idioma | Nombre; Codigo conserva la referencia es-AR/en-US |
| Traduccion | Texto |
| Producto | Nombre, Descripcion, Imagen (ya estaban cifrados) |
| Compra | Estado, TitularTarjeta, Ultimos4, Fecha |
| CompraItem | NombreProducto |
| InformeConsultas | NombreVeterinario, Generado |
| InformeConsultasDetalle | NombreVeterinario |

No se cifran claves primarias/foráneas (incluidos DNI), booleanos, importes, cantidades, meses/años de filtros ni códigos técnicos. Etiqueta conserva Formulario y ControlId para identificar controles; DigitoVerificador conserva el nombre de la tabla y sus hashes. Las tablas de relaciones, CarritoItem e InformeConsultasMes contienen únicamente claves y valores numéricos.

Las nuevas fechas cifradas usan `yyyy-MM-dd HH:mm:ss.fffffff`. Las de Consulta mantienen su formato previo. La aplicación devuelve DateTime y ordena/filtra después del descifrado. El mapa de Encriptacion también se usa en las tres rutas del cálculo de integridad para evitar discrepancias.

## Base existente o instalación nueva

1. Detener la aplicación y el web service. Compilar la solución actualizada.
2. Comprobar DBConnection en Web.config. Si es una instalación nueva, primero ejecutar script.sql y preparar las tablas de consultas e informes que requiere esta rama.
3. Ejecutar `Scripts/Actualizar-Cifrado.ps1` desde PowerShell. Requiere permisos de backup y modificación en SQL Server. Las utilidades toman un respaldo COPY_ONLY con CHECKSUM y lo verifican antes de cambiar datos. No se debe ejecutar el instalador completo sobre una base existente.
4. Las migraciones conservan los valores y filas, reconocen el cifrado previo, amplían las columnas de texto y actualizan los dígitos dentro de una transacción. Si falla una etapa, se revierte esa etapa y se informa el error. Se pueden volver a ejecutar.
5. Volver a iniciar ambos proyectos y cerrar/iniciar sesión para renovar los datos en memoria.

Los respaldos quedan en la carpeta configurada en SQL Server. Se respeta el esquema AES de la aplicación; no se cambiaron clave ni IV.

## Verificación realizada

Se verificó el valor de cada campo antes/después de migrar y se conservaron las cantidades de registros. Pasaron 28 comprobaciones de lecturas y escrituras en una copia aislada con el esquema completo: mascotas, fechas, roles, familias, permisos, idiomas, traducciones, productos, compra y detalle, informes históricos/nuevos y búsquedas/escrituras del servicio. La copia de pruebas se eliminó al terminar. No se realizaron compras ni altas de prueba en la base real.
