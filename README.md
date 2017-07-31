[AppVeyorProjectUrl]: https://ci.appveyor.com/project/GregTrevellick/autofindreplace
[AppVeyorProjectBuildStatusBadgeSvg]: https://ci.appveyor.com/api/projects/status/tcugu9rs3ihbgl7o?svg=true
[GitHubRepoURL]: https://github.com/GregTrevellick/AutoFindReplace
[GitHubRepoIssuesURL]: https://github.com/GregTrevellick/AutoFindReplace/issues
[GitHubRepoPullRequestsURL]: https://github.com/GregTrevellick/AutoFindReplace/pulls
[VersionNumberBadgeURL]: https://vsmarketplacebadge.apphb.com/version/GregTrevellick.AutoFindReplace.svg
[VisualStudioURL]: https://www.visualstudio.com/
[VSMarketplaceUrl]: https://marketplace.visualstudio.com/items?itemName=GregTrevellick.AutoFindReplace
[VSMarketplaceReviewsUrl]: https://marketplace.visualstudio.com/items?itemName=GregTrevellick.AutoFindReplace#review-details


# SolutionOpenPopUp

[![Licence](https://img.shields.io/github/license/gittools/gitlink.svg)](/LICENSE.txt)
[![Build status][AppVeyorProjectBuildStatusBadgeSvg]][AppVeyorProjectUrl]
[![][VersionNumberBadgeURL]][VSMarketplaceUrl]
<!--![](https://vsmarketplacebadge.apphb.com/installs/GregTrevellick.AutoFindReplace.svg)-->
<!--![](https://vsmarketplacebadge.apphb.com/rating/GregTrevellick.AutoFindReplace.svg)-->
<!--[![Source Browser](https://img.shields.io/badge/Browse-Source-green.svg)](http://sourcebrowser.io/Browse/GregTrevellick/AutoFindReplace)-->

Download this extension from the [VS Marketplace][VSMarketplaceUrl].

---------------------------------------

<!--COPY START FOR VS GALLERY-->

This [Visual Studio][VisualStudioUrl] extension will automatically find and replace specified text within specified files when a solution is opened.

The intention is to eliminate repetitive manual code modifications that a developer may find neccessary for certain Visual Studio solutions.

With this extension installed Visual Studio will automatically perform a find/replace action on specified file(s) within a specified project upon opening a named solution.

If you like this ***free*** extension, please give it a [review][VSMarketplaceReviewsUrl].

See the [change log](CHANGELOG.md) for road map and release history. Bugs can be logged [here][GitHubRepoIssuesURL].

## Example


## Who Is This Extension For ?

 - It is not possible for the change(s) to be persisted indefinately in the developer's source control repository, for any reason

 - It is not possible for the change(s) to be persisted durably in the developer's local file system, for any reason

## Use-Cases

 - Your source-control system freakishly doesn't retained a file's modifications, and so every time you open a new branch of your solution you need to manually and repeatedly make the same changes to address this. *This was the real-life problem I experienced that inspired me to create this extension.*
 
 - Many more. *If you have a use-case you would like listed here, just let me know via a review on the [VS Gallery][VSMarketplaceReviewsUrl].*
 
## Features

- Modifications can be limited to files under source control only. This is the default setting, but can be overridden to allow non-source controlled files to be modified.

- If a solution is opened whose name matches a rule, user is shown a summary of any modifications made. This summary can be surpressed if no modifications were made.

<!--COPY END FOR VS GALLERY-->

## Contribute

Contributions to this project are welcome by raising an [Issue][GitHubRepoIssuesURL] or submitting a [Pull Request][GitHubRepoPullRequestsURL].

## License

[MIT](/LICENSE.txt)

## Credits

The following authors / articles deserve special mention for their help whilst creating this extension:

[Mads Kristensen](https://channel9.msdn.com/Events/Build/2016/B886)

[Joshua Thompson](http://schmalls.com/2015/01/19/adventures-in-visual-studio-extension-development-part-2)

[Slaks.Blog](http://blog.slaks.net/2013-11-10/extending-visual-studio-part-2-core-concepts/)

[Daniel Schroeder](http://blog.danskingdom.com/category/visual-studio-extensions/)

[Sho Sato](https://vsmarketplacebadge.apphb.com/)

[![](chart.png)][VSMarketplaceUrl]
