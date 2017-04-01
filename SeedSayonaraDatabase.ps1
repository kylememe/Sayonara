[CmdletBinding()]
Param(    
    
    [Parameter(Mandatory=$true)]
    [string]$SqlServer,
    
    [Parameter(Mandatory=$true)]
    [string]$Database,

    [Parameter(Mandatory=$true)]
    [string]$SayonaraUrl          
)

$SayonaraSeedFacilitiesUrl = $SayonaraUrl + "/api/Facilities/Seed"
$SayonaraSeedDocumentationViewsUrl = $SayonaraUrl + "/api/DocumentationViews/Seed"

#Setup connection to use to get tables
$cn = new-object system.data.SqlClient.SqlConnection("Data Source=" + $SqlServer + ";Initial Catalog=" + $Database + ";Trusted_Connection=True;");

$facilities = @()
$facilitiesQuery = "Select FacilityID, FacilityName, FacilityAlias From tbl_Facility Where ActiveFacility = 1"

#Setup Facilities for JSON
$facilitiesDS = new-object "System.Data.DataSet" "tablesToExport"
$da = new-object "System.Data.SqlClient.SqlDataAdapter" ($facilitiesQuery, $cn)
$da.Fill($facilitiesDS)

$facilitiesTable = new-object "System.Data.DataTable" "tables"
$facilitiesTable = $facilitiesDS.Tables[0]

$facilitiesTable | FOREACH-OBJECT {
    
    $facility = @{
        ID = $_.FacilityID
        Name = $_.FacilityName
        Alias = $_.FacilityAlias
    }

    $facilities += $facility
}

$views = @()
$viewsQuery = "Select DocumentationFacilityViewID, FacilityID, Name, MedicalRecordCopy 
                From tbl_DocumentationFacilityViews 
                Where recActive = 1 
                And FacilityID Is Not Null
                And tbl_DocumentationFacilityViews.FacilityID In (Select FacilityID From tbl_Facility Where ActiveFacility = 1)"

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
    }

    $views += $view
}

$enc = [system.Text.Encoding]::UTF8

$facilitiesJSON = $facilities | ConvertTo-Json

$facilityJSONUTF = $enc.GetBytes($facilitiesJSON)

try{
Invoke-RestMethod $SayonaraSeedFacilitiesUrl -Method Post -Body $facilityJSONUTF -ContentType 'application/json;charset=utf-8'
}
catch{
    Write-Host "Something went wrong with the facility seed: StatusCode:" $_.Exception.Response.StatusCode.value__
}

$viewsJSON = $views | ConvertTo-Json 

$viewJSONUTF = $enc.GetBytes($viewsJSON)

try{
Invoke-RestMethod $SayonaraSeedDocumentationViewsUrl -Method Post -Body $viewJSONUTF -ContentType 'application/json;charset=utf-8'
}
catch{
    Write-Host "Something went wrong with the view seed: StatusCode:" $_.Exception.Response.StatusCode.value__
}

