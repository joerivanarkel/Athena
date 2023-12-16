# Athena

## Styling / SCSS

In this project we use SASS for styling. SASS is a CSS preprocessor that allows us to use variables, mixins, functions, and more, 
all with a fully CSS-compatible syntax. We combined this with TailwindCSS, a utility-first CSS framework for rapidly building custom designs. We are able to use them with the help of this MSBuild task:

In the Presentation project this task is used to compile the Global SCSS file and the Component SCSS files. The Global SCSS file is located in the `Styles` folder and the Component SCSS. For a Razor Class Library, only the Component SCSS files are compiled.

```xml
  <ItemGroup Label="Compile SCSS files">
    <ComponentScssFiles 
      Include="**/*/*.scss;**/*.scss"
      Exclude="node_modules/**;wwwroot/**;Styles/**" />
  </ItemGroup>

  <Target Name="CompileComponentSass" BeforeTargets="CompileGlobalSass">
    <Message Text="Compiling Component SCSS files" Importance="high" />
    <Exec
      Condition="!$([System.Text.RegularExpressions.Regex]::IsMatch('%(ComponentScssFiles.Identity)', `.*[/\\]_.*`))"
      Command="npm run sass -- --style=compressed --no-source-map --load-path=$(MSBuildProjectName)/Styles/Core $(MSBuildProjectName)/%(ComponentScssFiles.Identity) $(MSBuildProjectName)/%(relativedir)%(filename).css" />
  </Target>

  <Target Name="CompileGlobalSass" BeforeTargets="Compile">
    <Message Text="Compiling global SCSS file" Importance="high" />
    <Exec
      Command="npm run sass -- --style=compressed $(MSBuildProjectName)/Styles:$(MSBuildProjectName)/wwwroot/css" />
  </Target>
```

This task will compile all the SCSS files in the project and output them as CSS files. The outputted CSS files will be placed in the same directory as the SCSS files. The SCSS files that start with an underscore will not be compiled, but will be included in the other SCSS files. This is useful for creating partials.

To include the SCSS files in the `dotnet watch` command, you need to add the following to the project file:

```xml
  <ItemGroup Label="Add SCSS files to dotnet watch">
    <Watch Include="**\*.scss" />
    <None Update="**\*.css" watch="false" />
  </ItemGroup>
```

### Razor Class Library

In order to use the SASS files from a Razor Class Library, you need to add the following stylesheets to your `index.html` file:

```html
<link href="_content/YourClassLib/YourClassLib.bundle.scp.css" rel="stylesheet">
```