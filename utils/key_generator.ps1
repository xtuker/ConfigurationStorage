#Requires -Version 7.0
$keyLen = 32

$key = [byte[]]::CreateInstance([byte], $keyLen)
[System.Security.Cryptography.RandomNumberGenerator]::Fill($key)
[System.Convert]::ToBase64String($key)