param([string]$SqlServer = '.', [int]$Port = 50951)
$ErrorActionPreference = 'Stop'
$repo = (Resolve-Path (Join-Path $PSScriptRoot '../..')).Path
$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio/Installer/vswhere.exe'
$msbuild = & $vswhere -latest -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
if (!$msbuild) { throw 'No se encontró MSBuild de Visual Studio.' }
& $msbuild (Join-Path $PSScriptRoot 'ConsultasService.Tests.csproj') /t:Build /p:Configuration=Debug /v:minimal /nologo /nodeReuse:false
if ($LASTEXITCODE -ne 0) { throw 'Falló la compilación.' }

$id = [Guid]::NewGuid().ToString('N')
$database = 'VitalPetConsultas_Test_' + $id
$runRoot = Join-Path $repo ('.verification/' + $id)
$site = Join-Path $runRoot 'site'
New-Item -ItemType Directory -Force (Join-Path $site 'bin') | Out-Null
Copy-Item -LiteralPath (Join-Path $repo 'VitalPet.ConsultasService/ConsultasService.asmx') -Destination $site
Copy-Item -LiteralPath (Join-Path $repo 'VitalPet.ConsultasService/bin/VitalPet.ConsultasService.dll') -Destination (Join-Path $site 'bin')
$connection = New-Object System.Data.SqlClient.SqlConnectionStringBuilder
$connection['Data Source'] = $SqlServer
$connection['Initial Catalog'] = $database
$connection['Integrated Security'] = $true
$connection['TrustServerCertificate'] = $true
$connectionXml = [System.Security.SecurityElement]::Escape($connection.ConnectionString)
$settings = '<appSettings><add key="ConsultasService.Url" value="http://localhost:' + $Port + '/ConsultasService.asmx" /></appSettings>'
$connections = '<connectionStrings><add name="DBConnection" connectionString="' + $connectionXml + '" providerName="System.Data.SqlClient" /></connectionStrings>'
$webConfig = '<configuration>' + $settings + $connections + '<system.web><compilation targetFramework="4.8.1" /><httpRuntime targetFramework="4.8.1" /><authentication mode="None" /></system.web></configuration>'
[IO.File]::WriteAllText((Join-Path $site 'Web.config'), $webConfig)
$exe = Join-Path $PSScriptRoot 'bin/VitalPet.ConsultasService.Tests.exe'
[IO.File]::WriteAllText(($exe + '.config'), ('<configuration>' + $settings + $connections + '</configuration>'))

$connection['Initial Catalog'] = 'master'
$master = New-Object System.Data.SqlClient.SqlConnection($connection.ConnectionString)
$server = $null
$created = $false
try {
    $master.Open()
    $cmd = $master.CreateCommand()
    $cmd.CommandText = 'CREATE DATABASE [' + $database + ']'
    $cmd.ExecuteNonQuery() | Out-Null
    $created = $true
    $iis = Join-Path $env:ProgramFiles 'IIS Express/iisexpress.exe'
    $server = Start-Process -FilePath $iis -ArgumentList @('/path:"' + $site + '"', '/port:' + $Port, '/systray:false') -WindowStyle Hidden -PassThru -RedirectStandardOutput (Join-Path $runRoot 'iis.out.log') -RedirectStandardError (Join-Path $runRoot 'iis.err.log')
    # Espera acotada al WSDL, sin depender de la base ni de datos reales.
    $ready = $false
    for ($intento = 0; $intento -lt 20; $intento++) {
        try {
            $response = Invoke-WebRequest ('http://localhost:' + $Port + '/ConsultasService.asmx?WSDL') -UseBasicParsing
            if ($response.StatusCode -eq 200) { $ready = $true; break }
        } catch { Start-Sleep -Milliseconds 500 }
    }
    if (!$ready) { throw ('No inició el servicio. Revisar ' + $runRoot) }
    & $exe
    if ($LASTEXITCODE -ne 0) { throw 'Fallaron las pruebas de integración.' }
} finally {
    if ($server -and !$server.HasExited) { Stop-Process -Id $server.Id }
    if ($created) {
        if ($database -notmatch '^VitalPetConsultas_Test_[0-9a-f]{32}$') { throw 'Nombre de base de pruebas inválido.' }
        $cmd = $master.CreateCommand()
        $cmd.CommandText = 'ALTER DATABASE [' + $database + '] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [' + $database + ']'
        $cmd.ExecuteNonQuery() | Out-Null
    }
    $master.Dispose()
    $verifiedRoot = [IO.Path]::GetFullPath($runRoot)
    $allowedRoot = [IO.Path]::GetFullPath((Join-Path $repo '.verification')) + [IO.Path]::DirectorySeparatorChar
    if (!$verifiedRoot.StartsWith($allowedRoot, [StringComparison]::OrdinalIgnoreCase)) { throw 'Ruta temporal fuera del repositorio.' }
    Remove-Item -LiteralPath $verifiedRoot -Recurse -Force
}
