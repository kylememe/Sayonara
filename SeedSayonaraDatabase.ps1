$SayonaraUrl = "http://localhost:51287"
$SayonaraSeedFacilitiesUrl = $SayonaraUrl + "/api/Facilities/Seed"
$SayonaraSeedDocumentationViewsUrl = $SayonaraUrl + "/api/DocumentationViews/Seed"

$facilities = @()

for($i=1; $i -le 10; $i++)
{
    $facilityName = 'Facility Name' + $i
    $facilityAlias = 'Facility Alias' + $i

    $facility = @{
        ID = $i
        Name = $facilityName
        Alias = $facilityAlias
    }

    $facilities += $facility
}

$json = $facilities | ConvertTo-Json   
    

try{
Invoke-RestMethod $SayonaraSeedFacilitiesUrl -Method Post -Body $json -ContentType 'application/json'
}
catch{
    Write-Host "Something went wrong with the update: StatusCode:" $_.Exception.Response.StatusCode.value__
}