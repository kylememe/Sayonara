[CmdletBinding()]
Param(    
    
    [Parameter(Mandatory=$true)]
    [string]$SqlServer,
    
    [Parameter(Mandatory=$true)]
    [string]$Database,

    [Parameter(Mandatory=$true)]
    [string]$SayonaraUrl,

    [Parameter(Mandatory=$true)]
    [string]$SayonaraWorkerAzureAppID,
    
    [Parameter(Mandatory=$true)]
    [string]$SayonaraWorkerAzureAppKey,

    [Parameter(Mandatory=$true)]
    [string]$SayonaraAzureAppURI
)

function Split-array 
{

<#  
  .SYNOPSIS   
    Split an array 
  .PARAMETER inArray
   A one dimensional array you want to split
  .EXAMPLE  
   Split-array -inArray @(1,2,3,4,5,6,7,8,9,10) -parts 3
  .EXAMPLE  
   Split-array -inArray @(1,2,3,4,5,6,7,8,9,10) -size 3
#> 

  param($inArray,[int]$parts,[int]$size)
  
  if ($parts) {
    $PartSize = [Math]::Ceiling($inArray.count / $parts)
  } 
  if ($size) {
    $PartSize = $size
    $parts = [Math]::Ceiling($inArray.count / $size)
  }

  $outArray = @()
  for ($i=1; $i -le $parts; $i++) {
    $start = (($i-1)*$PartSize)
    $end = (($i)*$PartSize) - 1
    if ($end -ge $inArray.count) {$end = $inArray.count}
    $outArray+=,@($inArray[$start..$end])
  }
  return ,$outArray
}

$SayonaraSeedFacilitiesUrl = $SayonaraUrl + "/api/Facilities/Seed"
$SayonaraSeedDocumentationViewsUrl = $SayonaraUrl + "/api/DocumentationViews/Seed"

### Load ADAL
Add-Type -Path "$PSScriptRoot\Microsoft.IdentityModel.Clients.ActiveDirectory.dll"
# Create Authentication Context tied to Azure AD Tenant
$authContext = New-Object "Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext" -ArgumentList "https://login.microsoftonline.com/nethealth.com"
#Get client credential for app we are
$clientCredential = New-Object "Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential" -ArgumentList $SayonaraWorkerAzureAppID, $SayonaraWorkerAzureAppKey
#Get Access Token for use as Bearer Token
$AccessToken = $authContext.AcquireTokenAsync($SayonaraAzureAppURI, $clientCredential).Result.AccessToken

#Setup connection to use to get tables
$cn = new-object system.data.SqlClient.SqlConnection("Data Source=" + $SqlServer + ";Initial Catalog=" + $Database + ";Trusted_Connection=True;");

$facilities = @()
$facilitiesQuery = "Select FacilityID, FacilityName, FacilityAlias, 
                    AdminFName, AdminLName, FacilityAddress1, FacilityAddress2, FacilityCity, FacilityState, FacilityZip 
                    From tbl_Facility"

#Setup Facilities for JSON
$facilitiesDS = new-object "System.Data.DataSet" "tablesToExport"
$da = new-object "System.Data.SqlClient.SqlDataAdapter" ($facilitiesQuery, $cn)
$da.Fill($facilitiesDS)

$facilitiesTable = new-object "System.Data.DataTable" "tables"
$facilitiesTable = $facilitiesDS.Tables[0]

$facilitiesTable | FOREACH-OBJECT {
    $ContactName = $_.AdminFName + " " + $_.AdminLName
    $facility = @{
        ID = $_.FacilityID
        Name = $_.FacilityName
        Alias = $_.FacilityAlias
        ContactName = $ContactName
        Address1 = $_.FacilityAddress1
        Address2 = $_.FacilityAddress2
        City = $_.FacilityCity
        State = $_.FacilityState
        ZipCode = $_.FacilityZip
    }

    $facilities += $facility
}

$views = @()
$viewsQuery = "Select DocumentationFacilityViewID, FacilityID, Name, MedicalRecordCopy, recActive
                From tbl_DocumentationFacilityViews 
                Where FacilityID Is Not Null
                And tbl_DocumentationFacilityViews.FacilityID In (Select FacilityID From tbl_Facility)"

#Setup DocumentationViews for JSON
$viewsDS = new-object "System.Data.DataSet" "tablesToExport"
$da = new-object "System.Data.SqlClient.SqlDataAdapter" ($viewsQuery, $cn)
$da.Fill($viewsDS)

$viewsTable = new-object "System.Data.DataTable" "tables"
$viewsTable = $viewsDS.Tables[0]


$viewsTable | FOREACH-OBJECT {
    $view = @{
        ID = $_.DocumentationFacilityViewID
        FacilityID = $_.FacilityID
        Name = $_.Name
        MedicalRecordCopy = $_.MedicalRecordCopy
        recActive = $_.recActive
    }

    $views += $view
}

$enc = [system.Text.Encoding]::UTF8

#We chunk the facilities so the seed process doesn't time out.
$NumberOfChunks = [Math]::Ceiling($facilities.length / 500)
$chunkedFacilities = Split-array -inArray $facilities -parts $NumberOfChunks

for ($i=0; $i -lt $chunkedFacilities.length; $i++) {

  $facilitiesJSON = $chunkedFacilities[$i] | ConvertTo-Json 

  $facilityJSONUTF = $enc.GetBytes($facilitiesJSON)

  try{
    Invoke-RestMethod $SayonaraSeedFacilitiesUrl -Method Post -Body $facilityJSONUTF -ContentType 'application/json;charset=utf-8' -Headers @{"Authorization" = "Bearer $($AccessToken)"}
    Write-Host "Succesful facilities chunk post"
  }
  catch{
    Write-Host "Something went wrong with the facility seed: StatusCode:" + $_.Exception.Response.StatusCode.value__
  }
    
}

#We chunk the documentation views so the seed process doesn't time out.
$NumberOfChunks = [Math]::Ceiling($views.length / 500)
$chunkedViews = Split-array -inArray $views -parts $NumberOfChunks

for ($i=0; $i -lt $chunkedViews.length; $i++) {

  $viewsJSON = $chunkedViews[$i] | ConvertTo-Json 

  $viewJSONUTF = $enc.GetBytes($viewsJSON)

  try{
    Invoke-RestMethod $SayonaraSeedDocumentationViewsUrl -Method Post -Body $viewJSONUTF -ContentType 'application/json;charset=utf-8' -Headers @{"Authorization" = "Bearer $($AccessToken)"}
    Write-Host "Succesful doc Views chunk post"
  }
  catch{
    Write-Host "Something went wrong with the view seed: StatusCode: " + $_.Exception.Response.StatusCode.value__
  }
    
}
