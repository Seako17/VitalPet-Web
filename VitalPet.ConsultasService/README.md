# Web service de consultas

Proyecto separado `VitalPet.ConsultasService`, dentro de `FGF-Finanzas.sln`. Usa ASMX con .NET Framework 4.8.1.

## Cómo está escrito

La implementación está en `ConsultasService.asmx.cs`, siguiendo el estilo del ejemplo de clase:

- Una clase que hereda de `WebService`.
- Métodos públicos con `[WebMethod]`.
- Parámetros comunes: `string`, `int` y `DateTime`.
- Conexiones `SqlConnection`, comandos `SqlCommand` y consultas con parámetros.
- Resultados como `DataTable`, listas de horarios, un identificador o un `bool`.
- Funciones privadas para las validaciones que se repiten.

No hay clases de contratos propias, firmas, claves compartidas ni un gestor intermedio. `Cifrado.cs` conserva el formato de los datos existentes. El servicio no llama a las BLL ni DAL de la web y no maneja dígitos verificadores.

## Cómo lo usa la web

Las pantallas llaman a `BLLConsulta`. La BLL obtiene el usuario de la sesión, coordina la integridad y utiliza `Capas/Servicios/ServicioConsultas` para comunicarse con el web service. Esta última clase crea `ConsultasService ws` desde la referencia web y llama a sus métodos, siguiendo el ejemplo de clase. La GUI no conoce SOAP, direcciones del servicio ni credenciales.

La clase usada por `ServicioConsultas` está en `FGF_Finanzas.ServiceReferences.Consultas`: es la referencia generada que hace la llamada HTTP al proyecto separado. No se instancia la clase que implementa el servicio en el otro proyecto. `ServiceReferences/Consultas/Reference.cs` es código generado: no hay que escribirlo ni modificarlo a mano.

Por ejemplo, `ObtenerMascotas(dni, claveUsuario)` abre una conexión, comprueba el usuario y devuelve una tabla con sus mascotas. `Agendar(...)` comprueba el horario y el veterinario antes de insertar la consulta. `CancelarPropia(...)` actualiza el estado solo si el turno pertenece al cliente y todavía puede cancelarse.

`claveUsuario` es el hash que ya tiene el usuario autenticado en la sesión de VitalPet. El servicio lo compara con el registro de Usuario mediante una consulta con parámetros y comprueba que no esté bloqueado; no se pide volver a ingresar la contraseña ni se conserva una contraseña en texto plano. El hash se usa como credencial: no se registra ni se muestra en pantalla.

Los permisos de acceso a pantallas se controlan en la aplicación. El web service no consulta permisos ni familias. Identifica a los veterinarios por el rol cuyo nombre es `Veterinario`, sin depender de un ID fijo ni del permiso `ConsultaAtender`. Conserva la validación de propiedad de las mascotas, consultas propias y disponibilidad al reservar o cancelar.

## Ejecutarlo

1. Abrir `FGF-Finanzas.sln` en Visual Studio.
2. En **Propiedades de la solución → Proyectos de inicio → Varios proyectos de inicio**, elegir **Iniciar** para `FGF-Finanzas` y `VitalPet.ConsultasService`.
3. Revisar que `DBConnection` de ambos proyectos apunte a la base local `FGF-BDD`.
4. Iniciar y usar **Agendar cita** o **Mis consultas** con un usuario cliente.

El servicio usa `http://localhost:50950/ConsultasService.asmx`. La web tiene esa dirección en `ConsultasService.Url` de su `Web.config`. No hay archivos de claves ni certificados adicionales que configurar para ejecutarlo localmente. Si se aloja en otra computadora, se debe usar HTTPS para proteger las credenciales.

## Operaciones

`ObtenerMascotas`, `ObtenerHorarios`, `ObtenerVeterinariosDisponibles`, `Agendar`, `ObtenerMisConsultas`, `ObtenerProximasConsultas`, `ObtenerDetallePropio` y `CancelarPropia`.

`ObtenerDisponibilidadDia` devuelve en una llamada los 25 horarios del día con sus cupos y estado para la mascota propia. El formulario muestra solo la hora en cada opción, deshabilita y marca en gris los horarios pasados, los ocupados por la mascota y los que no tienen veterinarios disponibles. El resumen suma los cupos de veterinarios y muestra además la cantidad de horarios que pueden elegirse. Los turnos cancelados no ocupan cupos. Al confirmar se vuelve a validar la disponibilidad dentro de la transacción de reserva.

Reserva y cancelación guardan el cambio y la auditoría en una transacción dentro del servicio. Se bloquean las escrituras simultáneas sobre consultas mientras se valida el cupo, porque las fechas existentes están cifradas. Las consultas canceladas dejan de ocupar el horario.

La integridad se coordina en `BLLConsulta`, dentro de la aplicación web: antes de escribir llama a `BLLDigitoVerificador.ValidarIntegridadDelSistema()`; después de que el servicio confirma la reserva o cancelación, llama a `InicializarTablaCompleta("Consulta")`. Las consultas de lectura no recalculan dígitos. El guardado y el recálculo son pasos separados: si falla el segundo, el cambio ya quedó guardado y la página informa el error. El proyecto del web service mantiene sus operaciones y SQL propios sin referencias a las BLL ni DAL de la web.

## Verificación

`./tests/ConsultasService/Run.ps1` ejecuta las pruebas con una base temporal de datos ficticios, la elimina al terminar y detiene su servidor de pruebas. No modifica `FGF-BDD`.

Para actualizar la referencia después de cambiar los métodos del servicio, iniciar el servicio y usar **Agregar referencia web** en Visual Studio, o ejecutar `wsdl.exe /language:CS /namespace:FGF_Finanzas.ServiceReferences.Consultas /out:FGF-Finanzas/ServiceReferences/Consultas/Reference.cs http://localhost:50950/ConsultasService.asmx?WSDL` desde la raíz.

El resumen inferior usa texto blanco. ObtenerVeterinariosDelHorario devuelve los veterinarios habilitados con una columna Disponible; el selector conserva visibles en gris y deshabilitados los ocupados. ObtenerVeterinariosDisponibles sigue devolviendo solo los libres para validar reservas.

## Integración con atención veterinaria

`BLLAtencion` obtiene las consultas mediante `BLLConsulta.ObtenerConsultas` → `ServicioConsultas.ObtenerConsultasVeterinario` → SOAP. El servicio devuelve únicamente las consultas asignadas al DNI autenticado, excluyendo canceladas. La aplicación conserva el control de acceso a las pantallas. El listado pendiente también excluye consultas con estado Atendida y registros ya atendidos en Atencion.

El guardado de atención proveniente de develop sigue en `BLLAtencion`/`DALAtencion`, con diagnóstico y observaciones cifrados y recálculo de integridad de Atencion. Todavía no actualiza estado/diagnóstico en Consulta: esa unificación queda pendiente. Por eso guardar una atención aún no la refleja como Atendida en Mis consultas. No usar esta integración como cierre del circuito completo hasta resolver esa sincronización.
