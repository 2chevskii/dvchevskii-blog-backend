Write-Output 'Creating development SSH tunnels...'

$forwards = @(
  @{
    local  = 3306
    remote = 3306
  },
  @{
    local  = 8080
    remote = 8080
  }
)

$debug_string = $forwards | ForEach-Object {
  "$($_.local) => $($_.remote)"
} | Join-String -Separator ', '

Write-Output "Forwarding ports: $debug_string"

$fwd_string = $forwards | ForEach-Object {
  "-L $($_.local):localhost:$($_.remote)"
} | Join-String -Separator ' '

Start-Process -FilePath 'ssh' -ArgumentList "-N $fwd_string dev-tll" -NoNewWindow -Wait

Write-Output 'Shutting down tunnels...'
