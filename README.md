# Athena

## Styling
### SASS 

In this project we use SASS for styling. SASS is a CSS preprocessor that allows us to use variables, mixins, functions, and more, 
all with a fully CSS-compatible syntax. We combined this with TailwindCSS, a utility-first CSS framework for rapidly building custom designs. We are able to use them with the help of this MSBuild task:

```xml
  <Target Name="CompileComponentSass" BeforeTargets="CompileGlobalSass">
    <Message Text="Compiling Component SCSS files" Importance="high" />
    <Exec Condition="!$([System.Text.RegularExpressions.Regex]::IsMatch('%(ComponentScssFiles.Identity)', `.*[/\\]_.*`))" Command="npm run sass -- --style=compressed --no-source-map --load-path=Styles/Core %(ComponentScssFiles.Identity) %(relativedir)%(filename).css" />
  </Target>

  <Target Name="CompileGlobalSass" BeforeTargets="Compile">
    <Message Text="Compiling global SCSS file" Importance="high" />
    <Exec Command="npm run sass -- --style=compressed Styles:wwwroot/css" />
  </Target>
```

This task will compile all the SCSS files in the project and output them as CSS files. The outputted CSS files will be placed in the same directory as the SCSS files. The SCSS files that start with an underscore will not be compiled, but will be included in the other SCSS files. This is useful for creating partials.