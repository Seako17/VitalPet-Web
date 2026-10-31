# Informe mensual de consultas

Acceso desde Informes en el encabezado, mediante el permiso InformeVer. El permiso se asignó a los perfiles que ya tenían UsuarioGestionar; los perfiles se pueden administrar desde Gestión Perfiles. Una sesión abierta antes del cambio debe volver a iniciarse para cargar el permiso.

1. Elegir mes, año y todos los veterinarios o uno en particular. No se permiten períodos posteriores al mes actual.
2. Pulsar Generar y guardar. Cada generación crea un informe nuevo; los anteriores no se sobrescriben.
3. Elegir un registro en Informes guardados para mostrar Ver PDF, sin mostrar el contenido en la página.
4. Ver PDF abre ese registro en otra pestaña, con tabla, resumen y gráfico de barras. Desde el visor del navegador se puede descargar o imprimir. El acceso directo al PDF conserva la validación del permiso y de integridad.

El período se determina por la fecha del turno, no por la fecha en que se registró la atención. Se cuentan turnos, no personas. Consulta.estado determina atendidos, cancelados, ausencias y pendientes. Un turno pasado que siga agendado se cuenta como pendiente. El porcentaje es atendidos / total de turnos. Los empates se muestran completos.

El gráfico circular Consultas del mes por estado muestra las cantidades de atendidos, cancelados, ausencias y pendientes del resultado mensual guardado. Respeta el filtro del informe: todos los veterinarios o solo el seleccionado. Si no hay turnos se dibuja un círculo neutro con las cantidades en cero.

InformeConsultas guarda período, filtro, fecha y usuario de generación. InformeConsultasDetalle guarda las cantidades calculadas por veterinario y su nombre cifrado. El guardado se hace en una transacción; la BLL valida y actualiza la integridad de ambas tablas. Los informes guardados no cambian si después se corrige una consulta.

InformeConsultasMes guarda los doce totales mensuales de atenciones del año elegido, usando la fecha del turno y el mismo filtro de veterinario. Se guarda junto al informe y se controla su integridad. El PDF muestra ese resumen como gráfico de líneas con meses en el eje X y atendidos en el eje Y. Para todos los veterinarios conserva además el gráfico de barras del mes; al filtrar un solo veterinario omite las barras. Los informes anteriores a esta mejora conservan sus resultados originales y no muestran un resumen anual que no tenían guardado: generar uno nuevo para incluirlo.

Se conservan las capas GUI, BLL y DAL. El informe tiene su propia lectura para estadísticas; no modifica consultas ni depende de iniciar el web service. ServicioInformePdf arma el documento con iTextSharp y dibuja las barras directamente en el PDF.

Dependencias NuGet: iTextSharp 5.5.13.6 y BouncyCastle.Cryptography 2.6.2. Restaurar paquetes al abrir el proyecto en otro equipo. iTextSharp se distribuye bajo licencia AGPL/comercial; se conserva su información de productor en el PDF. Se usa Arial de Windows para las tildes y nombres.

La base debe contar con InformeConsultas, InformeConsultasDetalle, InformeConsultasMes, el permiso InformeVer y sus traducciones en Etiqueta/Traduccion. Estas estructuras se prepararon en la base local junto con la integridad. No se generaron informes con datos reales durante la implementación.

Validación realizada: compilación de solución y ASP.NET, verificación de integridad después de preparar la base, y generación/revisión visual de PDF en español e inglés con un ejemplo ficticio en memoria. No se ejecutó una generación de informes sobre los turnos reales ni un recorrido visual de la pantalla.
