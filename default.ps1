$framework = "4.0"
	
	properties {
	       $baseDir  = resolve-path ./
	       $publishDir = "$baseDir\publish"
	       $slnFile = "$basedir\SolrNet.sln"
	       $projectFile = "$baseDir\SolrNet\SolrNet.csproj"
	       $assemblyInfoFile = "$baseDir\SolrNet\Properties\AssemblyInfo.cs"
	}

	task Build {
    Invoke-NuGetRestore -nuGetExePath "$YDeployDir/$($systemParams.Nuget.Path)" -slnFile $slnFile
	Invoke-MsBuild -msBuildPath msbuild -slnFile $slnFile -config $build_cfg -skipVcsRevision
}
	task CreateNugetPackage {
	       Remove-Artifacts -publishDir $publishDir
	    
	    $version = "$($projectParams.Product.Version.major).$($projectParams.Product.Version.minor).$($projectParams.Product.Version.release).$(Get-VcsRevision)"
	    $vcsRevisionHash = Get-VcsRevisionHash
	       $branchName = $(Get-VcsBranch)
	
	       Invoke-NuGetRestore -nuGetExePath "$YDeployDir/$($systemParams.Nuget.Path)" -slnFile $slnFile
	
	    Set-AssemblyInfo -config "Release" -assemblyInfoFile $assemblyInfoFile -productName $projectParams.Product.Name -version $version -branch $branchName -revision $vcsRevisionHash
	
	       Invoke-MsBuild `
	             -msBuildPath msbuild `
	             -appProjectFile $projectFile `
	             -config "Release" `
	             -publishDir $publishDir `
	        -skipVcsRevision
		
		remove-item -path $publishDir\*.pdb
		remove-item -path $publishDir\*.xml
		
	    Invoke-PsakeRunnerExecuteTemplateTask -taskname CreateNugetPackage -YDeployDir $YDeployDir -parameters @{ "NugetPackage" = $projectParams.NugetPackage; "hashedVersion" = $version }
	    Invoke-PsakeRunnerExecuteTemplateTask -taskname PublishToNuget -YDeployDir $YDeployDir -parameters @{ "NugetPackage" = $projectParams.NugetPackage; }
	}

