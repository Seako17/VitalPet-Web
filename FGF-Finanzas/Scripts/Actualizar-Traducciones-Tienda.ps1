$ErrorActionPreference = 'Stop'

$projectRoot = Split-Path $PSScriptRoot -Parent
[xml]$webConfig = [System.IO.File]::ReadAllText((Join-Path $projectRoot 'Web.config'))
$connectionNode = @($webConfig.configuration.connectionStrings.add) |
    Where-Object { $_.name -eq 'DBConnection' } |
    Select-Object -First 1

if ($null -eq $connectionNode) {
    throw 'No se encontró la conexión DBConnection en Web.config.'
}

$connection = New-Object System.Data.SqlClient.SqlConnection($connectionNode.connectionString)
$encoding = [System.Text.Encoding]::UTF8
$sha = [System.Security.Cryptography.SHA256]::Create()

function Get-HashText([string]$value) {
    $bytes = $sha.ComputeHash($encoding.GetBytes($value))
    return ([System.BitConverter]::ToString($bytes).Replace('-', '')).ToLowerInvariant()
}

function Convert-HashToBigInteger([string]$hex) {
    return [System.Numerics.BigInteger]::Parse(
        '00' + $hex,
        [System.Globalization.NumberStyles]::HexNumber
    )
}

function Get-HorizontalDigest([string[]]$values) {
    $partial = [System.Numerics.BigInteger]::Zero
    foreach ($value in $values) {
        $partial += Convert-HashToBigInteger (Get-HashText $value)
    }

    $finalHash = Get-HashText ($partial.ToString())
    return (Convert-HashToBigInteger $finalHash).ToString('X')
}

try {
    $connection.Open()

    $migrationPath = Join-Path $PSScriptRoot 'Actualizar-Traducciones-Tienda.sql'
    $migrationCommand = $connection.CreateCommand()
    $migrationCommand.CommandText = [System.IO.File]::ReadAllText($migrationPath, $encoding)
    [void]$migrationCommand.ExecuteNonQuery()
    $migrationCommand.Dispose()

    $transaction = $connection.BeginTransaction()
    try {
        foreach ($table in @('Etiqueta', 'Traduccion')) {
            $select = $connection.CreateCommand()
            $select.Transaction = $transaction
            $select.CommandText = "SELECT * FROM dbo.[$table] ORDER BY 1"
            $reader = $select.ExecuteReader()
            $columns = @()
            for ($i = 0; $i -lt $reader.FieldCount; $i++) {
                $columns += $reader.GetName($i)
            }

            $primaryKey = $columns[0]
            $rows = [System.Collections.Generic.List[object]]::new()
            while ($reader.Read()) {
                $values = [System.Collections.Generic.List[string]]::new()
                for ($i = 0; $i -lt $reader.FieldCount; $i++) {
                    if ($columns[$i] -eq 'DV_Horizontal') { continue }
                    $value = $reader.GetValue($i)
                    if ($value -is [System.DBNull]) {
                        $values.Add('')
                    }
                    else {
                        $values.Add([Convert]::ToString($value, [System.Globalization.CultureInfo]::CurrentCulture))
                    }
                }

                $rows.Add([pscustomobject]@{
                    Id = $reader.GetValue(0)
                    Values = $values.ToArray()
                })
            }
            $reader.Close()
            $select.Dispose()

            $vertical = [System.Numerics.BigInteger]::Zero
            foreach ($row in $rows) {
                $horizontal = Get-HorizontalDigest $row.Values
                $vertical += Convert-HashToBigInteger (Get-HashText $horizontal)

                $update = $connection.CreateCommand()
                $update.Transaction = $transaction
                $update.CommandText = "UPDATE dbo.[$table] SET DV_Horizontal=@digest WHERE [$primaryKey]=@id"
                [void]$update.Parameters.AddWithValue('@digest', $horizontal)
                [void]$update.Parameters.AddWithValue('@id', $row.Id)
                [void]$update.ExecuteNonQuery()
                $update.Dispose()
            }

            $saveVertical = $connection.CreateCommand()
            $saveVertical.Transaction = $transaction
            $saveVertical.CommandText = @"
UPDATE dbo.DigitoVerificador
SET DigitoVertical=@vertical, CantidadRegistros=@count
WHERE NombreTabla=@table;
IF @@ROWCOUNT=0
    INSERT dbo.DigitoVerificador(NombreTabla,DigitoVertical,CantidadRegistros)
    VALUES(@table,@vertical,@count);
"@
            [void]$saveVertical.Parameters.AddWithValue('@vertical', $vertical.ToString('X'))
            [void]$saveVertical.Parameters.AddWithValue('@count', $rows.Count)
            [void]$saveVertical.Parameters.AddWithValue('@table', $table)
            [void]$saveVertical.ExecuteNonQuery()
            $saveVertical.Dispose()
        }

        $transaction.Commit()
    }
    catch {
        $transaction.Rollback()
        throw
    }
    finally {
        $transaction.Dispose()
    }

    Write-Output 'Traducciones sincronizadas y dígitos verificadores recalculados.'
}
finally {
    $sha.Dispose()
    $connection.Dispose()
}
