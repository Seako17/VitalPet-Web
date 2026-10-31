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
   La base debe incluir `Atencion`: `id_atencion` (int identity, clave primaria), `id_consulta` (int, referencia a Consulta), `diagnostico` y `observaciones` (nvarchar(max), cifrados) y `DV_Horizontal` (varchar(max)). Registrar también la integridad de esa tabla; al crearse vacía su cantidad es 0 y su dígito vertical es `0`.
   Si existe `CK_Consulta_Estado`, debe admitir los valores cifrados de Agendada, Atendida, Cancelada y NoSePresento. Una regla anterior que permita solo tres estados impedirá guardar ausencias, aunque el servicio esté disponible.
4. Iniciar y usar **Agendar cita** o **Mis consultas** con un usuario cliente.

El servicio usa `http://localhost:50950/ConsultasService.asmx`. La web tiene esa dirección en `ConsultasService.Url` de su `Web.config`. No hay archivos de claves ni certificados adicionales que configurar para ejecutarlo localmente. Si se aloja en otra computadora, se debe usar HTTPS para proteger las credenciales.

## Operaciones

`ObtenerMascotas`, `ObtenerHorarios`, `ObtenerVeterinariosDisponibles`, `Agendar`, `ObtenerMisConsultas`, `ObtenerProximasConsultas`, `ObtenerDetallePropio` y `CancelarPropia`.

`ObtenerDisponibilidadDia` devuelve en una llamada los 25 horarios del día con sus cupos y estado para la mascota propia. El formulario muestra solo la hora en cada opción, deshabilita y marca en gris los horarios pasados, los ocupados por la mascota y los que no tienen veterinarios disponibles. El resumen suma los cupos de veterinarios y muestra además la cantidad de horarios que pueden elegirse. Los turnos cancelados no ocupan cupos. Al confirmar se vuelve a validar la disponibilidad dentro de la transacción de reserva.

Reserva y cancelación guardan el cambio y la auditoría en una transacción dentro del servicio. Se bloquean las escrituras simultáneas sobre consultas mientras se valida el cupo, porque las fechas existentes están cifradas. Las consultas canceladas dejan de ocupar el horario.

La integridad se coordina en `BLLConsulta`, dentro de la aplicación web: antes de escribir llama a `BLLDigitoVerificador.ValidarIntegridadDelSistema()`; después de que el servicio confirma la reserva o cancelación, llama a `InicializarTablaCompleta("Consulta")`. Las consultas de lectura no recalculan dígitos. El guardado y el recálculo son pasos separados: si falla el segundo, el cambio ya quedó guardado y la página informa el error. El proyecto del web service mantiene sus operaciones y SQL propios sin referencias a las BLL ni DAL de la web.

## Verificación

La verificación habitual se realiza mediante revisión del código, compilación de la solución y precompilación ASP.NET. No ejecutar pruebas con datos salvo pedido del usuario o necesidad justificada.

Para actualizar la referencia después de cambiar los métodos del servicio, iniciar el servicio y usar **Agregar referencia web** en Visual Studio, o ejecutar `wsdl.exe /language:CS /namespace:FGF_Finanzas.ServiceReferences.Consultas /out:FGF-Finanzas/ServiceReferences/Consultas/Reference.cs http://localhost:50950/ConsultasService.asmx?WSDL` desde la raíz.

El resumen inferior usa texto blanco. ObtenerVeterinariosDelHorario devuelve los veterinarios habilitados con una columna Disponible; el selector conserva visibles en gris y deshabilitados los ocupados. ObtenerVeterinariosDisponibles sigue devolviendo solo los libres para validar reservas.

## Integración con atención veterinaria

`BLLAtencion` obtiene las consultas mediante `BLLConsulta.ObtenerConsultas` → `ServicioConsultas.ObtenerConsultasVeterinario` → SOAP. El servicio devuelve únicamente las consultas asignadas al DNI autenticado, excluyendo canceladas. La aplicación conserva el control de acceso a las pantallas. Los listados se filtran únicamente por Consulta.estado.

`BLLAtencion` usa `ServicioConsultas` para leer y guardar la atención mediante SOAP. `GuardarAtencion` inserta o corrige diagnóstico y observaciones cifrados en Atencion y cambia Consulta.estado a Atendida dentro de una misma transacción. Comprueba usuario, pertenencia al veterinario, consulta no cancelada y correspondencia del identificador de atención. No usa permisos de pantallas, BLL ni DAL dentro del servicio. Se retiró DALAtencion.

Se permite atender antes del horario del turno. En Atenciones se puede elegir Pendientes o Atendidas; desde Atendidas se selecciona una consulta y se usa Corregir atención para modificar el registro existente. No se agregó un control específico de simultaneidad entre atención y cancelación.

Antes de guardar, BLLAtencion verifica integridad; después del éxito llama a InicializarTablaCompleta para Atencion y Consulta. El servicio no calcula dígitos.

Mis consultas obtiene el diagnóstico de Atencion, por lo que refleja las correcciones. Para datos anteriores conserva la lectura de Consulta.diagnostico cuando no existe una atención registrada. Consulta.estado es la única fuente del estado; Atencion conserva el diagnóstico y las observaciones. Se retiró el campo redundante atendido, conservando los registros y actualizando la integridad. Las consultas atendidas dejan de ofrecer cancelación.

En Atenciones, Ver atención abre la consulta atendida con diagnóstico y observaciones de solo lectura, sin botón de guardado. Corregir atención mantiene la edición separada.

No se presentó es un estado manual: el veterinario selecciona un turno pendiente y confirma el botón. El servicio valida que corresponda al veterinario, que el horario haya llegado y que no esté atendido ni cancelado; guarda el estado cifrado y la auditoría, y la BLL recalcula la integridad de Consulta. No se crea una atención clínica para una ausencia. Mis consultas y Atenciones permiten filtrar este estado.

El paso del tiempo nunca marca ausencias automáticamente: un turno pasado sigue Agendado y puede mostrarse como Pendiente de registro hasta que el veterinario cargue la atención o marque la ausencia manualmente.

Si el paciente llega tarde, en el filtro No se presentó el veterinario puede seleccionar el turno y usar Atender. Se abre el formulario habitual; solo al guardar la atención el estado cambia a Atendida. Volver sin guardar conserva NoSePresento. La auditoría previa de ausencia se conserva y se agrega el evento del guardado de atención.

En ejecución local, los fallos técnicos de la llamada SOAP se registran en `FGF-Finanzas/App_Data/ConsultasService.errores.log` para diagnóstico. No se registran los parámetros enviados al servicio; el archivo está excluido de Git por la regla de archivos `.log`.

Las pantallas Atenciones y AtenderConsulta usan el catálogo Etiqueta/Traduccion en es-AR y en-US, incluidos títulos, filtros, columnas, selección, acciones, confirmación de ausencia, mensajes de validación y alerta de guardado. Los mensajes visibles y los textos dinámicos se actualizan al cambiar el idioma. Se incluyen el enlace del menú y el estado No se presentó del cliente. La actualización del catálogo requiere recalcular la integridad de Etiqueta y Traduccion.
