Input variables:

<<<=== Important ===>>>

-input=<string>    -  Specify the root files directory path

<<<===   Other   ====>>>

-serialize=<bool>  -  Specifies that the files used in the project 
					  should be serialized for further restoration
							Values:
								"true"  - create .restore files,
								"false" - not saving files - default
-output=<string>   -  Specifies the name of output files
-mode=<string>	   -  Specifies the current project mode (create new or restore exists)
							Values:
								"new"     - create new project - default,
								"restore" - if project exists with .restore files. Continue rendering
-env=<string>	   -  Development or Production (dev or prod)
							Values:
								"dev"  or "development",
								"prod" or "production" - default
-quality=<string>  -  Specify output video resolution
							Values:
								"preview" - 360p,
								"small"   - 480p,
								"hd"      - 720p - default,
								"fhd"     - 1080p,
								"qhd"     - 1440p,
								"uhd"     - 2160p