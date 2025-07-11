$path = "$env:RIMWORLD\Mods\$(($PSScriptRoot | gi).Name).Experimental"

Copy-Item "$path\About\PublishedFileId.txt" "$PSScriptRoot\About\PublishedFileId.Experimental.txt"
Remove-Item -Recurse "$path\*"
mkdir $path
@(
	".",
	"1.2",
	"1.3",
	"1.4",
	"1.5",
	"1.6"
) | %{
	$base = $_
	@(
		"About",
		"Assemblies",
		"Defs",
		"Languages",
		"Patches",
		"Songs",
		"Sounds",
		"Source",
		"Textures"
	) | %{ Copy-Item -Recurse "$PSScriptRoot\$base\$_" "$path\$base\$_" }
	Remove-Item -Recurse "$path\$base\Source\bin"
	Remove-Item -Recurse "$path\$base\Source\obj"
	Remove-Item "$path\$base\Source\packages.config"
}
(Get-Content "$path\About\About.xml").Replace("<name>1trickPwnyta's Defaults</name>", "<name>[Experimental] 1trickPwnyta's Defaults</name>") | Set-Content "$path\About\About.xml"
(Get-Content "$path\About\About.xml").Replace("<packageId>defaults.1trickPwnyta</packageId>", "<packageId>experimental.defaults.1trickPwnyta</packageId>") | Set-Content "$path\About\About.xml"
(Get-Content "$path\About\About.xml").Replace("<li>experimental.defaults.1trickPwnyta</li>", "<li>defaults.1trickPwnyta</li>") | Set-Content "$path\About\About.xml"
Remove-Item "$path\About\PublishedFileId.txt"
Rename-Item "$path\About\PublishedFileId.Experimental.txt" "$path\About\PublishedFileId.txt"