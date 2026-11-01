# Ejecutar con la aplicación detenida y después de compilar la solución.
$ErrorActionPreference = 'Stop'
$proyecto = Split-Path $PSScriptRoot -Parent
$dll = Join-Path $proyecto 'bin/FGF-Finanzas.dll'
if (-not (Test-Path -LiteralPath $dll)) { throw 'Primero compilá la solución.' }
$trabajo = Join-Path $env:TEMP ('VitalPet-Cifrado-' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $trabajo | Out-Null
Copy-Item -LiteralPath $dll -Destination $trabajo
[xml]$web = Get-Content -LiteralPath (Join-Path $proyecto 'Web.config') -Raw
$config = '<configuration>' + $web.configuration.connectionStrings.OuterXml + '</configuration>'
$compilador = Join-Path $env:WINDIR 'Microsoft.NET/Framework64/v4.0.30319/csc.exe'
foreach ($nombre in @('Migrar-Cifrado', 'Migrar-Fechas')) {
    $exe = Join-Path $trabajo ($nombre + '.exe')
    & $compilador /nologo "/r:$dll" /r:System.Data.dll /r:System.Data.DataSetExtensions.dll /r:System.Configuration.dll /r:System.Numerics.dll "/out:$exe" (Join-Path $PSScriptRoot ($nombre + '.cs'))
    if ($LASTEXITCODE -ne 0) { throw "No se pudo compilar $nombre." }
    [IO.File]::WriteAllText($exe + '.config', $config)
    if ($nombre -eq 'Migrar-Cifrado') { & $exe migrar } else { & $exe }
    if ($LASTEXITCODE -ne 0) { throw "Se detuvo $nombre. Revisá el mensaje anterior." }
}
Write-Output 'Cifrado actualizado. Los respaldos quedan en la carpeta de backups de SQL Server.'
