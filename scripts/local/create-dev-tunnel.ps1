Write-Output 'Creating development SSH tunnels...'

$forwards = @(
  @{
    name   = 'mysql'
    local  = 13306
    remote = 13306
  },
  @{
    name   = 'phpmyadmin'
    local  = 13307
    remote = 13307
  }
)

$debug_string = $forwards | ForEach-Object {
  "[$($_.name)]$($_.local)=>$($_.remote)"
} | Join-String -Separator ', '

Write-Output "Forwarding ports: $debug_string"

$fwd_string = $forwards | ForEach-Object {
  "-L $($_.local):localhost:$($_.remote)"
} | Join-String -Separator ' '

Start-Process -FilePath 'ssh' -ArgumentList "-N $fwd_string dev-tll" -NoNewWindow -Wait

Write-Output 'Shutting down tunnels...'
