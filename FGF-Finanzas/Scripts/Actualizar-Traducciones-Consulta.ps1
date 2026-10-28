# Ejecutar con Windows PowerShell. Solo agrega claves faltantes y conserva traducciones existentes.
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Data
Add-Type -AssemblyName System.Numerics
[xml]$config = Get-Content (Join-Path $PSScriptRoot '../Web.config')
$connectionString = ($config.configuration.connectionStrings.add | Where-Object name -eq 'DBConnection').connectionString
$cn = New-Object System.Data.SqlClient.SqlConnection $connectionString
$cn.Open()
$tx = $cn.BeginTransaction([System.Data.IsolationLevel]::Serializable)
function Query($sql, $parameters = @{}) {
    $cmd = $cn.CreateCommand()
    $cmd.Transaction = $tx
    $cmd.CommandText = $sql
    foreach ($key in $parameters.Keys) { [void]$cmd.Parameters.AddWithValue($key, $parameters[$key]) }
    $dt = New-Object System.Data.DataTable
    $reader = $cmd.ExecuteReader()
    try { $dt.Load($reader) } finally { $reader.Dispose(); $cmd.Dispose() }
    return ,$dt
}
function HashText([string]$value) {
    if (!$value) { return '' }
    $sha = [System.Security.Cryptography.SHA256]::Create()
    try { return ([BitConverter]::ToString($sha.ComputeHash([Text.Encoding]::UTF8.GetBytes($value)))).Replace('-', '').ToLowerInvariant() }
    finally { $sha.Dispose() }
}
function HexNumber([string]$hex) { return [System.Numerics.BigInteger]::Parse(('00' + $hex), [Globalization.NumberStyles]::HexNumber) }
function RowHash($row) {
    $sum = [System.Numerics.BigInteger]::Zero
    foreach ($col in $row.Table.Columns) {
        if ($col.ColumnName -ne 'DV_Horizontal') { $sum += HexNumber (HashText ([string]$row[$col])) }
    }
    return (HexNumber (HashText $sum.ToString())).ToString('X')
}
$entries = @(
    @('Gestion-Idioma.aspx', 'MSG_CONFIRMAR_ELIMINAR_IDIOMA', '¿Eliminar el idioma {0}? Se perderán todas sus traducciones.', 'Delete language {0}? All its translations will be lost.'),
    @('Errores', 'ERR_TRADUCCION_SOLO_TEXTO', 'Ingrese solo texto, sin etiquetas HTML ni referencias numéricas de caracteres.', 'Enter plain text without HTML tags or numeric character references.'),
    @('Consulta.aspx', 'ddlHora_Seleccione', 'Hora', 'Time'),
    @('Site.Master', 'lblMenuConsultas', 'Consultas', 'Appointments'),
    @('Site.Master', 'lnkNuevaConsulta', 'Nueva consulta', 'New appointment'),
    @('Site.Master', 'lnkMisConsultas', 'Mis consultas', 'My appointments'),
    @('Errores', 'ERR_CONSULTA_PERMISO_FALTANTE', 'No se encontró el permiso {0} en el sistema.', 'Permission {0} was not found in the system.'),
    @('Errores', 'ERR_CONSULTA_MASCOTA_OCUPADA', 'La mascota ya tiene una consulta el {0:dd/MM/yyyy} a las {0:HH:mm}.', 'The pet already has an appointment on {0:MM/dd/yyyy} at {0:HH:mm}.'),
    @('Errores', 'ERR_CONSULTA_SIN_VETERINARIOS', 'No hay veterinarios disponibles el {0:dd/MM/yyyy} a las {0:HH:mm}.', 'No veterinarians are available on {0:MM/dd/yyyy} at {0:HH:mm}.'),
    @('Errores', 'ERR_CONSULTA_MANTENIMIENTO', 'No se pueden registrar consultas. El sistema se encuentra en mantenimiento.', 'Appointments cannot be scheduled while the system is under maintenance.'),
    @('Errores', 'ERR_CONSULTA_MOTIVO', 'Ingrese el motivo de la consulta.', 'Enter the reason for the appointment.'),
    @('Errores', 'ERR_CONSULTA_MASCOTA_INVALIDA', 'La mascota seleccionada no es válida.', 'The selected pet is invalid.'),
    @('Errores', 'ERR_CONSULTA_VETERINARIO_OCUPADO', 'El veterinario/a elegido ya no está disponible en ese horario.', 'The selected veterinarian is no longer available at that time.'),
    @('Errores', 'ERR_CONSULTA_FECHA_PASADA', 'No se puede agendar una consulta en una fecha y hora pasadas.', 'An appointment cannot be scheduled in the past.'),
    @('Errores', 'ERR_CONSULTA_FUERA_HORARIO', 'El horario elegido está fuera de la agenda de atención.', 'The selected time is outside appointment hours.')
)
try {
    # Verificar la integridad antes de agregar datos; nunca reparar inconsistencias previas.
    foreach ($table in @('Etiqueta', 'Traduccion')) {
        $rows = Query "SELECT * FROM [$table]"
        $vertical = [System.Numerics.BigInteger]::Zero
        foreach ($row in $rows.Rows) {
            if ((RowHash $row) -cne [string]$row.DV_Horizontal) { throw "Integridad incorrecta en $table. No se realizaron cambios." }
            $vertical += HexNumber (HashText $row.DV_Horizontal)
        }
        $stored = Query 'SELECT * FROM DigitoVerificador WHERE NombreTabla=@table' @{ '@table' = $table }
        if ($stored.Rows.Count -ne 1 -or $stored.Rows[0].CantidadRegistros -ne $rows.Rows.Count -or $stored.Rows[0].DigitoVertical -cne $vertical.ToString('X')) {
            throw "Dígito vertical incorrecto en $table. No se realizaron cambios."
        }
    }
    foreach ($entry in $entries) {
        $argsSql = @{ '@form' = $entry[0]; '@key' = $entry[1] }
        $label = Query 'SELECT IdEtiqueta FROM Etiqueta WHERE Formulario=@form AND ControlId=@key' $argsSql
        if ($label.Rows.Count -eq 0) {
            $label = Query 'INSERT INTO Etiqueta(Formulario,ControlId) OUTPUT INSERTED.IdEtiqueta VALUES(@form,@key)' $argsSql
        }
        foreach ($language in @(@('es-AR', 2), @('en-US', 3))) {
            [void](Query 'INSERT INTO Traduccion(IdIdioma,IdEtiqueta,Texto) SELECT i.IdIdioma,@id,@text FROM Idioma i WHERE i.Codigo=@code AND NOT EXISTS(SELECT 1 FROM Traduccion t WHERE t.IdIdioma=i.IdIdioma AND t.IdEtiqueta=@id)' @{
                '@id' = $label.Rows[0].IdEtiqueta; '@text' = $entry[$language[1]]; '@code' = $language[0]
            })
            # Corregir solo la variante exacta que produce Windows PowerShell al leer UTF-8 sin BOM.
            $text = $entry[$language[1]]
            $encoded = [Text.Encoding]::GetEncoding(1252).GetString([Text.Encoding]::UTF8.GetBytes($text))
            if ($encoded -cne $text) {
                [void](Query 'UPDATE t SET Texto=@text FROM Traduccion t JOIN Idioma i ON i.IdIdioma=t.IdIdioma WHERE t.IdEtiqueta=@id AND i.Codigo=@code AND t.Texto=@encoded' @{
                    '@id'=$label.Rows[0].IdEtiqueta; '@code'=$language[0]; '@text'=$text; '@encoded'=$encoded
                })
            }
        }
    }
    foreach ($table in @('Etiqueta', 'Traduccion')) {
        $pk = if ($table -eq 'Etiqueta') { 'IdEtiqueta' } else { 'IdTraduccion' }
        $rows = Query "SELECT * FROM [$table]"
        $vertical = [System.Numerics.BigInteger]::Zero
        foreach ($row in $rows.Rows) {
            $hash = RowHash $row
            if ([string]$row.DV_Horizontal -cne $hash) {
                [void](Query "UPDATE [$table] SET DV_Horizontal=@hash WHERE [$pk]=@id" @{ '@hash'=$hash; '@id'=$row[$pk] })
            }
            $vertical += HexNumber (HashText $hash)
        }
        [void](Query 'UPDATE DigitoVerificador SET DigitoVertical=@hash,CantidadRegistros=@count WHERE NombreTabla=@table' @{
            '@hash'=$vertical.ToString('X'); '@count'=$rows.Rows.Count; '@table'=$table
        })
    }
    $tx.Commit()
    Write-Output 'Traducciones de consultas actualizadas; integridad preservada.'
} catch {
    $tx.Rollback()
    throw
} finally { $cn.Dispose() }
