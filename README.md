![Cerberus](src/Cerberus/Properties/Cerberus.png?raw=true "Cerberus")
# Sitecore Cerberus

After [Rainbow](<https://github.com/SitecoreUnicorn/Rainbow>) and [Unicorn](https://github.com/SitecoreUnicorn/Unicorn) now there is the Cerberus!

Cerberus is a rule validation framework, which is designed to validate [Sitecore Helix](https://helix.sitecore.net/) principles for the Sitecore Items.

Currently the support is available only for unicorn.

Features:

- Item Readers

  - Template Reader
  - Rendering Reader
  - ....

- Analyzers

  - Template Analyzer
  - Rendering Analyzer
  - ....

- Rules

  - Template Inheritance
  - Template Duplicate Field
  - Rendering Name Convention
  - Rendering DataSource
  - ....

- Logger

  - Console Logger

  - Html Logger

Here is some demo after modifying habitat:

![DemoConsole](/assets/pics/DemoConsoleHabitat.png?raw=true "Cerberus")	

And for html logger:

![HtmlLogDemo1](/assets/pics/DemoHtmlTemplates.jpg?raw=true "HtmlLogDemo1")

![HtmlLogDemo2](/assets/pics/DemoHtmlRenderings.jpg?raw=true "HtmlLogDemo1")

### Quick Start!

To use the Cerberus you need to set the relative path(relative to cerberus.config) of your project inside the cerberus.config:

- on `<dataSourceLocationProvider dataSourceLocation="$(configPath)..\..\..\..\..\Habitat\src">`
- on `<sourceDataStore physicalRootPath="$(configPath)..\..\..\..\..\Habitat\src\$(layer)\$(module)\Serialization">`

after that you should specify the report output folder:

- `<logger name="HtmlLogger" type="Cerberus.Core.Logging.HtmlLogger, Cerberus.Core" singleInstance="true" outputPath="$(configPath)..\..\..\..\..\Reports\" />`

then you can call it like:

`Cerberus.exe -c "[path to cerberus.config]]\Cerberus.config"`

For more information and extending the Cerberus, check out the WIKI!



### Caution!

This project is really young and will go trough changes. Currently the release version is 1.0-alpha. 

So feel free to develop new rules,analyzer or even readers but keep in mind, the changes coming (they always do!).






