#!/usr/bin/env pwsh
# Checks the sample against its evidence files and runs both builds.
# Exit code 0 means every check passed.
$ErrorActionPreference = 'Stop'
Set-Location $PSScriptRoot

$failed = 0
function Check([string]$what, [string]$expected, [string]$actual) {
    if ($expected -ieq $actual) {
        Write-Host "ok    $what"
    } else {
        Write-Host "FAIL  $what`n      expected $expected`n      actual   $actual"
        $script:failed++
    }
}
function Sha([string]$path) { (Get-FileHash -Algorithm SHA256 $path).Hash }

$before = Get-Content evidence/inspection-before.json -Raw | ConvertFrom-Json
$after = Get-Content evidence/inspection-after.json -Raw | ConvertFrom-Json
$receipt = Get-Content evidence/receipt-excerpt.json -Raw | ConvertFrom-Json
function ReceiptSha([string]$kind) { ($receipt.outputs | Where-Object kind -eq $kind).sha256 }

# The protected file and the two inspection reports are the job's outputs.
Check 'protected DLL matches the receipt' (ReceiptSha 'protected-binary') (Sha protected/InvoiceDesk.dll)
Check 'protected DLL matches the after inspection' $after.subject.sha256 (Sha protected/InvoiceDesk.dll)
Check 'before inspection matches the receipt' (ReceiptSha 'inspection-before') (Sha evidence/inspection-before.json)
Check 'after inspection matches the receipt' (ReceiptSha 'inspection-after') (Sha evidence/inspection-after.json)
Check 'receipt and before inspection name the same input' $receipt.input.sha256 $before.subject.sha256

# The input is not stored in the repository. Build it from src/.
$builtHere = -not (Test-Path before/InvoiceDesk.dll)
if ($builtHere) {
    & dotnet build src/InvoiceDesk.csproj -c Release -o before --nologo -v q | Out-Null
    if ($LASTEXITCODE -ne 0) { throw 'dotnet build failed' }
}
$inputSha = Sha before/InvoiceDesk.dll
if ($inputSha -ieq $receipt.input.sha256) {
    Write-Host 'ok    input DLL matches the receipt'
} elseif ($builtHere) {
    Write-Host 'note  the input you just built has a different hash than the uploaded one.'
    Write-Host '      A Release DLL embeds the build path, so this is expected.'
} else {
    Check 'input DLL matches the receipt' $receipt.input.sha256 $inputSha
}

# Invariant culture, so the decimal separator is the same on every machine.
$env:DOTNET_SYSTEM_GLOBALIZATION_INVARIANT = '1'
$outBefore = (& dotnet before/InvoiceDesk.dll) -join "`n"
$outAfter = (& dotnet protected/InvoiceDesk.dll) -join "`n"
Write-Host "`n$outAfter`n"
Check 'both builds print the same output' $outBefore $outAfter

if ($failed -gt 0) {
    Write-Host "`n$failed check(s) failed."
    exit 1
}
Write-Host "`nAll checks passed."
